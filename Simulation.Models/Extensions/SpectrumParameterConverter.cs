using System;
using System.Collections.Generic;

namespace Simulation.Models
{
    public static class SpectrumParameterConverter
    {

        private const double TwoPI = 2.0 * Math.PI;

        private static readonly Dictionary<SpectrumParameterType, Func<double, double>> ConvertFromType = new Dictionary
            <SpectrumParameterType, Func<double, double>>
        {
            {SpectrumParameterType.CycleFrequency, v => v},
            {SpectrumParameterType.Frequency, v => TwoPI * v},
            {SpectrumParameterType.PhotonEnergy, v => v / Fundamentals.ReducedPlanckConst},
            {SpectrumParameterType.EVEnergy, v => v * Fundamentals.QElektron / Fundamentals.ReducedPlanckConst},
            {SpectrumParameterType.WaveLength, v => TwoPI * Fundamentals.LightVelocity / v},
            {SpectrumParameterType.WaveNumber, v => Fundamentals.LightVelocity * v}
        };

        private static readonly Dictionary<SpectrumParameterType, Func<double, double>> ConvertToType = new Dictionary
            <SpectrumParameterType, Func<double, double>>
        {
            {SpectrumParameterType.CycleFrequency, v => v},
            {SpectrumParameterType.Frequency, v => v / TwoPI},
            {SpectrumParameterType.PhotonEnergy, v => Fundamentals.ReducedPlanckConst * v},
            {SpectrumParameterType.EVEnergy, v => Fundamentals.ReducedPlanckConst * v / Fundamentals.QElektron},
            {SpectrumParameterType.WaveLength, v => TwoPI * Fundamentals.LightVelocity / v},
            {SpectrumParameterType.WaveNumber, v => v / Fundamentals.LightVelocity}
        };


        public static double Convert(double value, SpectrumParameterType fromType, SpectrumParameterType toType)
        {
            if (fromType == toType)
                return value;

            var cycleFreq = ConvertFromType[fromType].Invoke(value);

            return ConvertToType[toType].Invoke(cycleFreq);
        }

    }
}