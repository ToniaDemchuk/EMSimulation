using Simulation.Medium.Medium;

namespace Simulation.Medium.Factors
{
    public class DielectricFactor
    {
        public DielectricFactor(Dielectric medium)
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
