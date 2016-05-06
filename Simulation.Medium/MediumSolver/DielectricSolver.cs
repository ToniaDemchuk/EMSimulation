using Simulation.Medium.Factors;
using Simulation.Medium.Models;
using Simulation.Models.Coordinates;

namespace Simulation.Medium.MediumSolver
{
    /// <summary>
    /// The DielectricSolver class.
    /// </summary>
    public class DielectricSolver : BaseMediumSolver
    {
        private readonly DielectricFactor param;

        /// <summary>
        /// Initializes a new instance of the <see cref="DielectricSolver" /> class.
        /// </summary>
        /// <param name="param">The parameter.</param>
        public DielectricSolver(DielectricFactor param)
            : base(param)
        {
            this.param = param;
        }
        
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