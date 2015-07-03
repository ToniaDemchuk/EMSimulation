using System;
using System.Numerics;

namespace Simulation.Models.Coordinates
{
    /// <summary>
    /// The CartesianCoordinate class.
    /// </summary>
    public class ComplexCoordinate : BaseCoordinate<Complex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CartesianCoordinate"/> class.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="z">The z coordinate.</param>
        public ComplexCoordinate(Complex x, Complex y, Complex z)
            : base(x, y, z)
        {
            this.LazyNorm = new Lazy<double>(
                () =>
                {
                    double xmagn = this.X.Magnitude;
                    double ymagn = this.Y.Magnitude;
                    double zmagn = this.Z.Magnitude;
                    return Math.Sqrt(xmagn * xmagn + ymagn * ymagn +
                                     zmagn * zmagn);
                });
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComplexCoordinate"/> class.
        /// </summary>
        /// <param name="point">The cartesian point.</param>
        /// <param name="angle">The angle.</param>
        public ComplexCoordinate(CartesianCoordinate point, double angle)
            : this(Complex.FromPolarCoordinates(point.X, angle), Complex.FromPolarCoordinates(point.Y, angle), Complex.FromPolarCoordinates(point.Z, angle))
        {
        }

        #region Operators overload

        /// <summary>
        /// Implements the operator + (Addition of two points).
        /// </summary>
        /// <param name="point1">The point1.</param>
        /// <param name="point2">The point2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static ComplexCoordinate operator +(ComplexCoordinate point1, ComplexCoordinate point2)
        {
            Complex x = point1.X + point2.X;
            Complex y = point1.Y + point2.Y;
            Complex z = point1.Z + point2.Z;

            return new ComplexCoordinate(x, y, z);
        }

        /// <summary>
        /// Implements the operator + (Add number to the coordinates).
        /// </summary>
        /// <param name="point1">The point1.</param>
        /// <param name="num">The number.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static ComplexCoordinate operator +(ComplexCoordinate point1, Complex num)
        {
            Complex x = point1.X + num;
            Complex y = point1.Y + num;
            Complex z = point1.Z + num;

            return new ComplexCoordinate(x, y, z);
        }

        /// <summary>
        /// Implements the operator + (Add number to the coordinates).
        /// </summary>
        /// <param name="num">The number.</param>
        /// <param name="point1">The point1.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static ComplexCoordinate operator +(Complex num, ComplexCoordinate point1)
        {
            return point1 + num;
        }

        /// <summary>
        /// Implements the operator - (Subtract two points).
        /// </summary>
        /// <param name="point1">The point1.</param>
        /// <param name="point2">The point2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static ComplexCoordinate operator -(ComplexCoordinate point1, ComplexCoordinate point2)
        {
            Complex x = point1.X - point2.X;
            Complex y = point1.Y - point2.Y;
            Complex z = point1.Z - point2.Z;

            return new ComplexCoordinate(x, y, z);
        }

        /// <summary>
        /// Implements the operator - (Subtract number from coordinates).
        /// </summary>
        /// <param name="point1">The point1.</param>
        /// <param name="num">The number.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static ComplexCoordinate operator -(ComplexCoordinate point1, Complex num)
        {
            Complex x = point1.X - num;
            Complex y = point1.Y - num;
            Complex z = point1.Z - num;

            return new ComplexCoordinate(x, y, z);
        }

        /// <summary>
        /// Implements the operator - (Negate coordinates).
        /// </summary>
        /// <param name="point1">The point1.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static ComplexCoordinate operator -(ComplexCoordinate point1)
        {
            Complex x = -point1.X;
            Complex y = -point1.Y;
            Complex z = -point1.Z;

            return new ComplexCoordinate(x, y, z);
        }

        /// <summary>
        /// Implements the operator - (Subtract coordinate from number).
        /// </summary>
        /// <param name="num">The number.</param>
        /// <param name="point1">The point1.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static ComplexCoordinate operator -(Complex num, ComplexCoordinate point1)
        {
            return -point1 + num;
        }

        /// <summary>
        /// Implements the operator * (Multiply coordinates on number).
        /// </summary>
        /// <param name="point1">The point1.</param>
        /// <param name="num">The number.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static ComplexCoordinate operator *(ComplexCoordinate point1, Complex num)
        {
            Complex x = point1.X * num;
            Complex y = point1.Y * num;
            Complex z = point1.Z * num;

            return new ComplexCoordinate(x, y, z);
        }

        /// <summary>
        /// Implements the operator * (Multiply coordinates on number).
        /// </summary>
        /// <param name="num">The number.</param>
        /// <param name="point1">The point1.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static ComplexCoordinate operator *(Complex num, ComplexCoordinate point1)
        {
            return point1 * num;
        }

        /// <summary>
        /// Implements the operator / (Divide coordinates to complex).
        /// </summary>
        /// <param name="point1">The point1.</param>
        /// <param name="num">The number.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static ComplexCoordinate operator /(ComplexCoordinate point1, Complex num)
        {
            Complex x = point1.X / num;
            Complex y = point1.Y / num;
            Complex z = point1.Z / num;

            return new ComplexCoordinate(x, y, z);
        }

        /// <summary>
        /// Implements the operator * (Scalar product).
        /// </summary>
        /// <param name="point1">The point1.</param>
        /// <param name="point2">The point2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Complex operator *(ComplexCoordinate point1, ComplexCoordinate point2)
        {
            return point1.X * Complex.Conjugate(point2.X) + point1.Y * Complex.Conjugate(point2.Y) + point1.Z * Complex.Conjugate(point2.Z);
        }

        #endregion
    }
}
