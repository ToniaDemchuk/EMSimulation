using AwokeKnowing.GnuplotCSharp;
using Simulation.FDTD.EventArgs;

namespace Simulation.FDTD.Plotters
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
