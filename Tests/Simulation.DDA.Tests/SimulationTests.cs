using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

using AwokeKnowing.GnuplotCSharp;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Simulation.DDA.Console.Simulation;
using Simulation.DDA.Models;
using Simulation.Infrastructure;
using Simulation.Medium.Medium;
using Simulation.Medium.Models;
using Simulation.Models.Coordinates;
using Simulation.Models.Enums;
using Simulation.Models.Spectrum;

namespace Simulation.DDA.Tests
{
    [TestClass]
    public class SimulationTests
    {
        private const string BasePath = @"../../SimulationResults";

        private DDAParameters ddaConfig;

        private BaseMedium opticalConstants;

        /// <summary>
        ///     Gets or sets the test context which provides
        ///     information about and functionality for the current test run.
        /// </summary>
        /// <value>
        ///     The test context.
        /// </value>
        public TestContext TestContext { get; set; }

        /// <summary>
        ///     Test initialization logic.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.ddaConfig = XmlSerializerHelper.DeserializeObject<DDAParameters>("ddaParameters.xml");
            this.opticalConstants = ParameterHelper.ReadOpticalConstants("opt_const.txt");
        }

        [TestMethod]
        public void AreClose_Azimuth45_AzimuthSum()
        {
            // Arrange
            const int DistanceMax = 3;
            const double DistanceStep = 0.1;
            var radiuses = new List<double> { 4, 10, 20, 40, 70, 100, 200 };
            List<double> distances = getDistances(DistanceStep, DistanceMax);
            string dirAzimuthSum = "RadiusDistanceOutput_AzimuthSum";
            string dirAzimuth45 = "RadiusDistanceOutput_Azimuth45";
            var gp = new GnuPlot();
            gp.Set("style data lines");

            foreach (double radius in radiuses)
            {
                foreach (double distance in distances)
                {
                    Dictionary<double, double> azim45 = SimpleFormatter.Read(
                        this.getFileFormat(dirAzimuth45, distance, radius));
                    Dictionary<double, double> azimSum = SimpleFormatter.Read(
                        this.getFileFormat(dirAzimuthSum, distance, radius));
                    gp.HoldOn();

                    gp.Plot(azim45.Keys.ToArray(), azim45.Values.ToArray());
                    gp.Plot(azimSum.Keys.ToArray(), azimSum.Values.ToArray());
                    gp.HoldOff();
                    AssertHelper.DictionaryAreClose(azim45, azimSum, 0.5);
                }
            }
        }

        [TestMethod]
        public void AreClose_Azimuth45_AzimuthSum_EffectiveCrossExt()
        {
            // Arrange
            const int DistanceMax = 3;
            const double DistanceStep = 0.1;
            var radiuses = new List<double> { 4, 10, 20, 40, 70, 100, 200 };
            List<double> distances = getDistances(DistanceStep, DistanceMax);
            string dirAzimuthSum = "RadiusDistanceOutput_AzimuthSum_EffectiveCrossExt";
            string dirAzimuth45 = "RadiusDistanceOutput_Azimuth45_EffectiveCrossExt";

            foreach (double radius in radiuses)
            {
                foreach (double distance in distances)
                {
                    Dictionary<double, double> azim45 = SimpleFormatter.Read(
                        this.getFileFormat(dirAzimuth45, distance, radius));
                    Dictionary<double, double> azimSum = SimpleFormatter.Read(
                        this.getFileFormat(dirAzimuthSum, distance, radius));

                    AssertHelper.DictionaryAreClose(azim45, azimSum, 0.1);
                }
            }
        }

        [TestMethod]
        public void Radius10_Azimuth_Spectrum()
        {
            // Arrange
            const int DistanceMax = 3;
            const double DistanceStep = 0.1;
            int radius = 10;
            List<double> distances = getDistances(DistanceStep, DistanceMax);
            string dirAzimuth45 = "RadiusDistanceOutput_Azimuth45_EffectiveCrossExt";
            string dirAzimuth0 = "RadiusDistanceOutput_Azimuth0_EffectiveCrossExt";
            string dirAzimuth90 = "RadiusDistanceOutput_Azimuth90_EffectiveCrossExt";

            var gp = new GnuPlot();
            gp.Set("style data lines");
            gp.HoldOn();

            Dictionary<decimal, List<double>> spectrum = this.zipToDictionary(distances, dirAzimuth45, radius);
            string filename = Path.Combine(BasePath, this.TestContext.TestName, "Azim45.txt");
            SimpleFormatter.WriteDictionary(filename, spectrum, distances);

            foreach (double distance in distances)
            {
                Dictionary<double, double> azim45 = SimpleFormatter.Read(
                    this.getFileFormat(dirAzimuth45, distance, radius));
                gp.Plot(
                    azim45.Select(x => x.Key).ToArray(),
                    azim45.Select(x => x.Value).ToArray(),
                    string.Format(@"title ""{0}""", distance));
            }

            spectrum = this.zipToDictionary(distances, dirAzimuth0, radius);
            filename = Path.Combine(BasePath, this.TestContext.TestName, "Azim0.txt");
            SimpleFormatter.WriteDictionary(filename, spectrum, distances);

            spectrum = this.zipToDictionary(distances, dirAzimuth90, radius);
            filename = Path.Combine(BasePath, this.TestContext.TestName, "Azim90.txt");
            SimpleFormatter.WriteDictionary(filename, spectrum, distances);

            gp.Wait();
        }

        [TestMethod]
        public void Peaks_Azimuth0_Azimuth90()
        {
            // Arrange
            const int DistanceMax = 3;
            const double DistanceStep = 0.1;
            var radiuses = new List<double> { 4, 10, 20, 40, 70, 100, 200 };
            List<double> distances = getDistances(DistanceStep, DistanceMax);
            string dirAzimuth0 = "RadiusDistanceOutput_Azimuth0";
            string dirAzimuth90 = "RadiusDistanceOutput_Azimuth90";
            var gp = new GnuPlot();
            gp.Set("style data lines");
            gp.HoldOn();
            radiuses.Reverse();
            foreach (double radius in radiuses)
            {
                var peaks0 = new Dictionary<double, double>();

                var peaks90 = new Dictionary<double, double>();

                foreach (double distance in distances)
                {
                    Dictionary<double, double> azim0 = SimpleFormatter.Read(
                        this.getFileFormat(dirAzimuth0, distance, radius));
                    Dictionary<double, double> azim90 = SimpleFormatter.Read(
                        this.getFileFormat(dirAzimuth90, distance, radius));

                    peaks0.Add(distance, azim0.Aggregate((l, r) => l.Value > r.Value ? l : r).Key);

                    peaks90.Add(distance, azim90.Aggregate((l, r) => l.Value > r.Value ? l : r).Key);
                }
                // gp.HoldOn();
                // gp.Set(string.Format("terminal win {0}", radius));
                gp.Plot(peaks0.Select(x => x.Key).ToArray(), peaks0.Select(x => x.Value).ToArray());
                gp.Plot(peaks90.Select(x => x.Key).ToArray(), peaks90.Select(x => x.Value).ToArray());
                // gp.HoldOff();

                string basepath = Path.Combine(BasePath, this.TestContext.TestName);
                string filename0 = Path.Combine(
                    basepath,
                    string.Format("peaks_0deg_{0}.txt", radius));
                SimpleFormatter.Write(filename0, peaks0);
                string filename90 = Path.Combine(
                    basepath,
                    string.Format("peaks_90deg_{0}.txt", radius));
                SimpleFormatter.Write(filename90, peaks90);
            }
            gp.Wait();
        }

        [TestMethod]
        //[Ignore]
        public void RadiusDistanceDrudeLorentz_Azimuth45_EffectiveCrossExt()
        {
            // Arrange
            const int DistanceMax = 3;
            const double DistanceStep = 0.1;
            List<double> distances = getDistances(DistanceStep, DistanceMax);
            var radiuses = new List<double> { 4, 10, 20, 40, 70, 100, 200 };
            this.ddaConfig.IncidentMagnitude.Azimuth = 45;
            this.opticalConstants = new DrudeLorentz();
            double maxRadius = radiuses.Max();
            foreach (double radius in radiuses)
            {
                foreach (double distance in distances)
                {
                    this.calculateOneStep(radius, distance, maxRadius, x => x.EffectiveCrossSectionExtinction);
                }
            }

            this.writeParameters(radiuses, distances);
        }

        [TestMethod]
        public void AreClose_Radius40_OpticalConst_DrudeLorentz()
        {
            // Arrange
            const double DistanceMax = 0.5;
            const double DistanceStep = 0.1;
            List<double> distances = getDistances(DistanceStep, DistanceMax);
            var radius = 40;

            string dirOpticalConst = "RadiusDistanceOutput_Azimuth45_EffectiveCrossExt";
            string dirDrudeLorentz = "RadiusDistanceDrudeLorentz_Azimuth45_EffectiveCrossExt";
            var gp = new GnuPlot();
            gp.Set("style data lines");
            gp.HoldOn();

            foreach (double distance in distances)
            {
                Dictionary<double, double> optical = SimpleFormatter.Read(
                    this.getFileFormat(dirOpticalConst, distance, radius));
                Dictionary<double, double> drudeLorentz = SimpleFormatter.Read(
                    this.getFileFormat(dirDrudeLorentz, distance, radius));

                gp.Plot(
                    optical.Select(x => x.Key).ToArray(),
                    optical.Select(x => x.Value).ToArray(),
                    @"title ""optical""");
                gp.Plot(
                    drudeLorentz.Select(x => x.Key).ToArray(),
                    drudeLorentz.Select(x => x.Value).ToArray(),
                    @"title ""drude-lorentz""");

                //AssertHelper.DictionaryAreClose(optical, drudeLorentz, 0.1);
            }

            gp.Wait();
        }

        [TestMethod]
        public void AreCloseDistance3_OpticalConst_DrudeLorentz()
        {
            // Arrange
            var distance = 2.9;
            var radiuses = new List<double> { 4, 10, 20, 40, 70, 100, 200 };

            string dirOpticalConst = "RadiusDistanceOutput_Azimuth45_EffectiveCrossExt";
            string dirDrudeLorentz = "RadiusDistanceDrudeLorentz_Azimuth45_EffectiveCrossExt";
            var gp = new GnuPlot();
            gp.Set("style data lines");
            gp.HoldOn();

            foreach (double radius in radiuses)
            {
                Dictionary<double, double> optical = SimpleFormatter.Read(
                    this.getFileFormat(dirOpticalConst, distance, radius));
                Dictionary<double, double> drudeLorentz = SimpleFormatter.Read(
                    this.getFileFormat(dirDrudeLorentz, distance, radius));

                gp.Plot(
                    optical.Select(x => x.Key).ToArray(),
                    optical.Select(x => x.Value).ToArray(),
                    @"title ""optical""");
                gp.Plot(
                    drudeLorentz.Select(x => x.Key).ToArray(),
                    drudeLorentz.Select(x => x.Value).ToArray(),
                    @"title ""drude-lorentz""");

                //AssertHelper.DictionaryAreClose(optical, drudeLorentz, 0.1);
            }

            gp.Wait();
        }

        #region Output simulations

        [TestMethod]
        [Ignore]
        public void RadiusDistanceOutput_Azimuth45()
        {
            // Arrange
            const int DistanceMax = 3;
            const double DistanceStep = 0.1;
            List<double> distances = getDistances(DistanceStep, DistanceMax);
            var radiuses = new List<double> { 4, 10, 20, 40, 70, 100, 200 };
            this.ddaConfig.IncidentMagnitude.Azimuth = 45;

            double maxRadius = radiuses.Max();

            foreach (double radius in radiuses)
            {
                foreach (double distance in distances)
                {
                    this.calculateOneStep(radius, distance, maxRadius, x => x.CrossSectionExtinction);
                }
            }

            this.writeParameters(radiuses, distances);
        }

        [TestMethod]
        [Ignore]
        public void RadiusDistanceOutput_Azimuth0()
        {
            // Arrange
            const int DistanceMax = 3;
            const double DistanceStep = 0.1;
            List<double> distances = getDistances(DistanceStep, DistanceMax);
            var radiuses = new List<double> { 4, 10, 20, 40, 70, 100, 200 };

            this.ddaConfig.IncidentMagnitude.Azimuth = 0;

            double maxRadius = radiuses.Max();

            foreach (double radius in radiuses)
            {
                foreach (double distance in distances)
                {
                    this.calculateOneStep(radius, distance, maxRadius, x => x.CrossSectionExtinction);
                }
            }
            this.writeParameters(radiuses, distances);
        }

        [TestMethod]
        [Ignore]
        public void RadiusDistanceOutput_Azimuth90()
        {
            // Arrange
            const int DistanceMax = 3;
            const double DistanceStep = 0.1;
            List<double> distances = getDistances(DistanceStep, DistanceMax);
            var radiuses = new List<double> { 4, 10, 20, 40, 70, 100, 200 };
            this.ddaConfig.IncidentMagnitude.Azimuth = 90;

            double maxRadius = radiuses.Max();

            foreach (double radius in radiuses)
            {
                foreach (double distance in distances)
                {
                    this.calculateOneStep(radius, distance, maxRadius, x => x.CrossSectionExtinction);
                }
            }
            this.writeParameters(radiuses, distances);
        }

        [TestMethod]
        [Ignore]
        public void RadiusDistanceOutput_Azimuth45_EffectiveCrossExt()
        {
            // Arrange
            const int DistanceMax = 3;
            const double DistanceStep = 0.1;
            List<double> distances = getDistances(DistanceStep, DistanceMax);
            var radiuses = new List<double> { 4, 10, 20, 40, 70, 100, 200 };
            this.ddaConfig.IncidentMagnitude.Azimuth = 45;

            double maxRadius = radiuses.Max();

            foreach (double radius in radiuses)
            {
                foreach (double distance in distances)
                {
                    this.calculateOneStep(radius, distance, maxRadius, x => x.EffectiveCrossSectionExtinction);
                }
            }
            this.writeParameters(radiuses, distances);
        }

        [TestMethod]
        [Ignore]
        public void RadiusDistanceOutput_Azimuth0_EffectiveCrossExt()
        {
            // Arrange
            const int DistanceMax = 3;
            const double DistanceStep = 0.1;
            List<double> distances = getDistances(DistanceStep, DistanceMax);
            var radiuses = new List<double> { 4, 10, 20, 40, 70, 100, 200 };
            this.ddaConfig.IncidentMagnitude.Azimuth = 0;

            double maxRadius = radiuses.Max();

            foreach (double radius in radiuses)
            {
                foreach (double distance in distances)
                {
                    this.calculateOneStep(radius, distance, maxRadius, x => x.EffectiveCrossSectionExtinction);
                }
            }
            this.writeParameters(radiuses, distances);
        }

        [TestMethod]
        [Ignore]
        public void RadiusDistanceOutput_Azimuth90_EffectiveCrossExt()
        {
            // Arrange
            const int DistanceMax = 3;
            const double DistanceStep = 0.1;
            List<double> distances = getDistances(DistanceStep, DistanceMax);
            var radiuses = new List<double> { 4, 10, 20, 40, 70, 100, 200 };
            this.ddaConfig.IncidentMagnitude.Azimuth = 90;

            double maxRadius = radiuses.Max();

            foreach (double radius in radiuses)
            {
                foreach (double distance in distances)
                {
                    this.calculateOneStep(radius, distance, maxRadius, x => x.EffectiveCrossSectionExtinction);
                }
            }
            this.writeParameters(radiuses, distances);
        }


        [TestMethod]
        [Ignore]
        public void RadiusDistanceOutput_AzimuthSum()
        {
            // Arrange
            const int DistanceMax = 3;
            const double DistanceStep = 0.1;
            var radiuses = new List<double> { 4, 10, 20, 40, 70, 100, 200 };
            List<double> distances = getDistances(DistanceStep, DistanceMax);
            string dirAzimuth0 = "RadiusDistanceOutput_Azimuth0";
            string dirAzimuth90 = "RadiusDistanceOutput_Azimuth90";

            // Calculate
            foreach (double radius in radiuses)
            {
                foreach (double distance in distances)
                {
                    Dictionary<double, double> azim0 = SimpleFormatter.Read(
                        this.getFileFormat(dirAzimuth0, distance, radius));

                    Dictionary<double, double> azim90 = SimpleFormatter.Read(
                        this.getFileFormat(dirAzimuth90, distance, radius));

                    Dictionary<double, double> azimSum = azim0.Zip(
                        azim90,
                        (x, y) =>
                        new KeyValuePair<double, double>(x.Key, (x.Value + y.Value) / 2))
                                                              .ToDictionary(key => key.Key, value => value.Value);

                    string filename = this.getFileFormat(this.TestContext.TestName, distance, radius);
                    SimpleFormatter.Write(filename, azimSum);
                }
            }
            this.writeParameters(radiuses, distances);
        }

        [TestMethod]
        [Ignore]
        public void RadiusDistanceOutput_AzimuthSum_EffectiveCrossExt()
        {
            // Arrange
            const int DistanceMax = 3;
            const double DistanceStep = 0.1;
            var radiuses = new List<double> { 4, 10, 20, 40, 70, 100, 200 };
            List<double> distances = getDistances(DistanceStep, DistanceMax);
            string dirAzimuth0 = "RadiusDistanceOutput_Azimuth0_EffectiveCrossExt";
            string dirAzimuth90 = "RadiusDistanceOutput_Azimuth90_EffectiveCrossExt";

            // Calculate
            foreach (double radius in radiuses)
            {
                foreach (double distance in distances)
                {
                    Dictionary<double, double> azim0 = SimpleFormatter.Read(
                        this.getFileFormat(dirAzimuth0, distance, radius));

                    Dictionary<double, double> azim90 = SimpleFormatter.Read(
                        this.getFileFormat(dirAzimuth90, distance, radius));

                    Dictionary<double, double> azimSum = azim0.Zip(
                        azim90,
                        (x, y) =>
                        new KeyValuePair<double, double>(x.Key, (x.Value + y.Value) / 2))
                                                              .ToDictionary(key => key.Key, value => value.Value);

                    string filename = this.getFileFormat(this.TestContext.TestName, distance, radius);
                    SimpleFormatter.Write(filename, azimSum);
                }
            }
            this.writeParameters(radiuses, distances);
        }

        #endregion

        #region Private methods

        private static List<double> getDistances(double distanceStep, double distanceMax)
        {
            var distances = new List<double>();
            for (double distance = 0; distance <= distanceMax; distance += distanceStep)
            {
                distances.Add(distance);
            }
            return distances;
        }

        private void calculateOneStep(
            double radius,
            double distance,
            double maxRadius,
            Func<SimulationResult, double> valueSelector)
        {
            var firstPoint = new CartesianCoordinate(maxRadius, maxRadius, 0);

            double secondPointCoord = maxRadius + 2 * radius + distance * radius;
            var secondPoint = new CartesianCoordinate(maxRadius, secondPointCoord, 0);

            var systConfig = new SystemConfig(
                new List<double>
                {
                    radius,
                    radius
                },
                new List<CartesianCoordinate>
                {
                    firstPoint,
                    secondPoint
                });

            SimulationResultDictionary result = DDAProgram.Calculate(this.ddaConfig, systConfig, this.opticalConstants);

            string filename = this.getFileFormat(this.TestContext.TestName, distance, radius);

            SimpleFormatter.Write(
                filename,
                result.ToDictionary(x => x.ToType(SpectrumUnitType.WaveLength), valueSelector));
        }

        private string getFileFormat(string dirPath, double distance, double radius)
        {
            string format = string.Format(
                CultureInfo.InvariantCulture,
                "rezult_ext_{0}_{1}.txt",
                (decimal) distance,
                (decimal) radius);
            return Path.Combine(BasePath, dirPath, format);
        }

        private void writeParameters(List<double> radiuses, List<double> distances)
        {
            string filename = Path.Combine(BasePath, this.TestContext.TestName, "parameters.txt");
            using (var sw = new StreamWriter(filename))
            {
                string radiusJoin = string.Join(
                    ",",
                    radiuses.Select(x => ((decimal) x).ToString(CultureInfo.InvariantCulture)));
                sw.WriteLine("radius = {0}", radiusJoin);
                string distanceJoin = string.Join(
                    ",",
                    distances.Select(x => ((decimal) x).ToString(CultureInfo.InvariantCulture)));
                sw.WriteLine("distance = {0}", distanceJoin);
            }
        }

        private Dictionary<decimal, List<double>> zipToDictionary(List<double> distances, string dirAzim, int radius)
        {
            var spectrum = new Dictionary<decimal, List<double>>();
            distances.Aggregate(
                spectrum,
                (dict, distance) =>
                {
                    Dictionary<double, double> azimuthDict = SimpleFormatter.Read(
                        this.getFileFormat(dirAzim, distance, radius));
                    foreach (KeyValuePair<double, double> azim in azimuthDict)
                    {
                        if (!dict.ContainsKey((decimal) azim.Key))
                        {
                            dict[(decimal) azim.Key] = new List<double>();
                        }
                        dict[(decimal) azim.Key].Add(azim.Value);
                    }
                    return dict;
                });
            return spectrum;
        }

        #endregion
    }
}