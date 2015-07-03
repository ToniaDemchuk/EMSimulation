using Microsoft.VisualStudio.TestTools.UnitTesting;

using Simulation.DDA.Console.Simulation;
using Simulation.Infrastructure;
using Simulation.Models.Enums;

namespace Simulation.DDA.Tests
{
    /// <summary>
    /// The ProgramTests class.
    /// </summary>
    [TestClass]
    public class ProgramTests
    {
        /// <summary>
        /// Tests Calculate_: calculate_ - success.
        /// </summary>
        [TestMethod]
        public void Calculate_Success()
        {
            // Arrange
            var filename = "../../rezult_ext.txt";

            var dict = SimpleFormatter.Read(filename);

            // Act
            var result = DDAProgram.Calculate().ToDictionary(x => x.ToType(SpectrumUnitType.WaveLength), x => x.CrossSectionExtinction);

            // Assert
            AssertHelper.DictionaryAreClose(dict, result, 0.01);
        }
    }
}
