using System;
using System.Collections.Generic;

using Simulation.Models.Spectrum;

namespace Simulation.Models.Extensions
{
    /// <summary>
    /// The SimulationResultExtensions class.
    /// </summary>
    public static class SimulationResultExtensions
    {
        /// <summary>
        /// Converts IEnumerable to the simulation result dictionary.
        /// </summary>
        /// <param name="spectrum">The spectrum.</param>
        /// <param name="selector">The selector.</param>
        /// <returns>The new instance of SimulationResultDictionary.</returns>
        public static SimulationResultDictionary ToSimulationResult(
            this IEnumerable<SpectrumUnit> spectrum,
            Func<SpectrumUnit, SimulationResult> selector)
        {
            var result = new SimulationResultDictionary();
            foreach (SpectrumUnit freq in spectrum)
            {
                result.Add(freq, selector(freq));
            }
            return result;
        }
    }
}
