using Simulation.Models.Matrices;
using System.Numerics;

namespace Simulation.Models.Coordinates
{
    public abstract class BaseDyadCoordinate<T> : IMatrix<T> where T: struct
    {
        public const int DyadLength = 3;

        public int Length
        {
            get
            {
                return DyadLength;
            }
        }

        public static implicit operator BaseDyadCoordinate<Complex>(BaseDyadCoordinate<T> value)
        {
            var newDyad = new Complex[DyadLength, DyadLength];

            for (int i = 0; i < DyadLength; i++)
            {
                for (int j = 0; j < DyadLength; j++)
                {
                    newDyad[i, j] = (dynamic)value[i, j];
                }
            }

            return new DyadCoordinate<Complex>(newDyad);
        }

        /// <summary>
        /// Implements the operator * (Multiply dyad on the value).
        /// </summary>
        /// <param name="point1">The point1.</param>
        /// <param name="num">The number.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static BaseDyadCoordinate<T> operator *(
            BaseDyadCoordinate<T> point1,
            T num)
        {
            var newDyad =
                new T[DyadLength, DyadLength];

            for (int i = 0; i < DyadLength; i++)
            {
                for (int j = 0; j < DyadLength; j++)
                {
                    newDyad[i, j] = (dynamic)point1[i, j] * num;
                }
            }

            return new DyadCoordinate<T>(newDyad);
        }

        /// <summary>
        /// Implements the operator * (Multiply dyad on the value).
        /// </summary>
        /// <param name="num">The number.</param>
        /// <param name="point1">The point1.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static BaseDyadCoordinate<T> operator *(
            T num,
            BaseDyadCoordinate<T> point1)
        {
            return point1 * num;
        }

        /// <summary>
        /// Implements the operator - (Subtract two dyads).
        /// </summary>
        /// <param name="point1">The point1.</param>
        /// <param name="point2">The point2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static BaseDyadCoordinate<T> operator -(
            BaseDyadCoordinate<T> point1,
            BaseDyadCoordinate<T> point2)
        {
            var newDyad =
                new T[DyadLength, DyadLength];

            for (int i = 0; i < DyadLength; i++)
            {
                for (int j = 0; j < DyadLength; j++)
                {
                    newDyad[i, j] = (dynamic)point1[i, j] - point2[i, j];
                }
            }

            return new DyadCoordinate<T>(newDyad);
        }

        /// <summary>
        /// Implements the operator - (Subtract two dyads).
        /// </summary>
        /// <param name="point1">The point1.</param>
        /// <param name="point2">The point2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static BaseDyadCoordinate<T> operator +(
            BaseDyadCoordinate<T> point1,
            BaseDyadCoordinate<T> point2)
        {
            var newDyad =
                new T[DyadLength, DyadLength];

            for (int i = 0; i < DyadLength; i++)
            {
                for (int j = 0; j < DyadLength; j++)
                {
                    newDyad[i, j] = (dynamic)point1[i, j] + point2[i, j];
                }
            }

            return new DyadCoordinate<T>(newDyad);
        }

        public abstract T this[int i, int j] { get; }
    }
}
