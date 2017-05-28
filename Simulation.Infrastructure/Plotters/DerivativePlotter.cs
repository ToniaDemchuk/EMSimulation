using System;
using System.Collections.Generic;
using System.Linq;
using AwokeKnowing.GnuplotCSharp;
using GnuplotCSharp;
using Simulation.Models.Enums;
using Simulation.Models.Spectrum;

namespace Simulation.Infrastructure.Plotters
{
    /// <summary>
    /// The plotter for spectrum.
    /// </summary>
    public class DerivativePlotter
    {
        private readonly GnuPlot gp;

        public DerivativePlotter()
        {
            this.gp = new GnuPlot();
        }

        /// <summary>
        /// Plots the specified result.
        /// </summary>
        /// <param name="result">The result.</param>
        public void Plot(Dictionary<SpectrumUnit, double> result)
        {
            Func<KeyValuePair<SpectrumUnit, double>, double> selector = x => x.Key.ToType(SpectrumUnitType.WaveLength);
            var waveLengths = result.Select(selector).Select(x=>x/1e-9).ToList();

            var values = result.OrderBy(selector).Select(x => x.Value).ToList();

            this.gp.Set("style data lines");
            this.gp.HoldOn();
            this.gp.Plot(waveLengths, values, "title 'second der'");
        }
    }
}
