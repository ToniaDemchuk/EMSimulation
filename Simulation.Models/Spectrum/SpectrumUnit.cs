using Simulation.Models.Enums;
using Simulation.Models.Extensions;

namespace Simulation.Models.Spectrum
{
    /// <summary>
    /// The SpectrumParameter class.
    /// </summary>
    public class SpectrumUnit
    {
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
            return SpectrumUnitConverter.Convert(this.value, this.type, toType);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            SpectrumUnit p = obj as SpectrumUnit;
            if (p == null)
            {
                return false;
            }

            return (this.type == p.type) && (this.value == p.value);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return base.GetHashCode() ^ (int)this.type;
        }
    }
}
