using System;
using Simulation.Models.Coordinates;

namespace Simulation.Models.Extensions
{
    public static class ArrayExtensions
    {
        public static void For<T>(this T[] array, Func<int, T> function)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = function(i);
            }
        }

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

        public static void For<T>(this T[, ,] array, Func<int, int, int, T> function)
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
    }
}
