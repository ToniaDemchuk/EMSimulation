using System.Numerics;

using Simulation.Models.Extensions;

namespace Simulation.Models
{
    /// <summary>
    /// The CartesianCoordinate class.
    /// </summary>
    /// <typeparam name="T">The type of underlying value.</typeparam>
    public class DyadCoordinate<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CartesianCoordinate" /> class.
        /// </summary>
        /// <param name="xx">The xx.</param>
        /// <param name="xy">The xy.</param>
        /// <param name="xz">The xz.</param>
        /// <param name="yx">The yx.</param>
        /// <param name="yy">The yy.</param>
        /// <param name="yz">The yz.</param>
        /// <param name="zx">The zx.</param>
        /// <param name="zy">The zy.</param>
        /// <param name="zz">The zz.</param>
        public DyadCoordinate(T xx, T xy, T xz, T yx, T yy, T yz, T zx, T zy, T zz)
        {
            this.Dyad = new T[CoordinateEntensions.DyadLength,
                CoordinateEntensions.DyadLength]
            {
                { xx, xy, xz },
                { yx, yy, yz },
                { zx, zy, zz }
            };
        }

        public DyadCoordinate(T initValue)
        {
            this.Dyad = new T[CoordinateEntensions.DyadLength,
                CoordinateEntensions.DyadLength]
            {
                { initValue, (dynamic)0, (dynamic)0 },
                { (dynamic)0, initValue, (dynamic)0 },
                { (dynamic)0, (dynamic)0, initValue }
            };

        }

        protected DyadCoordinate(T[,] dyad)
        {
            this.Dyad = dyad;
        }

        public T[,] Dyad { get; protected set; }

        public static DyadCoordinate<T> operator *(
            DyadCoordinate<T> point1,
            T num)
        {
            var newDyad =
                new T[CoordinateEntensions.DyadLength,
                    CoordinateEntensions.DyadLength];

            for (int i = 0; i < CoordinateEntensions.DyadLength; i++)
            {
                for (int j = 0; j < CoordinateEntensions.DyadLength; j++)
                {
                    newDyad[i, j] = (dynamic)point1.Dyad[i, j] * num;
                }
            }

            return new DyadCoordinate<T>(newDyad);
        }

        public static DyadCoordinate<T> operator *(
            T num,
            DyadCoordinate<T> point1)
        {
            return point1 * num;
        }

        /// <summary>
        /// Implements the operator - (Subtract two points).
        /// </summary>
        /// <param name="point1">The point1.</param>
        /// <param name="point2">The point2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static DyadCoordinate<T> operator -(
            DyadCoordinate<T> point1,
            DyadCoordinate<T> point2)
        {
            var newDyad =
                new T[CoordinateEntensions.DyadLength,
                    CoordinateEntensions.DyadLength];

            for (int i = 0; i < CoordinateEntensions.DyadLength; i++)
            {
                for (int j = 0; j < CoordinateEntensions.DyadLength; j++)
                {
                    newDyad[i, j] = (dynamic)point1.Dyad[i, j] - point2.Dyad[i, j];
                }
            }

            return new DyadCoordinate<T>(newDyad);
        }

        /// <summary>
        /// Implements the operator - (Subtract two points).
        /// </summary>
        /// <param name="point1">The point1.</param>
        /// <param name="point2">The point2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static DyadCoordinate<T> operator +(
            DyadCoordinate<T> point1,
            DyadCoordinate<T> point2)
        {
            var newDyad =
                new T[CoordinateEntensions.DyadLength,
                    CoordinateEntensions.DyadLength];

            for (int i = 0; i < CoordinateEntensions.DyadLength; i++)
            {
                for (int j = 0; j < CoordinateEntensions.DyadLength; j++)
                {
                    newDyad[i, j] = (dynamic)point1.Dyad[i, j] + point2.Dyad[i, j];
                }
            }

            return new DyadCoordinate<T>(newDyad);
        }

        public static implicit operator DyadCoordinate<Complex>(DyadCoordinate<T> value)
        {
            var newDyad = new Complex[CoordinateEntensions.DyadLength,
                CoordinateEntensions.DyadLength];

            for (int i = 0; i < CoordinateEntensions.DyadLength; i++)
            {
                for (int j = 0; j < CoordinateEntensions.DyadLength; j++)
                {
                    newDyad[i, j] = (dynamic)value.Dyad[i, j];
                }
            }

            return new DyadCoordinate<Complex>(newDyad);
        }
    }
}
