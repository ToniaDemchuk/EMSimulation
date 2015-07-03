using System;
using System.Collections.Generic;
using System.Linq;

namespace Simulation.Models.Spectrum
{
    /// <summary>
    /// The SimulationResultDictionary class.
    /// </summary>
    public class SimulationResultDictionary : Dictionary<SpectrumUnit, SimulationResult>
    {
        /// <summary>
        /// Creates Dictionary according to specified key and value selector.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="keySelector">The key selector.</param>
        /// <param name="valueSelector">The value selector.</param>
        /// <returns>
        /// The new dictionary.
        /// </returns>
        public Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(Func<SpectrumUnit, TKey> keySelector, Func<SimulationResult, TValue> valueSelector)
        {
            return this.ToDictionary(x => keySelector(x.Key), x => valueSelector(x.Value));
        }
    }
}