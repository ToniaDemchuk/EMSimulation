using System;
using System.Numerics;

namespace Simulation.Models
{
    /// <summary>
    /// The CoordinateExtensions class.
    /// </summary>
    public static class CoordinateExtensions
    {
        public const int ComplexMultiplier = 6;

        public static double[] ConvertToPlainArray(ComplexCoordinate[] coordinate)
        {
            double[] array = new double[coordinate.Length * ComplexMultiplier];
            for (int i = 0; i < coordinate.Length; i++)
            {
                array[ComplexMultiplier * i + 0] = coordinate[i].X.Real;
                array[ComplexMultiplier * i + 1] = coordinate[i].X.Imaginary;
                array[ComplexMultiplier * i + 2] = coordinate[i].Y.Real;
                array[ComplexMultiplier * i + 3] = coordinate[i].Y.Imaginary;
                array[ComplexMultiplier * i + 4] = coordinate[i].Z.Real;
                array[ComplexMultiplier * i + 5] = coordinate[i].Z.Imaginary;
            }
            return array;
        }

        public static double[] ConvertToPlainArray(DyadCoordinate<Complex>[,] coordinate)
        {
            var size = coordinate.GetLength(0);
            var dyadSize = DyadCoordinateEntensions.DyadLength;
            
            double[] array = new double[size * size * ComplexMultiplier * dyadSize];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    for (int ii = 0; ii < dyadSize; ii++)
                    {
                        for (int jj = 0; jj < dyadSize; jj++)
                        {
                            array[ComplexMultiplier * size * (dyadSize * i + ii) + ComplexMultiplier * j + 2 * jj + 0] = coordinate[i, j].Dyad[ii, jj].Real;
                            array[ComplexMultiplier * size * (dyadSize * i + ii) + ComplexMultiplier * j + 2 * jj + 1] = coordinate[i, j].Dyad[ii, jj].Imaginary;
                        }
                    }
                }
            }
            return array;
        }

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