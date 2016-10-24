using System;
using System.Numerics;

using Simulation.Models.Calculators;
using Simulation.Models.Coordinates;
using Simulation.Models.Matrices;

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

        public static double[] ConvertToPlainArrayMatrix(IMatrix<IMatrix<Complex>> coordinate)
        {
            var size = coordinate.Length;
            var dyadSize = DyadCoordinate<Complex, ComplexCalculator>.DyadLength;

            double[] array = new double[size * size * ComplexMultiplier * dyadSize];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    for (int ii = 0; ii < dyadSize; ii++)
                    {
                        for (int jj = 0; jj < dyadSize; jj++)
                        {
                            Complex dyadCoord = coordinate[i, j][ii, jj];
                            array[ComplexMultiplier * size * (dyadSize * i + ii) + ComplexMultiplier * j + 2 * jj + 0] =
                                dyadCoord.Real;
                            array[ComplexMultiplier * size * (dyadSize * i + ii) + ComplexMultiplier * j + 2 * jj + 1] =
                                dyadCoord.Imaginary;
                        }
                    }
                }
            }
            return array;
        }

        /// <summary>
        /// Converts to plain array.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The double type array.</returns>
        public static double[] ConvertToPlainArray(DyadCoordinate<Complex, ComplexCalculator>[,] coordinate)
        {
            var size = coordinate.GetLength(0);
            var dyadSize = DyadCoordinate<Complex, ComplexCalculator>.DyadLength;

            double[] array = new double[size * size * ComplexMultiplier * dyadSize];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Complex[,] dyadic = coordinate[i, j].Dyad;
                    for (int ii = 0; ii < dyadSize; ii++)
                    {
                        for (int jj = 0; jj < dyadSize; jj++)
                        {
                            Complex dyadCoord = dyadic[ii, jj];
                            array[ComplexMultiplier * size * (dyadSize * i + ii) + ComplexMultiplier * j + 2 * jj + 0] =
                                dyadCoord.Real;
                            array[ComplexMultiplier * size * (dyadSize * i + ii) + ComplexMultiplier * j + 2 * jj + 1] =
                                dyadCoord.Imaginary;
                        }
                    }
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