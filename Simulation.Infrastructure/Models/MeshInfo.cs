using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.Infrastructure.Models
{
    public class MeshInfo
    {
        public List<Voxel> Voxels { get; set; }

        public Voxel Resolution { get; set; }
    }
}
