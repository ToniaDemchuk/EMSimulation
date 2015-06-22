using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using AwokeKnowing.GnuplotCSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Simulation.DDA.Console;
using Simulation.Infrastructure;
using Simulation.Models;

namespace Simulation.DDA.Tests
{
    [TestClass]
    public class SimulationTests
    {
        private TestContext testContextInstance;

        private DDAParameters ddaConfig;

        private OpticalConstants opticalConstants;

        /// <summary>
        /// Gets or sets the test context which provides
        /// information about and functionality for the current test run.
        /// </summary>
        /// <value>
        /// The test context.
        /// </value>
        public TestContext TestContext
        {
            get { return this.testContextInstance; }
            set { this.testContextInstance = value; }
        }

        /// <summary>
        /// Test initialization logic.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.ddaConfig = XmlSerializerHelper.DeserializeObject<DDAParameters>("ddaParameters.xml");
            this.opticalConstants = ParameterHelper.ReadOpticalConstants("opt_const.txt");
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

        #endregion

        [TestMethod]
        [Ignore]
        public void RadiusDistanceOutput_AzimuthSum()
        {
            // Arrange
            const int DistanceMax = 3;
            const double DistanceStep = 0.1;
            var radiuses = new List<double> { 4, 10, 20, 40, 70, 100, 200 };
            List<double> distances = getDistances(DistanceStep, DistanceMax);
            var dirAzimuth0 = "RadiusDistanceOutput_Azimuth0";
            var dirAzimuth90 = "RadiusDistanceOutput_Azimuth90";

            // Calculate
            foreach (double radius in radiuses)
            {
                foreach (double distance in distances)
                {
                    var azim0 = SimpleFormatter.Read(
                        this.getFileFormat(dirAzimuth0, distance, radius));

                    var azim90 = SimpleFormatter.Read(
                        this.getFileFormat(dirAzimuth90, distance, radius));

                    var azimSum = azim0.Zip(
                        azim90,
                        (x, y) =>
                        new KeyValuePair<double, double>(x.Key, (x.Value + y.Value) / 2))
                                       .ToDictionary(key => key.Key, value => value.Value);

                    var filename = this.getFileFormat(this.TestContext.TestName, distance, radius);
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
            var dirAzimuth0 = "RadiusDistanceOutput_Azimuth0_EffectiveCrossExt";
            var dirAzimuth90 = "RadiusDistanceOutput_Azimuth90_EffectiveCrossExt";

            // Calculate
            foreach (double radius in radiuses)
            {
                foreach (double distance in distances)
                {
                    var azim0 = SimpleFormatter.Read(
                        this.getFileFormat(dirAzimuth0, distance, radius));

                    var azim90 = SimpleFormatter.Read(
                        this.getFileFormat(dirAzimuth90, distance, radius));

                    var azimSum = azim0.Zip(
                        azim90,
                        (x, y) =>
                        new KeyValuePair<double, double>(x.Key, (x.Value + y.Value) / 2))
                                       .ToDictionary(key => key.Key, value => value.Value);

                    var filename = this.getFileFormat(this.TestContext.TestName, distance, radius);
                    SimpleFormatter.Write(filename, azimSum);
                }
            }
            this.writeParameters(radiuses, distances);
        }

        [TestMethod]
        public void AreClose_Azimuth45_AzimuthSum()
        {
            // Arrange
            const int DistanceMax = 3;
            const double DistanceStep = 0.1;
            var radiuses = new List<double> { 4, 10, 20, 40, 70, 100, 200 };
            List<double> distances = getDistances(DistanceStep, DistanceMax);
            var dirAzimuthSum = "RadiusDistanceOutput_AzimuthSum";
            var dirAzimuth45 = "RadiusDistanceOutput_Azimuth45";
            var gp = new GnuPlot();
            gp.Set("style data lines");

            foreach (double radius in radiuses)
            {
                foreach (double distance in distances)
                {
                    var azim45 = SimpleFormatter.Read(
                        this.getFileFormat(dirAzimuth45, distance, radius));
                    var azimSum = SimpleFormatter.Read(
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
            var dirAzimuthSum = "RadiusDistanceOutput_AzimuthSum_EffectiveCrossExt";
            var dirAzimuth45 = "RadiusDistanceOutput_Azimuth45_EffectiveCrossExt";

            foreach (double radius in radiuses)
            {
                foreach (double distance in distances)
                {
                    var azim45 = SimpleFormatter.Read(
                        this.getFileFormat(dirAzimuth45, distance, radius));
                    var azimSum = SimpleFormatter.Read(
                        this.getFileFormat(dirAzimuthSum, distance, radius));

                    AssertHelper.DictionaryAreClose(azim45, azimSum, 0.1);
                }
            }
        }

        [TestMethod]
        public void Peaks_Azimuth0_Azimuth90()
        {
            // Arrange
            const int DistanceMax = 3;
            const double DistanceStep = 0.1;
            var radiuses = new List<double> { 4, 10, 20, 40, 70, 100, 200 };
            List<double> distances = getDistances(DistanceStep, DistanceMax);
            var dirAzimuth0 = "RadiusDistanceOutput_Azimuth0";
            var dirAzimuth90 = "RadiusDistanceOutput_Azimuth90";
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
                    var azim0 = SimpleFormatter.Read(
                        this.getFileFormat(dirAzimuth0, distance, radius));
                    var azim90 = SimpleFormatter.Read(
                        this.getFileFormat(dirAzimuth90, distance, radius));

                    peaks0.Add(distance, azim0.Aggregate((l, r) => l.Value > r.Value ? l : r).Key);

                    peaks90.Add(distance, azim90.Aggregate((l, r) => l.Value > r.Value ? l : r).Key);
                }
                // gp.HoldOn();
                // gp.Set(string.Format("terminal win {0}", radius));
                gp.Plot(peaks0.Select(x => x.Key).ToArray(), peaks0.Select(x => x.Value).ToArray());
                gp.Plot(peaks90.Select(x => x.Key).ToArray(), peaks90.Select(x => x.Value).ToArray());
                // gp.HoldOff();
                var filename0 = string.Format(@"../{0}/peaks_0deg_{1}.txt", this.TestContext.TestName, radius);
                SimpleFormatter.Write(filename0, peaks0);
                var filename90 = string.Format(@"../{0}/peaks_90deg_{1}.txt", this.TestContext.TestName, radius);
                SimpleFormatter.Write(filename90, peaks90);
            }
            gp.Wait();
        }

        #region Private methods

        private static List<double> getDistances(double distanceStep, int distanceMax)
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

            var result = DDAProgram.Calculate(this.ddaConfig, systConfig, this.opticalConstants);

            var filename = this.getFileFormat(this.TestContext.TestName, distance, radius);

            SimpleFormatter.Write(filename, result.ToDictionary(x => x.ToType(SpectrumParameterType.WaveLength), valueSelector));
        }

        private string getFileFormat(string dirPath, double distance, double radius)
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                @"../{0}/rezult_ext_{1}_{2}.txt",
                dirPath,
                (decimal) distance,
                (decimal) radius);
        }

        private void writeParameters(List<double> radiuses, List<double> distances)
        {
            var filename = string.Format(CultureInfo.InvariantCulture, @"../{0}/parameters.txt", this.TestContext.TestName);
            using (var sw = new StreamWriter(filename))
            {
                sw.WriteLine(
                    "radius = {0}",
                    string.Join(
                        ",",
                        radiuses.Select(x => string.Format(CultureInfo.InvariantCulture, "{0}", (decimal) x))));
                sw.WriteLine(
                    "distance = {0}",
                    string.Join(
                        ",",
                        distances.Select(x => string.Format(CultureInfo.InvariantCulture, "{0}", (decimal) x))));
            }
        }

        #endregion
    }
}