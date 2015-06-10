using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.Models
{
    public interface IMedium
    {
        CartesianCoordinate Solve(CartesianCoordinate displacementField);
    }
}
