using Simulation.Medium.Medium;
using Simulation.Medium.Models;
using Simulation.Models.Coordinates;

namespace Simulation.Medium.MediumSolver
{
    /// <summary>
    /// The DielectricSolver class.
    /// </summary>
    public class DielectricSolver : BaseMediumSolver
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DielectricSolver"/> class.
        /// </summary>
        /// <param name="medium">The medium.</param>
        public DielectricSolver(Dielectric medium)
            : base(medium)
        {
            this.DisplacementFactor = 1 / medium.Epsilon;
        }

        /// <summary>
        /// Gets or sets the displacement factor.
        /// </summary>
        /// <value>
        /// The displacement factor.
        /// </value>
        public double DisplacementFactor { get; set; }

        /// <summary>
        /// Solves the electric field using specified displacement field.
        /// </summary>
        /// <param name="displacementField">The displacement field.</param>
        /// <returns>
        /// The electric field.
        /// </returns>
        public override CartesianCoordinate Solve(CartesianCoordinate displacementField)
        {
            return this.DisplacementFactor * displacementField;
        }

    }
}