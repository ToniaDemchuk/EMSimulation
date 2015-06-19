using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.Models
{
    public class SimulationResultDictionary:Dictionary<SpectrumParameter, SimulationResult>
    {
        public Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(Func<SpectrumParameter, TKey> keySelector, Func<SimulationResult, TValue> valueSelector)
        {
            return this.ToDictionary(x => keySelector(x.Key), x => valueSelector(x.Value));
        }
    }
}
