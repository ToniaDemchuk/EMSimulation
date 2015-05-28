using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Simulation.Infrastructure
{
    public static class SimpleFormatter
    {
        public static void Write(string filename, Dictionary<double, double> cext)
        {
            using (var sw = new StreamWriter(filename))
            {
                foreach (var pair in cext)
                {
                    sw.WriteLine("{0} {1}", pair.Key, pair.Value);
                }
            }
        }

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

    }
}