using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.Models
{
    public class CartesianCoordinate
    {
        public CartesianCoordinate(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;

            this.Norm = Math.Sqrt(X * X + Y * Y + Z * Z);
        }

        public double X { get; private set; }

        public double Y { get; private set; }

        public double Z { get; private set; }

        public double Norm { get; private set; }

        public double GetDistance(CartesianCoordinate point)
        {
            return Math.Sqrt(
                (X - point.X) * (X - point.X) +
                (Y - point.Y) * (Y - point.Y) +
                (Z - point.Z) * (Z - point.Z));
        }

        public static CartesianCoordinate operator +(CartesianCoordinate point1, CartesianCoordinate point2)
        {
            var x = point1.X + point2.X;
            var y = point1.Y + point2.Y;
            var z = point1.Z + point2.Z;

            return new CartesianCoordinate(x, y, z);
        }

        public static CartesianCoordinate operator +(CartesianCoordinate point1, int num)
        {
            var x = point1.X + num;
            var y = point1.Y + num;
            var z = point1.Z + num;

            return new CartesianCoordinate(x, y, z);
        }
        public static CartesianCoordinate operator +(int num, CartesianCoordinate point1)
        {
            return point1 + num;
        }
        public static CartesianCoordinate operator -(CartesianCoordinate point1, CartesianCoordinate point2)
        {
            var x = point1.X - point2.X;
            var y = point1.Y - point2.Y;
            var z = point1.Z - point2.Z;

            return new CartesianCoordinate(x, y, z);
        }
        public static CartesianCoordinate operator -(CartesianCoordinate point1, int num)
        {
            var x = point1.X - num;
            var y = point1.Y - num;
            var z = point1.Z - num;

            return new CartesianCoordinate(x, y, z);
        }
        public static CartesianCoordinate operator -(CartesianCoordinate point1)
        {
            var x = -point1.X;
            var y = -point1.Y;
            var z = -point1.Z;

            return new CartesianCoordinate(x, y, z);
        }
        public static CartesianCoordinate operator -(int num, CartesianCoordinate point1)
        {
            return -point1 + num;
        }
        public static double operator *(CartesianCoordinate point1, CartesianCoordinate point2)
        {
            return point1.X*point2.X + point1.Y *point2.Y + point1.Z*point2.Z;
        }
    }
}
