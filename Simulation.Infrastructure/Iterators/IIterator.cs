using Simulation.Models.Coordinates;
using System;

namespace Simulation.Infrastructure.Iterators
{
    public interface IIterator
    {
        void For(IndexStore indices, Action<int, int, int> action);
        void ForExceptI(IndexStore indices, Action<int, int> action);
        void ForExceptJ(IndexStore indices, Action<int, int> action);
        void ForExceptK(IndexStore indices, Action<int, int> action);
        double Sum(IndexStore indices, Func<int, int, int, double> action);

    }
}
