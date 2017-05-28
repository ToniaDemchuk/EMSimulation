using System;

using Simulation.Medium.Models;
using Simulation.Models.Constants;
using Simulation.Models.Coordinates;
using Simulation.Models.Spectrum;

namespace Simulation.FDTD.Models
{
    /// <summary>
    /// The SimulationParameters class.
    /// </summary>
    public class SimulationParameters
    {
        /// <summary>
        /// Gets or sets the spectrum.
        /// </summary>
        /// <value>
        /// The spectrum.
        /// </value>
        public OpticalSpectrum Spectrum { get; set; }

        /// <summary>
        /// Gets or sets the medium.
        /// </summary>
        /// <value>
        /// The medium.
        /// </value>
        public IMediumSolver[,,] Medium { get; set; }

        /// <summary>
        /// Gets or sets the indices.
        /// </summary>
        /// <value>
        /// The indices.
        /// </value>
        public IndexStore Indices { get; set; }

        /// <summary>
        /// Gets or sets the size of the cell.
        /// </summary>
        /// <value>
        /// The size of the cell.
        /// </value>
        public double CellSize { get; set; }

        public double TimeStep {
            get
            {
                if (!this.timeStep.HasValue)
                {
                    this.timeStep = this.CellSize * this.CourantNumber / (Fundamentals.SpeedOfLight);
                }
                return this.timeStep.Value;
            }
        }

        /// <summary>
        /// Gets or sets the lazy timeStep.
        /// </summary>
        /// <value>
        /// The lazy timeStep.
        /// </value>
        private double? timeStep;

        public int NumSteps { get; set; }

        /// <summary>
        /// Gets or sets the length of the PML.
        /// </summary>
        /// <value>
        /// The length of the PML.
        /// </value>
        public int PmlLength { get; set; }

        /// <summary>
        /// Gets or sets the courant number.
        /// </summary>
        /// <value>
        /// The courant number.
        /// </value>
        public double CourantNumber { get; set; }

        /// <summary>
        /// Gets or sets the wave function.
        /// </summary>
        /// <value>
        /// The wave function.
        /// </value>
        public Func<int, CartesianCoordinate> WaveFunc { get; set; }

        public bool IsSpectrumCalculated { get; set; }

    }
}