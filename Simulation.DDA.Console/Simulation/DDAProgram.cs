using System.Linq;

using Simulation.DDA.Models;
using Simulation.Infrastructure;
using Simulation.Medium.Models;
using Simulation.Models.Enums;
using Simulation.Models.Extensions;
using Simulation.Models.Spectrum;

namespace Simulation.DDA.Console.Simulation
{
    /// <summary>
    /// The Program class.
    /// </summary>
    public class DDAProgram
    {
        /// <summary>
        /// Mains the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public static void Main(string[] args)
        {
            var result = Calculate();

            SimpleFormatter.Write(
                "rezult_ext.txt",
                result.ToDictionary(
                    x => x.Key.ToType(SpectrumUnitType.WaveLength),
                    x => x.Value.CrossSectionExtinction));
        }

        public static SimulationResultDictionary Calculate()
        {
            return Calculate(
                "ddaParameters.xml",
                "opt_const.txt",
                ParameterHelper.ReadSystemConfig("dipols.txt"));
        }

        public static SimulationResultDictionary Calculate(string ddaConfigFilename, string optConstTxt, SystemConfig systemConfig)
        {
            var ddaConfig =
                XmlSerializerHelper.DeserializeObject<DDAParameters>(ddaConfigFilename);

            return Calculate(ddaConfig, systemConfig, ParameterHelper.ReadOpticalConstants(optConstTxt));
        }

        public static SimulationResultDictionary Calculate(DDAParameters ddaConfig, SystemConfig systemConfig, BaseMedium readOpticalConstants)
        {
            var manager = new MediumManager(readOpticalConstants, ddaConfig.IsSolidMaterial);
            var ext = new ExtinctionManager(manager);

            SimulationParameters parameters = new SimulationParameters
            {
                WavePropagation = ddaConfig.WavePropagation,
                IncidentMagnitude = ddaConfig.IncidentMagnitude.ConvertToCartesian(),
                SystemConfig = systemConfig,
                Spectrum = ParameterHelper.ReadWavelengthFromConfiguration(ddaConfig)
            };

            return ext.CalculateCrossExtinction(parameters);
        }
    }
}