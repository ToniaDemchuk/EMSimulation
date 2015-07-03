using System.Xml.Serialization;

using Simulation.Models.Common;
using Simulation.Models.Coordinates;

namespace Simulation.DDA.Models
{
    /// <summary>
    /// The DDAParameters class.
    /// </summary>
    [XmlRoot]
    public class DDAParameters
    {
        /// <summary>
        /// Gets or sets the wave propagation direction.
        /// </summary>
        /// <value>
        /// The wave propagation direction.
        /// </value>
        public SphericalCoordinate WavePropagation { get; set; }

        /// <summary>
        /// Gets or sets the incident magnitude.
        /// </summary>
        /// <value>
        /// The incident magnitude.
        /// </value>
        public SphericalCoordinate IncidentMagnitude { get; set; }

        /// <summary>
        /// Gets or sets the wave length configuration.
        /// </summary>
        /// <value>
        /// The wave length configuration.
        /// </value>
        public LinearDiscreteElement WaveLengthConfig { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether material is solid.
        /// </summary>
        /// <value>
        ///   <c>true</c> if material is solid; otherwise, <c>false</c>.
        /// </value>
        public bool IsSolidMaterial { get; set; }
    }
}
