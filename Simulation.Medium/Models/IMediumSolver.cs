using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.Models
{
    public interface IMediumSolver
    {
        bool IsBody { get; set; }

        CartesianCoordinate Solve(CartesianCoordinate displacementField);

        Complex GetEpsilon(SpectrumParameter frequency);
    }
}
