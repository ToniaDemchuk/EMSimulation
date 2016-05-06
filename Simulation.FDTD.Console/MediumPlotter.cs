using System.Linq;

using AwokeKnowing.GnuplotCSharp;

using Simulation.Infrastructure.Models;

namespace Simulation.FDTD.Console
{
    /// <summary>
    /// The plotter for medium
    /// </summary>
    public class MediumPlotter
    {
        private readonly GnuPlot gp;
        
        public MediumPlotter()
        {
            this.gp = new GnuPlot();
        }

        public void Plot(MeshInfo meshInfo)
        {
            var voxels = meshInfo.Voxels;
            gp.Set(string.Format("xrange [0:{0}]", meshInfo.Resolution.I));
            gp.Set(string.Format("yrange [0:{0}]", meshInfo.Resolution.J));
            gp.Set(string.Format("zrange [0:{0}]", meshInfo.Resolution.K));
            gp.HoldOn();

            gp.Unset("key");

            var groups = voxels.GroupBy(x => x.Material);
            int pointIndex = 0;
            foreach (var @group in groups)
            {
                gp.SPlot(
                @group.Select(x => (double)x.I).ToArray(),
                @group.Select(x => (double)x.J).ToArray(),
                @group.Select(x => (double)x.K).ToArray(),
                string.Format("with points pt {0}", ++pointIndex));
            }

        }
    }
}
