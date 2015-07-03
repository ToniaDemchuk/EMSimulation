using System;

namespace Simulation.Models.Coordinates
{
    /// <summary>
    /// The CartesianCoordinate class.
    /// </summary>
    public class CartesianCoordinate : BaseCoordinate<double>
    {
        public static readonly CartesianCoordinate One = new CartesianCoordinate(1, 1, 1);
        public static readonly CartesianCoordinate Zero = new CartesianCoordinate(0, 0, 0);
        public static readonly CartesianCoordinate XOrt = new CartesianCoordinate(1, 0, 0);
        public static readonly CartesianCoordinate YOrt = new CartesianCoordinate(0, 1, 0);
        public static readonly CartesianCoordinate ZOrt = new CartesianCoordinate(0, 0, 1);

        /// <summary>
        /// Initializes a new instance of the <see cref="CartesianCoordinate" /> class.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="z">The z coordinate.</param>
        public CartesianCoordinate(double x, double y, double z)
            : base(x, y, z)
        {
            this.LazyNorm = new Lazy<double>(() => Math.Sqrt(this.X * this.X + this.Y * this.Y + this.Z * this.Z));
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
            double x = point1.X + point2.X;
            double y = point1.Y + point2.Y;
            double z = point1.Z + point2.Z;

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
            double x = point1.X + num;
            double y = point1.Y + num;
            double z = point1.Z + num;

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
            double x = point1.X - point2.X;
            double y = point1.Y - point2.Y;
            double z = point1.Z - point2.Z;

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
            double x = point1.X - num;
            double y = point1.Y - num;
            double z = point1.Z - num;

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
            double x = -point1.X;
            double y = -point1.Y;
            double z = -point1.Z;

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
            double x = point1.X * num;
            double y = point1.Y * num;
            double z = point1.Z * num;

            return new CartesianCoordinate(x, y, z);
        }

        /// <summary>
        /// Implements the operator * (Multiply coordinates on number).
        /// </summary>
        /// <param name="num">The number.</param>
        /// <param name="point1">The point1.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static CartesianCoordinate operator *(double num, CartesianCoordinate point1)
        {
            return point1 * num;
        }

        /// <summary>
        /// Implements the operator / (Divide point to the number).
        /// </summary>
        /// <param name="point1">The point1.</param>
        /// <param name="num">The number.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static CartesianCoordinate operator /(CartesianCoordinate point1, double num)
        {
            double x = point1.X / num;
            double y = point1.Y / num;
            double z = point1.Z / num;

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
