using System;
using System.Collections.Generic;

namespace Simulation.Models.Matrices
{
    using System.Collections.Concurrent;

    public class LazyDiagonalMatrix<TKey, TResult>: IMatrix<TResult>
    {
        private readonly ConcurrentDictionary<TKey, TResult> dict; 
        private readonly ConcurrentDictionary<int, TResult> diagonalDict = new ConcurrentDictionary<int, TResult>();

        private readonly Func<int,int,TKey> keySelector;

        private readonly Func<int, int, TResult> resultSelector;
        private readonly Func<int, TResult> diagonalSelector;

        public LazyDiagonalMatrix(int size, Func<int, int, TKey> keySelector, Func<int,int, TResult> resultSelector, Func<int, TResult> diagonalSelector, IEqualityComparer<TKey> comparer = null)
        {
            this.Length = size;

            this.dict = comparer== null ? 
                new ConcurrentDictionary<TKey, TResult>() : 
                new ConcurrentDictionary<TKey, TResult>(comparer);

            this.keySelector = keySelector;
            this.resultSelector = resultSelector;
            this.diagonalSelector = diagonalSelector;
        }

        public int Length { get; protected set; }

        public TResult this[int i, int j]
        {
            get
            {
                return i != j ? this.getValue(i,j) : this.getDiagonalValue(i);
            }
        }

        private TResult getValue(int i, int j)
        {
            TKey key = this.keySelector(i, j);

            return this.dict.GetOrAdd(key, k => this.resultSelector(i, j));
        }

        private TResult getDiagonalValue(int i)
        {
            return this.diagonalDict.GetOrAdd(i, k => this.diagonalSelector(i));
        }
    }
}
