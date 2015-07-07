using System.Collections;
using System.Collections.Generic;

namespace Simulation.Models.Common
{
    /// <summary>
    /// The LinearDiscreteCollection class.
    /// </summary>
    public class LinearDiscreteCollection : IReadOnlyList<double>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LinearDiscreteCollection"/> class.
        /// </summary>
        /// <param name="lower">The lower boundary.</param>
        /// <param name="upper">The upper boundary.</param>
        /// <param name="count">The count of discrete values.</param>
        public LinearDiscreteCollection(double lower, double upper, int count)
        {
            this.Lower = lower;
            this.Upper = upper;
            this.Count = count;
            this.Step = this.calculateStep();
        }

        /// <summary>
        /// Gets the lower boundary.
        /// </summary>
        /// <value>
        /// The lower boundary.
        /// </value>
        public double Lower { get; protected set; }

        /// <summary>
        /// Gets the upper boundary.
        /// </summary>
        /// <value>
        /// The upper boundary.
        /// </value>
        public double Upper { get; protected set; }

        /// <summary>
        /// Gets or sets the count of discrete values.
        /// </summary>
        /// <value>
        /// The count of discrete values.
        /// </value>
        public int Count { get; protected set; }

        /// <summary>
        /// Gets or sets the step of discretization
        /// </summary>
        /// <value>
        /// The step of discretization.
        /// </value>
        public double Step { get; protected set; }

        /// <summary>
        /// Calculates the step of discretization.
        /// </summary>
        /// <returns>The step of discretization</returns>
        private double calculateStep()
        {
            return (this.Upper - this.Lower) / (this.Count - 1);
        }

        /// <summary>
        /// Gets the <see cref="System.Double" /> at the specified index.
        /// </summary>
        /// <value>
        /// The <see cref="System.Double" />.
        /// </value>
        /// <param name="index">The index.</param>
        /// <returns>The value at index.</returns>
        public double this[int index]
        {
            get
            {
                return this.GetValue(index);
            }
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The value at specified index.</returns>
        protected double GetValue(int index)
        {
            return this.Lower + this.Step * index;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<double> GetEnumerator()
        {
            for (int i = 0; i < this.Count; i++)
            {
                yield return this.GetValue(i);
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
