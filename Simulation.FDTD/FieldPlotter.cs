using AwokeKnowing.GnuplotCSharp;

using Simulation.FDTD.EventArgs;
using Simulation.Infrastructure.Iterators;

namespace Simulation.FDTD.Plotters
{
    public class FieldPlotter
    {
        private readonly GnuPlot gp;

        private readonly IIterator iterator;

        public FieldPlotter()
        {
            this.gp = new GnuPlot(@"C:\Program Files\gnuplot\bin\gnuplot.exe");
            this.iterator  = new ParallelIterator();
        }

        public void Plot(object sender, TimeStepCalculatedEventArgs args)
        {
            var surf = new double[args.Parameters.Indices.ILength, args.Parameters.Indices.JLength];

            this.iterator.ForExceptK(
                args.Parameters.Indices,
                (i, j) =>
                {
                    surf[i, j] = args.Fields.E[i, j, args.Parameters.Indices.GetCenter().KLength].Z;
                });
            this.gp.Set("zrange [-1:1]");
            this.gp.SPlot(surf);
        }
    }
}
