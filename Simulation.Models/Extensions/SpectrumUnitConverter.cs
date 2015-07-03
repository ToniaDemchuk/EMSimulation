using System;
using System.Collections.Generic;

using Simulation.Models.Constants;
using Simulation.Models.Enums;

namespace Simulation.Models.Extensions
{
    /// <summary>
    /// The SpectrumParameterConverter class.
    /// </summary>
    public static class SpectrumUnitConverter
    {
        private const double TwoPI = 2.0 * Math.PI;

        private static readonly Dictionary<SpectrumUnitType, Func<double, double>> ConvertFromType =
            new Dictionary<SpectrumUnitType, Func<double, double>>
            {
                { SpectrumUnitType.CycleFrequency, v => v },
                { SpectrumUnitType.Frequency, v => TwoPI * v },
                { SpectrumUnitType.PhotonEnergy, v => v / Fundamentals.ReducedPlanckConst },
                { SpectrumUnitType.EVEnergy, v => v * Fundamentals.ElementaryCharge / Fundamentals.ReducedPlanckConst },
                { SpectrumUnitType.WaveLength, v => TwoPI * Fundamentals.SpeedOfLight / v },
                { SpectrumUnitType.WaveNumber, v => Fundamentals.SpeedOfLight * v }
            };

        private static readonly Dictionary<SpectrumUnitType, Func<double, double>> ConvertToType =
            new Dictionary<SpectrumUnitType, Func<double, double>>
            {
                { SpectrumUnitType.CycleFrequency, v => v },
                { SpectrumUnitType.Frequency, v => v / TwoPI },
                { SpectrumUnitType.PhotonEnergy, v => Fundamentals.ReducedPlanckConst * v },
                { SpectrumUnitType.EVEnergy, v => Fundamentals.ReducedPlanckConst * v / Fundamentals.ElementaryCharge },
                { SpectrumUnitType.WaveLength, v => TwoPI * Fundamentals.SpeedOfLight / v },
                { SpectrumUnitType.WaveNumber, v => v / Fundamentals.SpeedOfLight }
            };

        /// <summary>
        /// Converts the specified value to value of another spectrum parameter type.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="fromType">From spectrum parameter type.</param>
        /// <param name="toType">To spectrum parameter type.</param>
        /// <returns>The new value of spectrum parameter type.</returns>
        public static double Convert(double value, SpectrumUnitType fromType, SpectrumUnitType toType)
        {
            if (fromType == toType)
            {
                return value;
            }

            var cycleFreq = ConvertFromType[fromType].Invoke(value);

            return ConvertToType[toType].Invoke(cycleFreq);
        }
    }
}