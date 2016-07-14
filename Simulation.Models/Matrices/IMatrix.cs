namespace Simulation.Models.Matrices
{
    public interface IMatrix<out T>
    {
        int Length { get; }

        T this[int i, int j] { get; }
    }
}
