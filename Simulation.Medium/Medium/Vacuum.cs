using System.Numerics;

using Simulation.Medium.Models;
using Simulation.Models.Spectrum;

namespace Simulation.Medium.Medium
{
    /// <summary>
    /// The Vacuum class.
    /// </summary>
    public class Vacuum : BaseMedium
    {
        /// <summary>
        /// Gets the permittivity at specific frequency.
        /// </summary>
        /// <param name="frequency">The frequency.</param>
        /// <returns>
        /// The complex permittivity.
        /// </returns>
        public override Complex GetPermittivity(SpectrumUnit frequency)
        {
            return Complex.One;
        }
    }
}
