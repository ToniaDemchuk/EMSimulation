using Simulation.Medium.Factors;
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
        /// Initializes a new instance of the <see cref="DielectricSolver" /> class.
        /// </summary>
        /// <param name="medium">The medium.</param>
        /// <param name="param">The parameter.</param>
        public DielectricSolver(Dielectric medium, DielectricFactor param)
            : base(medium)
        {
            this.param = param;
        }

        private readonly DielectricFactor param;

        /// <summary>
        /// Solves the electric field using specified displacement field.
        /// </summary>
        /// <param name="displacementField">The displacement field.</param>
        /// <returns>
        /// The electric field.
        /// </returns>
        public override CartesianCoordinate Solve(CartesianCoordinate displacementField)
        {
            return this.param.Displacement * displacementField;
        }

    }
}