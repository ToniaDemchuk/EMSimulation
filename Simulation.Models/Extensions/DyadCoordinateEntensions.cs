using System.Numerics;

namespace Simulation.Models
{
    /// <summary>
    /// The CartesianCoordinate class.
    /// </summary>
    public class DyadCoordinateEntensions
    {
        /// <summary>
        /// The dyad length
        /// </summary>
        public const int DyadLength = 3;

        /// <summary>
        /// Dyads the product.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns></returns>
        public static DyadCoordinate<T> DyadProduct<T>(BaseCoordinate<T> a, BaseCoordinate<T> b)
        {
            return new DyadCoordinate<T>(
                (dynamic)a.X * b.X,
                (dynamic)a.X * b.Y,
                (dynamic)a.X * b.Z,
                (dynamic)a.Y * b.X,
                (dynamic)a.Y * b.Y,
                (dynamic)a.Y * b.Z,
                (dynamic)a.Z * b.X,
                (dynamic)a.Z * b.Y,
                (dynamic)a.Z * b.Z);
        }
    }
}
