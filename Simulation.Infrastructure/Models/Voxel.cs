using System;
using System.Security.Cryptography;

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
            this.I = (int) Math.Round(point.X);
            this.J = (int) Math.Round(point.Y);
            this.K = (int) Math.Round(point.Z);
        }

        public int I { get; set; }
        public int J { get; set; }
        public int K { get; set; }
        public string Material { get; set; }
    }
}
