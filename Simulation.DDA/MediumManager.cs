using System;
using System.Numerics;

using Simulation.Models;

namespace Simulation.DDA
{
    public class MediumManager
    {
        private readonly bool solid;

        private readonly OpticalConstants opticalConstants;

        public MediumManager(OpticalConstants opticalConstants, bool solid)
        {
            this.solid = solid;
            this.opticalConstants = opticalConstants;
        }

        public DispersionParameter GetDispersionParameters(double waveLength, SphericalCoordinate angle)
        {
            var parameters = new DispersionParameter
            {
                WaveLength = waveLength,
                MediumRefractiveIndex = this.GetMediumCoeficient(waveLength),
                Permitivity = this.opticalConstants.SelectOpticalConst(waveLength)
            };

            var waveVector = getWaveVector(
                waveLength,
                parameters.MediumRefractiveIndex,
                angle);

            parameters.WaveVector = waveVector;
            return parameters;
        }

        public double GetMediumCoeficient(double waveLength)
        {
            double refractiveIndex;

            //// n_med = sqrt( 1.0 + 2.121*WaveLength*WaveLength / (WaveLength*WaveLength-263.3*263.3) ); // CdBr2 (300K)
            //// n_med = 0.7746 + 0.14147*exp(-(WaveLength-59.0737)/122.56035)
            //// + 0.99808*exp(-(WaveLength-59.0737)/42.78953) + 0.56471*exp(-(WaveLength-59.0737)/36297.71849); // Waher
            refractiveIndex = 1; ////air

            //// n_med = 1.39; //water

            //// n_med = 1.51; // Glass 1.51

            //// n_med = 1.65; // Casein has a refractive index of 1.51-1.65

            refractiveIndex = this.solid ? 1.0 : refractiveIndex;

            return refractiveIndex;
        }
        
        public Complex GetEpsilon(
            DispersionParameter param,
            double radius)
        {
            double resize = 197.35;
            double w = resize * param.WaveVector.Norm; // [eV] kmod
            double wp = 9.2; // [eV] Ag.
            double gamma0 = 0.02; ////[eV]
            double vf = 0.915; ////2.6;//0.915; // [eV*nm] Зміна оптичних констант зі зміною радіуса частинки
            double Aparam = 0.75;

            if (solid)
            {
                return param.Permitivity;
            }

            double gamma = gamma0 + Aparam * vf / radius;
            var wp2 = wp * wp;

            var wgamma0 = 1 / (w * w + gamma0 * gamma0);
            var wgamma = 1 / (w * w + gamma * gamma);

            var eps_re_mod = param.Permitivity.Real + wp2 * (wgamma0 - wgamma);

            var eps_im_mod = param.Permitivity.Imaginary - (wp2 / w) * (gamma0 * wgamma0 - gamma * wgamma);

            return new Complex(eps_re_mod, eps_im_mod);
        }

        private CartesianCoordinate getWaveVector(
            double waveLength,
            double refractiveIndex,
            SphericalCoordinate angle)
        {
            var radianAngles = angle.ConvertToRadians();

            radianAngles.Radius = 2.0 * Math.PI * refractiveIndex / waveLength; // k_mod

            return radianAngles.ConvertToCartesian();
        }
    }
}
