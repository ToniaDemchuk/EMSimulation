using Simulation.Models.Spectrum;

namespace Simulation.Medium.Models
{
    /// <summary>
    /// The ResonanceTerm class.
    /// </summary>
    public class ResonanceTerm
    {
        /// <summary>
        /// Gets or sets the collision frequency.
        /// </summary>
        /// <value>
        /// The collision frequency.
        /// </value>
        public SpectrumUnit CollisionFrequency { get; set; }

        /// <summary>
        /// Gets or sets the strength factor.
        /// </summary>
        /// <value>
        /// The strength factor.
        /// </value>
        public double StrengthFactor { get; set; }

        /// <summary>
        /// Gets or sets the resonance frequency.
        /// </summary>
        /// <value>
        /// The resonance frequency.
        /// </value>
        public SpectrumUnit ResonanceFrequency { get; set; }
    }
}
