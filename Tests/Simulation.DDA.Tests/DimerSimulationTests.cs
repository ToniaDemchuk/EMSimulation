using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Globalization;
using System.IO;
using System.Linq;

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
    [Ignore]
    public class DimerSimulationTests
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

        [TestMethod]
        public void RadiusChangeOutput_Azimuth45()
        {
            // Arrange
            const int DistanceMax = 3;
            const double DistanceStep = 0.5;
            List<double> distances = getDistances(DistanceStep, DistanceMax);
            var radiuses = new List<double> { 4, 10, 20, 40 };
            //var radius1 = 4;
            this.ddaConfig.IncidentMagnitude.Azimuth = 45;

            double maxRadius = radiuses.Max();
            foreach (var radius1 in radiuses)
            foreach (double radius in radiuses)
            {
                foreach (double distance in distances)
                {
                    this.calculateOneStepDiffRadius(radius1, radius, distance, maxRadius, x => x.CrossSectionExtinction);
                }
            }

            this.writeParameters(radiuses, distances);
        }

        [TestMethod]
        public void RadiusChangeOutput_Azimuth0()
        {
            // Arrange
            const int DistanceMax = 3;
            const double DistanceStep = 0.5;
            List<double> distances = getDistances(DistanceStep, DistanceMax);
            var radiuses = new List<double> { 4, 10, 20, 40 };
            //var radius1 = 4;
            this.ddaConfig.IncidentMagnitude.Azimuth = 0;

            double maxRadius = radiuses.Max();

            foreach (double radius1 in radiuses)
                foreach (double radius in radiuses)
            {
                foreach (double distance in distances)
                {
                    this.calculateOneStepDiffRadius(radius1, radius, distance, maxRadius, x => x.CrossSectionExtinction);
                }
            }

            this.writeParameters(radiuses, distances);
        }

        [TestMethod]
        public void RadiusChangeOutput_Azimuth90()
        {
            // Arrange
            const int DistanceMax = 3;
            const double DistanceStep = 0.5;
            List<double> distances = getDistances(DistanceStep, DistanceMax);
            var radiuses = new List<double> {4, 10, 20, 40};
            //var radius1 = 4;
            this.ddaConfig.IncidentMagnitude.Azimuth = 90;

            double maxRadius = radiuses.Max();
            foreach (var radius1 in radiuses)
            {

                foreach (double radius in radiuses)
                {
                    foreach (double distance in distances)
                    {
                        this.calculateOneStepDiffRadius(radius1, radius, distance, maxRadius,
                            x => x.CrossSectionExtinction);
                    }
                }
            }

            this.writeParameters(radiuses, distances);
        }


        [TestMethod]
        public void RadiusChangeOutput_Azimuth45_EffectiveExtinction()
        {
            // Arrange
            const int DistanceMax = 3;
            const double DistanceStep = 0.5;
            List<double> distances = getDistances(DistanceStep, DistanceMax);
            var radiuses = new List<double> {4, 10, 20, 40};
            //var radius1 = 4;
            this.ddaConfig.IncidentMagnitude.Azimuth = 45;

            double maxRadius = radiuses.Max();

            foreach (double radius1 in radiuses)
            foreach (double radius in radiuses)
            {
                foreach (double distance in distances)
                {
                    this.calculateOneStepDiffRadius(radius1, radius, distance, maxRadius,
                        x => x.EffectiveCrossSectionExtinction);
                }
            }

            this.writeParameters(radiuses, distances);
        }

        [TestMethod]
        public void RadiusChangeOutput_Azimuth0_EffectiveExtinction()
        {
            // Arrange
            const int DistanceMax = 3;
            const double DistanceStep = 0.5;
            List<double> distances = getDistances(DistanceStep, DistanceMax);
            var radiuses = new List<double> { 4, 10, 20, 40 };
            //var radius1 = 4;
            this.ddaConfig.IncidentMagnitude.Azimuth = 0;

            double maxRadius = radiuses.Max();
            foreach (double radius1 in radiuses)
            foreach (double radius in radiuses)
            {
                foreach (double distance in distances)
                {
                    this.calculateOneStepDiffRadius(radius1, radius, distance, maxRadius, x => x.EffectiveCrossSectionExtinction);
                }
            }

            this.writeParameters(radiuses, distances);
        }

        [TestMethod]
        public void RadiusChangeOutput_Azimuth90_EffectiveExtinction()
        {
            // Arrange
            const int DistanceMax = 3;
            const double DistanceStep = 0.5;
            List<double> distances = getDistances(DistanceStep, DistanceMax);
            var radiuses = new List<double> { 4, 10, 20, 40 };
            //var radius1 = 4;
            this.ddaConfig.IncidentMagnitude.Azimuth = 90;

            double maxRadius = radiuses.Max();
            foreach (double radius1 in radiuses)
            foreach (double radius in radiuses)
            {
                foreach (double distance in distances)
                {
                    this.calculateOneStepDiffRadius(radius1, radius, distance, maxRadius, x => x.EffectiveCrossSectionExtinction);
                }
            }

            this.writeParameters(radiuses, distances);
        }

        [TestMethod]
        public void RadiusChangeOutput_InterparticleDistance_Spectrum()
        {
            // Arrange
            var radiuses = new List<double> {4, 10, 15, 20};
            double distance = 70;
            this.ddaConfig.IncidentMagnitude.Azimuth = 45;
            double maxRadius = radiuses.Max();
            foreach (var radius1 in radiuses)
            {
                foreach (double radius in radiuses)
                {
                    this.calculateOneStepDiffRadiusDistance(radius1, radius, distance, maxRadius,
                        x => x.EffectiveCrossSectionExtinction);
                }
            }
        }

        [TestMethod]
        public void DimmerVsOne_Azimuth45_EffectiveCrossExt()
        {
            // Arrange
            this.ddaConfig.IncidentMagnitude.Azimuth = 45;
            var radiuses = new List<double> { 4, 10, 20, 40 };
            var distance = 0;

            foreach (double radius in radiuses)
            {
                this.calculateOneStep(radius, distance, radius, x => x.EffectiveCrossSectionExtinction);
            }
        }

        [TestMethod]
        public void OneParticle_EffectiveCrossExt()
        {
            // Arrange
            var radiuses = new List<double> { 4, 10, 20, 40 };

            foreach (double radius in radiuses)
            {
                this.calculateOneParticle(radius, x => x.EffectiveCrossSectionExtinction);
            }
        }

        [TestMethod]
        public void OneParticle_CrossExt()
        {
            // Arrange
            var radiuses = new List<double> { 4, 10, 20, 40 };

            foreach (double radius in radiuses)
            {
                this.calculateOneParticle(radius, x => x.CrossSectionExtinction);
            }
        }

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

        private async Task calculateOneParticle(
            double radius,
            Func<SimulationResult, double> valueSelector)
        {
            var firstPoint = new CartesianCoordinate(radius, radius, 0);

            var systConfig = new SystemConfig(
                new List<double>
                {
                    radius
                },
                new List<CartesianCoordinate>
                {
                    firstPoint
                });

            SimulationResultDictionary result = await DDAProgram.Calculate(this.ddaConfig, systConfig, this.opticalConstants);

            string filename = this.getFileFormatOneParticle(this.TestContext.TestName, radius);

            SimpleFormatter.Write(
                filename,
                result.ToDictionary(x => x.ToType(SpectrumUnitType.WaveLength), valueSelector));
        }

        private async Task calculateOneStep(
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

            SimulationResultDictionary result = await DDAProgram.Calculate(this.ddaConfig, systConfig, this.opticalConstants);

            string filename = this.getFileFormat(this.TestContext.TestName, distance, radius);

            SimpleFormatter.Write(
                filename,
                result.ToDictionary(x => x.ToType(SpectrumUnitType.WaveLength), valueSelector));
        }

        private async Task calculateOneStepDiffRadius(
            double radius1,
            double radius2,
            double distance,
            double maxRadius,
            Func<SimulationResult, double> valueSelector)
        {
            var firstPoint = new CartesianCoordinate(maxRadius, maxRadius, 0);

            double secondPointCoord = maxRadius + radius1 + radius2 + distance * radius1;
            var secondPoint = new CartesianCoordinate(maxRadius, secondPointCoord, 0);

            var systConfig = new SystemConfig(
                new List<double>
                {
                    radius1,
                    radius2
                },
                new List<CartesianCoordinate>
                {
                    firstPoint,
                    secondPoint
                });

            SimulationResultDictionary result = await DDAProgram.Calculate(this.ddaConfig, systConfig, this.opticalConstants);

            string filename = this.getFileFormatDiffRadiuses(this.TestContext.TestName, distance, radius1, radius2);

            SimpleFormatter.Write(
                filename,
                result.ToDictionary(x => x.ToType(SpectrumUnitType.WaveLength), valueSelector));
        }

        private async Task calculateOneStepDiffRadiusDistance(
            double radius1,
            double radius2,
            double distance,
            double maxRadius,
            Func<SimulationResult, double> valueSelector)
        {
            var firstPoint = new CartesianCoordinate(maxRadius, maxRadius, 0);

            double secondPointCoord = maxRadius + distance;
            var secondPoint = new CartesianCoordinate(maxRadius, secondPointCoord, 0);

            var systConfig = new SystemConfig(
                new List<double>
                {
                    radius1,
                    radius2
                },
                new List<CartesianCoordinate>
                {
                    firstPoint,
                    secondPoint
                });

            SimulationResultDictionary result = await DDAProgram.Calculate(this.ddaConfig, systConfig, this.opticalConstants);

            string filename = this.getFileFormatDiffRadiuses(this.TestContext.TestName, distance, radius1, radius2);

            SimpleFormatter.Write(
                filename,
                result.ToDictionary(x => x.ToType(SpectrumUnitType.WaveLength), valueSelector));
        }
        private string getFileFormat(string dirPath, double distance, double radius)
        {
            string format = string.Format(
                CultureInfo.InvariantCulture,
                "rezult_ext_{0}_{1}.txt",
                (decimal)distance,
                (decimal)radius);
            return Path.Combine(BasePath, dirPath, format);
        }
        private string getFileFormatOneParticle(string dirPath, double radius)
        {
            return this.getFileFormat(dirPath, -1, radius);
        }

        private string getFileFormatDiffRadiuses(string dirPath, double distance, double radius1, double radius2)
        {
            string format = string.Format(
                CultureInfo.InvariantCulture,
                "rezult_ext_{0:0.00}_{1:0.00}_{2:0.00}.txt",
                (decimal)distance,
                (decimal)radius1,
                (decimal)radius2);
            return Path.Combine(BasePath, dirPath, format);
        }


        private void writeParameters(List<double> radiuses, List<double> distances)
        {
            string filename = Path.Combine(BasePath, this.TestContext.TestName, "parameters.txt");
            using (var sw = new StreamWriter(filename))
            {
                string radiusJoin = string.Join(
                    ",",
                    radiuses.Select(SimpleFormatter.ToDecimalString));
                sw.WriteLine("radius = {0}", radiusJoin);
                string distanceJoin = string.Join(
                    ",",
                    distances.Select(SimpleFormatter.ToDecimalString));
                sw.WriteLine("distance = {0}", distanceJoin);
            }
        }

        #endregion
    }
}
