using System.Numerics;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Simulation.Medium.Medium;
using Simulation.Models.Enums;
using Simulation.Models.Spectrum;

namespace Simulation.Medium.Tests.Medium
{
    [TestClass]
    public class VacuumTests
    {
        [TestMethod]
        public void GetPermittivity_AnyUnit_ReturnsOne()
        {
            // Arrange
            var parameter = new SpectrumUnit(300e-9, SpectrumUnitType.WaveLength);
            var target = new Vacuum();

            // Act
            Complex result = target.GetPermittivity(parameter);

            // Assert
            Assert.AreEqual(Complex.One, result);
        }
    }
}
