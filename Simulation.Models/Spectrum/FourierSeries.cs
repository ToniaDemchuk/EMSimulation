using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Simulation.Models.Spectrum
{
    /// <summary>
    /// The FourierSeries class.
    /// </summary>
    /// <typeparam name="T">The type of sequence values.</typeparam>
    public class FourierSeries<T> : Dictionary<SpectrumUnit, T> where T : struct
    {
        public void Aggregate(IEnumerable<SpectrumUnit> spectrum, Func<SpectrumUnit, T> valueSelector)
        {
            foreach (var freq in spectrum)
            {
                T value;
                if (this.TryGetValue(freq, out value))
                {
                    this[freq] = value + (dynamic) valueSelector(freq);
                }
                else
                {
                    this.Add(freq, valueSelector(freq));
                }
            }
        }
    }
}