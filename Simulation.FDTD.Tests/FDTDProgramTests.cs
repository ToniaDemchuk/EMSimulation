using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using AwokeKnowing.GnuplotCSharp;

using GnuplotCSharp;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Simulation.FDTD.Console;

namespace Simulation.FDTD.Tests
{
    [TestClass]
    public class FDTDProgramTests
    {
        [TestMethod]
        public void Calculate_PerformanceMetrics()
        {
            var serialTicks = 192769194.79518071;
            List<double> all = new List<double>();
            var gp = new GnuPlot();
            gp.Set("style data linespoints");
            gp.HoldOn();

           for (int i = 256; i <= 1024; i = i * 2)
            {
                List<long> list = new List<long>();
                for (int j = 0; j < 20; j++)
                {
                    Stopwatch watch = Stopwatch.StartNew();
                    //ArrayExtensions.MaxDegreeOfParallelism = i;

                    FDTDProgram.Calculate();
                    watch.Stop();
                    list.Add(watch.ElapsedTicks);
                }
                all.Add(list.Average());
                
            }

           gp.Plot(all.Select(x => serialTicks/x));

            gp.Wait();
        }
    }
}