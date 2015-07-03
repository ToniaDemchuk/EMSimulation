using Simulation.Models.Coordinates;

namespace Simulation.Medium.Models
{
    /// <summary>
    /// The IMediumSolver interface.
    /// </summary>
    public interface IMediumSolver
    {
        /// <summary>
        /// Gets or sets a value indicating whether this medium is included in extinction.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is included; otherwise, <c>false</c>.
        /// </value>
        bool IsBody { get; set; }

        /// <summary>
        /// Solves the electric field using specified displacement field.
        /// </summary>
        /// <param name="displacementField">The displacement field.</param>
        /// <returns>The electric field.</returns>
        CartesianCoordinate Solve(CartesianCoordinate displacementField);

        /// <summary>
        /// Gets the permittivity of medium.
        /// </summary>
        /// <value>
        /// The permittivity of medium.
        /// </value>
        BaseMedium Permittivity { get; }
    }
}
