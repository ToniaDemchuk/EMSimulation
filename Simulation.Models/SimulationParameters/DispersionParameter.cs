using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.Models
{
    public class DispersionParameter
    {
        public SpectrumParameter SpectrumParameter { get; set; }

        public Complex Permitivity { get; set; }

        public double MediumRefractiveIndex { get; set; }

        public CartesianCoordinate WaveVector { get; set; }
    }
}
