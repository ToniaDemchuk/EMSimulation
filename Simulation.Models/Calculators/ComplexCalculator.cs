using System.Numerics;

namespace Simulation.Models.Calculators
{
    /// <summary>
    /// Calculator for <see cref="Complex"/> type.
    /// </summary>
    public struct ComplexCalculator : ICalculator<Complex>
    {
        /// <summary>
        /// Adds the two <see cref="!:T" /> objects.
        /// </summary>
        /// <param name="a">First <see cref="!:T" /> object.</param>
        /// <param name="b">Second <see cref="!:T" /> object.</param>
        /// <returns>
        /// Sum of objects.
        /// </returns>
        public Complex Add(ref Complex a, ref Complex b)
        {
            return a + b;
        }

        /// <summary>
        /// Substracts two <see cref="!:T" /> objects.
        /// </summary>
        /// <param name="a">First <see cref="!:T" /> object.</param>
        /// <param name="b">Second <see cref="!:T" /> object.</param>
        /// <returns>
        /// Difference of objects.
        /// </returns>
        public Complex Substract(ref Complex a, ref Complex b)
        {
            return a - b;
        }

        /// <summary>
        /// Multiplies the two <see cref="!:T" /> objects.
        /// </summary>
        /// <param name="a">First <see cref="!:T" /> object.</param>
        /// <param name="b">Second <see cref="!:T" /> object.</param>
        /// <returns>
        /// Product of objects.
        /// </returns>
        public Complex Multiply(ref Complex a, ref Complex b)
        {
            return a * b;
        }
    }
}
