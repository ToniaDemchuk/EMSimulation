using Simulation.Models.Coordinates;
using Simulation.Models.Extensions;

namespace Simulation.FDTD.Models
{
    public class PmlCoefficient
    {
        private CartesianCoordinate fieldFactor;
        private CartesianCoordinate curlFactor;
        private CartesianCoordinate integralFactor;

        public PmlCoefficient()
        {
            this.fieldFactor = CartesianCoordinate.One;
            this.curlFactor = CartesianCoordinate.One;
            this.integralFactor = CartesianCoordinate.Zero;
        }

        public PmlCoefficient(CartesianCoordinate fieldFactor, CartesianCoordinate curlFactor, CartesianCoordinate integralFactor)
        {
            this.fieldFactor = fieldFactor;
            this.curlFactor = curlFactor;
            this.integralFactor = integralFactor;
        }

        public CartesianCoordinate FieldFactor(CartesianCoordinate coordinate)
        {
            return this.fieldFactor.ComponentProduct(coordinate);
        }

        public CartesianCoordinate CurlFactor(CartesianCoordinate coordinate)
        {
            return this.curlFactor.ComponentProduct(coordinate);
        }

        public CartesianCoordinate IntegralFactor(CartesianCoordinate coordinate)
        {
            return this.integralFactor.ComponentProduct(coordinate);
        }
    }
}
