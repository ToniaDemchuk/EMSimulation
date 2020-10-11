using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Grpc.Core;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Simulation.DDA.Grpc;
using Simulation.FDTD.Grpc;
using Simulation.Models.Spectrum;
using Simulation.Infrastructure.Readers;
using Simulation.Infrastructure.Models;

namespace Simulation.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpcClient<DDACalculator.DDACalculatorClient>(options =>
            {
                options.ChannelOptionsActions.Add(channel => {
                    channel.MaxReceiveMessageSize = null;
                });
                options.Address = new Uri("https://localhost:3001");
            });
            services.AddGrpcClient<FDTDCalculator.FDTDCalculatorClient>(options =>
            {
                options.ChannelOptionsActions.Add(channel => {
                    channel.MaxReceiveMessageSize = null;
                });
                options.Address = new Uri("https://localhost:3002");
            });
            services
                .AddRazorPages().AddRazorRuntimeCompilation();
            services.AddSignalR(options =>
                {
                    options.EnableDetailedErrors = true;
                })
                .AddNewtonsoftJsonProtocol();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();

                endpoints.MapHub<DDAHub>("/ddahub");
                endpoints.MapHub<FDTDHub>("/fdtdhub");
            });
        }
    }

    public class FDTDHub : Hub<ProgressHubClient>
    {
        private readonly FDTDCalculator.FDTDCalculatorClient calculatorClient;

        public FDTDHub(FDTDCalculator.FDTDCalculatorClient calculatorClient)
        {
            this.calculatorClient = calculatorClient;
        }

        public async Task<Dictionary<double, SimulationResult>> StartCalculation(
            string mesh,
            int numSteps,
            Simulation.FDTD.Grpc.SphericalCoordinate incidentMagnitude,
            Simulation.FDTD.Grpc.WaveLengthConfig config)
        {
            var call = calculatorClient.CalculateFDTD(new FDTDRequest
            {
                Mesh = mesh,
                NumSteps = numSteps,
                IncidentMagnitude = incidentMagnitude,
                WavelengthConfig = config
            });

            await foreach (var reply in call.ResponseStream.ReadAllAsync())
            {
                if (reply.Done)
                {
                    return JsonConvert.DeserializeObject<Dictionary<double, SimulationResult>>(reply.Result);
                }
                else
                {
                    await this.Clients.Caller.Progress(reply.Time);
                }
            }

            return null;
        }
    }

    public class DDAHub : Hub<ProgressHubClient>
    {
        private readonly DDACalculator.DDACalculatorClient calculatorClient;

        public DDAHub(DDACalculator.DDACalculatorClient calculatorClient)
        {
            this.calculatorClient = calculatorClient;
        }

        public async Task<Dictionary<double, SimulationResult>> StartCalculation(
            string mesh,
            Simulation.DDA.Grpc.SphericalCoordinate incidentMagnitude,
            Simulation.DDA.Grpc.SphericalCoordinate wavePropagation,
            Simulation.DDA.Grpc.WaveLengthConfig config)
        {
            var call = calculatorClient.CalculateDDA(new DDARequest
            {
                Mesh = mesh,
                IncidentMagnitude = incidentMagnitude,
                WavePropagation = wavePropagation,
                WavelengthConfig = config
            });

            await foreach (var reply in call.ResponseStream.ReadAllAsync())
            {
                if (reply.Done)
                {
                    return JsonConvert.DeserializeObject<Dictionary<double, SimulationResult>>(reply.Result);
                }
                else
                {
                    await this.Clients.Caller.Progress(reply.Wave);
                }
            }

            return null;
        }

        public async Task<List<Voxel>> GetMesh(string meshContent)
        {
           var mesh = new FDSToVoxelReader().ReadInfo(meshContent.Split('\n'));

            return mesh.Voxels;
        }
    }

    public interface ProgressHubClient
    {
        Task Progress(double replyWave);
    }
}
