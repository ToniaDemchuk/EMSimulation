using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Simulation.Models.Enums;

namespace Simulation.Models.Spectrum
{
    /// <summary>
    /// The OpticalSpectrum class.
    /// </summary>
    public class OpticalSpectrum : IEnumerable<SpectrumUnit>
    {
        private readonly IEnumerable<SpectrumUnit> list;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpticalSpectrum"/> class.
        /// </summary>
        public OpticalSpectrum()
        {
            this.list = new List<SpectrumUnit>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OpticalSpectrum"/> class.
        /// </summary>
        /// <param name="list">The list of values.</param>
        /// <param name="type">The type of values.</param>
        public OpticalSpectrum(IEnumerable<double> list, SpectrumUnitType type)
        {
            this.list = list.Select(x => new SpectrumUnit(x, type));
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<SpectrumUnit> GetEnumerator()
        {
            return this.list.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}