using System.Collections.Generic;

namespace Simulation.Infrastructure.Models
{
    public class MeshInfo
    {
        public List<Voxel> Voxels { get; set; }

        public Voxel Resolution { get; set; }
    }
}
