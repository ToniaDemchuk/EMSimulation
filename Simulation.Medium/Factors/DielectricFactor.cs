using Simulation.Medium.Medium;
using Simulation.Medium.Models;

namespace Simulation.Medium.Factors
{
    /// <summary>
    /// The factors for dielectric medium
    /// </summary>
    public class DielectricFactor : BaseMediumFactor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DielectricFactor"/> class.
        /// </summary>
        /// <param name="medium">The medium.</param>
        public DielectricFactor(Dielectric medium) : base(medium)
        {
            this.Displacement = 1 / medium.Epsilon;
        }

        /// <summary>
        /// Gets or sets the displacement factor.
        /// </summary>
        /// <value>
        /// The displacement factor.
        /// </value>
        public double Displacement { get; protected set; }
    }
}
