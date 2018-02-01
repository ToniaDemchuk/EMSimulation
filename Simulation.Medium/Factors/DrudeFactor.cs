using System;

using Simulation.Medium.Medium;
using Simulation.Medium.Models;
using Simulation.Models.Enums;

namespace Simulation.Medium.Factors
{
    /// <summary>
    /// The factors for Drude medium.
    /// </summary>
    public class DrudeFactor : BaseMediumFactor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DrudeFactor"/> class.
        /// </summary>
        /// <param name="medium">The medium.</param>
        /// <param name="timeStep">The time step.</param>
        public DrudeFactor(Drude medium, double timeStep) : base(medium)
        {
            double collisionFreq = medium.PlasmaTerm.CollisionFrequency.ToType(SpectrumUnitType.Frequency);
            double plasmaFreq = medium.PlasmaTerm.ResonanceFrequency.ToType(SpectrumUnitType.CycleFrequency);
            double epsvc = collisionFreq * timeStep;
            double exp = Math.Exp(-epsvc);

            this.SampledTimeShift1 = 1.0 + exp;
            this.SampledTimeShift2 = exp;
            this.Electric = ((plasmaFreq * plasmaFreq * medium.PlasmaTerm.StrengthFactor) * timeStep / collisionFreq) * (1.0 - exp);
            this.EpsilonInfinity = medium.EpsilonInfinity;
        }

        /// <summary>
        ///     Gets or sets the sampled time factor1.
        /// </summary>
        /// <value>
        ///     The sampled time factor1.
        /// </value>
        public double SampledTimeShift1 { get; protected set; }

        /// <summary>
        ///     Gets or sets the sampled time factor2.
        /// </summary>
        /// <value>
        ///     The sampled time factor2.
        /// </value>
        public double SampledTimeShift2 { get; protected set; }

        /// <summary>
        ///     Gets or sets the electric factor.
        /// </summary>
        /// <value>
        ///     The electric factor.
        /// </value>
        public double Electric { get; protected set; }

        /// <summary>
        ///     Gets or sets the epsilon infinity.
        /// </summary>
        /// <value>
        ///     The epsilon infinity.
        /// </value>
        public double EpsilonInfinity { get; protected set; }
    }
}
