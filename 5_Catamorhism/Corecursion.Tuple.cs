//https://medium.com/@dimpapadim3/infinite-data-structures-in-c-b3655386befe
//Infinite Data structures in C#
using System;
using static Catamorphisms.Corecursion.CoFree.LazyTuple<int>;

namespace Catamorphisms.Corecursion.CoFree
{
    public class LazyTuple<T> : Tuple<T, Func<LazyTuple<T>>>
    {
        public LazyTuple(T item1, Func<LazyTuple<T>> item2) : base(item1, item2) { }

        public static LazyTuple<T> Stream(T item1, Func<LazyTuple<T>> item2) => new LazyTuple<T>(item1, item2);
    }

    public class Demo
    {
        public static void Run()
        {
            Func<int, LazyTuple<int>> StreamAnaLazy = null;
            StreamAnaLazy = n => Stream(n, () => StreamAnaLazy(n + 1));
            var x = StreamAnaLazy(0).Item2().Item2().Item2();
            Console.WriteLine(x.Item1);
        }
    }
}