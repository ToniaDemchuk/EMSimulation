using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using Simulation.Models;

namespace Simulation.Medium.Models
{
    public abstract class BaseMediumSolver : IMediumSolver
    {
        protected BaseMediumSolver(BaseMedium permittivity)
        {
            this.Permittivity = permittivity;
        }

        public bool IsBody { get; set; }

        protected BaseMedium Permittivity { get; private set; }

        public abstract CartesianCoordinate Solve(CartesianCoordinate displacementField);

        public Complex GetEpsilon(SpectrumParameter frequency)
        {
            throw new NotImplementedException();
        }
    }
}
