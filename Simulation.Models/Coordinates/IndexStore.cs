using System;

namespace Simulation.Models.Coordinates
{
    public class IndexStore
    {
        private const string ParameterMustBeNotLessThanZero = "Parameter must be not less than zero";

        public IndexStore(int i, int j, int k)
            : this(i, j, k, 0)
        {
        }

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

        public int ILength { get; private set; }

        public int JLength { get; private set; }

        public int KLength { get; private set; }

        public int Lower { get; private set; }

        public IndexStore ShiftLower(int shift)
        {
            return new IndexStore(this.ILength, this.JLength, this.KLength, this.Lower + shift);
        }

        public IndexStore ShiftUpper(int shift)
        {
            if (shift < 0)
            {
                throw new ArgumentOutOfRangeException("shift", shift, ParameterMustBeNotLessThanZero);
            }
            return new IndexStore(this.ILength - shift, this.JLength - shift, this.KLength - shift, this.Lower);
        }
    }
}