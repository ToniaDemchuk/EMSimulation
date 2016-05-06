using Simulation.Medium.Medium;
using Simulation.Medium.Models;
using Simulation.Models.Coordinates;

namespace Simulation.Medium.MediumSolver
{
    /// <summary>
    /// The VacuumSolver class.
    /// </summary>
    public class VacuumSolver : BaseMediumSolver
    {
        public static VacuumSolver Default = new VacuumSolver();

        /// <summary>
        /// Initializes a new instance of the <see cref="VacuumSolver" /> class.
        /// </summary>
        protected VacuumSolver()
            : base(new BaseMediumFactor(new Vacuum()))
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