using System.Numerics;

using Simulation.Models.Constants;
using Simulation.Models.Enums;
using Simulation.Models.Spectrum;

namespace Simulation.Medium.Medium
{
    /// <summary>
    /// The LossyDielectric class.
    /// </summary>
    public class LossyDielectric : Dielectric
    {
        /// <summary>
        /// Gets or sets the conductivity.
        /// </summary>
        /// <value>
        /// The conductivity.
        /// </value>
        public double Conductivity { get; set; }

        /// <summary>
        /// Gets the permittivity at specific frequency.
        /// </summary>
        /// <param name="frequency">The frequency.</param>
        /// <returns>
        /// The complex permittivity.
        /// </returns>
        public override Complex GetPermittivity(SpectrumUnit frequency)
        {
            double w = frequency.ToType(SpectrumUnitType.CycleFrequency);
            return this.Epsilon - Complex.ImaginaryOne * this.Conductivity / Fundamentals.ElectricConst / w;
        }
    }
}
