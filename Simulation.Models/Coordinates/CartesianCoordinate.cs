using System;

namespace Simulation.Models
{
    /// <summary>
    /// The CartesianCoordinate class.
    /// </summary>
    public class CartesianCoordinate : BaseCoordinate<double>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CartesianCoordinate" /> class.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="z">The z coordinate.</param>
        public CartesianCoordinate(double x, double y, double z)
            : base(x, y, z)
        {
            this.Norm = Math.Sqrt(this.X * this.X + this.Y * this.Y + this.Z * this.Z);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CartesianCoordinate"/> class.
        /// </summary>
        /// <param name="radius">The radius.</param>
        /// <param name="unitX">The unit x coordinate.</param>
        /// <param name="unitY">The unit y coordinate.</param>
        /// <param name="unitZ">The unit z coordinate.</param>
        public CartesianCoordinate(double radius, double unitX, double unitY, double unitZ)
            : base(radius * unitX, radius * unitY, radius * unitZ)
        {
            this.Norm = radius;
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
        public static CartesianCoordinate operator +(CartesianCoordinate point1, CartesianCoordinate point2)
        {
            dynamic x = point1.X + point2.X;
            dynamic y = point1.Y + point2.Y;
            dynamic z = point1.Z + point2.Z;

            return new CartesianCoordinate(x, y, z);
        }

        /// <summary>
        /// Implements the operator + (Add number to the coordinates).
        /// </summary>
        /// <param name="point1">The point1.</param>
        /// <param name="num">The number.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static CartesianCoordinate operator +(CartesianCoordinate point1, double num)
        {
            var x = point1.X + num;
            var y = point1.Y + num;
            var z = point1.Z + num;

            return new CartesianCoordinate(x, y, z);
        }

        /// <summary>
        /// Implements the operator + (Add number to the coordinates).
        /// </summary>
        /// <param name="num">The number.</param>
        /// <param name="point1">The point1.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static CartesianCoordinate operator +(double num, CartesianCoordinate point1)
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
        public static CartesianCoordinate operator -(CartesianCoordinate point1, CartesianCoordinate point2)
        {
            var x = point1.X - point2.X;
            var y = point1.Y - point2.Y;
            var z = point1.Z - point2.Z;

            return new CartesianCoordinate(x, y, z);
        }

        /// <summary>
        /// Implements the operator - (Subtract number from coordinates).
        /// </summary>
        /// <param name="point1">The point1.</param>
        /// <param name="num">The number.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static CartesianCoordinate operator -(CartesianCoordinate point1, double num)
        {
            var x = point1.X - num;
            var y = point1.Y - num;
            var z = point1.Z - num;

            return new CartesianCoordinate(x, y, z);
        }

        /// <summary>
        /// Implements the operator - (Negate coordinates).
        /// </summary>
        /// <param name="point1">The point1.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static CartesianCoordinate operator -(CartesianCoordinate point1)
        {
            var x = -point1.X;
            var y = -point1.Y;
            var z = -point1.Z;

            return new CartesianCoordinate(x, y, z);
        }

        /// <summary>
        /// Implements the operator - (Subtract coordinate from number).
        /// </summary>
        /// <param name="num">The number.</param>
        /// <param name="point1">The point1.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static CartesianCoordinate operator -(double num, CartesianCoordinate point1)
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
        public static CartesianCoordinate operator *(CartesianCoordinate point1, double num)
        {
            var x = point1.X * num;
            var y = point1.Y * num;
            var z = point1.Z * num;

            return new CartesianCoordinate(x, y, z);
        }

        /// <summary>
        /// Implements the operator * (Multiply coordinates on number).
        /// </summary>
        /// <param name="point1">The point1.</param>
        /// <param name="num">The number.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static CartesianCoordinate operator *(double num, CartesianCoordinate point1)
        {
            return point1 * num;
        }

        public static CartesianCoordinate operator /(CartesianCoordinate point1, double num)
        {
            var x = point1.X / num;
            var y = point1.Y / num;
            var z = point1.Z / num;

            return new CartesianCoordinate(x, y, z);
        }

        /// <summary>
        /// Implements the operator * (Scalar product).
        /// </summary>
        /// <param name="point1">The point1.</param>
        /// <param name="point2">The point2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static double operator *(CartesianCoordinate point1, CartesianCoordinate point2)
        {
            return point1.X * point2.X + point1.Y * point2.Y + point1.Z * point2.Z;
        }

        #endregion

    }
}
