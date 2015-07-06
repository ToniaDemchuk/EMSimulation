using System;

namespace Simulation.Models.Coordinates
{
    /// <summary>
    /// The IndexStore class.
    /// </summary>
    public class IndexStore
    {
        private const string ParameterMustBeNotLessThanZero = "Parameter must be not less than zero";

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexStore"/> class.
        /// </summary>
        /// <param name="i">The i component length.</param>
        /// <param name="j">The j component length.</param>
        /// <param name="k">The k component length.</param>
        public IndexStore(int i, int j, int k)
            : this(i, j, k, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexStore"/> class.
        /// </summary>
        /// <param name="i">The i component length.</param>
        /// <param name="j">The j component length.</param>
        /// <param name="k">The k component length.</param>
        /// <param name="lower">The lower boundary.</param>
        public IndexStore(int i, int j, int k, int lower)
        {
            if (i < 0)
            {
                throw new ArgumentOutOfRangeException("i", i, ParameterMustBeNotLessThanZero);
            }
            if (j < 0)
            {
                throw new ArgumentOutOfRangeException("j", j, ParameterMustBeNotLessThanZero);
            }
            if (k < 0)
            {
                throw new ArgumentOutOfRangeException("k", k, ParameterMustBeNotLessThanZero);
            }
            if (lower < 0)
            {
                throw new ArgumentOutOfRangeException("lower", lower, ParameterMustBeNotLessThanZero);
            }

            this.ILength = i;
            this.JLength = j;
            this.KLength = k;
            this.Lower = lower;
        }

        /// <summary>
        /// Gets the length of the i component.
        /// </summary>
        /// <value>
        /// The length of the i component.
        /// </value>
        public int ILength { get; private set; }

        /// <summary>
        /// Gets the length of the j component.
        /// </summary>
        /// <value>
        /// The length of the j component.
        /// </value>
        public int JLength { get; private set; }

        /// <summary>
        /// Gets the length of the k component.
        /// </summary>
        /// <value>
        /// The length of the k component.
        /// </value>
        public int KLength { get; private set; }

        /// <summary>
        /// Gets the lower boundary.
        /// </summary>
        /// <value>
        /// The lower boundary.
        /// </value>
        public int Lower { get; private set; }

        /// <summary>
        /// Shifts the lower boundary.
        /// </summary>
        /// <param name="shift">The shift value.</param>
        /// <returns>The new index store with shifted lower boundary.</returns>
        public IndexStore ShiftLower(int shift)
        {
            return new IndexStore(this.ILength, this.JLength, this.KLength, this.Lower + shift);
        }

        /// <summary>
        /// Shifts the upper boundary.
        /// </summary>
        /// <param name="shift">The shift value.</param>
        /// <returns>The new index store with shifted upper boundary.</returns>
        public IndexStore ShiftUpper(int shift)
        {
            if (shift < 0)
            {
                throw new ArgumentOutOfRangeException("shift", shift, ParameterMustBeNotLessThanZero);
            }
            return new IndexStore(this.ILength - shift, this.JLength - shift, this.KLength - shift, this.Lower);
        }

        /// <summary>
        /// Gets the center.
        /// </summary>
        /// <returns></returns>
        public IndexStore GetCenter()
        {
            return new IndexStore(this.ILength / 2, this.JLength / 2, this.KLength / 2, this.Lower);
        }
    }
}