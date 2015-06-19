using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.Models
{
    public class SpectrumParameter
    {
        public SpectrumParameter(double value, SpectrumParameterType type)
        {
            this.Value = value;
            this.Type = type;
        }

        private readonly SpectrumParameterType Type;

        private readonly double Value;

        public double ToType(SpectrumParameterType type)
        {
            return SpectrumParameterConverter.Convert(this.Value, this.Type, type);
        }
        
        public override bool Equals(object obj)
        {
            // If parameter cannot be cast to Point return false.
            SpectrumParameter p = obj as SpectrumParameter;
            if (p == null)
            {
                return false;
            }

            return (this.Type == p.Type) && (this.Value == p.Value);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^ (int)this.Type;
        }
    }
}
