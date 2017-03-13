using System;
using System.Collections.Generic;

using System.Diagnostics;
using System.Linq;


using AwokeKnowing.GnuplotCSharp;

using GnuplotCSharp;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Simulation.FDTD.Console;
using Simulation.Models.Extensions;
using Simulation.Infrastructure.Iterators;

namespace Simulation.FDTD.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Stopwatch watch = new Stopwatch();

            var gp = new GnuPlot();
            gp.HoldOn();
            for (int j = 0; j < 5; j++)
            {
                List<TimeSpan> list = new List<TimeSpan>();

                for (var i = 1; i <= 16; i++)
                {
                    watch.Start();
                    ParallelIterator.MaxDegreeOfParallelism = i;

                    FDTDProgram.Calculate();
                    watch.Stop();
                    list.Add(watch.Elapsed);
                    watch.Reset();
                }

                gp.Plot(list.Select(x => (double) x.Milliseconds));
            }
            gp.Wait();



        }
    }
}
