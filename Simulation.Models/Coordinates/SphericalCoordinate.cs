using System;
using System.Xml.Serialization;

namespace Simulation.Models
{
    /// <summary>
    /// The EulerAngles class.
    /// </summary>
    [Serializable]
    public class SphericalCoordinate
    {
        public SphericalCoordinate()
        {
            this.Radius = 1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SphericalCoordinate" /> class with units of measurement.
        /// </summary>
        /// <param name="radius">The radius.</param>
        /// <param name="polar">The polar.</param>
        /// <param name="azimuth">The azimuth.</param>
        /// <param name="unit">The unit of measurement.</param>
        public SphericalCoordinate(double radius, double polar, double azimuth, UnitOfMeasurement unit)
        {
            this.Radius = radius;
            this.Polar = polar;
            this.Azimuth = azimuth;
            this.Units = unit;
        }

        /// <summary>
        /// Gets the units of measurements.
        /// </summary>
        /// <value>
        /// The units of measurements.
        /// </value>
        [XmlAttribute("units")]
        public UnitOfMeasurement Units { get; set; }

        /// <summary>
        /// Gets the polar angle.
        /// </summary>
        /// <value>
        /// The polar angle.
        /// </value>
        [XmlAttribute("radius")]
        public double Radius { get; set; }

        /// <summary>
        /// Gets the theta angle.
        /// </summary>
        /// <value>
        /// The theta angle.
        /// </value>
        [XmlAttribute("polar")]
        public double Polar { get; set; }

        /// <summary>
        /// Gets the psi angle.
        /// </summary>
        /// <value>
        /// The psi angle.
        /// </value>
        [XmlAttribute("azimuth")]
        public double Azimuth { get; set; }
    }
}
