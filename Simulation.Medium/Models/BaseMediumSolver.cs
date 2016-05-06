using Simulation.Models.Coordinates;

namespace Simulation.Medium.Models
{
    /// <summary>
    /// The BaseMediumSolver class.
    /// </summary>
    public abstract class BaseMediumSolver : IMediumSolver 
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseMediumSolver"/> class.
        /// </summary>
        /// <param name="factor">The permittivity.</param>
        protected BaseMediumSolver(BaseMediumFactor factor)
        {
            this.Permittivity = factor.Permittivity;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this medium is included in extinction.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is included; otherwise, <c>false</c>.
        /// </value>
        public bool IsBody { get; set; }

        /// <summary>
        /// Gets the permittivity of medium.
        /// </summary>
        /// <value>
        /// The permittivity of medium.
        /// </value>
        public BaseMedium Permittivity { get; private set; }

        /// <summary>
        /// Solves the electric field using specified displacement field.
        /// </summary>
        /// <param name="displacementField">The displacement field.</param>
        /// <returns>
        /// The electric field.
        /// </returns>
        public abstract CartesianCoordinate Solve(CartesianCoordinate displacementField);
    }
}
