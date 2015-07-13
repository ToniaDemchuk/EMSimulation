using System;
using System.Linq;
using System.Numerics;

using Simulation.DDA.Models;
using Simulation.Medium.Models;
using Simulation.Models.Coordinates;
using Simulation.Models.Extensions;
using Simulation.Models.Spectrum;

namespace Simulation.DDA
{
    /// <summary>
    ///     The ExtinctionManager class.
    /// </summary>
    public class ExtinctionManager
    {
        private readonly MediumManager mediumManager;

        private double[] polarization;

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
            this.initPolarization(parameters);
            return parameters.Spectrum
                             .ToSimulationResult(
                                 x => this.CalculateSingleDDA(x, parameters));
        }

        /// <summary>
        /// Calculates the single DDA.
        /// </summary>
        /// <param name="waveLength">Length of the wave.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The single simulation result.</returns>
        public SimulationResult CalculateSingleDDA(
            SpectrumUnit waveLength,
            SimulationParameters parameters)
        {
            DispersionParameter dispersion =
                this.mediumManager.GetDispersionParameters(
                    waveLength,
                    parameters.WavePropagation);

            ComplexCoordinate[] e = this.getIncidentField(
                parameters.SystemConfig,
                parameters.IncidentMagnitude,
                dispersion);

            DyadCoordinate<Complex>[,] a = this.buildMatrixA(
                parameters.SystemConfig,
                dispersion);

            ModernKDDAEntryPoint.OpenMPCGMethod(
                this.polarization.Length,
                CoordinateHelper.ConvertToPlainArray(a),
                this.polarization,
                CoordinateHelper.ConvertToPlainArray(e));

            var result = new SimulationResult
            {
                Polarization = CoordinateHelper.ConvertFromPlainArray(this.polarization),
                ElectricField = e
            };

            this.calculateCrossSectionExtinction(
                result,
                parameters,
                dispersion);

            return result;
        }

        private DyadCoordinate<Complex>[,] buildMatrixA(SystemConfig system, DispersionParameter dispersion)
        {
            var a = new DyadCoordinate<Complex>[system.Size, system.Size];

            for (int i = 0; i < system.Size; i++)
            {
                for (int j = 0; j < system.Size; j++)
                {
                    a[i, j] = i == j
                                  ? this.setDiagonalElements(system, i, dispersion)
                                  : this.setNonDiagonalElements(system, i, j, dispersion);
                }
            }

            return a;
        }

        private ComplexCoordinate[] getIncidentField(
            SystemConfig system,
            CartesianCoordinate exyz,
            DispersionParameter dispersion)
        {
            double medRef = dispersion.MediumRefractiveIndex * dispersion.MediumRefractiveIndex;
            var e = new ComplexCoordinate[system.Size];

            for (int j = 0; j < system.Size; j++)
            {
                CartesianCoordinate point = system.Points[j];
                double kr = dispersion.WaveVector * point;
                e[j] = ComplexCoordinate.FromPolarCoordinates(exyz * medRef, kr);
            }

            return e;
        }

        private void initPolarization(SimulationParameters parameters)
        {
            int sizeAdbl = parameters.SystemConfig.Size * CoordinateHelper.ComplexMultiplier;
            // розмірність матриці А, Р, Е представлена в дійсних числах
            this.polarization = new double[sizeAdbl];
        }

        private DyadCoordinate<Complex> setNonDiagonalElements(
            SystemConfig system,
            int i,
            int j,
            DispersionParameter dispersion)
        {
            CartesianCoordinate r = system.Points[j] - system.Points[i];
            double rmod = r.Norm;
            double rmod2 = rmod * rmod;
            double rmod3 = rmod2 * rmod;

            double kmod = dispersion.WaveVector.Norm;
            double kr = kmod * rmod;

            DyadCoordinate<Complex> dyadProduct = r.DyadProduct(r);

            var initDyad = new DyadCoordinate<Complex>(rmod2);

            DyadCoordinate<Complex> firstMember = (kmod * kmod) * (dyadProduct - initDyad);

            DyadCoordinate<Complex> secondMember = (1 / rmod2) * (Complex.ImaginaryOne * kr - 1) *
                                                   (3 * dyadProduct - initDyad);

            Complex multiplier = Complex.FromPolarCoordinates(1 / rmod3, kr);

            return multiplier * (firstMember + secondMember);
        }

        private DyadCoordinate<Complex> setDiagonalElements(
            SystemConfig system,
            int index,
            DispersionParameter dispersion)
        {
            double medRef = dispersion.MediumRefractiveIndex * dispersion.MediumRefractiveIndex;

            double radius = system.Radius[index];
            Complex eps = this.mediumManager.GetEpsilon(dispersion, radius);

            // Complex value inverted to Clausius-Mossotti polarization.
            Complex clausiusMosottiPolar = (eps - 1.0 * medRef) / (eps + 2.0 * medRef);
            Complex multiplier = Complex.Reciprocal(clausiusMosottiPolar);

            double volumeFactorInverted = 1 / (radius * radius * radius);

            var complex = new DyadCoordinate<Complex>(multiplier * volumeFactorInverted);

            double kmod = dispersion.WaveVector.Norm;
            double radiation = 2.0 / 3.0 * kmod * kmod * kmod; // доданок, що відповідає за релаксаційне випромінювання.
            var radiativeReaction = new DyadCoordinate<Complex>(Complex.ImaginaryOne * radiation);

            return complex - radiativeReaction;
        }

        private void calculateCrossSectionExtinction(
            SimulationResult result,
            SimulationParameters parameters,
            DispersionParameter dispersion)
        {
            double epsM = dispersion.MediumRefractiveIndex * dispersion.MediumRefractiveIndex;
            double exyzMod = parameters.IncidentMagnitude.Norm;
            double factorCext = 4.0 * Math.PI * dispersion.WaveVector.Norm / (exyzMod * exyzMod * epsM);

            double crossExt = result.ElectricField.Select((eInc, j) => (eInc * result.Polarization[j]).Imaginary).Sum();

            double crossSectionExt = crossExt * factorCext;

            result.CrossSectionExtinction = crossSectionExt;
            result.EffectiveCrossSectionExtinction =
                crossSectionExt /
                parameters.SystemConfig.Radius.Sum(radius => Math.PI * radius * radius); // todo
        }
    }
}