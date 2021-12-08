using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using Simulation.FDTD.Models;
using Simulation.Infrastructure.Iterators;
using Simulation.Infrastructure.Models;
using Simulation.Infrastructure.Readers;
using Simulation.Medium.Factories;
using Simulation.Medium.Models;
using Simulation.Models.Common;
using Simulation.Models.Coordinates;
using Simulation.Models.Enums;
using Simulation.Models.Extensions;
using Simulation.Models.Spectrum;
using Simulation.Models.Constants;

namespace Simulation.FDTD.Grpc
{
    public class FDTDCalculatorService : FDTDCalculator.FDTDCalculatorBase
    {
        private readonly ILogger<FDTDCalculatorService> _logger;

        public FDTDCalculatorService(ILogger<FDTDCalculatorService> logger)
        {
            _logger = logger;
        }

        public override async Task CalculateFDTD(FDTDRequest request, IServerStreamWriter<FDTDReply> responseStream, ServerCallContext context)
        {
            var numSteps = request.NumSteps;
            var pmlLength = 10;

            var iterator = new ParallelIterator();
            var ext = new FDTDSimulation(iterator);

            var mesh = new FDSToVoxelReader().ReadInfo(request.Mesh.Split("\n"));

            SimulationParameters parameters = new SimulationParameters
            {
                Indices = getIndexStore(pmlLength, mesh.Resolution),
                CellSize = 1e-9,
                Spectrum =
                    new OpticalSpectrum(new LinearDiscreteCollection(request.WavelengthConfig.Lower, request.WavelengthConfig.Upper, request.WavelengthConfig.Count), SpectrumUnitType.WaveLength),
                CourantNumber = 0.5,
                PmlLength = pmlLength,
                NumSteps = numSteps,
                WaveFunc = time => new Simulation.Models.Coordinates.SphericalCoordinate(request.IncidentMagnitude.Radius, request.IncidentMagnitude.Polar, request.IncidentMagnitude.Azimuth, UnitOfMeasurement.Degree).ConvertToCartesian() * Math.Exp(-0.5 * Math.Pow((30 - time) / 5.0, 2.0)),
                IsSpectrumCalculated = true
            };

            setMedium(parameters, mesh);

            ext.TimeStepCalculated += async (sender, args) =>
            {
                var surf = new CartesianCoordinate[args.Parameters.Indices.ILength*args.Parameters.Indices.JLength];
				var iterator = new SequentialIterator();
                var counter = 0;
				iterator.ForExceptK(
					args.Parameters.Indices,
					(i, j) =>
					{
						surf[counter++] = new CartesianCoordinate(i, j, args.Fields.E[i, j, args.Parameters.Indices.GetCenter().KLength].Z);
					});
				await responseStream.WriteAsync(new FDTDReply
				{
					Done = false,
					Time = args.Time,
					Result = JsonConvert.SerializeObject(surf)

				});
            };

            var result = ext.Calculate(parameters);

            await responseStream.WriteAsync(new FDTDReply()
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

        private static IndexStore getIndexStore(int pmlLength, Voxel resolution)
        {
            var res = resolution ?? new Voxel { I = 50, J = 50, K = 50 };
            return new IndexStore(res.I + 2 * pmlLength, res.J + 2 * pmlLength, res.K + 2 * pmlLength);
        }

        private static void setMedium(SimulationParameters parameters, MeshInfo meshInfo)
        {
            var factory = new MediumSolverFactory(parameters.TimeStep);
            //parameters.Medium = setSphere(parameters, silver, drudeLorentzParam, vacuum);
            parameters.Medium = setObject(parameters, meshInfo.Voxels, factory);
        }


        private static IMediumSolver[,,] setObject(
            SimulationParameters parameters,
            List<Voxel> voxels,
            MediumSolverFactory fact)
        {
            var medium = parameters.Indices.CreateArray(
                (i, j, k) => fact.GetMediumSolver("vacuum"));

            var offset = parameters.PmlLength;
            foreach (var voxel in voxels)
            {
                medium[voxel.I + offset, voxel.J + offset, voxel.K + offset] = fact.GetMediumSolver(voxel.Material, true);
            }
            return medium;
        }
    }
}
