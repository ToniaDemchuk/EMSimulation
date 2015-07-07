using System.Collections.Generic;

using Simulation.Models.Enums;
using Simulation.Models.Extensions;

namespace Simulation.Models.Spectrum
{
    /// <summary>
    /// The SpectrumParameter class.
    /// </summary>
    public class SpectrumUnit
    {
        private Dictionary<SpectrumUnitType, double> cacheDictionary = new Dictionary<SpectrumUnitType, double>(); 
        /// <summary>
        /// Initializes a new instance of the <see cref="SpectrumUnit"/> class.
        /// </summary>
        /// <param name="value">The double value.</param>
        /// <param name="type">The underlying type.</param>
        public SpectrumUnit(double value, SpectrumUnitType type)
        {
            this.value = value;
            this.type = type;
        }

        private readonly SpectrumUnitType type;

        private readonly double value;

        /// <summary>
        /// Converts value to specified type.
        /// </summary>
        /// <param name="toType">To spectrum parameter type.</param>
        /// <returns>The value of spectrum parameter type.</returns>
        public double ToType(SpectrumUnitType toType)
        {
            double newValue;
            if (cacheDictionary.TryGetValue(toType, out newValue))
            {
                return newValue;
            }
            newValue = SpectrumUnitConverter.Convert(this.value, this.type, toType);
            cacheDictionary.Add(toType, newValue);
            return newValue;
        }

        protected bool Equals(SpectrumUnit other)
        {
            return this.type == other.type && this.value.Equals(other.value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != typeof (SpectrumUnit))
            {
                return false;
            }
            return Equals((SpectrumUnit) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int) this.type * 397) ^ this.value.GetHashCode();
            }
        }
    }
}
