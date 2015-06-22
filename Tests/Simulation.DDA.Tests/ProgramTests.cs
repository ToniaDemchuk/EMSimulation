using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Simulation.DDA.Console;
using Simulation.Infrastructure;
using Simulation.Models;

namespace Simulation.DDA.Tests
{
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
            var result = DDAProgram.Calculate().ToDictionary(x => x.ToType(SpectrumParameterType.WaveLength), x => x.CrossSectionExtinction);

            // Assert
            AssertHelper.DictionaryAreClose(dict, result, 0.01);
        }
    }
}
