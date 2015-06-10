using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using AwokeKnowing.GnuplotCSharp;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Simulation.Infrastructure;
using Simulation.Models;

namespace Simulation.DDA.Tests
{

    [TestClass]
    public class OpticalConstantTest
    {


        [TestMethod]
        public void CalculateOpticalConstants_ScilabEngine()
        {
            // Arrange
            var optConst = ParameterHelper.ReadOpticalConstants("opt_const.txt");

            var EpsInfinity = 3.9943;
            var OmegaP = 1.369e+16;
            var DEps0 = 8.45e-1;
            var Gamma0 = 7.292e+13;

            var dict = new Dictionary<double, Complex>();
            foreach (var waveLength in optConst.WaveLengthList)
            {
                var omeg = 2 * Math.PI * Fundamentals.LightVelocity / (waveLength * 1e-9);
                var compl = EpsInfinity -
                            OmegaP * OmegaP /
                            (omeg * omeg -
                             Complex.ImaginaryOne * Gamma0 * omeg);
                dict.Add(waveLength, compl);
            }

            ParameterHelper.WriteOpticalConstants("opt_const_new.txt", dict);

            using (var engine = new ScilabEngine.Engine.ScilabEngine())
            {
                string waveLength = ScilabHelper.FormatToArray(dict.Select(x=>x.Key));
                var strReal = ScilabHelper.FormatToArray(dict.Select(x => x.Value.Real));
                var strImag = ScilabHelper.FormatToArray(dict.Select(x => x.Value.Imaginary));
                engine.Execute(@"plot({0}, {1}, {0}, {2})", waveLength, strReal, strImag);

                waveLength = ScilabHelper.FormatToArray(dict.Select(x => x.Key));
                strReal = ScilabHelper.FormatToArray(optConst.PermittivityList.Select(x => x.Real));
                strImag = ScilabHelper.FormatToArray(optConst.PermittivityList.Select(x => x.Imaginary));
                engine.Execute(@"plot({0}, {1}, {0}, {2})", waveLength, strReal, strImag);
                //engine.Wait();
            }

            // Act

            // Assert
        }

        [TestMethod]
        public void CalculateOpticalConstants_Gnuplot()
        {
            // Arrange
            var optConst = ParameterHelper.ReadOpticalConstants("opt_const.txt");

            var EpsInfinity = 1;//3.9943;
            var OmegaP = 1.369e+16;
            var DEps0 = 8.45e-1;
            var Gamma0 = 7.292e+13;

            var dict = new Dictionary<double, Complex>();
            foreach (var waveLength in optConst.WaveLengthList)
            {
                var omeg = 2 * Math.PI * Fundamentals.LightVelocity / (waveLength * 1e-9);
                var compl = EpsInfinity -
                            OmegaP * OmegaP /
                            (omeg * omeg -
                             Complex.ImaginaryOne * Gamma0 * omeg);
                dict.Add(waveLength, compl);
            }

            ParameterHelper.WriteOpticalConstants("opt_const_new.txt", dict);

            using (var gp = new GnuPlot())
            {
                gp.HoldOn();
                gp.Set("style data lines");
                gp.Plot(optConst.WaveLengthList.ToArray(), optConst.PermittivityList.Select(x => x.Real).ToArray());
                gp.Plot(optConst.WaveLengthList.ToArray(), optConst.PermittivityList.Select(x => x.Imaginary).ToArray());

                gp.Plot(dict.Keys.ToArray(), dict.Values.Select(x => x.Real).ToArray());
                gp.Plot(dict.Keys.ToArray(), dict.Values.Select(x => x.Imaginary).ToArray());
                gp.Wait();
            }
            // Act

            // Assert
        }
    }
}
