using System;
using System.Collections.Generic;

namespace Simulation.Models.Common
{
    /// <summary>
    /// Stores functions with one parameter by the parameters type 
    /// </summary>
    public class FuncDictionary
    {
        private readonly Dictionary<Type, Delegate> storage;

        /// <summary>
        /// Initializes a new instance of the <see cref="FuncDictionary"/> class.
        /// </summary>
        public FuncDictionary()
        {
            this.storage = new Dictionary<Type, Delegate>();
        }

        /// <summary>
        /// Adds the function.
        /// </summary>
        /// <typeparam name="TIn">The type of the in parameter.</typeparam>
        /// <typeparam name="TOut">The type of the out parameter.</typeparam>
        /// <param name="func">The function.</param>
        public void Add<TIn, TOut>(Func<TIn, TOut> func)
        {
            this.storage.Add(typeof(TIn), func);
        }

        /// <summary>
        /// Gets the function result.
        /// </summary>
        /// <typeparam name="TOut">The type of the result.</typeparam>
        /// <param name="inputValue">The input value.</param>
        /// <returns>The result of executing function.</returns>
        public TOut GetResult<TOut>(object inputValue)
        {
            if (inputValue == null)
            {
                inputValue = new object();
            }
            Delegate func;
            if (this.storage.TryGetValue(inputValue.GetType(), out func))
            {
                return (TOut)func.DynamicInvoke(inputValue);
            }

            return default(TOut);
        }
    }
}
