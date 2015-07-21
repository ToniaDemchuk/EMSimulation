using Simulation.Medium.Medium;
using Simulation.Models.Constants;

namespace Simulation.Medium.Factors
{
    public class LossyDielectricFactor : DielectricFactor
    {
        public LossyDielectricFactor(LossyDielectric medium, double timeStep):base(medium)
        {
            this.Displacement = 1.0 /
                (medium.Epsilon + (medium.Conductivity * timeStep / Fundamentals.ElectricConst));

            this.Electric = medium.Conductivity * timeStep / Fundamentals.ElectricConst;
        }

        /// <summary>
        ///     Gets or sets the integral factor.
        /// </summary>
        /// <value>
        ///     The integral factor.
        /// </value>
        public double Electric { get; protected set; }
    }
}
