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
        private readonly List<double> waveLengthList;

        private readonly List<Complex> permittivityList;

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

            this.waveLengthList = waveLengthList;
            this.permittivityList = permittivityList;
        }

        /// <summary>
        /// Selects the optical constant.
        /// </summary>
        /// <param name="waveLength">Length of the wave.</param>
        /// <returns>The complex epsilon.</returns>
        public Complex SelectOpticalConst(double waveLength)
        {
            var tuple = this.getNearestIndexes(waveLength);
            var lower = tuple.Item1;
            var upper = tuple.Item2;

            var deltaWaveLength = waveLength - waveLengthList[lower];
            var stepWaveLength = waveLengthList[upper] - waveLengthList[lower];
            var coefWaveLength = deltaWaveLength / stepWaveLength;

            double epsRe = this.permittivityList[lower].Real + coefWaveLength *
                (this.permittivityList[upper].Real - this.permittivityList[lower].Real);

            double epsIm = this.permittivityList[lower].Imaginary + coefWaveLength *
                (this.permittivityList[upper].Imaginary - this.permittivityList[lower].Imaginary);

            return new Complex(epsRe, epsIm);

            ////eps_re = 3.9943 - (13.29e+15*13.29e+15)/((4.0*Pi*Pi*3.0e8*3e8/WaveLength/WaveLength*1e18)+(0.1128e+15*0.1128e+15));
            ////eps_im = (13.29e+15*13.29e+15*0.1128e+15)/((4.0*Pi*Pi*3e8*3e8/WaveLength/WaveLength*1e18)+(0.1128e+15*0.1128e+15))/(2.0*Pi*3e8/WaveLength*1e9);

        }

        private Tuple<int, int> getNearestIndexes(double waveLength)
        {
            int i;
            for (i = 0; i < this.waveLengthList.Count - 1; i++)
            {
                if (waveLength >= this.waveLengthList[i] && waveLength < this.waveLengthList[i + 1])
                {
                    break;
                }
            }
            return new Tuple<int, int>(i, i + 1);
        }
    }
}
