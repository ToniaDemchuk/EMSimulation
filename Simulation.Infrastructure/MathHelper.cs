using System;

namespace Simulation.Infrastructure
{
    public class MathHelper
    {
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
