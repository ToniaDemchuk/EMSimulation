using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;

using Simulation.Models.Coordinates;
using Simulation.Models.Enums;

namespace Simulation.Models.Spectrum
{
    /// <summary>
    /// The FourierSeries class.
    /// </summary>
    public class FourierSeries : List<double>
    {
        public FourierSeries()
        {
        }

        public FourierSeries(int capacity)
            : base(capacity)
        {
        }
        /// <summary>
        /// Get the Fourier transform for the specified.
        /// </summary>
        /// <param name="spec">The spec.</param>
        /// <param name="timeStep">The time step.</param>
        /// <returns></returns>
        public Complex Transform(SpectrumUnit spec, double timeStep)
        {
            var freq = spec.ToType(SpectrumUnitType.CycleFrequency);
            var seed = Complex.Zero;
            for (int i = 0; i < this.Count; i++)
            {
                seed += Complex.FromPolarCoordinates(this[i], freq * timeStep * i);
            }
            return seed;
        }
    }

    /// <summary>
    /// The FourierSeriesCoordinate class.
    /// </summary>
    public class FourierSeriesCoordinate : List<CartesianCoordinate>
    {
        public FourierSeriesCoordinate()
        {
        }

        public FourierSeriesCoordinate(int capacity) : base(capacity)
        {
        }

        public ComplexCoordinate Transform(SpectrumUnit spec, double timeStep)
        {
            var freq = spec.ToType(SpectrumUnitType.CycleFrequency);
            var seed = ComplexCoordinate.Zero;
            for (int i = 0; i < this.Count; i++)
            {
                seed += ComplexCoordinate.FromPolarCoordinates(this[i], freq * timeStep * i);
            }
            return seed;

        }
    }
}