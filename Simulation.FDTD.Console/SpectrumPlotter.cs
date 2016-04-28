using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AwokeKnowing.GnuplotCSharp;

using Simulation.FDTD.EventArgs;
using Simulation.Infrastructure;
using Simulation.Infrastructure.Iterators;
using Simulation.Models.Enums;
using Simulation.Models.Spectrum;
using GnuplotCSharp;

namespace Simulation.FDTD.Console
{
    public class SpectrumPlotter
    {
        private readonly GnuPlot gp;

        public SpectrumPlotter()
        {
            this.gp = new GnuPlot();
        }

        public void Plot(SimulationResultDictionary result)
        {
            gp.Set("style data lines");

            gp.Plot(result.Select(x => x.Value.EffectiveCrossSectionAbsorption));
            SimpleFormatter.Write(
               "rezult_ext.txt",
               result.ToDictionary(
                   x => x.Key.ToType(SpectrumUnitType.WaveLength),
                   x => new List<double>() { x.Value.CrossSectionAbsorption, x.Value.EffectiveCrossSectionAbsorption }));
            gp.Wait();
        }
    }
}
