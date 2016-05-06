using System;
using System.Collections.Generic;
using System.Linq;

namespace Simulation.Models.Extensions
{
    /// <summary>
    /// The extensions for collections
    /// </summary>
    public static class CollectionExtensions
    {
        public static IEnumerable<IEnumerable<T>> SplitOn<T>(this IEnumerable<T> source, Func<T, bool> startIncludingPredicate, Func<T, bool> endExcludingPredicate)
        {
            var list1 = source.Aggregate(new List<List<T>> { new List<T>() },
                                    (list, value) =>
                                    {
                                        if (endExcludingPredicate(value) && list.Last().Count != 0)
                                        {
                                            list.Add(new List<T>());
                                            return list;
                                        }

                                        if (list.Last().Count != 0 || startIncludingPredicate(value))
                                        {
                                            list.Last().Add(value);
                                        }

                                        return list;
                                    });

            if (list1.Last().Count == 0)
            {
                list1.RemoveAt(list1.Count - 1);
            }
            return list1;
        }
    }
}
