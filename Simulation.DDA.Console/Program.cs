using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;

using Simulation.Infrastructure;
using Simulation.Models;

namespace Simulation.DDA.Console
{
    public class Program
    {
        static void Main(string[] args)
        {
            
            var result = Calculate();

            SimpleFormatter.Write("rezult_ext.txt", result);
        }

        public static Dictionary<double, double> Calculate()
        {
            bool solid = false;
            var manager = new MediumManager(ParameterHelper.ReadOpticalConstants("opt_const.txt"), solid);
            var ext = new ExtinctionManager(manager);

            SimulationParameters parameters = new SimulationParameters
            {
                WavePropagation = ParameterHelper.ReadWavePropagationFromConfiguration("ddaParameters.xml"),
                IncidentMagnitude = ParameterHelper.ReadIncidentMagnitudeFromConfiguration("ddaParameters.xml"),
                SystemConfig = ParameterHelper.ReadSystemConfig("dipols.txt"),
                WaveConfig = ParameterHelper.ReadWavelengthFromConfigration("ddaParameters.xml")
            };

            return ext.CalculateCrossExtinction(parameters);
        }

    }
}
