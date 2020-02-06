using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Monoids.MapReduce
{

    public interface IMonoid<T>
    {
        T Empty { get; }
        T Concat(T x, T y);
    }




    public class Demo
    {
        public static void Fused()
        {
            var bigList = Enumerable.Range(1, 100000000);

            Func<int, int> map = x => 1;

            (decimal Empty, Func<decimal, decimal, decimal> Concat) monoid = (0, (x, y) => x + y);

            Func<IEnumerable<int>, Func<int, decimal>> mapReduce = list => size =>
            {
                var accumulation = new List<Task<decimal>>();

                for (int i = 0; i < list.Count(); i += size)
                {
                    accumulation.Add(Task<decimal>.Run(() =>
                    {
                        IEnumerable<int> chunk = list.Skip(i).Take(size);

                        decimal total = 0;
                        foreach (var item in list)
                            total = monoid.Concat(total, map(item));
                        return total;

                    }));
                }
                Task<decimal>.WaitAll(accumulation.ToArray());

                return accumulation.Select(s => s.Result).Sum();
            };

            var t = mapReduce(bigList)(10000);

        }

        public async static void Run()
        {
            var bigList = Enumerable.Range(1, 1000000);

            Func<IEnumerable<int>, Func<int, IEnumerable<IEnumerable<int>>>> slice = list => size =>
            {
                var accumulation = new List<IEnumerable<int>>();
                for (int i = 0; i < list.Count(); i += size)
                    accumulation.Add(list.Skip(i).Take(size));
                return accumulation;
            };

            Func<IEnumerable<IEnumerable<int>>, Func<IMonoid<decimal>, Func<Func<decimal, decimal>, decimal>>>
                mapReduce = list => monoid => map =>
            list.Select(chunk =>
                chunk.Select(Convert.ToDecimal).Select(map).Aggregate(monoid.Empty, monoid.Concat)
                )
                .Aggregate(monoid.Empty, monoid.Concat);

            var chunks = slice(bigList)(10000);
            var sum = mapReduce(chunks)(new SumDecimal())(x => x + 1);
        }

        public class SumDecimal : IMonoid<decimal>
        {
            public decimal Concat(decimal x, decimal y) => x + y;
            public decimal Empty => 0;
        }


    }

}