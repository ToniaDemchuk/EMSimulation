using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Simulation.DDA.Console;
using Simulation.Infrastructure;

namespace Simulation.DDA.Tests
{
    [TestClass]
    public class ExtinctionManagerTests
    {
        [TestMethod]
        public void MethodName_Condition()
        {
            // Arrange
            var filename = "../../rezult_ext.txt";

            var dict = SimpleFormatter.Read(filename);

            // Act
            var result = Program.Calculate();

            // Assert
            Assert.AreEqual(dict.Count, result.Count);
            foreach (var value in result)
            {
                var dictkey = dict.Keys.First(x => AreClose(x, value.Key));
                //HACK: remove 10 epsilon
                Assert.IsTrue(AreClose(dict[dictkey], value.Value, 11), "Are not close {0} and {1}", dict[dictkey], value.Value);

            }
        }

        private static bool AreClose(double x, double value, double epsilon = 1e-5)
        {
            return Math.Abs(x - value) < epsilon;
        }
    }
}
