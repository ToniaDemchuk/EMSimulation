using System;

using Simulation.Models.Coordinates;

namespace Simulation.Infrastructure.Models
{
    public class Voxel
    {
        public Voxel()
        {
        }

        public Voxel(CartesianCoordinate point)
        {
            this.I = (int) Math.Floor(point.X);
            this.J = (int) Math.Floor(point.Y);
            this.K = (int) Math.Floor(point.Z);
        }

        public int I { get; set; }
        public int J { get; set; }
        public int K { get; set; }
        public string Material { get; set; }
    }
}
