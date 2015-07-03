using Simulation.Models.Coordinates;

namespace Simulation.Models.Extensions
{
    /// <summary>
    /// The CartesianCoordinate class.
    /// </summary>
    public static class CoordinateEntensions
    {
        /// <summary>
        /// Calculates the dyad product.
        /// </summary>
        /// <typeparam name="T">The type of coordinate values.</typeparam>
        /// <param name="point1">The point1.</param>
        /// <param name="point2">The point2.</param>
        /// <returns>The result of dyad product operation.</returns>
        public static DyadCoordinate<T> DyadProduct<T>(this BaseCoordinate<T> point1, BaseCoordinate<T> point2) where T : struct
        {
            return new DyadCoordinate<T>(
                (dynamic)point1.X * point2.X,
                (dynamic)point1.X * point2.Y,
                (dynamic)point1.X * point2.Z,
                (dynamic)point1.Y * point2.X,
                (dynamic)point1.Y * point2.Y,
                (dynamic)point1.Y * point2.Z,
                (dynamic)point1.Z * point2.X,
                (dynamic)point1.Z * point2.Y,
                (dynamic)point1.Z * point2.Z);
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
