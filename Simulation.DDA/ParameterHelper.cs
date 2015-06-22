using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;

using Simulation.DDA.Console;
using Simulation.Models;
using Simulation.Models.Extensions;

namespace Simulation.Infrastructure
{
    public static class ParameterHelper
    {
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

        public static OpticalSpectrum ReadWavelengthFromConfigration(string fileName)
        {
            var config = XmlSerializerHelper.DeserializeObject<DDAParameters>(fileName);

            return ReadWavelengthFromConfigration(config);
        }

        public static OpticalSpectrum ReadWavelengthFromConfigration(DDAParameters config)
        {
            var waveLength = config.WaveLengthConfig;

            return new OpticalSpectrum(waveLength.ToLinearCollection(), SpectrumParameterType.WaveLength);
        }

        public static SphericalCoordinate ReadWavePropagationFromConfiguration(string fileName)
        {
            return XmlSerializerHelper.DeserializeObject<DDAParameters>(fileName).WavePropagation;
        }

        public static CartesianCoordinate ReadIncidentMagnitudeFromConfiguration(string fileName)
        {
            var aaa = XmlSerializerHelper.DeserializeObject<DDAParameters>(fileName);
            return aaa.IncidentMagnitude.ConvertToCartesian();
        }
    }
}
