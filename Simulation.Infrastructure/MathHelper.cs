using System;

namespace Simulation.Infrastructure
{
    /// <summary>
    /// The MathHelper class.
    /// </summary>
    public class MathHelper
    {
        /// <summary>
        /// Checks whether double values are close.
        /// </summary>
        /// <param name="value1">The value1.</param>
        /// <param name="value2">The value2.</param>
        /// <param name="epsilon">The epsilon.</param>
        /// <returns>True if values are close; otherwise false.</returns>
        public static bool AreClose(double value1, double value2, double epsilon = 1e-5)
        {
            if (Math.Abs(value1 - value2) < epsilon)
            {
                return true;
            }
            
            var max = Math.Max(value1, value2);
            var relativeDiff = Math.Abs((value1 - value2) / max);
            return relativeDiff < epsilon;
        }
    }
}
