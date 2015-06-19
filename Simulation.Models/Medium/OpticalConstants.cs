using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Simulation.Models
{
    /// <summary>
    /// The OpticalConstants class.
    /// </summary>
    public class OpticalConstants
    {
        public List<double> WaveLengthList { get; private set; }

        public List<Complex> PermittivityList { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OpticalConstants" /> class.
        /// </summary>
        /// <param name="waveLengthList">The wave length list.</param>
        /// <param name="permittivityList">The permittivity list.</param>
        /// <exception cref="System.ArgumentException">Lists have different count.</exception>
        public OpticalConstants(List<double> waveLengthList, List<Complex> permittivityList)
        {
            if (waveLengthList.Count != permittivityList.Count)
            {
                throw new ArgumentException("Lists have different count.");
            }

            this.WaveLengthList = waveLengthList;
            this.PermittivityList = permittivityList;
        }

        /// <summary>
        /// Selects the optical constant.
        /// </summary>
        /// <param name="waveLength">Length of the wave.</param>
        /// <returns>The complex epsilon.</returns>
        public Complex SelectOpticalConst(SpectrumParameter parameter)
        {
            var waveLength = parameter.ToType(SpectrumParameterType.WaveLength);
            var tuple = this.getNearestIndexes(waveLength);
            var lower = tuple.Item1;
            var upper = tuple.Item2;

            var deltaWaveLength = waveLength - this.WaveLengthList[lower];
            var stepWaveLength = this.WaveLengthList[upper] - this.WaveLengthList[lower];
            var coefWaveLength = deltaWaveLength / stepWaveLength;

            double epsRe = this.PermittivityList[lower].Real + coefWaveLength *
                (this.PermittivityList[upper].Real - this.PermittivityList[lower].Real);

            double epsIm = this.PermittivityList[lower].Imaginary + coefWaveLength *
                (this.PermittivityList[upper].Imaginary - this.PermittivityList[lower].Imaginary);

            return new Complex(epsRe, epsIm);

            ////eps_re = 3.9943 - (13.29e+15*13.29e+15)/((4.0*Pi*Pi*3.0e8*3e8/WaveLength/WaveLength*1e18)+(0.1128e+15*0.1128e+15));
            ////eps_im = (13.29e+15*13.29e+15*0.1128e+15)/((4.0*Pi*Pi*3e8*3e8/WaveLength/WaveLength*1e18)+(0.1128e+15*0.1128e+15))/(2.0*Pi*3e8/WaveLength*1e9);

        }

        private Tuple<int, int> getNearestIndexes(double waveLength)
        {
            int i;
            for (i = 0; i < this.WaveLengthList.Count - 1; i++)
            {
                if (waveLength >= this.WaveLengthList[i] && waveLength < this.WaveLengthList[i + 1])
                {
                    return new Tuple<int, int>(i, i + 1);
                }
            }
            return new Tuple<int, int>(this.WaveLengthList.Count - 1, this.WaveLengthList.Count - 1);
        }
    }
}
