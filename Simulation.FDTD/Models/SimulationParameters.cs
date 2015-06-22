using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Simulation.Models;
using Simulation.Models.Coordinates;

namespace Simulation.FDTD.Models
{
    public class SimulationParameters
    {
        /// <summary>
        /// Gets or sets the wave configuration.
        /// </summary>
        /// <value>
        /// The wave configuration.
        /// </value>
        public OpticalSpectrum Spectrum { get; set; }

        public IMediumSolver[,,] Medium { get; set; }

        public IndexStore Indices { get; set; }

        public int cellSize { get; set; }

        public int pmlLength { get; set; }

        public double CourantNumber { get; set; }

        public Func<double, double> waveFunc { get; set; }
    }
}