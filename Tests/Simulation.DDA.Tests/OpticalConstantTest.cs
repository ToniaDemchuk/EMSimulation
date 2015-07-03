using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

using AwokeKnowing.GnuplotCSharp;

using GnuplotCSharp;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Simulation.Medium.Medium;
using Simulation.Models.Constants;
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
        //[TestMethod]
        //[Ignore]
        //public void CalculateOpticalConstants_ScilabEngine()
        //{
        //    // Arrange
        //    var optConst = ParameterHelper.ReadOpticalConstants("opt_const.txt");

        //    var EpsInfinity = 3.9943;
        //    var OmegaP = 1.369e+16;
        //    var DEps0 = 8.45e-1;
        //    var Gamma0 = 7.292e+13;

        //    var dict = new Dictionary<double, Complex>();
        //    foreach (var waveLength in optConst.WaveLengthList)
        //    {
        //        var omeg = SpectrumUnitConverter.Convert(waveLength * 1e-9, SpectrumUnitType.WaveLength, SpectrumUnitType.CycleFrequency);
        //        var compl = EpsInfinity -
        //                    OmegaP * OmegaP /
        //                    (omeg * omeg -
        //                     Complex.ImaginaryOne * Gamma0 * omeg);
        //        dict.Add(waveLength, compl);
        //    }

        //    ParameterHelper.WriteOpticalConstants("opt_const_new.txt", dict);

        //    using (var engine = new ScilabEngine.Engine.ScilabEngine())
        //    {
        //        string waveLength = ScilabHelper.FormatToArray(dict.Select(x => x.Key));
        //        var strReal = ScilabHelper.FormatToArray(dict.Select(x => x.Value.Real));
        //        var strImag = ScilabHelper.FormatToArray(dict.Select(x => x.Value.Imaginary));
        //        engine.Execute(@"plot({0}, {1}, {0}, {2})", waveLength, strReal, strImag);

        //        waveLength = ScilabHelper.FormatToArray(dict.Select(x => x.Key));
        //        strReal = ScilabHelper.FormatToArray(optConst.PermittivityList.Select(x => x.Real));
        //        strImag = ScilabHelper.FormatToArray(optConst.PermittivityList.Select(x => x.Imaginary));
        //        engine.Execute(@"plot({0}, {1}, {0}, {2})", waveLength, strReal, strImag);
        //        // engine.Wait();
        //    }

        //    // Act

        //    // Assert
        //}

        [TestMethod]
        public void CalculateOpticalConstants_DrudeLorentz_Gnuplot()
        {
            // Arrange
            var optConst = ParameterHelper.ReadOpticalConstants("opt_const.txt");
            var drudeLorentz = new DrudeLorentz();
            var dict = new Dictionary<double, Complex>();
            foreach (var waveLength in optConst.WaveLengthList)
            {
                var freq = new SpectrumUnit(waveLength*1e-9, SpectrumUnitType.WaveLength);
                dict.Add(waveLength, drudeLorentz.GetPermittivity(freq));
            }

            ParameterHelper.WriteOpticalConstants("opt_const_drudeLorentz.txt", dict);

            using (var gp = new GnuPlot())
            {
                gp.HoldOn();
                gp.Set("style data lines");

                gp.Plot(optConst.WaveLengthList, optConst.PermittivityList);
                gp.Plot(dict);
                
                gp.Wait();
            }
            // Act

            // Assert
        }
    }
}