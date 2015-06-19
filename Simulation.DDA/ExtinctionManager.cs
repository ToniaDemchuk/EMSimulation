using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;

using Simulation.Infrastructure;
using Simulation.Models;
using Simulation.Models.Extensions;

namespace Simulation.DDA
{
    public class ExtinctionManager
    {
        private readonly MediumManager mediumManager;
        public ExtinctionManager(MediumManager mediumManager)
        {
            this.mediumManager = mediumManager;
        }

        private double[] polarization = null;

        [DllImport("ModernKDDA.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int OpenMPCGMethod(int syst, double[] a, double[] x, double[] b);

        public SimulationResultDictionary CalculateCrossExtinction(SimulationParameters parameters)
        {
            this.initPolarization(parameters);
            return parameters.Spectrum
                .ToSimulationResult(
                    x => this.CalculateSingleDDA(x, parameters));
        }

        public SimulationResult CalculateSingleDDA(
            SpectrumParameter waveLength,
            SimulationParameters parameters)
        {
            DispersionParameter dispersion =
                this.mediumManager.GetDispersionParameters(
                    waveLength,
                    parameters.WavePropagation);

            ComplexCoordinate[] e = this.GetIncidentField(
                parameters.SystemConfig,
                parameters.IncidentMagnitude,
                dispersion);

            DyadCoordinate<Complex>[,] a = this.BuildMatrixA(
                parameters.SystemConfig,
                dispersion);

            OpenMPCGMethod(
                this.polarization.Length,
                CoordinateHelper.ConvertToPlainArray(a),
                this.polarization,
                CoordinateHelper.ConvertToPlainArray(e));

            var result = new SimulationResult
            {
                Polarization = CoordinateHelper.ConvertFromPlainArray(this.polarization),
                ElectricField = e
            };

            this.CalculateCrossSectionExtinction(
                result,
                parameters,
                dispersion);

            return result;
        }

        private void initPolarization(SimulationParameters parameters)
        {
            int sizeAdbl = parameters.SystemConfig.Size * CoordinateHelper.ComplexMultiplier;
            // розмірність матриці А, Р, Е представлена в дійсних числах

            this.polarization = new double[sizeAdbl];
        }

        public DyadCoordinate<Complex>[,] BuildMatrixA(SystemConfig system, DispersionParameter dispersion)
        {
            DyadCoordinate<Complex>[,] a = new DyadCoordinate<Complex>[system.Size, system.Size];

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

        private DyadCoordinate<Complex> setNonDiagonalElements(SystemConfig system, int i, int j, DispersionParameter dispersion)
        {
            var r = system.Points[j] - system.Points[i];
            double rmod = r.Norm;
            double rmod_2 = rmod * rmod;
            double rmod_3 = rmod_2 * rmod;

            var kmod = dispersion.WaveVector.Norm;
            double kr = kmod * rmod;

            DyadCoordinate<Complex> dyadProduct = CoordinateEntensions.DyadProduct(r, r);

            var initDyad = new DyadCoordinate<Complex>(rmod_2);

            var firstMember = (kmod * kmod) * (dyadProduct - initDyad);

            var secondMember = (1 / rmod_2) * (new Complex(-1, kr)) * (3 * dyadProduct - initDyad);

            var multiplier = Complex.FromPolarCoordinates(1 / rmod_3, kr);

            return multiplier * (firstMember + secondMember);
        }

        private DyadCoordinate<Complex> setDiagonalElements(SystemConfig system, int index, DispersionParameter dispersion)
        {
            double eps_m = dispersion.MediumRefractiveIndex * dispersion.MediumRefractiveIndex;

            double radius = system.Radius[index];
            Complex eps = this.mediumManager.GetEpsilon(dispersion, radius);

            // Complex value inverted to Clausius-Mossotti polarization.
            Complex multiplier = (eps + 2.0 * eps_m) / (eps - 1.0 * eps_m);

            double volumeFactorInverted = 1 / (radius * radius * radius);

            DyadCoordinate<Complex> complex = new DyadCoordinate<Complex>(multiplier * volumeFactorInverted);

            var kmod = dispersion.WaveVector.Norm;
            double radiation = 2.0 / 3.0 * kmod * kmod * kmod; // доданок, що відповідає за релаксаційне випромінювання.
            var radiativeReaction = new DyadCoordinate<Complex>(Complex.ImaginaryOne * radiation);

            return complex - radiativeReaction;
        }

        public ComplexCoordinate[] GetIncidentField(SystemConfig system, CartesianCoordinate exyz, DispersionParameter dispersion)
        {
            double eps_m = dispersion.MediumRefractiveIndex * dispersion.MediumRefractiveIndex;
            ComplexCoordinate[] e = new ComplexCoordinate[system.Size];

            for (int j = 0; j < system.Size; j++)
            {
                CartesianCoordinate point = system.Points[j];
                double kr = dispersion.WaveVector * point;
                e[j] = new ComplexCoordinate(exyz * eps_m, kr);
            }

            return e;
        }

        private void CalculateCrossSectionExtinction(SimulationResult result, SimulationParameters parameters, DispersionParameter dispersion)
        {
            double epsM = dispersion.MediumRefractiveIndex * dispersion.MediumRefractiveIndex;
            var exyzMod = parameters.IncidentMagnitude.Norm;
            double factorCext = 4.0 * Math.PI * dispersion.WaveVector.Norm / (exyzMod * exyzMod * epsM);

            double crossExt = result.ElectricField.Select((eInc, j) => (eInc * result.Polarization[j]).Imaginary).Sum();

            var crossSectionExt = crossExt * factorCext;

            result.CrossSectionExtinction = crossSectionExt;
            result.EffectiveCrossSectionExtinction =
                crossSectionExt /
                parameters.SystemConfig.Radius.Sum(radius => MathHelper.Area(radius)); //todo
        }

    }
}
