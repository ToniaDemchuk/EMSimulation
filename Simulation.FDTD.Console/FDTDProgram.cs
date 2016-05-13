using System;
using System.Collections.Generic;
using System.Linq;

using Simulation.FDTD.Models;
using Simulation.Infrastructure;
using Simulation.Medium.Factors;
using Simulation.Medium.MediumSolver;
using Simulation.Medium.Models;
using Simulation.Models.Common;
using Simulation.Models.Coordinates;
using Simulation.Models.Enums;
using Simulation.Models.Extensions;
using Simulation.Models.Spectrum;
using Simulation.Infrastructure.Iterators;
using Simulation.Infrastructure.Models;
using Simulation.Infrastructure.Readers;
using Simulation.Medium.Factories;

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
                    x => new List<double>() { x.Value.CrossSectionAbsorption, x.Value.EffectiveCrossSectionAbsorption }));
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
            var fieldPlotter = new FieldPlotter();
            var pulsePlotter = new PulsePlotter();
            var spectrumPlotter = new SpectrumPlotter();
            var mediumPlotter = new MediumPlotter();
            var mesh = getMeshInfo();

            mediumPlotter.Plot(mesh);

            var pmlLength = 10;

            SimulationParameters parameters = new SimulationParameters
            {
                Indices = getIndexStore(pmlLength, mesh.Resolution),
                CellSize = 1e-9,
                Spectrum =
                    new OpticalSpectrum(new LinearDiscreteCollection(200e-9, 500e-9, 100), SpectrumUnitType.WaveLength),
                CourantNumber = 0.5,
                PmlLength = pmlLength,
                NumSteps = 150,
                WaveFunc = time => Math.Exp(-0.5 * Math.Pow((30 - time) / 5.0, 2.0)),
                IsSpectrumCalculated = true
            };

            setMedium(parameters, mesh);

            ext.TimeStepCalculated += fieldPlotter.Plot;
            ext.TimeStepCalculated += pulsePlotter.Plot;

            var res = ext.Calculate(parameters);

            spectrumPlotter.Plot(res);
            return res;
        }

        private static IndexStore getIndexStore(int pmlLength, Voxel resolution)
        {
            var res = resolution ?? new Voxel() { I = 50, J = 50, K = 50 };
            return new IndexStore(res.I + 2 * pmlLength, res.J + 2 * pmlLength, res.K + 2 * pmlLength);
        }

        #region Set Medium

        private static void setMedium(SimulationParameters parameters, MeshInfo meshInfo)
        {
            var factory = new MediumSolverFactory(parameters.TimeStep);
            //parameters.Medium = setSphere(parameters, silver, drudeLorentzParam, vacuum); 
            parameters.Medium = setObject(parameters, meshInfo.Voxels, factory);
        }

        private static IMediumSolver[,,] setSphere(
            SimulationParameters parameters,
            DrudeLorentzFactor drudeLorentzParam,
            VacuumSolver vacuum)
        {
            const double Radius = 10;

            var centerIndices = parameters.Indices.GetCenter();
            var center = new CartesianCoordinate(centerIndices.ILength, centerIndices.JLength, centerIndices.KLength);

            return parameters.Indices.CreateArray<IMediumSolver>(
                (i, j, k) =>
                {
                    var point = new CartesianCoordinate(i, j, k) - center;
                    if (point.Norm <= Radius)
                    {
                        return getMaterial(drudeLorentzParam);
                    }
                    return vacuum;
                });
        }

        private static IMediumSolver[,,] setObject(
            SimulationParameters parameters,
            List<Voxel> voxels,
            MediumSolverFactory fact)
        {
            var medium = parameters.Indices.CreateArray<IMediumSolver>(
                (i, j, k) => fact.GetMediumSolver(string.Empty));

            var offset = parameters.PmlLength;
            foreach (var voxel in voxels)
            {
                medium[voxel.I + offset, voxel.J + offset, voxel.K + offset] = fact.GetMediumSolver(voxel.Material, true);
            }
            return medium;
        }

        private static DrudeLorentzSolver getMaterial(DrudeLorentzFactor drudeLorentzParam)
        {
            return new DrudeLorentzSolver(drudeLorentzParam) { IsBody = true };
        }

        private static MeshInfo getMeshInfo()
        {
            //var mesh = new VoxelReader().ReadInfo(@"D:\study\vozelizer\conf1.obj.v80.voxels");
            //var mesh = new ObjToVoxelReader().ReadInfo(@"C:\Users\akolkev\Documents\sphere444.obj");
            //var mesh = new MagicaVoxelReader().ReadInfo(@"C:\Users\akolkev\Documents\spherevox.vox");
            var mesh = new FDSToVoxelReader().ReadInfo(@"D:\sphere.fds");
            return mesh;
        }

        #endregion
    }
}