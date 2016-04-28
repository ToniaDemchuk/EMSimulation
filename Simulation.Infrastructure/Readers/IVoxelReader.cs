using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Simulation.Infrastructure.Models;

namespace Simulation.Infrastructure.Readers
{
    public interface IVoxelReader
    {
        MeshInfo ReadInfo(string fileName);
    }
}
