using Simulation.Models.Matrices;
using Simulation.Models.Calculators;

namespace Simulation.Models.Coordinates
{
    public abstract class BaseDyadCoordinate<T, TC> : IMatrix<T>
        where T: struct
        where TC: ICalculator<T>, new()
    {
        public const int DyadLength = 3;

        private static readonly TC Calculator = new TC();

        public int Length
        {
            get
            {
                return DyadLength;
            }
        }

        /// <summary>
        /// Implements the operator * (Multiply dyad on the value).
        /// </summary>
        /// <param name="point1">The point1.</param>
        /// <param name="num">The number.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static BaseDyadCoordinate<T, TC> operator *(
            BaseDyadCoordinate<T, TC> point1,
            T num)
        {
            var newDyad =
                new T[DyadLength, DyadLength];

            for (int i = 0; i < DyadLength; i++)
            {
                for (int j = 0; j < DyadLength; j++)
                {
                    T a = point1[i, j];
                    newDyad[i, j] = Calculator.Multiply(ref a, ref num);
                }
            }

            return new DyadCoordinate<T, TC>(newDyad);
        }

        /// <summary>
        /// Implements the operator * (Multiply dyad on the value).
        /// </summary>
        /// <param name="num">The number.</param>
        /// <param name="point1">The point1.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static BaseDyadCoordinate<T, TC> operator *(
            T num,
            BaseDyadCoordinate<T, TC> point1)
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
        public static BaseDyadCoordinate<T, TC> operator -(
            BaseDyadCoordinate<T, TC> point1,
            BaseDyadCoordinate<T, TC> point2)
        {
            var newDyad =
                new T[DyadLength, DyadLength];

            for (int i = 0; i < DyadLength; i++)
            {
                for (int j = 0; j < DyadLength; j++)
                {
                    T a = point1[i, j];
                    T b = point2[i, j];
                    newDyad[i, j] = Calculator.Substract(ref a, ref b);
                }
            }

            return new DyadCoordinate<T, TC>(newDyad);
        }

        /// <summary>
        /// Implements the operator - (Subtract two dyads).
        /// </summary>
        /// <param name="point1">The point1.</param>
        /// <param name="point2">The point2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static BaseDyadCoordinate<T, TC> operator +(
            BaseDyadCoordinate<T, TC> point1,
            BaseDyadCoordinate<T, TC> point2)
        {
            var newDyad =
                new T[DyadLength, DyadLength];

            for (int i = 0; i < DyadLength; i++)
            {
                for (int j = 0; j < DyadLength; j++)
                {
                    T a = point1[i, j];
                    T b = point2[i, j];
                    newDyad[i, j] = Calculator.Add(ref a, ref b);
                }
            }

            return new DyadCoordinate<T, TC>(newDyad);
        }

        public abstract T this[int i, int j] { get; }
    }
}
