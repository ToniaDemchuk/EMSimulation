using System.Collections.Generic;
using System.Linq;
using System.Numerics;

using Simulation.Models.Coordinates;
using Simulation.Models.Enums;

namespace Simulation.Models.Spectrum
{
    /// <summary>
    /// The FourierSeries class.
    /// </summary>
    public class FourierSeries : Dictionary<int, double>
    {
        /// <summary>
        /// Get the Fourier transform for the specified.
        /// </summary>
        /// <param name="spec">The spec.</param>
        /// <param name="timeStep">The time step.</param>
        /// <returns></returns>
        public Complex Transform(SpectrumUnit spec, double timeStep)
        {
            var freq = spec.ToType(SpectrumUnitType.CycleFrequency);

            return this.Aggregate(
                Complex.Zero,
                (sum, x) => sum + Complex.FromPolarCoordinates(x.Value, freq * timeStep * x.Key));
        }
    }

    /// <summary>
    /// The FourierSeriesCoordinate class.
    /// </summary>
    public class FourierSeriesCoordinate : Dictionary<int, CartesianCoordinate>
    {
        public ComplexCoordinate Transform(SpectrumUnit spec, double timeStep)
        {
            var freq = spec.ToType(SpectrumUnitType.CycleFrequency);
            return this.Aggregate(
                ComplexCoordinate.Zero,
                (sum, x) => sum + ComplexCoordinate.FromPolarCoordinates(x.Value, freq * timeStep * x.Key));
        }
    }
}