using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using Simulation.Models;

namespace Simulation.DDA
{
    public class ExtinctionManager
    {
        private readonly MediumManager mediumManager;
        public ExtinctionManager(MediumManager mediumManager)
        {
            this.mediumManager = mediumManager;
        }

        [DllImport("ModernKDDA.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int OpenMPCGMethod(int syst, double[] A, double[] x, double[] b);

        public Dictionary<double, double> CalculateCrossExtinction(SimulationParameters parameters)
        {
            int size_Adbl = parameters.SystemConfig.Size * CoordinateExtensions.ComplexMultiplier;  // розмірність матриці А, Р, Е представлена в дійсних числах

            double[] P = new double[size_Adbl];

            return parameters.WaveConfig
                .ToDictionary(
                    x => x,
                    x => this.CalculateSingleDDA(P, x, parameters));

        }

        public double CalculateSingleDDA(
            double[] P,
            double waveLength,
            SimulationParameters parameters)
        {
            DispersionParameter dispersion =
                this.mediumManager.GetDispersionParameters(
                    waveLength,
                    parameters.WavePropagation);

            ComplexCoordinate[] E = this.GetIncidentField(
                parameters.SystemConfig,
                parameters.IncidentMagnitude,
                dispersion);

            DyadCoordinate<Complex>[,] A = this.BuildMatrixA(
                parameters.SystemConfig,
                dispersion);

            OpenMPCGMethod(
                P.Length,
                CoordinateExtensions.ConvertToPlainArray(A),
                P,
                CoordinateExtensions.ConvertToPlainArray(E));

            return this.CalculateCrossSectionExtinction(
                CoordinateExtensions.ConvertFromPlainArray(P),
                E, parameters, dispersion);
        }


        DyadCoordinate<Complex>[,] BuildMatrixA(SystemConfig system, DispersionParameter dispersion)
        {
            DyadCoordinate<Complex>[,] A = new DyadCoordinate<Complex>[system.Size, system.Size];

            for (int j = 0; j < system.Size; j++)
            {
                for (int i = 0; i < system.Size; i++)
                {
                    A[i, j] = i == j
                        ? setDiagonalElements(system, i, dispersion)
                        : setNonDiagonalElements(system, i, j, dispersion);
                }
            }

            return A;
        }

        private static DyadCoordinate<Complex> setNonDiagonalElements(SystemConfig system, int i, int j, DispersionParameter dispersion)
        {
            var r = system.Points[j] - system.Points[i];
            double rmod = r.Norm;
            double rmod_2 = rmod * rmod;
            double rmod_3 = rmod_2 * rmod;

            var kmod = dispersion.WaveVector.Norm;
            double kr = kmod * rmod;

            DyadCoordinate<Complex> dyadProduct = DyadCoordinateEntensions.DyadProduct(r, r);

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
            Complex eps = mediumManager.GetEpsilon(dispersion, radius);

            // Complex value inverted to Clausius-Mossotti polarization.
            Complex multiplier = (eps + 2.0 * eps_m) / (eps - 1.0 * eps_m);

            double rad_inv = 1 / (radius * radius * radius);

            DyadCoordinate<Complex> complex = new DyadCoordinate<Complex>(multiplier * rad_inv);

            var kmod = dispersion.WaveVector.Norm;
            double radiation = 2.0 / 3.0 * kmod * kmod * kmod; // доданок, що відповідає за релаксаційне випромінювання.
            var radiativeReaction = new DyadCoordinate<Complex>(Complex.ImaginaryOne * radiation);

            return complex - radiativeReaction;
        }


        public ComplexCoordinate[] GetIncidentField(SystemConfig system, CartesianCoordinate Exyz, DispersionParameter dispersion)
        {
            double eps_m = dispersion.MediumRefractiveIndex * dispersion.MediumRefractiveIndex;
            ComplexCoordinate[] E = new ComplexCoordinate[system.Size];

            for (int j = 0; j < system.Size; j++)
            {
                CartesianCoordinate point = system.Points[j];
                double kr = dispersion.WaveVector * point;
                E[j] = new ComplexCoordinate(Exyz * eps_m, kr);
            }

            return E;
        }

        private double CalculateCrossSectionExtinction(ComplexCoordinate[] P, ComplexCoordinate[] Einc, SimulationParameters parameters, DispersionParameter dispersion)
        {
            double eps_m = dispersion.MediumRefractiveIndex * dispersion.MediumRefractiveIndex;
            var Exyz_mod = parameters.IncidentMagnitude.Norm;
            double constCext = 4.0 * Math.PI * dispersion.WaveVector.Norm / (Exyz_mod * Exyz_mod * eps_m);

            double crossExt = Einc.Select((eInc, j) => (eInc * P[j]).Imaginary).Sum();

            return crossExt * constCext;
        }
    }
}
