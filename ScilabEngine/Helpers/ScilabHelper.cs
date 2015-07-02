using System.Collections.Generic;

namespace ScilabEngine.Helpers
{
    /// <summary>
    /// The ScilabHelper class.
    /// </summary>
    public class ScilabHelper
    {
        /// <summary>
        /// Formats to array.
        /// </summary>
        /// <typeparam name="T">The type of enumerable.</typeparam>
        /// <param name="dict">The dictionary.</param>
        /// <returns>The string representing scilab array.</returns>
        public static string FormatToArray<T>(IEnumerable<T> dict)
        {
            var list = string.Join(",", dict);
            return string.Format("[{0}]", list);
        }
    }
}