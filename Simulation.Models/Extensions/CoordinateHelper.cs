using System;
using System.Numerics;

using Simulation.Models.Coordinates;

namespace Simulation.Models.Extensions
{
    /// <summary>
    /// The CoordinateExtensions class.
    /// </summary>
    public static class CoordinateHelper
    {
        /// <summary>
        /// The complex multiplier.
        /// </summary>
        public const int ComplexMultiplier = 6;

        /// <summary>
        /// Converts to plain array.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The double type array.</returns>
        public static double[] ConvertToPlainArray(ComplexCoordinate[] coordinate)
        {
            double[] array = new double[coordinate.Length * ComplexMultiplier];
            for (int i = 0; i < coordinate.Length; i++)
            {
                ComplexCoordinate complexCoordinate = coordinate[i];
                array[ComplexMultiplier * i + 0] = complexCoordinate.X.Real;
                array[ComplexMultiplier * i + 1] = complexCoordinate.X.Imaginary;
                array[ComplexMultiplier * i + 2] = complexCoordinate.Y.Real;
                array[ComplexMultiplier * i + 3] = complexCoordinate.Y.Imaginary;
                array[ComplexMultiplier * i + 4] = complexCoordinate.Z.Real;
                array[ComplexMultiplier * i + 5] = complexCoordinate.Z.Imaginary;
            }
            return array;
        }

        /// <summary>
        /// Converts to plain array.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The double type array.</returns>
        public static double[] ConvertToPlainArray(DyadCoordinate<Complex>[,] coordinate)
        {
            var size = coordinate.GetLength(0);
            var dyadSize = 3;

            double[] array = new double[size * size * ComplexMultiplier * dyadSize];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    DyadCoordinate<Complex> dyadic = coordinate[i, j];

                    array[ComplexMultiplier * size * (dyadSize * i + 0) + ComplexMultiplier * j + 2 * 0 + 0] =
                        dyadic.xx.Real;
                    array[ComplexMultiplier * size * (dyadSize * i + 0) + ComplexMultiplier * j + 2 * 0 + 1] =
                        dyadic.xx.Imaginary;
                    
                    array[ComplexMultiplier * size * (dyadSize * i + 1) + ComplexMultiplier * j + 2 * 0 + 0] =
                        dyadic.xy.Real;
                    array[ComplexMultiplier * size * (dyadSize * i + 1) + ComplexMultiplier * j + 2 * 0 + 1] =
                        dyadic.xy.Imaginary;
                    
                    array[ComplexMultiplier * size * (dyadSize * i + 2) + ComplexMultiplier * j + 2 * 0 + 0] =
                        dyadic.xz.Real;
                    array[ComplexMultiplier * size * (dyadSize * i + 2) + ComplexMultiplier * j + 2 * 0 + 1] =
                        dyadic.xz.Imaginary;


                    array[ComplexMultiplier * size * (dyadSize * i + 0) + ComplexMultiplier * j + 2 * 1 + 0] =
                        dyadic.yx.Real;
                    array[ComplexMultiplier * size * (dyadSize * i + 0) + ComplexMultiplier * j + 2 * 1 + 1] =
                        dyadic.yx.Imaginary;

                    array[ComplexMultiplier * size * (dyadSize * i + 1) + ComplexMultiplier * j + 2 * 1 + 0] =
                        dyadic.yy.Real;
                    array[ComplexMultiplier * size * (dyadSize * i + 1) + ComplexMultiplier * j + 2 * 1 + 1] =
                        dyadic.yy.Imaginary;

                    array[ComplexMultiplier * size * (dyadSize * i + 2) + ComplexMultiplier * j + 2 * 1 + 0] =
                        dyadic.yz.Real;
                    array[ComplexMultiplier * size * (dyadSize * i + 2) + ComplexMultiplier * j + 2 * 1 + 1] =
                        dyadic.yz.Imaginary;


                    array[ComplexMultiplier * size * (dyadSize * i + 0) + ComplexMultiplier * j + 2 * 2 + 0] =
                        dyadic.zx.Real;
                    array[ComplexMultiplier * size * (dyadSize * i + 0) + ComplexMultiplier * j + 2 * 2 + 1] =
                        dyadic.zx.Imaginary;

                    array[ComplexMultiplier * size * (dyadSize * i + 1) + ComplexMultiplier * j + 2 * 2 + 0] =
                        dyadic.zy.Real;
                    array[ComplexMultiplier * size * (dyadSize * i + 1) + ComplexMultiplier * j + 2 * 2 + 1] =
                        dyadic.zy.Imaginary;

                    array[ComplexMultiplier * size * (dyadSize * i + 2) + ComplexMultiplier * j + 2 * 2 + 0] =
                        dyadic.zz.Real;
                    array[ComplexMultiplier * size * (dyadSize * i + 2) + ComplexMultiplier * j + 2 * 2 + 1] =
                        dyadic.zz.Imaginary;
                }
            }
            return array;
        }

        /// <summary>
        /// Converts from plain array.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <returns>The complex coordinate array.</returns>
        public static ComplexCoordinate[] ConvertFromPlainArray(double[] array)
        {
            if (array.Length % ComplexMultiplier != 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            int complexLength = array.Length / ComplexMultiplier;
            var complex = new ComplexCoordinate[complexLength];

            for (int i = 0; i < complexLength; i++)
            {
                var xPoint = new Complex(array[ComplexMultiplier * i + 0], array[ComplexMultiplier * i + 1]);
                var yPoint = new Complex(array[ComplexMultiplier * i + 2], array[ComplexMultiplier * i + 3]);
                var zPoint = new Complex(array[ComplexMultiplier * i + 4], array[ComplexMultiplier * i + 5]);

                complex[i] = new ComplexCoordinate(xPoint, yPoint, zPoint);
            }
            return complex;
        }
    }
}