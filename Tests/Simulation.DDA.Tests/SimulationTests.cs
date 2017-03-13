using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

using AwokeKnowing.GnuplotCSharp;

using GnuplotCSharp;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Simulation.Infrastructure;
using Simulation.Tests.Common;

namespace Simulation.DDA.Tests
{
    [TestClass]//todo: extract write peaks
    public class SimulationTests
    {
        private const string BasePath = @"../../SimulationResults";

        /// <summary>
        ///     Gets or sets the test context which provides
        ///     information about and functionality for the current test run.
        /// </summary>
        /// <value>
        ///     The test context.
        /// </value>
        public TestContext TestContext { get; set; }

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

                    gp.Plot(azim45);
                    gp.Plot(azimSum);
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
            const double DistanceStep = 0.5;
            int radius = 10;
            List<double> distances = getDistances(DistanceStep, DistanceMax);
            string dirAzimuth45 = "RadiusDistanceOutput_Azimuth45_EffectiveCrossExt";
            string dirAzimuth0 = "RadiusDistanceOutput_Azimuth0_EffectiveCrossExt";
            string dirAzimuth90 = "RadiusDistanceOutput_Azimuth90_EffectiveCrossExt";

            var gp = new GnuPlot();
            gp.Set("style data lines");
            gp.Set("xtics 25");
            gp.Set("grid xtics ytics");
            gp.Set("size square");
            //Dictionary<decimal, List<double>> spectrum = this.zipToDictionary(distances, dirAzimuth45, radius);
            //string filename = Path.Combine(BasePath, this.TestContext.TestName, "Azim45.txt");
            //SimpleFormatter.WriteDictionary(filename, spectrum, distances);

            //spectrum = this.zipToDictionary(distances, dirAzimuth0, radius);
            //filename = Path.Combine(BasePath, this.TestContext.TestName, "Azim0.txt");
            //SimpleFormatter.WriteDictionary(filename, spectrum, distances);

            //spectrum = this.zipToDictionary(distances, dirAzimuth90, radius);
            //filename = Path.Combine(BasePath, this.TestContext.TestName, "Azim90.txt");
            //SimpleFormatter.WriteDictionary(filename, spectrum, distances);

            foreach (double distance in distances)
            {
                gp.HoldOn();
                Dictionary<double, double> azim45 = SimpleFormatter.Read(
                    this.getFileFormat(dirAzimuth45, distance, radius)).Where(x => x.Key <= 500).ToDictionary(x => x.Key * 1e9, x => x.Value * 2);
                gp.Plot(azim45, string.Format(@"title ""{0}""", distance));
                Dictionary<double, double> azim0 = SimpleFormatter.Read(
    this.getFileFormat(dirAzimuth0, distance, radius)).Where(x => x.Key <= 500).ToDictionary(x => x.Key * 1e9, x => x.Value);
                gp.Plot(azim0, string.Format(@"title ""{0}""", distance));

                Dictionary<double, double> azim90 = SimpleFormatter.Read(
this.getFileFormat(dirAzimuth90, distance, radius)).Where(x => x.Key <= 500).ToDictionary(x => x.Key*1e9, x => x.Value);
                gp.Plot(azim90, string.Format(@"title ""{0}""", distance));
            }



            //gp.Wait();
        }

        [TestMethod]
        public void Peaks_Azimuth0_Azimuth90()
        {
            // Arrange
            const int DistanceMax = 3;
            const double DistanceStep = 0.1;
            var radiuses = new List<double> { 4, 10, 20, 40 };
            List<double> distances = getDistances(DistanceStep, DistanceMax);
            string dirAzimuth0 = "RadiusDistanceOutput_Azimuth0";
            string dirAzimuth90 = "RadiusDistanceOutput_Azimuth90";
            var gp = new GnuPlot();
            gp.Set("style data lines");
            gp.Set("xtics 0.5");
            gp.Set("grid xtics ytics");
            gp.Set("size square");
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

                    peaks0.Add(distance, azim0.MaxPair().Key);

                    peaks90.Add(distance, azim90.MaxPair().Key);
                }
                //gp.HoldOn();
                // gp.Set(string.Format("terminal win {0}", radius));
                gp.Plot(peaks0);
                gp.Plot(peaks90);
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

                gp.Plot(optical, @"title ""optical""");
                gp.Plot(drudeLorentz, @"title ""drude-lorentz""");

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

                gp.Plot(optical, @"title ""optical""");
                gp.Plot(drudeLorentz, @"title ""drude-lorentz""");

                //AssertHelper.DictionaryAreClose(optical, drudeLorentz, 0.1);
            }

            gp.Wait();
        }

        #region Output simulations


        [TestMethod]
        public void DimmerVsOne_Azimuth45_EffectiveCrossExt()
        {
            // Arrange
            var radiuses = new List<double> { 4, 10, 20, 40 };
            var distance = 0;
            string dirAzimuth45 = "DimmerVsOne_Azimuth45_EffectiveCrossExt";
            string dirAzimOne = "OneParticle_EffectiveCrossExt";
            foreach (double radius in radiuses)
            {
                var gp = new GnuPlot();
                gp.Set("style data lines");
                gp.Set("xtics 25");
                gp.Set("grid xtics ytics");
                gp.Set("size square");
                gp.HoldOn();
                Dictionary<double, double> azim45 = SimpleFormatter.Read(
                    this.getFileFormat(dirAzimuth45, distance, radius))
                    .Where(x => x.Key <= 500)
                    .ToDictionary(x => x.Key*1e9, x => x.Value*2);
                gp.Plot(azim45, string.Format(@"title ""{0}""", distance));

                Dictionary<double, double> azimOne = SimpleFormatter.Read(
                    this.getFileFormatOneParticle(dirAzimOne, radius))
                    .Where(x => x.Key <= 500)
                    .ToDictionary(x => x.Key*1e9, x => x.Value*2);
                gp.Plot(azimOne, string.Format(@"title ""{0}""", 0));
            }
        }

        #endregion


        [TestMethod]
        public void RadiusChangeOutput_Azimuth_Spectrum()
        {
            // Arrange
            const int DistanceMax = 3;
            const double DistanceStep = 0.5;
            GnuPlot gp = null;
            var radius1 = 4;
            var radiuses = new List<double> { 4, 10, 20, 40 };
            List<double> distances = getDistances(DistanceStep, DistanceMax);
            string dirAzimuth45 = "RadiusChangeOutput_Azimuth45_EffectiveExtinction";
            string dirAzimuthOne = "OneParticle_EffectiveCrossExt";

            //foreach (var radius1 in radiuses)
            //{
            gp = new GnuPlot();
            //gp.Set("terminal png");
            gp.Set("style data lines");

            gp.Set("xtics 25");
            gp.Set("grid xtics ytics");
            gp.Set("size square");

            foreach (double distance in distances)
            {

                gp.HoldOn();
                //gp.Set(String.Format("title \"{0}\"", radius1));

                foreach (double radius in radiuses)
                {

                    //Dictionary<decimal, List<double>> spectrum = this.zipToDictionaryDiffRadiuses(distances,
                    //    dirAzimuth45, radius1, radius);
                    //string filename = Path.Combine(BasePath, this.TestContext.TestName, "Azim45.txt");
                    //SimpleFormatter.WriteDictionary(filename, spectrum, distances);



                        Dictionary<double, double> azim45 = SimpleFormatter.Read(
                            this.getFileFormatDiffRadiuses(dirAzimuth45, distance, radius1, radius)).Where(x => x.Key <= 500).ToDictionary(x => x.Key * 1e9, x => x.Value);
                        gp.Plot(azim45, string.Format(@"title ""{0}""", radius));



                    gp.Clear();
                    Dictionary<double, double> azim = SimpleFormatter.Read(
                        this.getFileFormatOneParticle(dirAzimuthOne, radius)).Where(x => x.Key <= 500).ToDictionary(x => x.Key * 1e9, x => x.Value);
                    gp.Plot(azim, string.Format(@"title ""single"""));
                }
            }
            //}
            gp.Wait();
        }


        [TestMethod]
        public void RadiusChangeOutput_Hybridization_Spectrum()
        {
            // Arrange
            const int DistanceMax = 3;
            const double DistanceStep = 0.5;
            GnuPlot gp = null;
            var radius1 = 10;
            var radiuses = new List<double> { 4, 10 };
            List<double> distances = getDistances(DistanceStep, DistanceMax);
            string dirAzimuth90 = "RadiusChangeOutput_Azimuth90";
            string dirAzimuth0 = "RadiusChangeOutput_Azimuth0";

            //foreach (var radius1 in radiuses)
            //{
            gp = new GnuPlot();
            //gp.Set("terminal png");
            gp.Set("style data lines");

            gp.Set("xtics 25");
            gp.Set("grid xtics ytics");
            gp.Set("size square");

            //gp.Set(String.Format("title \"{0}\"", radius1));

            foreach (double radius in radiuses)
            {
                double distance = 0;
                gp.HoldOn();

                Dictionary<double, double> azim90 = SimpleFormatter.Read(
                    this.getFileFormatDiffRadiuses(dirAzimuth90, distance, radius1, radius)).Where(x => x.Key <= 500).ToDictionary(x => x.Key * 1e9, x => x.Value);
                gp.Plot(azim90, string.Format(@"title ""90"""));

                Dictionary<double, double> azim0 = SimpleFormatter.Read(
    this.getFileFormatDiffRadiuses(dirAzimuth0, distance, radius1, radius)).Where(x => x.Key <= 500).ToDictionary(x => x.Key * 1e9, x => x.Value);
                gp.Plot(azim0, string.Format(@"title ""0"""));


                string basepath = Path.Combine(BasePath, this.TestContext.TestName);
                string filename0 = Path.Combine(
                    basepath,
                    string.Format("peaks_0deg_{0}_{1}.txt", radius1, radius));
                SimpleFormatter.Write(filename0, azim0);
                string filename90 = Path.Combine(
                    basepath,
                    string.Format("peaks_90deg_{0}_{1}.txt", radius1, radius));
                SimpleFormatter.Write(filename90, azim90);

            }

            //}
            gp.Wait();
        }

        [TestMethod]
        public void RadiusChangeOutput_InterparticleDistance_Spectrum()
        {
            // Arrange
            GnuPlot gp = null;
            var radius1 = 20;
            var radiuses = new List<double> { 4, 10, 15, 20 };
            double distance = 70;
            string dirAzimuth45 = "RadiusChangeOutput_InterparticleDistance_Spectrum";
            //foreach (var radius1 in radiuses)
            //{
            gp = new GnuPlot();
            //gp.Set("terminal png");
            gp.Set("style data lines");
            gp.Set("xtics 25");
            gp.Set("grid xtics ytics");
            gp.Set("size square");
            gp.Set("palette model HSV");
            gp.HoldOn();
            gp.WriteLine(File.ReadAllText("colorpalette.gnuplot"));

            gp.Set(String.Format("title \"{0}\"", radius1));

            foreach (double radius in radiuses)
            {
                Dictionary<double, double> azim45 = SimpleFormatter.Read(
                        this.getFileFormatDiffRadiuses(dirAzimuth45, distance, radius1, radius))
                    .ToDictionary(x => x.Key * 1e9, x => x.Value);
                gp.Plot(azim45, string.Format(@"title ""{0}""", radius));

                //gp.Clear();
            }
            //}
            gp.Wait();
        }


        [TestMethod]
        public void RadiusChangeOutputPeaks_Azimuth0_Azimuth90()
        {
            // Arrange
            const int DistanceMax = 10;
            const double DistanceStep = 0.02;
            var radiuses = new List<double> { 4, 10, 20, 40 };
            var radius1 = 4;
            List<double> distances = getDistances(DistanceStep, DistanceMax);
            string dirAzimuth0 = "RadiusChangeOutput_Azimuth0_EffectiveExtinction";
            string dirAzimuth90 = "RadiusChangeOutput_Azimuth90_EffectiveExtinction";
            GnuPlot gp = null;
            gp = new GnuPlot();
            gp.HoldOn();
            radiuses.Reverse();
            //foreach (var radius1 in radiuses)
            //{


                gp.Set("style data lines");


                //gp.Set("terminal svg");
                //gp.Set("output 'radius" + radius1 + ".svg");
                foreach (double radius in radiuses)
                {
                    var peaks0 = new Dictionary<double, double>();

                    var peaks90 = new Dictionary<double, double>();

                    foreach (double distance in distances)
                    {
                        Dictionary<double, double> azim0 = SimpleFormatter.Read(
                            this.getFileFormatDiffRadiuses(dirAzimuth0, distance, radius1, radius));
                        Dictionary<double, double> azim90 = SimpleFormatter.Read(
                            this.getFileFormatDiffRadiuses(dirAzimuth90, distance, radius1, radius));

                        peaks0.Add(distance, azim0.MaxPair().Key);

                        peaks90.Add(distance, azim90.MaxPair().Key);
                    }
                    // gp.HoldOn();
                    // gp.Set(string.Format("terminal win {0}", radius));
                    gp.Plot(peaks0, string.Format(@"smooth acsplines title ""0.{0}""", radius));
                    gp.Plot(peaks90, string.Format(@"smooth acsplines title ""90.{0}""", radius));
                    // gp.HoldOff();

                    //string basepath = Path.Combine(BasePath, this.TestContext.TestName);
                    //string filename0 = Path.Combine(
                    //    basepath,
                    //    string.Format("peaks_0deg_{0}.txt", radius));
                    //SimpleFormatter.Write(filename0, peaks0);
                    //string filename90 = Path.Combine(
                    //    basepath,
                    //    string.Format("peaks_90deg_{0}.txt", radius));
                    //SimpleFormatter.Write(filename90, peaks90);
                
            }
            
            gp.Wait();
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
            return getFileFormat(dirPath, -1, radius);
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
                        if (!dict.ContainsKey((decimal)azim.Key))
                        {
                            dict[(decimal)azim.Key] = new List<double>();
                        }
                        dict[(decimal)azim.Key].Add(azim.Value);
                    }
                    return dict;
                });
            return spectrum;
        }

        private Dictionary<decimal, List<double>> zipToDictionaryDiffRadiuses(List<double> distances, string dirAzim, double radius1, double radius2)
        {
            var spectrum = new Dictionary<decimal, List<double>>();
            distances.Aggregate(
                spectrum,
                (dict, distance) =>
                {
                    Dictionary<double, double> azimuthDict = SimpleFormatter.Read(
                        this.getFileFormatDiffRadiuses(dirAzim, distance, radius1, radius2));
                    foreach (KeyValuePair<double, double> azim in azimuthDict)
                    {
                        if (!dict.ContainsKey((decimal)azim.Key))
                        {
                            dict[(decimal)azim.Key] = new List<double>();
                        }
                        dict[(decimal)azim.Key].Add(azim.Value);
                    }
                    return dict;
                });
            return spectrum;
        }

        #endregion
    }
}