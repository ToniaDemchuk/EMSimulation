using System;
using System.Linq;
using System.Numerics;

using AwokeKnowing.GnuplotCSharp;

using GnuplotCSharp;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Simulation.Infrastructure;
using Simulation.Medium.Medium;
using Simulation.Models.Common;
using Simulation.Models.Enums;
using Simulation.Models.Spectrum;

namespace Simulation.Medium.Tests.Medium
{
    [TestClass]
    public class DrudeTests
    {
        [TestMethod]
        public void GetPermittivity_Silver_EqualValue()
        {
            // Arrange
            var target = new Drude();
            var collection = new OpticalSpectrum(
                new LinearDiscreteCollection(300e-9, 1000e-9, 10),
                SpectrumUnitType.WaveLength);
            var gp = new GnuPlot();

            // Act
            var dictionary = collection.ToDictionary(s => s.ToType(SpectrumUnitType.WaveLength), s => target.GetPermittivity(s));
            
            gp.Plot(dictionary);

            // Assert
            Assert.IsTrue(dictionary.All(x => x.Value.Imaginary >= 0));
            gp.Wait();
        }

    }
}