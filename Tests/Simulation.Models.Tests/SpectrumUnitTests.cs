using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Simulation.Models.Enums;
using Simulation.Models.Spectrum;

namespace Simulation.Models.Tests
{
    [TestClass]
    public class SpectrumUnitTests
    {
        [TestMethod]
        public void Equals_SameSpectrumType_ReturnsTrue()
        {
            // Arrange
            var unit = new SpectrumUnit(300, SpectrumUnitType.Frequency);
            var dict = new Dictionary<SpectrumUnit, double>();
            dict.Add(unit, 0);

            //Act
            var result = unit.Equals(unit);
            var contains = dict.ContainsKey(unit);

            // Assert
            Assert.IsTrue(result);
            Assert.IsTrue(contains);
        }

        [TestMethod]
        public void Equals_AnotherSpectrumType_ReturnsTrue()
        {
            // Arrange
            var unitExpected = new SpectrumUnit(300, SpectrumUnitType.Frequency);
            var unitAnother = new SpectrumUnit(300, SpectrumUnitType.Frequency);
            var dict = new Dictionary<SpectrumUnit, double>();
            dict.Add(unitExpected, 0);

            //Act
            var result = unitExpected.Equals(unitAnother);
            var contains = dict.ContainsKey(unitAnother);

            // Assert
            Assert.IsTrue(result);
            Assert.IsTrue(contains);
        }
    }
}
