using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Simulation.Infrastructure
{
    /// <summary>
    /// The SimpleFormatter class.
    /// </summary>
    public static class SimpleFormatter
    {
        /// <summary>
        /// Writes the specified dictionary to the file.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="dictionary">The dictionary of values.</param>
        public static void Write(string filename, Dictionary<double, double> dictionary)
        {
            CreateDirectory(filename);

            using (var sw = new StreamWriter(filename))
            {
                foreach (var pair in dictionary)
                {
                    sw.WriteLine(
                        string.Format(
                            CultureInfo.InvariantCulture,
                            "{0} {1}",
                            pair.Key,
                            pair.Value));
                }
            }
        }

        /// <summary>
        /// Writes the specified dictionary to the file.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="dictionary">The dictionary of values.</param>
        public static void Write(string filename, Dictionary<double, List<double>> dictionary)
        {
            CreateDirectory(filename);

            using (var sw = new StreamWriter(filename))
            {
                foreach (var pair in dictionary)
                {
                    sw.WriteLine(
                        string.Format(
                            CultureInfo.InvariantCulture,
                            "{0} {1}",
                            pair.Key,
                            string.Join(" ", pair.Value)));
                }
            }
        }

        /// <summary>
        /// Creates the directory if does not exist.
        /// </summary>
        /// <param name="filename">The filename.</param>
        public static void CreateDirectory(string filename)
        {
            var fileDir = new FileInfo(filename).Directory;
            if (fileDir != null && !fileDir.Exists)
            {
                fileDir.Create();
            }
        }

        /// <summary>
        /// Writes the dictionary to the file.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="spectrum">The spectrum.</param>
        /// <param name="distances">The distances.</param>
        public static void WriteDictionary(string filename, Dictionary<decimal, List<double>> spectrum, List<double> distances)
        {
            CreateDirectory(filename);
            using (var sw = new StreamWriter(filename, true))
            {
                sw.WriteLine("distances {0}", string.Join(" ", distances));
                foreach (var spec in spectrum)
                {
                    sw.WriteLine("{0} {1}", spec.Key, string.Join(" ", spec.Value));
                }
            }
        }

        /// <summary>
        /// Reads dictionary from the specified file.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns>The dictionary of doubles.</returns>
        public static Dictionary<double, double> Read(string filename)
        {
            var dict = new Dictionary<double, double>();

            using (var sr = new StreamReader(filename))
            {
                while (!sr.EndOfStream)
                {
                    var str = sr.ReadLine();
                    var split = str.Split(
                        new[] { ' ' },
                        StringSplitOptions.RemoveEmptyEntries)
                                   .Select(s => double.Parse(s, CultureInfo.InvariantCulture)).ToArray();

                    dict.Add(split[0], split[1]);
                }
                return dict;
            }
        }

        public static string ToDecimalString(double x)
        {
            return ((decimal)x).ToString(CultureInfo.InvariantCulture);
        }
    }
}