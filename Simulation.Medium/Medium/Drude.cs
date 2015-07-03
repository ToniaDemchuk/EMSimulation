using System.Numerics;

using Simulation.Medium.Models;
using Simulation.Models.Enums;
using Simulation.Models.Spectrum;

namespace Simulation.Medium.Medium
{
    /// <summary>
    /// The Drude class.
    /// </summary>
    public class Drude : BaseMedium
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Drude"/> class.
        /// </summary>
        public Drude()
        {
            this.PlasmaTerm = new ResonanceTerm
            {
                ResonanceFrequency = new SpectrumUnit(1.369e+16, SpectrumUnitType.CycleFrequency),
                CollisionFrequency = new SpectrumUnit(7.292e+13, SpectrumUnitType.CycleFrequency),
                StrengthFactor = 8.45e-1
            };
            this.EpsilonInfinity = 1; //3.9943;

            //this.PlasmaTerm = new ResonanceTerm
            //{
            //    ResonanceFrequency = new SpectrumUnit(9.01, SpectrumUnitType.EVEnergy),
            //    CollisionFrequency = new SpectrumUnit(0.048, SpectrumUnitType.EVEnergy),
            //    StrengthFactor = 0.845
            //};
        }

        /// <summary>
        /// Gets or sets the plasma term.
        /// </summary>
        /// <value>
        /// The plasma term.
        /// </value>
        public ResonanceTerm PlasmaTerm { get; set; }

        /// <summary>
        /// Gets or sets the epsilon infinity.
        /// </summary>
        /// <value>
        /// The epsilon infinity.
        /// </value>
        public double EpsilonInfinity { get; set; }

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
            double plasmaFreq = this.PlasmaTerm.ResonanceFrequency.ToType(SpectrumUnitType.CycleFrequency);
            double collisionFreq = this.PlasmaTerm.CollisionFrequency.ToType(SpectrumUnitType.CycleFrequency);

            Complex compl = this.EpsilonInfinity -
                            this.PlasmaTerm.StrengthFactor * plasmaFreq * plasmaFreq /
                            (w * w + Complex.ImaginaryOne * collisionFreq * w);
            return compl;
        }
    }
}