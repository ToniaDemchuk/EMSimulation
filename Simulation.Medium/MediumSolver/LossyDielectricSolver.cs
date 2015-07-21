using Simulation.Medium.Factors;
using Simulation.Medium.Medium;
using Simulation.Models.Constants;
using Simulation.Models.Coordinates;

namespace Simulation.Medium.MediumSolver
{
    /// <summary>
    ///     The LossyDielectricSolver class.
    /// </summary>
    public class LossyDielectricSolver : DielectricSolver
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LossyDielectricSolver" /> class.
        /// </summary>
        /// <param name="medium">The medium.</param>
        /// <param name="param">The parameter.</param>
        public LossyDielectricSolver(LossyDielectric medium, LossyDielectricFactor param)
            : base(medium, param)
        {
            this.param = param;
        }

        private readonly LossyDielectricFactor param;

        /// <summary>
        ///     Gets or sets the integral field.
        /// </summary>
        /// <value>
        ///     The integral field.
        /// </value>
        public CartesianCoordinate IntegralField { get; protected set; }

        /// <summary>
        ///     Solves the electric field using specified displacement field.
        /// </summary>
        /// <param name="displacementField">The displacement field.</param>
        /// <returns>
        ///     The electric field.
        /// </returns>
        public override CartesianCoordinate Solve(
            CartesianCoordinate displacementField)
        {
            CartesianCoordinate efield = (displacementField - this.IntegralField) * this.param.Displacement;
            this.IntegralField = this.IntegralField + efield * this.param.Electric;
            return efield;
        }
    }
}