using System.Linq;

using Simulation.DDA.Models;
using Simulation.Infrastructure;
using Simulation.Medium.Models;
using Simulation.Models.Enums;
using Simulation.Models.Extensions;
using Simulation.Models.Spectrum;

using Simulation.FDTD.Console;

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

            new SpectrumPlotter().Plot(result);
        }

        /// <summary>
        /// Calculate extinction using DDA method.
        /// </summary>
        /// <returns>The simulation result.</returns>
        public static SimulationResultDictionary Calculate()
        {
            return Calculate(
                "ddaParameters.xml",
                "opt_const.txt",
                ParameterHelper.ReadSystemConfigFromMesh(@"D:\FDSexamples\dipole.fds")
                //ParameterHelper.ReadSystemConfig("dipols.txt")
                );
        }

        /// <summary>
        /// Calculates extinction using DDA method.
        /// </summary>
        /// <param name="ddaConfigFilename">The DDA configuration filename.</param>
        /// <param name="optConstTxt">The optical constants filename.</param>
        /// <param name="systemConfig">The dipole system configuration.</param>
        /// <returns>The simulation result.</returns>
        public static SimulationResultDictionary Calculate(string ddaConfigFilename, string optConstTxt, SystemConfig systemConfig)
        {
            var ddaConfig =
                XmlSerializerHelper.DeserializeObject<DDAParameters>(ddaConfigFilename);

            return Calculate(ddaConfig, systemConfig, ParameterHelper.ReadOpticalConstants(optConstTxt));
        }

        /// <summary>
        /// Calculates extinction using DDA method.
        /// </summary>
        /// <param name="ddaConfig">The DDA configuration.</param>
        /// <param name="systemConfig">The dipole system configuration.</param>
        /// <param name="medium">The medium of the dipoles.</param>
        /// <returns>The simulation result.</returns>
        public static SimulationResultDictionary Calculate(DDAParameters ddaConfig, SystemConfig systemConfig, BaseMedium medium)
        {
            var manager = new MediumManager(medium, ddaConfig.IsSolidMaterial);
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