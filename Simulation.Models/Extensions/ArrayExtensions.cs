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
            array.For(initializer);
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
            array.For((i, j, k) => initializer());
            return array;
        }
    }
}