using Simulation.Models.Calculators;

namespace Simulation.Models.Coordinates
{
    /// <summary>
    /// The CartesianCoordinate class.
    /// </summary>
    /// <typeparam name="T">The type of underlying value.</typeparam>
    /// <typeparam name="TC">Calculator for type.</typeparam>
    /// <seealso cref="Coordinates.BaseDyadCoordinate{T, TC}" />
    public class DyadCoordinate<T, TC>: BaseDyadCoordinate<T, TC>
        where T : struct
        where TC : ICalculator<T>, new()
    {
        private static readonly TC Calculator = new TC();

        /// <summary>
        /// Initializes a new instance of the <see cref="CartesianCoordinate" /> class.
        /// </summary>
        /// <param name="xx">The x-x value.</param>
        /// <param name="xy">The x-y value.</param>
        /// <param name="xz">The x-z value.</param>
        /// <param name="yx">The y-x value.</param>
        /// <param name="yy">The y-y value.</param>
        /// <param name="yz">The y-z value.</param>
        /// <param name="zx">The z-x value.</param>
        /// <param name="zy">The z-y value.</param>
        /// <param name="zz">The z-z value.</param>
        public DyadCoordinate(T xx, T xy, T xz, T yx, T yy, T yz, T zx, T zy, T zz)
        {
            this.Dyad = new[,]
            {
                { xx, xy, xz },
                { yx, yy, yz },
                { zx, zy, zz }
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DyadCoordinate{T, TC}"/> class.
        /// </summary>
        /// <param name="initValue">The value on the main diagonal of identity matrix.</param>
        public DyadCoordinate(T initValue)
        {
            this.Dyad = new[,]
            {
                { initValue, default(T), default(T) },
                { default(T), initValue, default(T) },
                { default(T), default(T), initValue }
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DyadCoordinate{T, TC}"/> class.
        /// </summary>
        /// <param name="dyad">The dyad matrix.</param>
        public DyadCoordinate(T[,] dyad)
        {
            this.Dyad = dyad;
        }

        /// <summary>
        /// Gets or sets the dyad values.
        /// </summary>
        /// <value>
        /// The dyad values.
        /// </value>
        public T[,] Dyad { get; private set; }

        /// <summary>
        /// Implements the operator * (Multiply dyad on the value).
        /// </summary>
        /// <param name="point1">The point1.</param>
        /// <param name="num">The number.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static DyadCoordinate<T, TC> operator *(
            DyadCoordinate<T, TC> point1,
            T num)
        {
            var newDyad =
                new T[DyadLength, DyadLength];

            for (int i = 0; i < DyadLength; i++)
            {
                for (int j = 0; j < DyadLength; j++)
                {
                    newDyad[i, j] = Calculator.Multiply(ref point1.Dyad[i, j], ref num);
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
        public static DyadCoordinate<T, TC> operator *(
            T num,
            DyadCoordinate<T, TC> point1)
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
        public static DyadCoordinate<T, TC> operator -(
            DyadCoordinate<T, TC> point1,
            DyadCoordinate<T, TC> point2)
        {
            var newDyad =
                new T[DyadLength, DyadLength];

            for (int i = 0; i < DyadLength; i++)
            {
                for (int j = 0; j < DyadLength; j++)
                {
                    newDyad[i, j] = Calculator.Substract(ref point1.Dyad[i, j], ref point2.Dyad[i, j]);
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
        public static DyadCoordinate<T, TC> operator +(
            DyadCoordinate<T, TC> point1,
            DyadCoordinate<T, TC> point2)
        {
            var newDyad =
                new T[DyadLength, DyadLength];

            for (int i = 0; i < DyadLength; i++)
            {
                for (int j = 0; j < DyadLength; j++)
                {
                    newDyad[i, j] = Calculator.Add(ref point1.Dyad[i, j], ref point2.Dyad[i, j]);
                }
            }

            return new DyadCoordinate<T, TC>(newDyad);
        }

        public override T this[int i, int j]
        {
            get
            {
                return this.Dyad[i, j];
            }
        }
    }
}
