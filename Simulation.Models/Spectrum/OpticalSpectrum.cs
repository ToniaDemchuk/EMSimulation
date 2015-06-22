using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Simulation.Models
{
    public class OpticalSpectrum : IEnumerable<SpectrumParameter>
    {
        public OpticalSpectrum()
        {
        }

        public OpticalSpectrum(IEnumerable<double> list, SpectrumParameterType type)
        {
            List = list.Select(x => new SpectrumParameter(x, type));
        }

        private IEnumerable<SpectrumParameter> List { get; set; }

        public IEnumerator<SpectrumParameter> GetEnumerator()
        {
            return List.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
