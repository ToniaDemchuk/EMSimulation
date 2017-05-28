using Simulation.Models.Calculators;

namespace Simulation.Models.Coordinates
{
    /// <summary>
    /// The CartesianCoordinate class.
    /// </summary>
    /// <typeparam name="T">The type of underlying value.</typeparam>
    /// <typeparam name="TC">Calculator type.</typeparam>
    public class SymmetricDyadCoordinate<T, TC> : BaseDyadCoordinate<T, TC>
        where T : struct
        where TC : ICalculator<T>, new()
    {
        private readonly T[] diagonal = new T[DyadLength];

        private readonly T xy;
        private readonly T xz;
        private readonly T yz;

        public SymmetricDyadCoordinate(T xx, T xy, T xz, T yy, T yz, T zz)
        {
            this.diagonal[0] = xx;
            this.diagonal[1] = yy;
            this.diagonal[2] = zz;

            this.xy = xy;
            this.xz = xz;
            this.yz = yz;
        }

        public override T this[int i, int j]
        {
            get
            {
                return i == j ? this.diagonal[i] : this.getNonDiagonal(i, j);
            }
        }

        private T getNonDiagonal(int i, int j)
        {
            if ((i == 0 && j == 1) || (j == 0 && i == 1))
            {
                return this.xy;
            }

            if ((i == 0 && j == 2) || (j == 0 && i == 2))
            {
                return this.xz;
            }

            return this.yz;
        }
    }
}
