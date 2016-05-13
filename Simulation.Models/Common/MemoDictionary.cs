using System;
using System.Collections.Generic;

namespace Simulation.Models.Common
{
    /// <summary>
    /// Stores objects and create new one if don't exist.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public class MemoDictionary<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> storage;
        private readonly Dictionary<TKey, Func<TValue>> factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoDictionary{TKey, TValue}"/> class.
        /// </summary>
        public MemoDictionary()
        {
            this.storage = new Dictionary<TKey, TValue>();
            this.factory = new Dictionary<TKey, Func<TValue>>();
        }

        /// <summary>
        /// Adds the specified factory method for creating objects.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="func">The function.</param>
        public void Add(TKey key, Func<TValue> func)
        {
            this.factory.Add(key, func);
        }

        /// <summary>
        /// Gets the object by the key or creates new if don't exist.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The value.</returns>
        public TValue Get(TKey key)
        {
            TValue value;
            if (!this.storage.TryGetValue(key, out value))
            {
                Func<TValue> func;
                if (this.factory.TryGetValue(key, out func))
                {
                    value = func();
                    this.storage.Add(key, value);
                }
            }

            return value;
        }
    }
}
