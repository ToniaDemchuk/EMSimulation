using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using Simulation.DDA.Models;
using Simulation.Infrastructure.Readers;
using Simulation.Models.Common;
using Simulation.Models.Coordinates;
using Simulation.Models.Enums;
using Simulation.Models.Extensions;
using Simulation.Models.Spectrum;

namespace Simulation.DDA.Grpc
{
    public class DDACalculatorService : DDACalculator.DDACalculatorBase
    {
        private readonly ILogger<DDACalculatorService> _logger;

        public DDACalculatorService(ILogger<DDACalculatorService> logger)
        {
            _logger = logger;
        }

        public override async Task CalculateDDA(DDARequest request, IServerStreamWriter<DDAReply> responseStream, ServerCallContext context)
        {
            var ddaConfig = new DDAParameters
            {
                IncidentMagnitude = new Simulation.Models.Coordinates.SphericalCoordinate
                {
                    Azimuth = request.IncidentMagnitude.Azimuth,
                    Polar = request.IncidentMagnitude.Polar,
                    Radius = request.IncidentMagnitude.Radius,
                    Units = UnitOfMeasurement.Degree
                },
                WavePropagation = new Simulation.Models.Coordinates.SphericalCoordinate
                {
                    Azimuth = request.WavePropagation.Azimuth,
                    Polar = request.WavePropagation.Polar,
                    Radius = request.WavePropagation.Radius,
                    Units = UnitOfMeasurement.Degree
                },
                IsSolidMaterial = false,
                WaveLengthConfig = new LinearDiscreteElement
                {
                    Count = request.WavelengthConfig.Count,
                    Lower = request.WavelengthConfig.Lower,
                    Upper = request.WavelengthConfig.Upper
                }
            };
            var systemConfig = ReadSystemConfigFromMesh(request.Mesh.Split('\n'));
            var medium = ParameterHelper.ReadOpticalConstants("opt_const.txt");
            var manager = new MediumManager(medium, ddaConfig.IsSolidMaterial);
            var ext = new ExtinctionManager(manager);

            SimulationParameters parameters = new SimulationParameters
            {
                WavePropagation = ddaConfig.WavePropagation,
                IncidentMagnitude = ddaConfig.IncidentMagnitude.ConvertToCartesian(),
                SystemConfig = systemConfig,
                Spectrum = ParameterHelper.ReadWavelengthFromConfiguration(ddaConfig)
            };

            var result = await ext.CalculateCrossExtinction(parameters, (unit, result) =>
            {
                lock (responseStream)
                {
                    return responseStream.WriteAsync(new DDAReply
                    {
                        Done = false,
                        Wave = unit.Value,
                        Result = JsonConvert.SerializeObject(new SimulationResult
                        {
                            CrossSectionExtinction = result.CrossSectionExtinction,
                            CrossSectionAbsorption = result.CrossSectionAbsorption,
                            EffectiveCrossSectionAbsorption = result.EffectiveCrossSectionAbsorption,
                            EffectiveCrossSectionExtinction = result.EffectiveCrossSectionExtinction
                        })
                    });
                }
            });

            await responseStream.WriteAsync(new DDAReply
            {
                Done = true,
                Result = JsonConvert.SerializeObject(result
                    .ToDictionary(
                                  pair => pair.Key.Value,
                                  pair => new SimulationResult
                                  {
                                      CrossSectionExtinction = pair.Value.CrossSectionExtinction,
                                      CrossSectionAbsorption = pair.Value.CrossSectionAbsorption,
                                      EffectiveCrossSectionAbsorption = pair.Value.EffectiveCrossSectionAbsorption,
                                      EffectiveCrossSectionExtinction = pair.Value.EffectiveCrossSectionExtinction
                                  }))
            });
        }

        public static SystemConfig ReadSystemConfigFromMesh(string[] lines)
        {
            var mesh = new FDSToVoxelReader().ReadInfo(lines);

            var pointList = mesh.Voxels.Select(voxel => new CartesianCoordinate(voxel.I, voxel.J, voxel.K)).ToList();

            var radiusList = Enumerable.Repeat(0.5, pointList.Count).ToList();

            return new SystemConfig(radiusList, pointList);
        }
    }
}
