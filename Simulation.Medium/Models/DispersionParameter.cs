using System.Collections.Concurrent;
using System.Numerics;

using Simulation.Models.Coordinates;
using Simulation.Models.Spectrum;

namespace Simulation.Medium.Models
{
    /// <summary>
    /// The DispersionParameter class.
    /// </summary>
    public class DispersionParameter
    {
        /// <summary>
        /// Gets or sets the spectrum parameter.
        /// </summary>
        /// <value>
        /// The spectrum parameter.
        /// </value>
        public SpectrumUnit SpectrumParameter { get; set; }

        /// <summary>
        /// Gets or sets the permittivity at specified spectrum parameter.
        /// </summary>
        /// <value>
        /// The permittivity.
        /// </value>
        public Complex Permittivity { get; set; }

        /// <summary>
        /// Gets or sets the index of the medium refractive.
        /// </summary>
        /// <value>
        /// The index of the medium refractive.
        /// </value>
        public double MediumRefractiveIndex { get; set; }

        /// <summary>
        /// Gets or sets the wave vector.
        /// </summary>
        /// <value>
        /// The wave vector.
        /// </value>
        public CartesianCoordinate WaveVector { get; set; }
    }
}
