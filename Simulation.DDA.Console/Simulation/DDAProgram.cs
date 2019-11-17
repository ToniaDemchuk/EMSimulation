using System.Linq;

using Simulation.DDA.Models;
using Simulation.Infrastructure;
using Simulation.Infrastructure.Plotters;
using Simulation.Medium.Models;
using Simulation.Models.Enums;
using Simulation.Models.Extensions;
using Simulation.Models.Spectrum;
using System.Collections.Generic;
using System;
using Simulation.Medium.Medium;

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

            Dictionary<SpectrumUnit, double> secondDer = GetSecondDerivative(result, x => x.EffectiveCrossSectionExtinction);

            SimpleFormatter.Write(
                "rezult_ext.txt",
                result.ToDictionary(
                    x => x.Key.ToType(SpectrumUnitType.WaveLength),
                    x => x.Value.EffectiveCrossSectionExtinction));

            //new DerivativePlotter().Plot(secondDer);
            //new SpectrumPlotter().Plot(result);
            //new IncidentPlotter().Plot(result);
        }

        private static Dictionary<SpectrumUnit, double> GetSecondDerivative<TValue>(IDictionary<SpectrumUnit, TValue> result, Func<TValue, double> valueSelector)
        {
            var secondDer = new Dictionary<SpectrumUnit, double>();

            KeyValuePair<SpectrumUnit, TValue>? prev1 = null;
            KeyValuePair<SpectrumUnit, TValue>? prev2 = null;
            foreach (var res in result)
            {
                if (prev1.HasValue && prev2.HasValue)
                {
                    var keyDiff1 = res.Key.ToType(SpectrumUnitType.WaveLength) - prev2.Value.Key.ToType(SpectrumUnitType.WaveLength);
                    var keyDiff2 = prev2.Value.Key.ToType(SpectrumUnitType.WaveLength) - prev1.Value.Key.ToType(SpectrumUnitType.WaveLength);
                    var valDiff = valueSelector(res.Value) - 2 * valueSelector(prev2.Value.Value) + valueSelector(prev1.Value.Value);
                    secondDer.Add(prev2.Value.Key, (valDiff) / (keyDiff1 * keyDiff2));

                }
                prev1 = prev2;
                prev2 = res;
            }

            return secondDer;
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
                //ParameterHelper.ReadSystemConfigFromMesh(@"E:\Dropbox\DDA_Blender_issue\blender_models\sphere.fds")
                //ParameterHelper.ReadSystemConfigFromMesh(@"E:\dispersion_model\dimer3ort.fds")
                //ParameterHelper.ReadSystemConfig("dipols.txt")
                //ParameterHelper.ReadSystemArray(6, 10)
				ParameterHelper.ReadSystemCube(radius: 5, size: 12)
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
            return Calculate(ddaConfig, systemConfig,
                //new DrudeLorentz()
                ParameterHelper.ReadOpticalConstants(optConstTxt)
                );
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