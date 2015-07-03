using Simulation.Models.Coordinates;

namespace Simulation.Models.Spectrum
{
    /// <summary>
    /// The SimulationParameters class.
    /// </summary>
    public class SimulationResult
    {
        /// <summary>
        /// Gets or sets the cross section extinction.
        /// </summary>
        /// <value>
        /// The cross section extinction.
        /// </value>
        public double CrossSectionExtinction { get; set; }

        /// <summary>
        /// Gets or sets the cross section absorption.
        /// </summary>
        /// <value>
        /// The cross section absorption.
        /// </value>
        public double CrossSectionAbsorption { get; set; }

        /// <summary>
        /// Gets or sets the effective cross section extinction.
        /// </summary>
        /// <value>
        /// The effective cross section extinction.
        /// </value>
        public double EffectiveCrossSectionExtinction { get; set; }

        /// <summary>
        /// Gets or sets the effective cross section absorption.
        /// </summary>
        /// <value>
        /// The effective cross section absorption.
        /// </value>
        public double EffectiveCrossSectionAbsorption { get; set; }

        /// <summary>
        /// Gets or sets the polarization.
        /// </summary>
        /// <value>
        /// The polarization.
        /// </value>
        public ComplexCoordinate[] Polarization { get; set; }

        /// <summary>
        /// Gets or sets the electric field.
        /// </summary>
        /// <value>
        /// The electric field.
        /// </value>
        public ComplexCoordinate[] ElectricField { get; set; }
    }
}
