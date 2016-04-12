using System;
using System.Linq;

using Simulation.FDTD.Models;
using Simulation.Infrastructure;
using Simulation.Medium.Factors;
using Simulation.Medium.Medium;
using Simulation.Medium.MediumSolver;
using Simulation.Medium.Models;
using Simulation.Models.Common;
using Simulation.Models.Constants;
using Simulation.Models.Coordinates;
using Simulation.Models.Enums;
using Simulation.Models.Extensions;
using Simulation.Models.Spectrum;
using Simulation.Infrastructure.Iterators;

namespace Simulation.FDTD.Console
{
    public class FDTDProgram
    {
        private static void Main(string[] args)
        {
            var result = Calculate();
            SimpleFormatter.Write(
                "rezult_ext.txt",
                result.ToDictionary(
                    x => x.Key.ToType(SpectrumUnitType.WaveLength),
                    x => x.Value.EffectiveCrossSectionAbsorption));
        }

        public static SimulationResultDictionary Calculate()
        {
            var iterator = new ParallelIterator();
            var ext = new FDTDSimulation(iterator);

            SimulationParameters parameters = new SimulationParameters
            {
                Indices = new IndexStore(60, 60, 60),
                CellSize = 1e-9,
                Spectrum =
                    new OpticalSpectrum(new LinearDiscreteCollection(200e-9, 500e-9, 100), SpectrumUnitType.WaveLength),
                CourantNumber = 0.5,
                PmlLength = 10,
                NumSteps = 150,
                WaveFunc = time => Math.Exp(-0.5 * Math.Pow((30 - time) / 5.0, 2.0)),
                IsSpectrumCalculated = true
            };

            setMedium(parameters);

            return ext.Calculate(parameters);
        }

        private static void setMedium(SimulationParameters parameters)
        {
            double timeStep = parameters.CellSize * parameters.CourantNumber / (Fundamentals.SpeedOfLight);
            var vacuum = new VacuumSolver();
            var silver = new DrudeLorentz();
            var drudeLorentzParam = new DrudeLorentzFactor(silver, timeStep);
            parameters.Medium = setSphere(parameters, silver, drudeLorentzParam, vacuum); 
            parameters.Medium = setObject(parameters, silver, drudeLorentzParam, vacuum); 
        }

        private static IMediumSolver[,,] setSphere(
            SimulationParameters parameters,
            DrudeLorentz silver,
            DrudeLorentzFactor drudeLorentzParam,
            VacuumSolver vacuum)
        {
            double radius = 10;

            var centerIndices = parameters.Indices.GetCenter();
            var center = new CartesianCoordinate(centerIndices.ILength, centerIndices.JLength, centerIndices.KLength);

            return parameters.Indices.CreateArray<IMediumSolver>(
                (i, j, k) =>
                {
                    var point = new CartesianCoordinate(i, j, k) - center;
                    if (point.Norm <= radius)
                    {
                        return new DrudeLorentzSolver(silver, drudeLorentzParam) { IsBody = true };
                    }
                    return vacuum;
                });
        }

        private static IMediumSolver[,,] setObject(
            SimulationParameters parameters,
            DrudeLorentz silver,
            DrudeLorentzFactor drudeLorentzParam,
            VacuumSolver vacuum)
        {

            var mesh = VoxelReader.ReadInfo(@"D:\study\vozelizer\conf1.obj.v80.voxels");

            var medium = parameters.Indices.CreateArray<IMediumSolver>(
                (i, j, k) => vacuum);

            var offset = parameters.PmlLength + 3;
            foreach (var voxel in mesh.Voxels)
            {
                medium[voxel.I + offset, voxel.J + offset, voxel.K + offset] = new DrudeLorentzSolver(silver, drudeLorentzParam) { IsBody = true };
            }
            return medium;
        }

    }
}