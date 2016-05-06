namespace Simulation.Medium.Models
{
    public class BaseMediumFactor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseMediumSolver"/> class.
        /// </summary>
        /// <param name="permittivity">The permittivity.</param>
        public BaseMediumFactor(BaseMedium permittivity)
        {
            this.Permittivity = permittivity;
        }

        /// <summary>
        /// Gets the permittivity of medium.
        /// </summary>
        /// <value>
        /// The permittivity of medium.
        /// </value>
        public BaseMedium Permittivity { get; private set; }
    }
}
