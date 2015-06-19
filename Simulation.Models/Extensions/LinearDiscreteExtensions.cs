using Simulation.Models;

namespace Simulation.DDA.Console
{
    public static class LinearDiscreteExtensions
    {
        /// <summary>
        /// To the linear collection.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        public static LinearDiscreteCollection ToLinearCollection(this LinearDiscreteElement element)
        {
            return new LinearDiscreteCollection(element.Lower, element.Upper, element.Count);
        }
    }
}