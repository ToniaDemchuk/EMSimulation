using Simulation.Models.Coordinates;
using Simulation.Models.Spectrum;

namespace Simulation.DDA.Models
{
    /// <summary>
    /// The SimulationParameters class.
    /// </summary>
    public class SimulationParameters
    {
        /// <summary>
        /// Gets or sets the system configuration.
        /// </summary>
        /// <value>
        /// The system configuration.
        /// </value>
        public SystemConfig SystemConfig { get; set; }

        /// <summary>
        /// Gets or sets the wave configuration.
        /// </summary>
        /// <value>
        /// The wave configuration.
        /// </value>
        public OpticalSpectrum Spectrum { get; set; }

        /// <summary>
        /// Gets or sets the Euler angles.
        /// </summary>
        /// <value>
        /// The Euler angles.
        /// </value>
        public SphericalCoordinate WavePropagation { get; set; }

        /// <summary>
        /// Gets or sets the incident magnitude.
        /// </summary>
        /// <value>
        /// The incident magnitude.
        /// </value>
        public CartesianCoordinate IncidentMagnitude { get; set; }
    }
}
