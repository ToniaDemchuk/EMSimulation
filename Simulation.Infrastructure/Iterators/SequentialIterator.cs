using Simulation.Models.Coordinates;
using System;

namespace Simulation.Infrastructure.Iterators
{
    public class SequentialIterator : IIterator
    {
        /// <summary>
        /// Iterates through the specified array according to indices.
        /// </summary>
        /// <param name="indices">The indices.</param>
        /// <param name="action">The action to perform on every index.</param>
        public void For(IndexStore indices, Action<int, int, int> action)
        {
            for (int i = indices.Lower; i < indices.ILength; i++)
            {
                for (int j = indices.Lower; j < indices.JLength; j++)
                {
                    for (int k = indices.Lower; k < indices.KLength; k++)
                    {
                        action(i, j, k);
                    }
                }
            }
        }

        /// <summary>
        /// Iterates through the specified array except the first dimension.
        /// </summary>
        /// <param name="indices">The indices.</param>
        /// <param name="action">The action to perform on every index.</param>
        public void ForExceptI(IndexStore indices, Action<int, int> action)
        {
            for (int j = indices.Lower; j < indices.JLength; j++)
            {
                for (int k = indices.Lower; k < indices.KLength; k++)
                {
                    action(j, k);
                }
            }
        }

        /// <summary>
        /// Iterates through the specified array except the second dimension.
        /// </summary>
        /// <param name="indices">The indices.</param>
        /// <param name="action">The action to perform on every index.</param>
        public void ForExceptJ(IndexStore indices, Action<int, int> action)
        {
            for (int i = indices.Lower; i < indices.ILength; i++)
            {
                for (int k = indices.Lower; k < indices.KLength; k++)
                {
                    action(i, k);
                }
            }
        }

        /// <summary>
        /// Iterates through the specified array except the third dimension.
        /// </summary>
        /// <param name="indices">The indices.</param>
        /// <param name="action">The action to perform on every index.</param>
        public void ForExceptK(IndexStore indices, Action<int, int> action)
        {
            for (int i = indices.Lower; i < indices.ILength; i++)
            {
                for (int j = indices.Lower; j < indices.JLength; j++)
                {
                    action(i, j);
                }
            }
        }

        public double Sum(IndexStore indices, Func<int, int, int, double> action)
        {
            double sum = 0;
            this.For(indices, (i, j, k) => {
                sum += action(i, j, k);
            });
            return sum;
        }
    }
}
