using Simulation.Models.Coordinates;
using PmlCoef = System.Tuple<double, double, double>;

namespace Simulation.FDTD.Models
{
    public struct PmlCoefficient
    {
        private readonly PmlCoef iCoef;

        private readonly PmlCoef jCoef;

        private readonly PmlCoef kCoef;

        public PmlCoefficient(
            PmlCoef iCoef,
            PmlCoef jCoef,
            PmlCoef kCoef)
        {
            this.iCoef = iCoef;
            this.jCoef = jCoef;
            this.kCoef = kCoef;
        }

        public CartesianCoordinate FieldFactor(CartesianCoordinate coordinate)
        {
            return new CartesianCoordinate(
                coordinate.X * this.jCoef.Item3 * this.kCoef.Item3,
                coordinate.Y * this.iCoef.Item3 * this.kCoef.Item3,
                coordinate.Z * this.iCoef.Item3 * this.jCoef.Item3);
        }

        public CartesianCoordinate CurlFactor(CartesianCoordinate coordinate)
        {
            return new CartesianCoordinate(
                coordinate.X * this.jCoef.Item2 * this.kCoef.Item2,
                coordinate.Y * this.iCoef.Item2 * this.kCoef.Item2,
                coordinate.Z * this.iCoef.Item2 * this.jCoef.Item2);
        }

        public CartesianCoordinate IntegralFactor(CartesianCoordinate coordinate)
        {
            return new CartesianCoordinate(
                coordinate.X * this.iCoef.Item1,
                coordinate.Y * this.jCoef.Item1,
                coordinate.Z * this.kCoef.Item1);
        }
    }
}
