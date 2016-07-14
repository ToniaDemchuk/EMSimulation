using System.Numerics;

namespace Simulation.Models.Coordinates
{
    using Simulation.Models.Matrices;

    /// <summary>
    /// The CartesianCoordinate class.
    /// </summary>
    /// <typeparam name="T">The type of underlying value.</typeparam>
    public class DiagonalDyadCoordinate<T> : BaseDyadCoordinate<T> where T : struct
    {
        private readonly T[] diagonal = new T[DyadLength];

        /// <summary>
        /// Initializes a new instance of the <see cref="DyadCoordinate{T}"/> class.
        /// </summary>
        /// <param name="initValue">The value on the main diagonal of identity matrix.</param>
        public DiagonalDyadCoordinate(T initValue)
        {
            for (int i = 0; i < this.Length; i++)
            {
                this.diagonal[i] = initValue;
            }
        }

        public override T this[int i, int j]
        {
            get
            {
                return i == j ? this.diagonal[i] : default(T);
            }
        }

        ///// <summary>
        ///// Implements the operator * (Multiply dyad on the value).
        ///// </summary>
        ///// <param name="point1">The point1.</param>
        ///// <param name="num">The number.</param>
        ///// <returns>
        ///// The result of the operator.
        ///// </returns>
        //public static DyadCoordinate<T> operator *(
        //    DyadCoordinate<T> point1,
        //    T num)
        //{
        //    var newDyad =
        //        new T[DyadLength, DyadLength];

        //    for (int i = 0; i < DyadLength; i++)
        //    {
        //        for (int j = 0; j < DyadLength; j++)
        //        {
        //            newDyad[i, j] = (dynamic) point1.Dyad[i, j] * num;
        //        }
        //    }

        //    return new DyadCoordinate<T>(newDyad);
        //}

        ///// <summary>
        ///// Implements the operator * (Multiply dyad on the value).
        ///// </summary>
        ///// <param name="num">The number.</param>
        ///// <param name="point1">The point1.</param>
        ///// <returns>
        ///// The result of the operator.
        ///// </returns>
        //public static DyadCoordinate<T> operator *(
        //    T num,
        //    DyadCoordinate<T> point1)
        //{
        //    return point1 * num;
        //}

        ///// <summary>
        ///// Implements the operator - (Subtract two dyads).
        ///// </summary>
        ///// <param name="point1">The point1.</param>
        ///// <param name="point2">The point2.</param>
        ///// <returns>
        ///// The result of the operator.
        ///// </returns>
        //public static DyadCoordinate<T> operator -(
        //    DyadCoordinate<T> point1,
        //    DyadCoordinate<T> point2)
        //{
        //    var newDyad =
        //        new T[DyadLength, DyadLength];

        //    for (int i = 0; i < DyadLength; i++)
        //    {
        //        for (int j = 0; j < DyadLength; j++)
        //        {
        //            newDyad[i, j] = (dynamic) point1.Dyad[i, j] - point2.Dyad[i, j];
        //        }
        //    }

        //    return new DyadCoordinate<T>(newDyad);
        //}

        ///// <summary>
        ///// Implements the operator - (Subtract two dyads).
        ///// </summary>
        ///// <param name="point1">The point1.</param>
        ///// <param name="point2">The point2.</param>
        ///// <returns>
        ///// The result of the operator.
        ///// </returns>
        //public static DyadCoordinate<T> operator +(
        //    DyadCoordinate<T> point1,
        //    DyadCoordinate<T> point2)
        //{
        //    var newDyad =
        //        new T[DyadLength, DyadLength];

        //    for (int i = 0; i < DyadLength; i++)
        //    {
        //        for (int j = 0; j < DyadLength; j++)
        //        {
        //            newDyad[i, j] = (dynamic) point1.Dyad[i, j] + point2.Dyad[i, j];
        //        }
        //    }

        //    return new DyadCoordinate<T>(newDyad);
        //}

        ///// <summary>
        ///// Performs an implicit conversion from <see cref="DyadCoordinate{T}"/> to <see cref="DyadCoordinate{Complex}"/>.
        ///// </summary>
        ///// <param name="value">The value.</param>
        ///// <returns>
        ///// The result of the conversion.
        ///// </returns>
        //public static implicit operator DyadCoordinate<Complex>(DyadCoordinate<T> value)
        //{
        //    var newDyad = new Complex[DyadLength, DyadLength];

        //    for (int i = 0; i < DyadLength; i++)
        //    {
        //        for (int j = 0; j < DyadLength; j++)
        //        {
        //            newDyad[i, j] = (dynamic) value.Dyad[i, j];
        //        }
        //    }

        //    return new DyadCoordinate<Complex>(newDyad);
        //}
    }
}