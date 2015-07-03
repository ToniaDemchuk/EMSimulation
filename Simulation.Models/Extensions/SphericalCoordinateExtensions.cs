using System;
using System.ComponentModel;

using Simulation.Models.Coordinates;
using Simulation.Models.Enums;

namespace Simulation.Models.Extensions
{
    /// <summary>
    /// The SphericalCoordinateExtensions class.
    /// </summary>
    public static class SphericalCoordinateExtensions
    {
        /// <summary>
        /// The radian transformation coefficient
        /// </summary>
        public static readonly double RadianTransformationCoefficient = Math.PI / 180.0;

        /// <summary>
        /// Creates the Cartesian coordinate from Spherical coordinates.
        /// </summary>
        /// <param name="spherical">The spherical.</param>
        /// <returns>
        /// The Cartesian coordinate.
        /// </returns>
        public static CartesianCoordinate ConvertToCartesian(this SphericalCoordinate spherical)
        {
            var sphericalRad = spherical.ToRadians();

            double radsin = sphericalRad.Radius * Math.Sin(sphericalRad.Polar);

            return new CartesianCoordinate(
                radsin * Math.Cos(sphericalRad.Azimuth),
                radsin * Math.Sin(sphericalRad.Azimuth),
                sphericalRad.Radius * Math.Cos(sphericalRad.Polar));
        }

        /// <summary>
        /// Converts angles to radians.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>
        /// New angles in radians.
        /// </returns>
        /// <exception cref="System.ComponentModel.InvalidEnumArgumentException">Throws if unit do not support conversion.</exception>
        public static SphericalCoordinate ToRadians(this SphericalCoordinate coordinate)
        {
            switch (coordinate.Units)
            {
                case UnitOfMeasurement.Radian:
                    return coordinate;
                case UnitOfMeasurement.Degree:
                    double polar = coordinate.Polar * RadianTransformationCoefficient;
                    double azimuth = coordinate.Azimuth * RadianTransformationCoefficient;

                    return new SphericalCoordinate(coordinate.Radius, polar, azimuth, UnitOfMeasurement.Radian);
                default:
                    throw new InvalidEnumArgumentException(string.Format("Cannot convert from unit {0}", coordinate.Units));
            }
        }

        /// <summary>
        /// Converts to degrees.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>
        /// New angles in radians.
        /// </returns>
        /// <exception cref="System.ComponentModel.InvalidEnumArgumentException">Throws if unit do not support conversion.</exception>
        public static SphericalCoordinate ToDegrees(this SphericalCoordinate coordinate)
        {
            switch (coordinate.Units)
            {
                case UnitOfMeasurement.Degree:
                    return coordinate;
                case UnitOfMeasurement.Radian:
                    double polar = coordinate.Polar / RadianTransformationCoefficient;
                    double azimuth = coordinate.Azimuth / RadianTransformationCoefficient;

                    return new SphericalCoordinate(coordinate.Radius, polar, azimuth, UnitOfMeasurement.Degree);

                default:
                    throw new InvalidEnumArgumentException(string.Format("Cannot convert from unit {0}", coordinate.Units));
            }
        }
    }
}