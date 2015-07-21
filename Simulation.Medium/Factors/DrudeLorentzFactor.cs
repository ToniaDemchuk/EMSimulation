using System;

using Simulation.Medium.Medium;
using Simulation.Medium.MediumSolver;
using Simulation.Models.Enums;

namespace Simulation.Medium.Factors
{
    /// <summary>
    ///     The DrudeLorentzSolver class.
    /// </summary>
    public class DrudeLorentzFactor : DrudeFactor
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DrudeLorentzSolver" /> class.
        /// </summary>
        /// <param name="medium">The medium.</param>
        /// <param name="timeStep">The time step.</param>
        public DrudeLorentzFactor(DrudeLorentz medium, double timeStep)
            : base(medium, timeStep)
        {
            this.SampledLorentzShift1 = new double[medium.OscillatorTerms.Count];
            this.SampledLorentzShift2 = new double[medium.OscillatorTerms.Count];
            this.ElectricLorentz = new double[medium.OscillatorTerms.Count];

            double plasmaFreq = medium.PlasmaTerm.ResonanceFrequency.ToType(SpectrumUnitType.CycleFrequency);

            for (int l = 0; l < medium.OscillatorTerms.Count; l++)
            {
                double collisionFreq =
                    medium.OscillatorTerms[l].CollisionFrequency.ToType(SpectrumUnitType.Frequency);
                double resonatorFreq =
                    medium.OscillatorTerms[l].ResonanceFrequency.ToType(SpectrumUnitType.CycleFrequency);
                double alfadt = (collisionFreq / 2.0) * timeStep;
                double sqrtl =
                    Math.Sqrt(Math.Pow(resonatorFreq, 2.0) - (Math.Pow(collisionFreq, 2.0) / 4.0));
                double betadt = sqrtl * timeStep;

                double gammadt = Math.Pow(plasmaFreq, 2.0) / sqrtl * medium.OscillatorTerms[l].StrengthFactor * timeStep;
                double exp = Math.Exp(alfadt);
                this.SampledLorentzShift1[l] = 2.0 * exp * Math.Cos(betadt);
                this.SampledLorentzShift2[l] = Math.Exp(2.0 * alfadt);
                this.ElectricLorentz[l] = exp * Math.Sin(betadt) * gammadt;
            }
        }

        /// <summary>
        ///     Gets or sets the sampled lorentz factor1.
        /// </summary>
        /// <value>
        ///     The sampled lorentz factor1.
        /// </value>
        public double[] SampledLorentzShift1 { get; protected set; }

        /// <summary>
        ///     Gets or sets the sampled lorentz factor2.
        /// </summary>
        /// <value>
        ///     The sampled lorentz factor2.
        /// </value>
        public double[] SampledLorentzShift2 { get; protected set; }

        /// <summary>
        ///     Gets or sets the electric lorentz factor.
        /// </summary>
        /// <value>
        ///     The electric lorentz factor.
        /// </value>
        public double[] ElectricLorentz { get; protected set; }

    }
}