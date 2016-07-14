using Simulation.Models.Matrices;

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

        public abstract T this[int i, int j] { get; }
    }
}
