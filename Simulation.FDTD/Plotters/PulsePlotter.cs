using AwokeKnowing.GnuplotCSharp;
using Simulation.FDTD.EventArgs;
using Simulation.Models.Coordinates;
using System.Linq;

namespace Simulation.FDTD.Plotters
{
    public class PulsePlotter
    {
        private readonly GnuPlot gp;

        public PulsePlotter()
        {
            this.gp = new GnuPlot(@"C:\Program Files\gnuplot\bin\gnuplot.exe");
        }

        public void Plot(object sender, TimeStepCalculatedEventArgs args)
        {
            this.gp.Plot(args.Pulse.E.Select(coord => coord.Z).ToArray());
        }
    }
}
