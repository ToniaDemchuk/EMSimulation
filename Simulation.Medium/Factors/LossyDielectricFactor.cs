using Simulation.Medium.Medium;
using Simulation.Models.Constants;

namespace Simulation.Medium.Factors
{
    /// <summary>
    /// The factors for lossy dieectric medium.
    /// </summary>
    public class LossyDielectricFactor : DielectricFactor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LossyDielectricFactor"/> class.
        /// </summary>
        /// <param name="medium">The medium.</param>
        /// <param name="timeStep">The time step.</param>
        public LossyDielectricFactor(LossyDielectric medium, double timeStep) : base(medium)
        {
            this.Displacement = 1.0 /
                (medium.Epsilon + (medium.Conductivity * timeStep / Fundamentals.ElectricConst));

            this.Electric = medium.Conductivity * timeStep / Fundamentals.ElectricConst;
        }

        /// <summary>
        ///     Gets or sets the electric factor.
        /// </summary>
        /// <value>
        ///     The electric factor.
        /// </value>
        public double Electric { get; protected set; }
    }
}
