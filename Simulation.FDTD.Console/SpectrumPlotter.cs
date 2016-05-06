using System.Collections.Generic;
using System.Linq;

using AwokeKnowing.GnuplotCSharp;

using Simulation.Infrastructure;
using Simulation.Models.Enums;
using Simulation.Models.Spectrum;
using GnuplotCSharp;

namespace Simulation.FDTD.Console
{
    /// <summary>
    /// The plotter for spectrum.
    /// </summary>
    public class SpectrumPlotter
    {
        private readonly GnuPlot gp;

        public SpectrumPlotter()
        {
            this.gp = new GnuPlot();
        }

        /// <summary>
        /// Plots the specified result.
        /// </summary>
        /// <param name="result">The result.</param>
        public void Plot(SimulationResultDictionary result)
        {
            gp.Set("style data lines");

            gp.Plot(result.Select(x => x.Value.EffectiveCrossSectionAbsorption));

            gp.Wait();
        }
    }
}
