using System.Collections.Generic;

namespace Simulation.DDA.Tests
{
    public class ScilabHelper
    {
        public static string FormatToArray<T>(IEnumerable<T> dict)
        {
            var list = string.Join(",", dict);
            return string.Format("[{0}]", list);
        }
    }
}