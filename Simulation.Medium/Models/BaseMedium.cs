using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using Simulation.Models;

namespace Simulation.Medium.Models
{
    public abstract class BaseMedium
    {
        public abstract Complex GetPermittivity(SpectrumParameter frequency);
    }
}
