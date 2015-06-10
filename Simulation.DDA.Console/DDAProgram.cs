using System;
using System.Collections.Generic;
using System.Linq;

using Simulation.Infrastructure;
using Simulation.Models;
using Simulation.Models.Extensions;

namespace Simulation.DDA.Console
{
    /// <summary>
    /// The Program class.
    /// </summary>
    public class DDAProgram
    {
        public static void Main(string[] args)
        {
            var result = Calculate();

            SimpleFormatter.Write("rezult_ext.txt", result, x=>x.CrossSectionExtinction);
        }

        public static Dictionary<double, SimulationResult> Calculate()
        {
            return Calculate(
                "ddaParameters.xml",
                "opt_const.txt",
                ParameterHelper.ReadSystemConfig("dipols.txt"));
        }

        public static Dictionary<double, SimulationResult> Calculate(string ddaConfigFilename, string optConstTxt, SystemConfig systemConfig)
        {
            var ddaConfig =
                XmlSerializerHelper.DeserializeObject<DDAParameters>(ddaConfigFilename);

            return Calculate(ddaConfig, systemConfig, ParameterHelper.ReadOpticalConstants(optConstTxt));
        }

        public static Dictionary<double, SimulationResult> Calculate(DDAParameters ddaConfig, SystemConfig systemConfig, OpticalConstants readOpticalConstants)
        {
            var manager = new MediumManager(readOpticalConstants, ddaConfig.SolidMaterial);
            var ext = new ExtinctionManager(manager);

            SimulationParameters parameters = new SimulationParameters
            {
                WavePropagation = ddaConfig.WavePropagation,
                IncidentMagnitude = ddaConfig.IncidentMagnitude.ConvertToCartesian(),
                SystemConfig = systemConfig,
                WaveConfig = ParameterHelper.ReadWavelengthFromConfigration(ddaConfig)
            };

            return ext.CalculateCrossExtinction(parameters);
        }

    }
}
