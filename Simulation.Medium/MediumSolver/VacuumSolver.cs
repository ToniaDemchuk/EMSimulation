using Simulation.Medium.Models;
using Simulation.Models.Coordinates;

namespace Simulation.Medium.MediumSolver
{
    /// <summary>
    /// The VacuumSolver class.
    /// </summary>
    public class VacuumSolver : BaseMediumSolver
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VacuumSolver"/> class.
        /// </summary>
        /// <param name="permittivity">The permittivity.</param>
        public VacuumSolver(BaseMedium permittivity)
            : base(permittivity)
        {
        }

        /// <summary>
        /// Solves the electric field using specified displacement field.
        /// </summary>
        /// <param name="displacementField">The displacement field.</param>
        /// <returns>
        /// The electric field.
        /// </returns>
        public override CartesianCoordinate Solve(
            CartesianCoordinate displacementField)
        {
            return displacementField;
        }
    }
}