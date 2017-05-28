using System.Collections.Generic;
using Simulation.Models.Coordinates;
using System;

namespace Simulation.Models.Comparers
{
    public class CoordinateEqualityComparer: EqualityComparer<CartesianCoordinate>
    {
        public override bool Equals(CartesianCoordinate first, CartesianCoordinate second)
        {
            return (first.X == second.X
                   && first.Y == second.Y
                   && first.Z == second.Z) || 
                   (first.X == -second.X
                   && first.Y == -second.Y
                   && first.Z == -second.Z);
        }

        public override int GetHashCode(CartesianCoordinate obj)
        {
            int hash = 17;
            // Suitable nullity checks etc, of course :)
            hash = (hash * 397) ^ Math.Abs(obj.X).GetHashCode();
            hash = (hash * 397) ^ Math.Abs(obj.Y).GetHashCode();
            hash = (hash * 397) ^ Math.Abs(obj.Z).GetHashCode();
            return hash;
        }
    }
}