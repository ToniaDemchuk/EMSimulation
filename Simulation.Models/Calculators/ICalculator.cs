namespace Simulation.Models.Calculators
{
    /// <summary>
    /// Interface for class that implements arithmetic operators for <see cref="T" />.
    /// </summary>
    /// <typeparam name="T">Type of the objects.</typeparam>
    public interface ICalculator<T> where T : struct
    {
        /// <summary>
        /// Adds the two <see cref="T" /> objects.
        /// </summary>
        /// <param name="a">First <see cref="T"/> object.</param>
        /// <param name="b">Second <see cref="T"/> object.</param>
        /// <returns>Sum of objects.</returns>
        T Add(ref T a, ref T b);

        /// <summary>
        /// Substracts two <see cref="T" /> objects.
        /// </summary>
        /// <param name="a">First <see cref="T"/> object.</param>
        /// <param name="b">Second <see cref="T"/> object.</param>
        /// <returns>Difference of objects.</returns>
        T Substract(ref T a, ref T b);

        /// <summary>
        /// Multiplies the two <see cref="T" /> objects.
        /// </summary>
        /// <param name="a">First <see cref="T"/> object.</param>
        /// <param name="b">Second <see cref="T"/> object.</param>
        /// <returns>Product of objects.</returns>
        T Multiply(ref T a, ref T b);
    }
}
