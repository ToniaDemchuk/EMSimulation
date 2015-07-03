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
        ///     Initializes a new instance of the <see cref="LossyDielectricSolver" /> class.
        /// </summary>
        /// <param name="medium">The medium.</param>
        /// <param name="timeStep">The time step.</param>
        public LossyDielectricSolver(LossyDielectric medium, double timeStep)
            : base(medium)
        {
            this.DisplacementFactor = 1.0 /
                (medium.Epsilon + (medium.Conductivity * timeStep / Fundamentals.ElectricConst));

            this.ElectricFactor = medium.Conductivity * timeStep / Fundamentals.ElectricConst;
        }

        /// <summary>
        ///     Gets or sets the integral field.
        /// </summary>
        /// <value>
        ///     The integral field.
        /// </value>
        public CartesianCoordinate IntegralField { get; set; }

        /// <summary>
        ///     Gets or sets the integral factor.
        /// </summary>
        /// <value>
        ///     The integral factor.
        /// </value>
        public double ElectricFactor { get; set; }

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
            CartesianCoordinate efield = (displacementField - this.IntegralField) * this.DisplacementFactor;
            this.IntegralField = this.IntegralField + efield * this.ElectricFactor;
            return efield;
        }
    }
}