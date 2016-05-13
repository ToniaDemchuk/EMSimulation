using System;
using System.Threading.Tasks;
using Simulation.Models.Coordinates;

namespace Simulation.Infrastructure.Iterators
{
    public class ParallelIterator : IIterator
    {
        public static int MaxDegreeOfParallelism = 4;

        public void For(IndexStore indices, Action<int, int, int> action)
        {
            Parallel.For(
               indices.Lower,
               indices.ILength,
               new ParallelOptions { MaxDegreeOfParallelism = MaxDegreeOfParallelism },
               i =>
               {
                   for (int j = indices.Lower; j < indices.JLength; j++)
                   {
                       for (int k = indices.Lower; k < indices.KLength; k++)
                       {
                           action(i, j, k);
                       }
                   }

               });
        }

        public void ForExceptI(IndexStore indices, Action<int, int> action)
        {
            Parallel.For(
               indices.Lower,
               indices.JLength,
               new ParallelOptions { MaxDegreeOfParallelism = MaxDegreeOfParallelism },
               j =>
               {
                   for (int k = indices.Lower; k < indices.KLength; k++)
                   {
                       action(j, k);
                   }
               });
        }

        public void ForExceptJ(IndexStore indices, Action<int, int> action)
        {
            Parallel.For(
               indices.Lower,
               indices.ILength,
               new ParallelOptions { MaxDegreeOfParallelism = MaxDegreeOfParallelism },
               i =>
               {
                   for (int k = indices.Lower; k < indices.KLength; k++)
                   {
                       action(i, k);
                   }
               });
        }

        public void ForExceptK(IndexStore indices, Action<int, int> action)
        {
            Parallel.For(
               indices.Lower,
               indices.ILength,
               new ParallelOptions { MaxDegreeOfParallelism = MaxDegreeOfParallelism },
               i =>
               {
                   for (int j = indices.Lower; j < indices.JLength; j++)
                   {
                       action(i, j);
                   }
               });
        }

        public double Sum(IndexStore indices, Func<int, int, int, double> action)
        {
            double sum = 0;

            object obj = new object();
            this.For(
            indices,
                (i, j, k) =>
                {
                    var val = action(i, j, k);
                    lock (obj)
                    {
                        sum += val;
                    }

                });

            return sum;
        }
    }
}
