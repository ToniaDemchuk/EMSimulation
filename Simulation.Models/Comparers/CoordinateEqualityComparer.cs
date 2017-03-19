using System.Collections.Generic;
using Simulation.Models.Coordinates;

namespace Simulation.Models.Comparers
{
    public class CoordinateEqualityComparer: EqualityComparer<CartesianCoordinate>
    {
        public override bool Equals(CartesianCoordinate x, CartesianCoordinate y)
        {
            return this.areEqual(x, y) || this.areEqual(x, -y);
        }

        private bool areEqual(CartesianCoordinate first, CartesianCoordinate second)
        {
            return first.X.Equals(second.X) 
                   && first.Y.Equals(second.Y)
                   && first.Z.Equals(second.Z);
        }

        public override int GetHashCode(CartesianCoordinate obj)
        {
            return obj.Norm.GetHashCode();
        }
    }
}