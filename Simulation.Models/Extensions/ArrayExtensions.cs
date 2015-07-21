using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Simulation.Models.Coordinates;

namespace Simulation.Models.Extensions
{
    /// <summary>
    /// The ArrayExtensions class.
    /// </summary>
    public static class ArrayExtensions
    {
        /// <summary>
        /// The maximum degree of parallelism for parallel methods.
        /// </summary>
        public static int MaxDegreeOfParallelism = 32;

        /// <summary>
        /// Iterates through the specified array.
        /// </summary>
        /// <typeparam name="T">The underlying type of array.</typeparam>
        /// <param name="array">The array.</param>
        /// <param name="function">The function to set to the value.</param>
        public static void For<T>(this T[] array, Func<int, T> function)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = function(i);
            }
        }

        /// <summary>
        /// Iterates through the specified array.
        /// </summary>
        /// <typeparam name="T">The underlying type of array.</typeparam>
        /// <param name="array">The array.</param>
        /// <param name="function">The function to set to the value.</param>
        public static void For<T>(this T[,] array, Func<int, int, T> function)
        {
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    array[i, j] = function(i, j);
                }
            }
        }

        /// <summary>
        /// Iterates through the specified array.
        /// </summary>
        /// <typeparam name="T">The underlying type of array.</typeparam>
        /// <param name="array">The array.</param>
        /// <param name="function">The function to set to the value.</param>
        public static void For<T>(this T[,,] array, Func<int, int, int, T> function)
        {
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    for (int k = 0; k < array.GetLength(2); k++)
                    {
                        array[i, j, k] = function(i, j, k);
                    }
                }
            }
        }

        public static void ParallelLinqFor<T>(this T[,,] array, Func<int, int, int, T> function)
        {
            Parallel.For(
                0,
                array.GetLength(0),
                new ParallelOptions { MaxDegreeOfParallelism = MaxDegreeOfParallelism },
                i =>
                {
                    for (int j = 0; j < array.GetLength(1); j++)
                    {
                        for (int k = 0; k < array.GetLength(2); k++)
                        {
                            array[i, j, k] = function(i, j, k);
                        }
                    }
                });
        }

        private static IEnumerable<Tuple<int, int, int>> RangeIterator(this IndexStore indices)
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

        /// <summary>
        /// Parallels the linq for.
        /// </summary>
        /// <param name="indices">The indices.</param>
        /// <param name="action">The action.</param>
        public static void ParallelLinqFor(this IndexStore indices, Action<int, int, int> action)
        {
            Parallel.For(
                indices.Lower,
                indices.ILength,
                new ParallelOptions { MaxDegreeOfParallelism = MaxDegreeOfParallelism },
                i =>
                {
                    for (int j = indices.Lower; j < indices.JLength; j++)
                    {
                        for (int k = indices.Lower; k < indices.KLength; k++)
                        {
                            action(i, j, k);
                        }
                    }
                });
        }

        public static double ParallelForSum(this IndexStore indices, Func<int, int, int, double> action)
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

        public static void ParallelFor(this IndexStore indices, Action<int, int, int> action)
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

        /// <summary>
        /// Iterates through the specified array according to indices.
        /// </summary>
        /// <param name="indices">The indices.</param>
        /// <param name="action">The action to perform on every index.</param>
        public static void For(this IndexStore indices, Action<int, int, int> action)
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
        public static void ForExceptI(this IndexStore indices, Action<int, int> action)
        {
            for (int j = indices.Lower; j < indices.JLength; j++)
            {
                for (int k = indices.Lower; k < indices.KLength; k++)
                {
                    action(j, k);
                }
            }
        }

        public static void ParallelForExceptI(this IndexStore indices, Action<int, int> action)
        {
            Parallel.For(
                indices.Lower,
                indices.JLength,
                new ParallelOptions { MaxDegreeOfParallelism = MaxDegreeOfParallelism },
                i =>
                {
                    for (int k = indices.Lower; k < indices.KLength; k++)
                    {
                        action(i, k);
                    }
                });
        }

        /// <summary>
        /// Iterates through the specified array except the second dimension.
        /// </summary>
        /// <param name="indices">The indices.</param>
        /// <param name="action">The action to perform on every index.</param>
        public static void ForExceptJ(this IndexStore indices, Action<int, int> action)
        {
            for (int i = indices.Lower; i < indices.ILength; i++)
            {
                for (int k = indices.Lower; k < indices.KLength; k++)
                {
                    action(i, k);
                }
            }
        }

        public static void ParallelForExceptJ(this IndexStore indices, Action<int, int> action)
        {
            Parallel.For(
                indices.Lower,
                indices.ILength,
                new ParallelOptions { MaxDegreeOfParallelism = MaxDegreeOfParallelism },
                i =>
                {
                    for (int k = indices.Lower; k < indices.KLength; k++)
                    {
                        action(i, k);
                    }
                });
        }

        /// <summary>
        /// Iterates through the specified array except the third dimension.
        /// </summary>
        /// <param name="indices">The indices.</param>
        /// <param name="action">The action to perform on every index.</param>
        public static void ForExceptK(this IndexStore indices, Action<int, int> action)
        {
            for (int i = indices.Lower; i < indices.ILength; i++)
            {
                for (int j = indices.Lower; j < indices.JLength; j++)
                {
                    action(i, j);
                }
            }
        }

        public static void ParallelForExceptK(this IndexStore indices, Action<int, int> action)
        {
            Parallel.For(
                indices.Lower,
                indices.ILength,
                new ParallelOptions { MaxDegreeOfParallelism = MaxDegreeOfParallelism },
                i =>
                {
                    for (int j = indices.Lower; j < indices.JLength; j++)
                    {
                        action(i, j);
                    }
                });
        }

        /// <summary>
        /// Creates the array with dimensions set in index store and initialize with defaults.
        /// </summary>
        /// <typeparam name="T">The type of underlying values.</typeparam>
        /// <param name="indices">The indices.</param>
        /// <returns>The new three dimensional array.</returns>
        public static T[,,] CreateArray<T>(this IndexStore indices)
        {
            var array = new T[indices.ILength, indices.JLength, indices.KLength];
            return array;
        }

        /// <summary>
        /// Creates the array with dimensions set in index store and initialize with defaults.
        /// </summary>
        /// <typeparam name="T">The type of underlying values.</typeparam>
        /// <param name="indices">The indices.</param>
        /// <param name="initializer">The initializer.</param>
        /// <returns>
        /// The new three dimensional array.
        /// </returns>
        public static T[,,] CreateArray<T>(this IndexStore indices, Func<int, int, int, T> initializer)
        {
            var array = indices.CreateArray<T>();
            array.ParallelLinqFor(initializer);
            return array;
        }

        /// <summary>
        /// Creates the array with dimensions set in index store and initialize with defaults.
        /// </summary>
        /// <typeparam name="T">The type of underlying values.</typeparam>
        /// <param name="indices">The indices.</param>
        /// <param name="initializer">The initializer.</param>
        /// <returns>
        /// The new three dimensional array.
        /// </returns>
        public static T[,,] CreateArray<T>(this IndexStore indices, Func<T> initializer)
        {
            var array = indices.CreateArray<T>();
            array.ParallelLinqFor((i, j, k) => initializer());
            return array;
        }
    }
}