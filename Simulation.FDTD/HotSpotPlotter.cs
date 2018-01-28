using AwokeKnowing.GnuplotCSharp;

using Simulation.FDTD.EventArgs;
using Simulation.Infrastructure.Iterators;

namespace Simulation.FDTD.Plotters
{
    public class HotSpotPlotter
    {
        private readonly GnuPlot gp;

        private readonly IIterator iterator;

        public HotSpotPlotter()
        {
            this.gp = new GnuPlot();
            this.iterator  = new ParallelIterator();
        }

        public void Plot(object sender, TimeStepCalculatedEventArgs args)
        {
            //this.gp.Set("zrange [-1:1]");
            //this.gp.Set("palette color positive");
            //this.gp.Set("pm3d map");
            foreach (var freq in args.Parameters.Spectrum)
            {
                var surf = new double[args.Parameters.Indices.ILength, args.Parameters.Indices.JLength];

                this.iterator.ForExceptK(
                    args.Parameters.Indices,
                    (i, j) =>
                    {
                        var field = args.Fields.FourierE[i, j, args.Parameters.Indices.GetCenter().KLength];
                        if (field == null)
                        {
                            surf[i, j] = 0;
                            return;
                        }
                        surf[i, j] = field.Transform(freq, args.Parameters.TimeStep).Z.Real;
                    });

                this.gp.HeatMap(surf);
            }


        }
    }
}
