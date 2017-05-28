using System;
using System.Collections.Generic;
using System.Linq;
using AwokeKnowing.GnuplotCSharp;
using GnuplotCSharp;
using Simulation.Models.Enums;
using Simulation.Models.Spectrum;

namespace Simulation.Infrastructure.Plotters
{
    /// <summary>
    /// The plotter for spectrum.
    /// </summary>
    public class IncidentPlotter
    {
        private readonly GnuPlot gp;

        public IncidentPlotter()
        {
            this.gp = new GnuPlot();
        }

        /// <summary>
        /// Plots the specified result.
        /// </summary>
        /// <param name="result">The result.</param>
        public void Plot(SimulationResultDictionary result)
        {
            Func<KeyValuePair<SpectrumUnit, SimulationResult>, double> selector = x => x.Key.ToType(SpectrumUnitType.WaveLength);
            var waveLengths = result.Select(selector).Select(x=>x/1e-9).ToList();

            var values = result.OrderBy(selector).Select(x => x.Value).ToList();

            var point = result.First().Value.ElectricField.Length / 4;
            this.gp.Set("style data lines");
            this.gp.HoldOn();
            this.gp.Plot(waveLengths, values.Select(x => x.ElectricField[point].Norm), "title 'inc norm'");
            this.gp.Plot(waveLengths, values.Select(x => x.ElectricField[point].X.Real), "title 'inc x real'");
            this.gp.Plot(waveLengths, values.Select(x => x.ElectricField[point].X.Imaginary), "title 'inc x imag'");
            this.gp.Plot(waveLengths, values.Select(x => x.ElectricField[point].Y.Real), "title 'inc y real'");
            this.gp.Plot(waveLengths, values.Select(x => x.ElectricField[point].Y.Imaginary), "title 'inc y imag'");
            this.gp.Plot(waveLengths, values.Select(x => x.ElectricField[point].Z.Real), "title 'inc y real'");
            this.gp.Plot(waveLengths, values.Select(x => x.ElectricField[point].Z.Imaginary), "title 'inc y imag'");

            this.gp.Plot(waveLengths, values.Select(x => x.Polarization[point].Norm), "title 'pol norm'");
            this.gp.Plot(waveLengths, values.Select(x => x.Polarization[point].X.Real), "title 'pol x real'");
            this.gp.Plot(waveLengths, values.Select(x => x.Polarization[point].X.Imaginary), "title 'pol x imag'");
            this.gp.Plot(waveLengths, values.Select(x => x.Polarization[point].Y.Real), "title 'pol y real'");
            this.gp.Plot(waveLengths, values.Select(x => x.Polarization[point].Y.Imaginary), "title 'pol y imag'");
            this.gp.Plot(waveLengths, values.Select(x => x.Polarization[point].Z.Real), "title 'pol y real'");
            this.gp.Plot(waveLengths, values.Select(x => x.Polarization[point].Z.Imaginary), "title 'pol y imag'");

            this.gp.Wait();
        }
    }
}
