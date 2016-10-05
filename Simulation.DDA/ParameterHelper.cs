using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;

using Simulation.DDA.Models;
using Simulation.FDTD.Console;
using Simulation.Infrastructure;
using Simulation.Models.Coordinates;
using Simulation.Models.Enums;
using Simulation.Models.Extensions;
using Simulation.Models.Spectrum;
using Simulation.Infrastructure.Readers;

namespace Simulation.DDA
{
    /// <summary>
    /// The ParameterHelper class.
    /// </summary>
    public static class ParameterHelper
    {
        /// <summary>
        /// Reads the system configuration.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>The dipole locations.</returns>
        public static SystemConfig ReadSystemConfig(string fileName)
        {
            var radiusList = new List<double>();
            var pointList = new List<CartesianCoordinate>();

            using (var sr = new StreamReader(fileName))
            {
                while (!sr.EndOfStream)
                {
                    var str = sr.ReadLine();
                    if (string.IsNullOrWhiteSpace(str))
                    {
                        break;
                    }
                    var split = str.Split(
                        new[] { '\t' },
                        StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToArray();

                    radiusList.Add(split[0]);
                    pointList.Add(new CartesianCoordinate(split[1], split[2], split[3]));
                }

                return new SystemConfig(radiusList, pointList);
            }
        }

        public static SystemConfig ReadSystemConfigFromMesh(string fileName)
        {
            var mesh = new FDSToVoxelReader().ReadInfo(fileName);

            new MediumPlotter().Plot(mesh);

            var pointList = mesh.Voxels.Select(voxel => new CartesianCoordinate(voxel.I, voxel.J, voxel.K)).ToList();

            //var radiusList = Enumerable.Repeat(Math.Pow(3 / (4 * Math.PI), 1.0 / 3.0), pointList.Count).ToList();
            var radiusList = Enumerable.Repeat(0.5, pointList.Count).ToList();

            return new SystemConfig(radiusList, pointList);
        }

        /// <summary>
        /// Reads the optical constants.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>The optical constant.</returns>
        public static OpticalConstants ReadOpticalConstants(string fileName)
        {
            var waveLengthList = new List<double>();
            var permittivityList = new List<Complex>();
            using (var sr = new StreamReader(fileName))
            {
                while (!sr.EndOfStream)
                {
                    var str = sr.ReadLine();
                    var split = str.Split(
                        new[] { '\t' },
                        StringSplitOptions.RemoveEmptyEntries)
                        .Select(s => double.Parse(s, CultureInfo.InvariantCulture)).ToArray();
                    waveLengthList.Add(split[0]);
                    permittivityList.Add(new Complex(split[1], split[2]));
                }
            }

            return new OpticalConstants(waveLengthList, permittivityList);
        }

        /// <summary>
        /// Writes the optical constants to file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="dict">The dictionary.</param>
        public static void WriteOpticalConstants(string fileName, Dictionary<double, Complex> dict)
        {
            using (var sw = new StreamWriter(fileName))
            {
                foreach (var value in dict)
                {
                    sw.WriteLine("{0}\t{1}\t{2}", value.Key, value.Value.Real, value.Value.Imaginary);
                }
            }
        }

        /// <summary>
        /// Reads the wavelength from configuration.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>The optical spectrum.</returns>
        public static OpticalSpectrum ReadWavelengthFromConfiguration(string fileName)
        {
            var config = XmlSerializerHelper.DeserializeObject<DDAParameters>(fileName);

            return ReadWavelengthFromConfiguration(config);
        }

        /// <summary>
        /// Reads the wavelength from configuration.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <returns>The optical spectrum.</returns>
        public static OpticalSpectrum ReadWavelengthFromConfiguration(DDAParameters config)
        {
            var waveLength = config.WaveLengthConfig;

            return new OpticalSpectrum(waveLength.ToLinearCollection(), SpectrumUnitType.WaveLength);
        }

        /// <summary>
        /// Reads the wave propagation from configuration.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>The wave propagation direction.</returns>
        public static SphericalCoordinate ReadWavePropagationFromConfiguration(string fileName)
        {
            return XmlSerializerHelper.DeserializeObject<DDAParameters>(fileName).WavePropagation;
        }

        /// <summary>
        /// Reads the incident magnitude from configuration.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>The incident wave magnitude direction.</returns>
        public static CartesianCoordinate ReadIncidentMagnitudeFromConfiguration(string fileName)
        {
            var aaa = XmlSerializerHelper.DeserializeObject<DDAParameters>(fileName);
            return aaa.IncidentMagnitude.ConvertToCartesian();
        }
    }
}
