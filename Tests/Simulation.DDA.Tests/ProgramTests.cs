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
            var result = DDAProgram.Calculate().ToDictionary(x=>x.Key, x=>x.Value.CrossSectionExtinction);

            // Assert
            // HACK: remove 10 epsilon
            AssertHelper.DictionaryAreClose(dict, result, 10);
        }
    }
}
