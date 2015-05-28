using System.Xml.Serialization;

using Simulation.Models;

namespace Simulation.DDA.Console
{
    [XmlRoot]
    public class DDAParameters
    {
        /// <remarks/>
        public SphericalCoordinate WavePropagation { get; set; }

        public SphericalCoordinate IncidentMagnitude { get; set; }

        /// <remarks/>
        public WaveLengthElement WaveLengthConfig { get; set; }
    }
}
