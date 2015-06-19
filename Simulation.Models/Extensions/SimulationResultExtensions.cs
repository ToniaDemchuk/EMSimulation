using System;
using System.Collections.Generic;

namespace Simulation.Models.Extensions
{
    public static class SimulationResultExtensions
    {
        public static SimulationResultDictionary ToSimulationResult(this IEnumerable<SpectrumParameter> spectrum, Func<SpectrumParameter, SimulationResult> selector)
        {
            var result = new SimulationResultDictionary();
            foreach (SpectrumParameter freq in spectrum)
            {
                result.Add(freq, selector(freq));
            }
            return result;
        }
    }
}
