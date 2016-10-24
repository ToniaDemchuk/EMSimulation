using System;

using Simulation.Models.Calculators;

namespace Simulation.Models.Coordinates
{
    /// <summary>
    /// The CartesianCoordinate class.
    /// </summary>
    /// <typeparam name="T">The type of underlying value.</typeparam>
    /// <typeparam name="TC">Calculator for type.</typeparam>
    /// <seealso cref="Coordinates.BaseDyadCoordinate{T, TC}" />
    public class DiagonalDyadCoordinate<T, TC> : BaseDyadCoordinate<T, TC>
        where T : struct
        where TC : ICalculator<T>, new()
    {
        private readonly T[] diagonal = new T[DyadLength];

        /// <summary>
        /// Initializes a new instance of the <see cref="DyadCoordinate{T, TC}"/> class.
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
    }
}
