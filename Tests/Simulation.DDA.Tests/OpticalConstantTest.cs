using System.Collections.Generic;
using System.Linq;
using System.Numerics;

using AwokeKnowing.GnuplotCSharp;

using GnuplotCSharp;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Simulation.Medium.Medium;
using Simulation.Models.Enums;
using Simulation.Models.Extensions;
using Simulation.Models.Spectrum;

namespace Simulation.DDA.Tests
{
    /// <summary>
    /// The OpticalConstantTest class.
    /// </summary>
    [TestClass]
    public class OpticalConstantTest
    {
        [TestMethod]
        [TestCategory("Gnuplot")]
        public void CalculateOpticalConstants_Drude()
        {
            // Arrange
            var optConst = ParameterHelper.ReadOpticalConstants("opt_const.txt");

            var EpsInfinity = 3.9943;
            var OmegaP = 1.369e+16;
            var DEps0 = 8.45e-1;
            var Gamma0 = 7.292e+13;

            var funcPermitivitty = new Dictionary<double, Complex>();
            foreach (var waveLength in optConst.WaveLengthList)
            {
                var omeg = SpectrumUnitConverter.Convert(waveLength / OpticalConstants.WaveLengthMultiplier,
                    SpectrumUnitType.WaveLength, SpectrumUnitType.CycleFrequency);
                var compl = EpsInfinity -
                            OmegaP * OmegaP /
                            (omeg * omeg -
                             Complex.ImaginaryOne * Gamma0 * omeg);
                funcPermitivitty.Add(waveLength, compl);
            }

            ParameterHelper.WriteOpticalConstants("opt_const_new.txt", funcPermitivitty);

            using (var gnuplot = new GnuPlot())
            {
                gnuplot.HoldOn();
                gnuplot.Plot(funcPermitivitty);

                var permitivittyList = getPermitivittyFunc(optConst);

                gnuplot.Plot(permitivittyList);
            }

            // Act

            // Assert
        }

        [TestMethod]
        [TestCategory("Gnuplot")]
        public void CalculateOpticalConstants_DrudeLorentz_Gnuplot()
        {
            // Arrange
            var optConst = ParameterHelper.ReadOpticalConstants("opt_const.txt");
            var drudeLorentz = new DrudeLorentz();
            var dict = new Dictionary<double, Complex>();
            foreach (var waveLength in optConst.WaveLengthList)
            {
                var freq = new SpectrumUnit(waveLength / OpticalConstants.WaveLengthMultiplier,
                    SpectrumUnitType.WaveLength);
                dict.Add(waveLength, drudeLorentz.GetPermittivity(freq));
            }

            ParameterHelper.WriteOpticalConstants("opt_const_drudeLorentz.txt", dict);

            using (var gp = new GnuPlot())
            {
                gp.HoldOn();
                gp.Set("style data lines");

                gp.Plot(getPermitivittyFunc(optConst));
                gp.Plot(dict);

                gp.Wait();
            }
            // Act

            // Assert
        }

        private static Dictionary<double, Complex> getPermitivittyFunc(OpticalConstants optConst)
        {
            var permitivittyList =
                optConst.PermittivityList.Zip(optConst.WaveLengthList,
                        (perm, waveLength) => new KeyValuePair<double, Complex>(waveLength, perm))
                    .ToDictionary(x => x.Key, x => x.Value);
            return permitivittyList;
        }
    }
}