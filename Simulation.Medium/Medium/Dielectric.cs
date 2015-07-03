using System;
using System.Numerics;

using Simulation.Medium.Models;
using Simulation.Models.Spectrum;

namespace Simulation.Medium.Medium
{
    /// <summary>
    /// The Dielectric class.
    /// </summary>
    public class Dielectric : BaseMedium
    {
        private double epsilon;

        public Dielectric()
        {
            this.epsilon = 1.0;
        }

        /// <summary>
        /// Gets or sets the epsilon.
        /// </summary>
        /// <value>
        /// The epsilon.
        /// </value>
        public double Epsilon
        {
            get { return this.epsilon; }
            set
            {
                if (value < 1.0)
                {
                    throw new ArgumentOutOfRangeException("Epsilon", value, "Relative dielectric permittivity cannot be less than one.");
                }
                this.epsilon = value;
            }
        }

        /// <summary>
        /// Gets the permittivity at specific frequency.
        /// </summary>
        /// <param name="frequency">The frequency.</param>
        /// <returns>
        /// The complex permittivity.
        /// </returns>
        public override Complex GetPermittivity(SpectrumUnit frequency)
        {
            return this.epsilon;
        }
    }
}