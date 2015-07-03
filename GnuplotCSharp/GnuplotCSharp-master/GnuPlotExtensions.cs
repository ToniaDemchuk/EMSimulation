using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

using AwokeKnowing.GnuplotCSharp;

namespace GnuplotCSharp
{
    public static class GnuPlotExtensions
    {
        public static void Plot(this GnuPlot gnuplot, IEnumerable<double> y, string options = "")
        {
            gnuplot.Plot(y.ToArray(), options);
        }

        public static void Plot(this GnuPlot gnuplot, IEnumerable<double> x, IEnumerable<double> y, string options = "")
        {
            gnuplot.Plot(x.ToArray(), y.ToArray(), options);
        }

        public static void Plot(this GnuPlot gnuplot, IEnumerable<double> x, IEnumerable<Complex> y, string options = "")
        {
            string realOptions;
            string imagOptions;
            getOptions(out realOptions, options, out imagOptions);
            gnuplot.Plot(x.ToArray(), y.Select(a => a.Real).ToArray(), realOptions);
            gnuplot.Plot(x.ToArray(), y.Select(a => a.Imaginary).ToArray(), imagOptions);
        }

        public static void Plot(this GnuPlot gnuplot, IDictionary<double, double> dict, string options = "")
        {
            gnuplot.Plot(dict.Keys.ToArray(), dict.Values.ToArray(), options);
        }

        public static void Plot(this GnuPlot gnuplot, IDictionary<double, Complex> dict, string options = "")
        {
            string realOptions;
            string imagOptions;
            getOptions(out realOptions, options, out imagOptions);

            gnuplot.Plot(dict.Keys.ToArray(), dict.Values.Select(x => x.Real).ToArray(), realOptions);
            gnuplot.Plot(dict.Keys.ToArray(), dict.Values.Select(x => x.Imaginary).ToArray(), imagOptions);
        }

        private static void getOptions(out string realOptions, string options, out string imagOptions)
        {
            if (string.IsNullOrEmpty(options))
            {
                options = @"title ""{0}""";
            }
            realOptions = string.Format(options, "Re");
            imagOptions = string.Format(options, "Im");
        }
    }
}