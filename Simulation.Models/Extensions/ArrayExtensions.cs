using System;
using Simulation.Models.Coordinates;

namespace Simulation.Models.Extensions
{
    /// <summary>
    /// The ArrayExtensions class.
    /// </summary>
    public static class ArrayExtensions
    {
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

        /// <summary>
        /// Creates the array with dimensions set in index store and initialize with defaults.
        /// </summary>
        /// <typeparam name="T">The type of underlying values.</typeparam>
        /// <param name="indices">The indices.</param>
        /// <returns>The new three dimensional array.</returns>
        public static T[,,] CreateArray<T>(this IndexStore indices)
        {
            var array = new T[indices.ILength, indices.JLength, indices.KLength];
            array.Initialize();
            return array;
        }
    }
}