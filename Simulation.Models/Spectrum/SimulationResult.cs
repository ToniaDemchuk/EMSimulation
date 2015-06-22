using System.Collections.Generic;

namespace Simulation.Models
{
    /// <summary>
    /// The SimulationParameters class.
    /// </summary>
    public class SimulationResult
    {
        public double CrossSectionExtinction { get; set; }

        public double CrossSectionAbsorption { get; set; }

        public double EffectiveCrossSectionExtinction { get; set; }

        public double EffectiveCrossSectionAbsorption { get; set; }

        public ComplexCoordinate[] Polarization { get; set; }

        public ComplexCoordinate[] ElectricField { get; set; }
    }
}
