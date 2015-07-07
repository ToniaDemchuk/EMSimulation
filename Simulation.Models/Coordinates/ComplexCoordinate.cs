using System;
using System.Numerics;

namespace Simulation.Models.Coordinates
{
    /// <summary>
    /// The CartesianCoordinate class.
    /// </summary>
    public struct ComplexCoordinate : ICoordinate<Complex>
    {
        public static readonly ComplexCoordinate One = new ComplexCoordinate(Complex.One, Complex.One, Complex.One);

        public static readonly ComplexCoordinate Zero = new ComplexCoordinate(Complex.Zero, Complex.Zero, Complex.Zero);

        public static readonly ComplexCoordinate XOrt = new ComplexCoordinate(Complex.One, Complex.Zero, Complex.Zero);

        public static readonly ComplexCoordinate YOrt = new ComplexCoordinate(Complex.Zero, Complex.One, Complex.Zero);

        public static readonly ComplexCoordinate ZOrt = new ComplexCoordinate(Complex.Zero, Complex.Zero, Complex.One);

        /// <summary>
        /// Initializes a new instance of the <see cref="CartesianCoordinate"/> class.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="z">The z coordinate.</param>
        public ComplexCoordinate(Complex x, Complex y, Complex z) : this()
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComplexCoordinate" /> class.
        /// </summary>
        /// <param name="point">The cartesian point.</param>
        /// <param name="phase">The phase.</param>
        /// <returns>New instance of ComplexCoordinate.</returns>
        public static ComplexCoordinate FromPolarCoordinates(CartesianCoordinate point, double phase)
        {
            var cos = Math.Cos(phase);
            var sin = Math.Sin(phase);
            return new ComplexCoordinate(
                new Complex(point.X * cos, point.X * sin),
                new Complex(point.Y * cos, point.Y * sin),
                new Complex(point.Z * cos, point.Z * sin));
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
            return point1.X * Complex.Conjugate(point2.X) + point1.Y * Complex.Conjugate(point2.Y) +
                   point1.Z * Complex.Conjugate(point2.Z);
        }

        #endregion

        private double getNorm()
        {
            double xmagn = this.X.Magnitude;
            double ymagn = this.Y.Magnitude;
            double zmagn = this.Z.Magnitude;
            return Math.Sqrt(
                xmagn * xmagn + ymagn * ymagn + zmagn * zmagn);
        }

        public Complex X { get; private set; }

        public Complex Y { get; private set; }

        public Complex Z { get; private set; }

        /// <summary>
        /// Gets or sets the lazy norm.
        /// </summary>
        /// <value>
        /// The lazy norm.
        /// </value>
        double? norm;

        /// <summary>
        /// Gets the norm of the coordinate.
        /// </summary>
        /// <value>
        /// The norm of the coordinate.
        /// </value>
        public double Norm
        {
            get
            {
                if (!this.norm.HasValue)
                {
                    this.norm = this.getNorm();
                }
                return this.norm.Value;
            }
        }
    }
}