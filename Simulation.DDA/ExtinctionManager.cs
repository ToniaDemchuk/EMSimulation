using System;
using System.Linq;
using System.Numerics;

using Simulation.DDA.Models;
using Simulation.Medium.Models;
using Simulation.Models.Calculators;
using Simulation.Models.Comparers;
using Simulation.Models.Coordinates;
using Simulation.Models.Extensions;
using Simulation.Models.Matrices;
using Simulation.Models.Spectrum;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Simulation.Models.Constants;

namespace Simulation.DDA
{
    /// <summary>
    ///     The ExtinctionManager class.
    /// </summary>
    public class ExtinctionManager
    {
        private readonly MediumManager mediumManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtinctionManager"/> class.
        /// </summary>
        /// <param name="mediumManager">The medium manager.</param>
        public ExtinctionManager(MediumManager mediumManager)
        {
            this.mediumManager = mediumManager;
        }

        /// <summary>
        /// Calculates the cross extinction.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The simulation results.</returns>
        public SimulationResultDictionary CalculateCrossExtinction(SimulationParameters parameters)
        {
            var result = new SimulationResultDictionary();

            var partitions = Partitioner.Create(parameters.Spectrum).GetPartitions(Environment.ProcessorCount);

            var concurent = new ConcurrentDictionary<SpectrumUnit, SimulationResult>();

            var tasks = partitions
                .Select(partition => Task.Run(() =>
                {
                    using (partition)
                    {
                        var polarization = this.initPolarization(parameters);

                        while (partition.MoveNext())
                        {
                            concurent.TryAdd(partition.Current, this.CalculateSingleDDA(partition.Current, parameters, polarization));
                        }
                    }
                }))
                .ToArray();

            Task.WaitAll(tasks);

            foreach (var keyvalue in concurent.OrderBy(pair => pair.Key))
            {
                result.Add(keyvalue.Key, keyvalue.Value);
            }

            return result;
        }

        /// <summary>
        /// Calculates the single DDA.
        /// </summary>
        /// <param name="waveLength">Length of the wave.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The single simulation result.</returns>
        public SimulationResult CalculateSingleDDA(
            SpectrumUnit waveLength,
            SimulationParameters parameters,
            double[] polarization)
        {
            Console.WriteLine("start wave" + waveLength.ToType(Simulation.Models.Enums.SpectrumUnitType.WaveLength));

            DispersionParameter dispersion =
                this.mediumManager.GetDispersionParameters(
                    waveLength,
                    parameters.WavePropagation);

            ComplexCoordinate[] e = this.getIncidentField(
                parameters.SystemConfig,
                parameters.IncidentMagnitude,
                dispersion);

            IMatrix<IMatrix<Complex>> a = this.buildMatrixA(
                parameters.SystemConfig,
                dispersion);

            ModernKDDAEntryPoint.OpenMPCGMethod(
                polarization.Length,
                CoordinateHelper.ConvertToPlainArrayMatrix(a),
                polarization,
                CoordinateHelper.ConvertToPlainArray(e));

            var result = new SimulationResult
            {
                Polarization = CoordinateHelper.ConvertFromPlainArray(polarization),
                ElectricField = e
            };
            this.calculateCrossSectionExtinction(
                result,
                parameters,
                dispersion);
            Console.WriteLine("finish wave"+ waveLength.ToType(Simulation.Models.Enums.SpectrumUnitType.WaveLength));
            return result;
        }

        private IMatrix<IMatrix<Complex>> buildMatrixA(SystemConfig system, DispersionParameter dispersion)
        {
            return new LazyDiagonalMatrix<CartesianCoordinate, IMatrix<Complex>>(
                system.Size,
                system.GetDistanceUniform,
                (i, j) => this.setNonDiagonalElements(dispersion, system.GetDistance(i, j)),
                i => this.setDiagonalElements(dispersion, system.Radius[i], system.GetPoint(i)),
                new CoordinateEqualityComparer());
        }

        private ComplexCoordinate[] getIncidentField(
            SystemConfig system,
            CartesianCoordinate exyz,
            DispersionParameter dispersion)
        {
            double medRef = dispersion.MediumRefractiveIndex * dispersion.MediumRefractiveIndex;//this is correct
            var e = new ComplexCoordinate[system.Size];

            for (int j = 0; j < system.Size; j++)
            {
                CartesianCoordinate point = system.GetPoint(j);
                double kr = dispersion.WaveVector.ScalarProduct(point);

                e[j] = ComplexCoordinate.FromPolarCoordinates(exyz * medRef, kr);
            }

            return e;
        }

        private double[] initPolarization(SimulationParameters parameters)
        {
            int sizeAdbl = parameters.SystemConfig.Size * CoordinateHelper.ComplexMultiplier;
            // розмірність матриці А, Р, Е представлена в дійсних числах
            return Enumerable.Repeat<double>(0, sizeAdbl).ToArray();
        }

        private BaseDyadCoordinate<Complex, ComplexCalculator> setNonDiagonalElements(
            DispersionParameter dispersion,
            CartesianCoordinate displacement)
        {
            double rmod = displacement.Norm;
            double rmod2 = rmod * rmod;
            double rmod3 = rmod2 * rmod;

            double kmod = dispersion.WaveVector.Norm;
            double kr = kmod * rmod;

            BaseDyadCoordinate<Complex, ComplexCalculator> dyadProduct =
                CoordinateExtensions.DyadProduct(ref displacement, ref displacement);

            var initDyad = 
                new DiagonalDyadCoordinate<Complex, ComplexCalculator>(rmod2);

            BaseDyadCoordinate<Complex, ComplexCalculator> firstMember = (kmod * kmod) * (dyadProduct - initDyad);


            BaseDyadCoordinate<Complex, ComplexCalculator> secondMember = (1 / rmod2) * (Complex.ImaginaryOne * kr - 1) *
                                                   (3 * dyadProduct - initDyad);

            Complex multiplier = Complex.FromPolarCoordinates(1 / rmod3, kr);

            return multiplier * (firstMember + secondMember);
        }

        private BaseDyadCoordinate<Complex, ComplexCalculator> setDiagonalElements(
            DispersionParameter dispersion,
            double radius,
            CartesianCoordinate exyz)
        {
            double medRef = dispersion.MediumRefractiveIndex * dispersion.MediumRefractiveIndex;//this is correct

            Complex eps = this.mediumManager.GetEpsilon(dispersion, radius);

            // Complex value inverted to Clausius-Mossotti polarization.
            Complex clausiusMosottiPolar = (eps - 1.0 * medRef) / (eps + 2.0 * medRef);
            Complex multiplier = Complex.Reciprocal(clausiusMosottiPolar);

            double volumeFactorInverted = 1 / (radius * radius * radius);
            var complex =
                new DyadCoordinate<Complex, ComplexCalculator>(multiplier * volumeFactorInverted);

            double kmod = dispersion.WaveVector.Norm;
            double radiation = 2.0 / 3.0 * kmod * kmod * kmod; // доданок, що відповідає за релаксаційне випромінювання.

            var radiativeReaction =
                new DyadCoordinate<Complex, ComplexCalculator>(Complex.ImaginaryOne * radiation);

            BaseDyadCoordinate<Complex, ComplexCalculator> surfaceInteraction = SurfaceInteractionCoeff(dispersion, exyz);

            return complex - radiativeReaction + surfaceInteraction;
        }

        private BaseDyadCoordinate<Complex, ComplexCalculator> SurfaceInteractionCoeff(DispersionParameter dispersion, CartesianCoordinate position)
        {
            if (dispersion.SubstrateRefractiveIndex == 0)
            {
                return new DiagonalDyadCoordinate<Complex, ComplexCalculator>(0);
            }
            var medRef = Math.Pow(dispersion.MediumRefractiveIndex, 2);
            var subsRef = Math.Pow(dispersion.SubstrateRefractiveIndex, 2);
            var reflMult = ((subsRef - medRef) / (subsRef + medRef));
            return (reflMult) * setNonDiagonalElements(dispersion, new CartesianCoordinate(0, 0, 2 * position.Z));
        }

        private void calculateCrossSectionExtinction(
            SimulationResult result,
            SimulationParameters parameters,
            DispersionParameter dispersion)
        {
            double epsM = dispersion.MediumRefractiveIndex * dispersion.MediumRefractiveIndex;//this is correct
            double exyzMod = parameters.IncidentMagnitude.Norm;
            double factorCext = 4.0 * Math.PI * dispersion.WaveVector.Norm / (exyzMod * exyzMod * epsM);

            double crossExt = result.ElectricField.Select((eInc, j) => (eInc.ScalarProduct(result.Polarization[j])).Imaginary).Sum();

            double crossSectionExt = crossExt * factorCext;

            result.CrossSectionExtinction = crossSectionExt;
            result.EffectiveCrossSectionExtinction =
                crossSectionExt/
                parameters.SystemConfig.CrossSectionArea;
        }
    }
}