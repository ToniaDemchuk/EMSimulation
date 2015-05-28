using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;

using Simulation.DDA.Console;
using Simulation.Models;

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
                        .Select(x=>double.Parse(x, CultureInfo.InvariantCulture)).ToArray();

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


        public static LinearDiscreteCollection ReadWavelengthFromConfigration(string fileName)
        {
            var waveLength = XmlSerializerHelper.DeserializeObject<DDAParameters>(fileName).WaveLengthConfig;

            return new LinearDiscreteCollection(
                waveLength.Lower,
                waveLength.Upper,
                waveLength.Count);
        }
        
        public static SphericalCoordinate ReadWavePropagationFromConfiguration(string fileName)
        {
            return XmlSerializerHelper.DeserializeObject<DDAParameters>(fileName).WavePropagation;
        }

        public static CartesianCoordinate ReadIncidentMagnitudeFromConfiguration(string fileName)
        {
            return XmlSerializerHelper.DeserializeObject<DDAParameters>(fileName).IncidentMagnitude.ConvertToCartesian();
        }

        #region Obsolete methods

        [Obsolete]
        public static LinearDiscreteCollection ReadWaveConfig()
        {
            using (var sr = new StreamReader("control.txt"))
            {
                var strings =
                    sr.ReadToEnd()
                        .Split(
                            Environment.NewLine.ToCharArray(),
                            StringSplitOptions.RemoveEmptyEntries)
                        .Select(double.Parse)
                        .ToArray();

                return new LinearDiscreteCollection(
                    strings[0],
                    strings[1],
                    Convert.ToInt16(strings[2]));
            }
        }

        #endregion

    }
}
