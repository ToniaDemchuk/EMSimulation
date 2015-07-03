using Simulation.Models.Common;

namespace Simulation.Models.Extensions
{
    /// <summary>
    /// The LinearDiscreteExtensions class.
    /// </summary>
    public static class LinearDiscreteExtensions
    {
        /// <summary>
        /// To the linear collection.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>The linear discrete collection.</returns>
        public static LinearDiscreteCollection ToLinearCollection(this LinearDiscreteElement element)
        {
            return new LinearDiscreteCollection(element.Lower, element.Upper, element.Count);
        }
    }
}