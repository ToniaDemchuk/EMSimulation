using System;
using System.Numerics;

using Simulation.Medium.Models;
using Simulation.Models.Coordinates;
using Simulation.Models.Enums;
using Simulation.Models.Extensions;
using Simulation.Models.Spectrum;

namespace Simulation.DDA
{
    /// <summary>
    /// The MediumManager class.
    /// </summary>
    public class MediumManager
    {
        private readonly BaseMedium medium;

        private readonly bool solid;

        /// <summary>
        /// Initializes a new instance of the <see cref="MediumManager"/> class.
        /// </summary>
        /// <param name="medium">The medium.</param>
        /// <param name="solid">if set to <c>true</c> [solid].</param>
        public MediumManager(BaseMedium medium, bool solid)
        {
            this.solid = solid;
            this.medium = medium;
        }

        /// <summary>
        /// Gets the dispersion parameters.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <param name="angle">The angle.</param>
        /// <returns>The dispersion parameters.</returns>
        public DispersionParameter GetDispersionParameters(SpectrumUnit parameter, SphericalCoordinate angle)
        {
            var parameters = new DispersionParameter
            {
                SpectrumParameter = parameter,
                MediumRefractiveIndex = this.getMediumCoeficient(parameter),
                Permittivity = this.medium.GetPermittivity(parameter),
                SubstrateRefractiveIndex = this.getSubstrateCoefficient(parameter)
            };

            CartesianCoordinate waveVector = this.getWaveVector(
                parameter,
                parameters.MediumRefractiveIndex,
                angle);

            parameters.WaveVector = waveVector;
            return parameters;
        }

        private double getSubstrateCoefficient(SpectrumUnit parameter)
        {
			return 1.65;
        }

        private double getMediumCoeficient(SpectrumUnit waveLength)
        {
            double refractiveIndex;

            //// n_med = sqrt( 1.0 + 2.121*WaveLength*WaveLength / (WaveLength*WaveLength-263.3*263.3) ); // CdBr2 (300K)
            //// n_med = 0.7746 + 0.14147*exp(-(WaveLength-59.0737)/122.56035)
            //// + 0.99808*exp(-(WaveLength-59.0737)/42.78953) + 0.56471*exp(-(WaveLength-59.0737)/36297.71849); // Waher
            refractiveIndex = 1; ////air

            //refractiveIndex = 1.39; //water

			//refractiveIndex = 1.51; // Glass 1.51

			//refractiveIndex = 1.65; // Casein has a refractive index of 1.51-1.65

            //refractiveIndex = this.solid ? 1.0 : refractiveIndex;

            return refractiveIndex;
        }

        /// <summary>
        /// Gets the epsilon.
        /// </summary>
        /// <param name="param">The parameter.</param>
        /// <param name="radius">The radius.</param>
        /// <returns>The complex permittivity.</returns>
        public Complex GetEpsilon(
            DispersionParameter param,
            double radius)
        {
            double resize = 197.35;
            double w = resize * param.WaveVector.Norm; // [eV] kmod
            double wp = 9.2; // [eV] Ag.
            double gamma0 = 0.02; ////[eV]
            double vf = 0.915; ////2.6;//0.915; // [eV*nm] Зміна оптичних констант зі зміною радіуса частинки
            const double Aparam = 0.75; // todo: вплив розмірного параметра

            // todo: check validity of parameters to modify permittivity
            return param.Permittivity;
            //if (this.solid)
            //{
            //    return param.Permittivity;
            //}

            //double gamma = gamma0 + Aparam * vf / radius;
            //double wp2 = wp * wp;

            //double wgamma0 = 1 / (w * w + gamma0 * gamma0);
            //double wgamma = 1 / (w * w + gamma * gamma);

            //double epsReMod = param.Permittivity.Real + wp2 * (wgamma0 - wgamma);

            //double epsImMod = param.Permittivity.Imaginary - (wp2 / w) * (gamma0 * wgamma0 - gamma * wgamma);

            //return new Complex(epsReMod, epsImMod);
        }

        private CartesianCoordinate getWaveVector(
            SpectrumUnit waveLength,
            double refractiveIndex,
            SphericalCoordinate angle)
        {
            SphericalCoordinate radianAngles = angle.ToRadians();

            radianAngles.Radius = waveLength.ToType(SpectrumUnitType.WaveNumber) * refractiveIndex; // k_mod

            return radianAngles.ConvertToCartesian();
        }
    }
}