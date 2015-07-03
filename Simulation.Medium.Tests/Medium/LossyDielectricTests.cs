using System.Numerics;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Simulation.Medium.Medium;
using Simulation.Models.Enums;
using Simulation.Models.Spectrum;

namespace Simulation.Medium.Tests.Medium
{
    [TestClass]
    public class LossyDielectricTests
    {
        [TestMethod]
        public void GetPermittivity_Default_ReturnsOne()
        {
            // Arrange
            var parameter = new SpectrumUnit(300e-9, SpectrumUnitType.WaveLength);
            var target = new LossyDielectric();

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
            var target = new LossyDielectric { Epsilon = RelativePermittivity };

            // Act
            Complex result = target.GetPermittivity(parameter);

            // Assert
            Assert.AreEqual(RelativePermittivity, result);
        }

        [TestMethod]
        public void GetPermittivity_SetEpsilonAndConduct_ReturnsOne()
        {
            // Arrange
            const double RelativePermittivity = 5.0;
            const double Conductivity = 5.0;
            var parameter = new SpectrumUnit(300e-9, SpectrumUnitType.WaveLength);
            var target = new LossyDielectric
            {
                Epsilon = RelativePermittivity,
                Conductivity = Conductivity
            };

            // Act
            Complex result = target.GetPermittivity(parameter);

            // Assert
            Assert.AreEqual(RelativePermittivity, result.Real);
            Assert.IsTrue(result.Imaginary < 0);
        }
    }
}