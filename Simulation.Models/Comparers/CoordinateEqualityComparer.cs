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
            return Math.Abs(obj.X + obj.Y + obj.Z).GetHashCode();
        }
    }
}