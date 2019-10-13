using System.Linq;
using AwokeKnowing.GnuplotCSharp;
using Simulation.Infrastructure.Models;

namespace Simulation.Infrastructure.Plotters
{
    /// <summary>
    /// The plotter for medium
    /// </summary>
    public class MediumPlotter
    {
        private readonly GnuPlot gp;
        
        public MediumPlotter()
        {
            this.gp = new GnuPlot(@"C:\Program Files\gnuplot\bin\gnuplot.exe");
        }

        public void Plot(MeshInfo meshInfo)
        {
            var voxels = meshInfo.Voxels;
            this.gp.Set(string.Format("xrange [0:{0}]", meshInfo.Resolution.I));
            this.gp.Set(string.Format("yrange [0:{0}]", meshInfo.Resolution.J));
            this.gp.Set(string.Format("zrange [0:{0}]", meshInfo.Resolution.K));
            this.gp.HoldOn();

            this.gp.Unset("key");

            var groups = voxels.GroupBy(x => x.Material);
            int pointIndex = 0;
            foreach (var group in groups)
            {
                this.gp.SPlot(
                group.Select(x => (double)x.I).ToArray(),
                group.Select(x => (double)x.J).ToArray(),
                group.Select(x => (double)x.K).ToArray(),
                string.Format("with points pt {0}", ++pointIndex));
            }

        }
    }
}
