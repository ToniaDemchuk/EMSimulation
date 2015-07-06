using System.Collections.Generic;

namespace Simulation.Models.Spectrum
{
    /// <summary>
    /// The FourierSeries class.
    /// </summary>
    /// <typeparam name="T">The type of sequence values.</typeparam>
    public class FourierSeries<T> : Dictionary<SpectrumUnit, T>
    {
        public void Aggregate(SpectrumUnit freq, T coordinate)
        {
            if (!this.ContainsKey(freq))
            {
                this.Add(freq, coordinate);
            }
            else
            {
                this[freq] += (dynamic)coordinate;
            }
        }
    }
}
