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
using Simulation.Models.Spectrum;

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
            services.AddGrpcClient<Calculator.CalculatorClient>(options =>
            {
                options.ChannelOptionsActions.Add(channel => {
                    channel.MaxReceiveMessageSize = null;
                });
                options.Address = new Uri("https://dda-grpc:3000");
            });
            services
                .AddRazorPages()
                .AddRazorRuntimeCompilation();
            services.AddSignalR()
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
            });
        }
    }

    public class DDAHub : Hub<DDAHubClient>
    {
        private readonly Calculator.CalculatorClient calculatorClient;

        public DDAHub(Calculator.CalculatorClient calculatorClient)
        {
            this.calculatorClient = calculatorClient;
        }

        public async Task<object> StartCalculation(
            string mesh,
            SphericalCoordinate incidentMagnitude,
            SphericalCoordinate wavePropagation,
            WaveLengthConfig config)
        {
            var call = calculatorClient.Calculate(new DDARequest
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
    }

    public interface DDAHubClient
    {
        Task Progress(double replyWave);
    }
}
