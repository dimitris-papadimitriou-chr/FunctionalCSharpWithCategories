using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading;


namespace Monoids.Dictionary
{ 
    public interface IMonoid<T>
    {
        T Empty { get; }
        T Concat(T x, T y);
    }
     
    public class DictionaryMonoid<TKey, TValue> : IMonoid<IDictionary<TKey, TValue>>
    {
        private IMonoid<TValue> ValueMonoid { get; }
        public DictionaryMonoid(IMonoid<TValue> monoid) => ValueMonoid = monoid;
        public IDictionary<TKey, TValue> Empty => new Dictionary<TKey, TValue>();

        public IDictionary<TKey, TValue> Concat(IDictionary<TKey, TValue> x, IDictionary<TKey, TValue> y)
        {
            var merge = y.ToDictionary(entry => entry.Key, entry => entry.Value);
            foreach (var keyValue in x)
                if (merge.ContainsKey(keyValue.Key))
                    merge[keyValue.Key] = ValueMonoid.Concat(merge[keyValue.Key], keyValue.Value);
                else
                    merge[keyValue.Key] = keyValue.Value;
            return merge;
        }
    }

    public class Sum : IMonoid<int>
    {
        public int Concat(int x, int y) => x + y;
        public int Empty => 0;
    }
    public class Demo
    {

        public async static void Run()
        {
            (int empty, Func<int, int, int> concat) sum = (0, (x, y) => x + y);

            var dictionary1 = new Dictionary<string, int> {
                { "or", 2  },
                { "and", 3 },
                { "the", 4 }
            };
            var dictionary2 = new Dictionary<string, int> {
                { "or", 1  },
                { "and", 5 },
                { "jim", 1 },
                { "his", 1 }
            };


            var merged = new DictionaryMonoid<string, int>(new Sum()).Concat(dictionary1, dictionary2);
        }

    }

}