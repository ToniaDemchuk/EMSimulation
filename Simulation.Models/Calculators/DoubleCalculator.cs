namespace Simulation.Models.Calculators
{
    /// <summary>
    /// Calculator for <see cref="double"/> type.
    /// </summary>
    public struct DoubleCalculator : ICalculator<double>
    {
        /// <summary>
        /// Adds the two <see cref="!:T" /> objects.
        /// </summary>
        /// <param name="a">First <see cref="!:T" /> object.</param>
        /// <param name="b">Second <see cref="!:T" /> object.</param>
        /// <returns>
        /// Sum of objects.
        /// </returns>
        public double Add(ref double a, ref double b)
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
        public double Substract(ref double a, ref double b)
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
        public double Multiply(ref double a, ref double b)
        {
            return a * b;
        }
    }
}
