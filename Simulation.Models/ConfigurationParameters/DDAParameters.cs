using System.Xml.Serialization;

using Simulation.Models;

namespace Simulation.DDA.Console
{
    [XmlRoot]
    public class DDAParameters
    {
        public SphericalCoordinate WavePropagation { get; set; }

        public SphericalCoordinate IncidentMagnitude { get; set; }

        public WaveLengthElement WaveLengthConfig { get; set; }

        public bool SolidMaterial { get; set; }
    }
}
