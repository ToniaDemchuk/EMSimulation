using System;

using Simulation.Medium.Medium;
using Simulation.Medium.Models;
using Simulation.Models.Coordinates;
using Simulation.Models.Enums;

namespace Simulation.Medium.MediumSolver
{
    /// <summary>
    ///     The DrudeSolver class.
    /// </summary>
    public class DrudeSolver : BaseMediumSolver
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DrudeSolver" /> class.
        /// </summary>
        /// <param name="medium">The medium.</param>
        /// <param name="timeStep">The time step.</param>
        public DrudeSolver(Drude medium, double timeStep)
            : base(medium)
        {
            double collisionFreq = medium.PlasmaTerm.CollisionFrequency.ToType(SpectrumUnitType.Frequency);
            double plasmaFreq = medium.PlasmaTerm.ResonanceFrequency.ToType(SpectrumUnitType.CycleFrequency);
            double epsvc = collisionFreq * timeStep;
            double exp = Math.Exp(epsvc);

            this.SampledTimeDomain = CartesianCoordinate.Zero;
            this.SampledTimeDomain1 = CartesianCoordinate.Zero;
            this.SampledTimeDomain2 = CartesianCoordinate.Zero;

            this.SampledTimeFactor1 = 1.0 + exp;
            this.SampledTimeFactor2 = exp;
            this.ElectricFactor = ((plasmaFreq * plasmaFreq * medium.PlasmaTerm.StrengthFactor) * timeStep / collisionFreq) * (1.0 - exp);
            this.EpsilonInfinity = medium.EpsilonInfinity;
        }

        /// <summary>
        ///     Gets or sets the sampled time factor1.
        /// </summary>
        /// <value>
        ///     The sampled time factor1.
        /// </value>
        public double SampledTimeFactor1 { get; protected set; }

        /// <summary>
        ///     Gets or sets the sampled time factor2.
        /// </summary>
        /// <value>
        ///     The sampled time factor2.
        /// </value>
        public double SampledTimeFactor2 { get; protected set; }

        /// <summary>
        ///     Gets or sets the electric factor.
        /// </summary>
        /// <value>
        ///     The electric factor.
        /// </value>
        public double ElectricFactor { get; protected set; }

        /// <summary>
        ///     Gets or sets the epsilon infinity.
        /// </summary>
        /// <value>
        ///     The epsilon infinity.
        /// </value>
        public double EpsilonInfinity { get; protected set; }

        /// <summary>
        ///     Gets or sets the sampled time domain.
        /// </summary>
        /// <value>
        ///     The sampled time domain.
        /// </value>
        public CartesianCoordinate SampledTimeDomain { get; protected set; }

        /// <summary>
        ///     Gets or sets the sampled time domain1.
        /// </summary>
        /// <value>
        ///     The sampled time domain1.
        /// </value>
        public CartesianCoordinate SampledTimeDomain1 { get; protected set; }

        /// <summary>
        ///     Gets or sets the sampled time domain2.
        /// </summary>
        /// <value>
        ///     The sampled time domain2.
        /// </value>
        public CartesianCoordinate SampledTimeDomain2 { get; protected set; }

        /// <summary>
        ///     Solves the electric field using specified displacement field.
        /// </summary>
        /// <param name="displacementField">The displacement field.</param>
        /// <returns>
        ///     The electric field.
        /// </returns>
        public override CartesianCoordinate Solve(CartesianCoordinate displacementField)
        {
            CartesianCoordinate efield = (displacementField - this.SampledTimeDomain) / this.EpsilonInfinity;

            this.SampledTimeDomain = this.SampledTimeFactor1 * this.SampledTimeDomain1 -
                                     this.SampledTimeFactor2 * this.SampledTimeDomain2 -
                                     this.ElectricFactor * efield;

            this.SampledTimeDomain2 = this.SampledTimeDomain1;
            this.SampledTimeDomain1 = this.SampledTimeDomain;

            return efield;
        }
    }
}