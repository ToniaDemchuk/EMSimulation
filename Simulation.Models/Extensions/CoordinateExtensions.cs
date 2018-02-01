using System.Numerics;

using Simulation.Models.Calculators;
using Simulation.Models.Coordinates;

namespace Simulation.Models.Extensions
{
    /// <summary>
    /// The CartesianCoordinate class.
    /// </summary>
    public static class CoordinateExtensions
    {
        /// <summary>
        /// Calculates the dyad product.
        /// </summary>
        /// <param name="point1">The point1.</param>
        /// <param name="point2">The point2.</param>
        /// <returns>The result of dyad product operation.</returns>
        public static BaseDyadCoordinate<Complex, ComplexCalculator> DyadProduct(ref CartesianCoordinate point1, ref CartesianCoordinate point2)
        {
            if (point1.Equals(point2))
            {
                return new SymmetricDyadCoordinate<Complex, ComplexCalculator>(
                    point1.X * point2.X,
                    point1.X * point2.Y,
                    point1.X * point2.Z,
                    point1.Y * point2.Y,
                    point1.Y * point2.Z,
                    point1.Z * point2.Z);
            }

            return new DyadCoordinate<Complex, ComplexCalculator>(
                point1.X * point2.X,
                point1.X * point2.Y,
                point1.X * point2.Z,
                point1.Y * point2.X,
                point1.Y * point2.Y,
                point1.Y * point2.Z,
                point1.Z * point2.X,
                point1.Z * point2.Y,
                point1.Z * point2.Z);
        }

        /// <summary>
        /// Implements Scalar product.
        /// </summary>
        /// <param name="point1">The point1.</param>
        /// <param name="point2">The point2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static double ScalarProduct(this CartesianCoordinate point1, CartesianCoordinate point2)
        {
            return point1.X * point2.X + point1.Y * point2.Y + point1.Z * point2.Z;
        }


        /// <summary>
        /// Implements Scalar product.
        /// </summary>
        /// <param name="point1">The point1.</param>
        /// <param name="point2">The point2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Complex ScalarProduct(this ComplexCoordinate point1, ComplexCoordinate point2)
        {
            return point1.X * Complex.Conjugate(point2.X) + point1.Y * Complex.Conjugate(point2.Y) +
                   point1.Z * Complex.Conjugate(point2.Z);
        }

        /// <summary>
        /// Calculates the vector product.
        /// </summary>
        /// <param name="point1">The point1.</param>
        /// <param name="point2">The point2.</param>
        /// <returns>The result of vector product operation.</returns>
        public static CartesianCoordinate VectorProduct(this CartesianCoordinate point1, CartesianCoordinate point2)
        {
            return new CartesianCoordinate(
                point1.Y * point2.Z - point1.Z * point2.Y,
                point1.Z * point2.X - point1.X * point2.Z,
                point1.X * point2.Y - point1.Y * point2.X);
        }

        /// <summary>
        /// Calculates the vector product.
        /// </summary>
        /// <param name="point1">The point1.</param>
        /// <param name="point2">The point2.</param>
        /// <returns>The result of vector product operation.</returns>
        public static ComplexCoordinate VectorProduct(this ComplexCoordinate point1, ComplexCoordinate point2)
        {
            return new ComplexCoordinate(
                point1.Y * Complex.Conjugate(point2.Z) - point1.Z * Complex.Conjugate(point2.Y),
                point1.Z * Complex.Conjugate(point2.X) - point1.X * Complex.Conjugate(point2.Z),
                point1.X * Complex.Conjugate(point2.Y) - Complex.Conjugate(point1.Y * point2.X));
        }

        /// <summary>
        /// Calculates the curl of the specified field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="i">The i index.</param>
        /// <param name="j">The j index.</param>
        /// <param name="k">The k index.</param>
        /// <param name="shift">The curl shift.</param>
        /// <returns>The result of curl operation.</returns>
        public static CartesianCoordinate Curl(this CartesianCoordinate[,,] field, int i, int j, int k, int shift)
        {
            CartesianCoordinate current = field[i, j, k];
            CartesianCoordinate shiftedI = field[i + shift, j, k];
            CartesianCoordinate shiftedJ = field[i, j + shift, k];
            CartesianCoordinate shiftedK = field[i, j, k + shift];

            double x = current.Z - shiftedJ.Z + shiftedK.Y - current.Y;
            double y = current.X - shiftedK.X + shiftedI.Z - current.Z;
            double z = current.Y - shiftedI.Y + shiftedJ.X - current.X;

            return new CartesianCoordinate(x, y, z);
        }

        /// <summary>
        /// Calculate the component-wise product.
        /// </summary>
        /// <param name="point1">The point1.</param>
        /// <param name="point2">The point2.</param>
        /// <returns>The result of component-wise multiplication.</returns>
        public static CartesianCoordinate ComponentProduct(this CartesianCoordinate point1, CartesianCoordinate point2)
        {
            return new CartesianCoordinate(
                point1.X * point2.X,
                point1.Y * point2.Y,
                point1.Z * point2.Z);
        }
    }
}
