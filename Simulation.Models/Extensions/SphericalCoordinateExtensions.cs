using System;
using System.ComponentModel;

namespace Simulation.Models
{
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
            return new CartesianCoordinate(
                spherical.Radius,
                Math.Sin(spherical.Polar) * Math.Cos(spherical.Azimuth),
                Math.Sin(spherical.Polar) * Math.Sin(spherical.Azimuth),
                Math.Cos(spherical.Polar));
        }

        /// <summary>
        /// Converts angles to radians.
        /// </summary>
        /// <returns>New angles in radians.</returns>
        /// <exception cref="System.ComponentModel.InvalidEnumArgumentException">Throws if unit do not support conversion.</exception>
        public static SphericalCoordinate ConvertToRadians(this SphericalCoordinate coordinate)
        {
            switch (coordinate.Units)
            {
                case UnitOfMeasurement.Radian:
                    return coordinate;
                case UnitOfMeasurement.Degree:
                    double polar = coordinate.Polar * RadianTransformationCoefficient;
                    double azimuth = coordinate.Azimuth * RadianTransformationCoefficient;

                    return new SphericalCoordinate(coordinate.Radius, polar, azimuth);
                default:
                    throw new InvalidEnumArgumentException(string.Format("Cannot convert from unit {0}", coordinate.Units));
            }
        }

        /// <summary>
        /// Converts to degrees.
        /// </summary>
        /// <returns>New angles in radians.</returns>
        /// <exception cref="System.ComponentModel.InvalidEnumArgumentException">Throws if unit do not support conversion.</exception>
        public static SphericalCoordinate ConvertToDegrees(this SphericalCoordinate coordinate)
        {
            switch (coordinate.Units)
            {
                case UnitOfMeasurement.Degree:
                    return coordinate;
                case UnitOfMeasurement.Radian:
                    double polar = coordinate.Polar / RadianTransformationCoefficient;
                    double azimuth = coordinate.Azimuth / RadianTransformationCoefficient;

                    return new SphericalCoordinate(coordinate.Radius, polar, azimuth);

                default:
                    throw new InvalidEnumArgumentException(string.Format("Cannot convert from unit {0}", coordinate.Units));
            }
        }
    }
}