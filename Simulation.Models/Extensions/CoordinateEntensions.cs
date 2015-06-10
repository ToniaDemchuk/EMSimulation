using System;
using System.Linq;
using System.Runtime.Remoting.Messaging;

namespace Simulation.Models.Extensions
{
    /// <summary>
    /// The CartesianCoordinate class.
    /// </summary>
    public static class CoordinateEntensions
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
        public static DyadCoordinate<T> DyadProduct<T>(this BaseCoordinate<T> a, BaseCoordinate<T> b)
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

        public static CartesianCoordinate VectorProduct(this CartesianCoordinate a, CartesianCoordinate b)
        {
            return new CartesianCoordinate(
                a.Y * b.Z - a.Z * b.Y,
                a.Z * b.X - a.X * b.Z,
                a.X * b.Y - a.Y * b.X);
        }

        public static CartesianCoordinate Curl(this CartesianCoordinate[, ,] field, int i, int j, int k, int shift)
        {
            CartesianCoordinate current = field[i, j, k];
            CartesianCoordinate shiftedI = field[i + shift, j, k];
            CartesianCoordinate shiftedJ = field[i, j + shift, k];
            CartesianCoordinate shiftedK = field[i, j, k + shift];

            var x = current.Z - shiftedJ.Z + shiftedK.Y - current.Y;
            var y = current.X - shiftedK.X + shiftedI.Z - current.Z;
            var z = current.Y - shiftedI.Y + shiftedJ.X - current.X;

            return new CartesianCoordinate(x, y, z);
        }

        public static CartesianCoordinate ComponentProduct(this CartesianCoordinate a, CartesianCoordinate b)
        {
            return new CartesianCoordinate(
                a.X * b.X,
                a.Y * b.Y,
                a.Z * b.Z);
        }
    }
}
