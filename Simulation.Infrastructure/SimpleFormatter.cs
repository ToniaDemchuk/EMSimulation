using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

using Simulation.Models;

namespace Simulation.Infrastructure
{
    public static class SimpleFormatter
    {
        public static void Write(string filename, Dictionary<double, double> cext)
        {

            var fileDir = new FileInfo(filename).Directory;
            if (fileDir != null && !fileDir.Exists)
            {
                fileDir.Create();
            }

            using (var sw = new StreamWriter(filename))
            {
                foreach (var pair in cext)
                {
                    sw.WriteLine(string.Format(CultureInfo.InvariantCulture, "{0} {1}", pair.Key,
                        pair.Value));
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