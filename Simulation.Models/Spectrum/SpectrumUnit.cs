using Simulation.Models.Enums;
using Simulation.Models.Extensions;
using System;

namespace Simulation.Models.Spectrum
{
    /// <summary>
    /// The SpectrumParameter class.
    /// </summary>
    public class SpectrumUnit : IComparable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpectrumUnit"/> class.
        /// </summary>
        /// <param name="value">The double value.</param>
        /// <param name="type">The underlying type.</param>
        public SpectrumUnit(double value, SpectrumUnitType type)
        {
            this.Value = value;
            this.Type = type;
        }

        public SpectrumUnitType Type { get; set; }

        public double Value { get; set; }

        /// <summary>
        /// Converts value to specified type.
        /// </summary>
        /// <param name="toType">To spectrum parameter type.</param>
        /// <returns>The value of spectrum parameter type.</returns>
        public double ToType(SpectrumUnitType toType)
        {
            return SpectrumUnitConverter.Convert(this.Value, this.Type, toType);
        }

        protected bool Equals(SpectrumUnit other)
        {
            return this.Type == other.Type && this.Value.Equals(other.Value);
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
            return this.Equals((SpectrumUnit) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int) this.Type * 397) ^ this.Value.GetHashCode();
            }
        }

        public int CompareTo(object obj)
        {
            var other = (SpectrumUnit)obj;

            return Value.CompareTo(other.Value);
        }
    }
}
