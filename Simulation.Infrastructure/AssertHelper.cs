using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Simulation.Infrastructure
{
    /// <summary>
    /// The AssertHelper class.
    /// </summary>
    public class AssertHelper
    {
        /// <summary>
        /// Asserts whether double values are close.
        /// </summary>
        /// <param name="expected">The expected.</param>
        /// <param name="actual">The actual.</param>
        /// <param name="epsilon">The epsilon.</param>
        public static void AreClose(double expected, double actual, double epsilon = 1e-5)
        {
            Assert.IsTrue(MathHelper.AreClose(expected, actual, epsilon), "Values are not close.\r\n Expected<{0}>. Actual<{1}>", expected, actual);
        }

        /// <summary>
        /// Asserts whether dictionary of double values are close.
        /// </summary>
        /// <param name="expected">The expected.</param>
        /// <param name="actual">The actual.</param>
        /// <param name="epsilon">The epsilon.</param>
        public static void DictionaryAreClose(Dictionary<double, double> expected, Dictionary<double, double> actual, double epsilon = 1e-5)
        {
            Assert.AreEqual(expected.Count, actual.Count, "Dictionary count are not equal.");
            foreach (var value in actual)
            {
                var dictkey = expected.Keys.First(x => MathHelper.AreClose(x, value.Key));
                AreClose(expected[dictkey], value.Value, epsilon);
            }
        }
    }
}