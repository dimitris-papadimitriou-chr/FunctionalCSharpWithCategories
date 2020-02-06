using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Traversables.List.Fold.Compose 
{
    public static partial class funcEtxnesion
    {
        public static List<T> Concat<T>(this List<T> @this, List<T> range)
        {
            @this.AddRange(range);
            return @this;
        }

        public static T1 MatchWith<T, T1>(this List<T> @this, (Func<T1> Empty, Func<T, List<T>, T1> Cons) algebra) =>
                (@this.Count == 0) ?
                algebra.Empty() :
                algebra.Cons(@this[0], @this.GetRange(1, @this.Count - 1));

        [Pure]
        public static T Fold<T>(this List<T> @this, (Func<T> empty, Func<T, T, T> concat) monoid) =>
            @this.MatchWith(algebra: (
                Empty: monoid.empty,
                Cons: (v, r) => monoid.concat(v, r.Fold(monoid))
            ));
        public static T FoldT<T>(this List<List<T>> @this, (Func<T> empty, Func<T, T, T> concat) monoid)
           => @this.Select(x => x.Fold(monoid)).ToList().Fold(monoid);


        public static TM FoldMap<T, TM>(this List<T> @this, (Func<TM> Empty, Func<TM, TM, TM> Concat) m, Func<T, TM> f) =>
               @this.MatchWith(algebra: (
                   Empty: () => m.Empty(),
                   Cons: (v, r) => m.Concat(f(v), r.FoldMap(m, f))
             ));

        public static Id<T1> App<T, T1>(this Id<Func<T, T1>> @this, Id<T> fa) => @this.Map(f => fa.Map(f)).Value;
    }

    public class Id<T>
    {
        public T Value { get; set; }
        public Id(T value) => Value = value;
        public Id<T1> Map<T1>(Func<T, T1> f) => new Id<T1>(f(Value));
    }


    public class Demo
    {
        public static void Run()
        {
            (int Empty, Func<int, int, int> Concat) sum = (0, (x, y) => x + y);
            //var t =
            //    new Id<Func<List<int>, Func<List<int>, List<int>>>>(x => y => x.AddRangeFluid(y))
            //    .App(new Id<List<int>>(new List<int> { 10, 5, 2 }))
            //    .App(new Id<List<int>>(new List<int> { 10, 5, 2 }));


        }

    }



}