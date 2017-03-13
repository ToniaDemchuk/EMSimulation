using System;
using System.Numerics;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Simulation.Medium.Medium;
using Simulation.Models.Enums;
using Simulation.Models.Spectrum;

namespace Simulation.Medium.Tests.Medium
{
    [TestClass]
    public class DielectricTests
    {
        [TestMethod]
        public void GetPermittivity_DefaultEpsilon_ReturnsOne()
        {
            // Arrange
            var parameter = new SpectrumUnit(300e-9, SpectrumUnitType.WaveLength);
            var target = new Dielectric();

            // Act
            Complex result = target.GetPermittivity(parameter);

            // Assert
            Assert.AreEqual(Complex.One, result);
        }

        [TestMethod]
        public void GetPermittivity_SetEpsilon_ReturnsOne()
        {
            // Arrange
            const double RelativePermittivity = 5.0;
            var parameter = new SpectrumUnit(300e-9, SpectrumUnitType.WaveLength);
            var target = new Dielectric { Epsilon = RelativePermittivity };

            // Act
            Complex result = target.GetPermittivity(parameter);

            // Assert
            Assert.AreEqual(RelativePermittivity, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SetEpsilon_InvalidEpsilon_ThrowsException()
        {
            // Arrange
            const double InvalidPermittivity = -1.0;
            var target = new Dielectric();

            // Act
            Complex result = target.Epsilon = InvalidPermittivity;

            // Assert
            // See ExpectedException
        }
    }
}
