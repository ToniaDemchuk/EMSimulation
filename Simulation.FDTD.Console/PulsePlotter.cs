using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AwokeKnowing.GnuplotCSharp;

using Simulation.FDTD.EventArgs;
using Simulation.Infrastructure.Iterators;

namespace Simulation.FDTD.Console
{
    public class PulsePlotter
    {
        private readonly GnuPlot gp;

        public PulsePlotter()
        {
            this.gp = new GnuPlot();
        }

        public void Plot(object sender, TimeStepCalculatedEventArgs args)
        {
            this.gp.Plot(args.Pulse.E);
        }
    }
}
