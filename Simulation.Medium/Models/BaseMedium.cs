using System.Numerics;

using Simulation.Models.Spectrum;

namespace Simulation.Medium.Models
{
    /// <summary>
    /// The BaseMedium class.
    /// </summary>
    public abstract class BaseMedium
    {
        /// <summary>
        /// Gets the permittivity at specific frequency.
        /// </summary>
        /// <param name="frequency">The frequency.</param>
        /// <returns>The complex permittivity.</returns>
        public abstract Complex GetPermittivity(SpectrumUnit frequency);
    }
}
