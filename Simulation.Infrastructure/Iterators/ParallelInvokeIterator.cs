using Simulation.Models.Coordinates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.Infrastructure.Iterators
{
    public class ParallelInvokeIterator:IIterator
    {
        public int MaxDegreeOfParallelism = 4;
        public void For(IndexStore indices, Action<int, int, int> action)
        {
            Parallel.Invoke(
                () =>
                {
                    for (int i = indices.Lower; i < indices.ILength / 2; i++)
                    {
                        for (int j = indices.Lower; j < indices.JLength / 2; j++)
                        {
                            for (int k = indices.Lower; k < indices.KLength / 2; k++)
                            {
                                action(i, j, k);
                            }
                        }
                    }
                },
                () =>
                {
                    for (int i = indices.Lower; i < indices.ILength / 2; i++)
                    {
                        for (int j = indices.JLength / 2; j < indices.JLength; j++)
                        {
                            for (int k = indices.Lower; k < indices.KLength / 2; k++)
                            {
                                action(i, j, k);
                            }
                        }
                    }
                },
                () =>
                {
                    for (int i = indices.Lower; i < indices.ILength / 2; i++)
                    {
                        for (int j = indices.Lower; j < indices.JLength / 2; j++)
                        {
                            for (int k = indices.KLength / 2; k < indices.KLength; k++)
                            {
                                action(i, j, k);
                            }
                        }
                    }
                },
                () =>
                {
                    for (int i = indices.Lower; i < indices.ILength / 2; i++)
                    {
                        for (int j = indices.JLength / 2; j < indices.JLength; j++)
                        {
                            for (int k = indices.KLength / 2; k < indices.KLength; k++)
                            {
                                action(i, j, k);
                            }
                        }
                    }
                },
                () =>
                {
                    for (int i = indices.ILength / 2; i < indices.ILength; i++)
                    {
                        for (int j = indices.Lower; j < indices.JLength / 2; j++)
                        {
                            for (int k = indices.Lower; k < indices.KLength / 2; k++)
                            {
                                action(i, j, k);
                            }
                        }
                    }
                },
                () =>
                {
                    for (int i = indices.ILength / 2; i < indices.ILength; i++)
                    {
                        for (int j = indices.JLength / 2; j < indices.JLength; j++)
                        {
                            for (int k = indices.Lower; k < indices.KLength / 2; k++)
                            {
                                action(i, j, k);
                            }
                        }
                    }
                },
                () =>
                {
                    for (int i = indices.ILength / 2; i < indices.ILength; i++)
                    {
                        for (int j = indices.Lower; j < indices.JLength / 2; j++)
                        {
                            for (int k = indices.KLength / 2; k < indices.KLength; k++)
                            {
                                action(i, j, k);
                            }
                        }
                    }
                },
                () =>
                {
                    for (int i = indices.ILength / 2; i < indices.ILength; i++)
                    {
                        for (int j = indices.JLength / 2; j < indices.JLength; j++)
                        {
                            for (int k = indices.KLength / 2; k < indices.KLength; k++)
                            {
                                action(i, j, k);
                            }
                        }
                    }
                });
        }

        public void ForExceptI(IndexStore indices, Action<int, int> action)
        {
            throw new NotImplementedException();
        }

        public void ForExceptJ(IndexStore indices, Action<int, int> action)
        {
            throw new NotImplementedException();
        }

        public void ForExceptK(IndexStore indices, Action<int, int> action)
        {
            throw new NotImplementedException();
        }

        IEnumerable<Tuple<int, int, int>> RangeIterator(IndexStore indices)
        {
            for (int i = indices.Lower; i < indices.ILength; i++)
            {
                for (int j = indices.Lower; j < indices.JLength; j++)
                {
                    for (int k = indices.Lower; k < indices.KLength; k++)
                    {
                        yield return new Tuple<int, int, int>(i, j, k);
                    }
                }
            }
        }

        public double Sum(IndexStore indices, Func<int, int, int, double> action)
        {
            double sum = 0.0d;
            object lockObject = new object();
            Parallel.For(
                indices.Lower,
                indices.ILength,
                new ParallelOptions { MaxDegreeOfParallelism = MaxDegreeOfParallelism },
                () => 0d,
                (i, state, local) =>
                {
                    var sumloc = 0d;
                    for (int j = indices.Lower; j < indices.JLength; j++)
                    {
                        for (int k = indices.Lower; k < indices.KLength; k++)
                        {
                            sumloc += action(i, j, k);
                        }
                    }
                    return local + sumloc;
                },
                localPartialSum =>
                {
                    lock (lockObject)
                    {
                        sum += localPartialSum;
                    }
                });
            return sum;
        }
    }
}
