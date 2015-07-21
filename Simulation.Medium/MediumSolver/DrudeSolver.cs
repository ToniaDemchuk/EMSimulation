using System;

using Simulation.Medium.Factors;
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
        /// Initializes a new instance of the <see cref="DrudeSolver" /> class.
        /// </summary>
        /// <param name="medium">The medium.</param>
        /// <param name="param">The parameter.</param>
        public DrudeSolver(Drude medium, DrudeFactor param)
            : base(medium)
        {
            this.param = param;
        }

        private readonly DrudeFactor param;

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
            CartesianCoordinate efield = (displacementField - this.SampledTimeDomain) / param.EpsilonInfinity;

            this.SampledTimeDomain = param.SampledTimeShift1 * this.SampledTimeDomain1 -
                                     param.SampledTimeShift2 * this.SampledTimeDomain2 -
                                     param.Electric * efield;

            this.SampledTimeDomain2 = this.SampledTimeDomain1;
            this.SampledTimeDomain1 = this.SampledTimeDomain;

            return efield;
        }
    }
}