using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
 

namespace Traversables.List
{
    public interface Monoid<M>
    {
        M Value { get; set; }
        Monoid<M> Empty();
        Monoid<M> Concat(Monoid<M> m);
    }

    public static partial class FunctionalExt
    {
        public static List<T> AsList<T>(this T @this) => new List<T>() { @this };
        public static List<T> ToList<T>(this ValueTuple<T, T> @this) => new List<T> { @this.Item1, @this.Item2 };
        public static List<T> Concat<T>(this List<T> @this, List<T> range)
        {
            @this.AddRange(range);
            return @this;
        }
        public static Id<IEnumerable<T>> Distribute<T>(this IEnumerable<Id<T>> @this)
          =>
       @this.MatchWith(pattern: (
           Empty: () => new Id<IEnumerable<T>>(new List<T>()),
           Cons: (v, r) =>
              new Id<Func<T, Func<IEnumerable<T>, IEnumerable<T>>>>(x => y => x.AsList().Concat(y))
                  .Ap(v)
                  .Ap(r.Distribute())
     ));

        public static Id<List<T>> Distribute<T>(this List<Id<T>> @this)
            =>
             @this.MatchWith(pattern: (
                 Empty: () => new Id<List<T>>(new List<T>()),
                 Cons: (v, r) =>
                    new Id<Func<T, Func<List<T>, List<T>>>>(x => y => x.AsList().Concat(y))
                        .Ap(v)
                        .Ap(r.Distribute())
           ));

        public static Id<List<T1>> Traverse<T,T1>(this List<T> @this, Func<T, Id<T1>> f)
            =>
             @this.MatchWith(pattern: (
                 Empty: () => new Id<List<T1>>(new List<T1>()),
                 Cons: (v, r) =>
                    new Id<Func<T1, Func<List<T1>, List<T1>>>>(x => y => x.AsList().Concat(y))
                        .Ap(f(v))
                        .Ap(r.Traverse(f))
           ));

        public static T1 MatchWith<T, T1>(this List<T> @this, (Func<T1> Empty, Func<T, List<T>, T1> Cons) pattern) =>
                (@this.Count == 0) ?
                pattern.Empty() :
                pattern.Cons(@this[0], @this.GetRange(1, @this.Count - 1));


        public static T1 MatchWith<T, T1>(this IEnumerable<T> @this, (Func<T1> Empty, Func<T, IEnumerable<T>, T1> Cons) pattern) =>
        (@this.Count() == 0) ?
        pattern.Empty() :
        pattern.Cons(@this.First(), @this.Skip(1));




        public static Id<T1> Ap<T, T1>(this Id<Func<T, T1>> @this, Id<T> fa) => @this.Map(f => fa.Map(f)).Value;
    }

    public class Id<T>
    {
        public T Value { get; set; }
        public Id(T value) => Value = value;
        public Id<T1> Map<T1>(Func<T, T1> f) => new Id<T1>(f(Value));
    }

    public class Sum : Monoid<Id<List<int>>>
    {
        public Sum(Id<List<int>> v)
        {
            Value = v;
        }

        public Id<List<int>> Value { get; set; }

        public Monoid<Id<List<int>>> Concat(Monoid<Id<List<int>>> m)
        {
            return
                new Sum(new Id<Func<List<int>, Func<List<int>, List<int>>>>(x => y => x.Concat(y))
                  .Ap(m.Value)
                    .Ap(Value));
        }

        public Monoid<Id<List<int>>> Empty()
        {
            return new Sum(new Id<List<int>>(new List<int> { }));
        }
    }
    public class Demo
    {


        public static void Run()
        {
            var t =
                new Id<Func<List<int>, Func<List<int>, List<int>>>>(x => y => x.Concat(y))
                .Ap(new Id<List<int>>((10, 5).ToList()))
                .Ap(new Id<List<int>>((3, 6).ToList()));

            var t2 =
                new Id<Func<List<int>, Func<List<int>, List<int>>>>(x => y => x.Concat(y))
                    .Ap(new Id<List<int>>((10, 5).ToList()))
                            .Ap(new Id<Func<List<int>, Func<List<int>, List<int>>>>(x => y => x.Concat(y))
                            .Ap(new Id<List<int>>((10, 5).ToList()))
                            .Ap(new Id<List<int>>((3, 6).ToList()))
                        );

            var distributed = new List<Id<int>> { new Id<int>(2), new Id<int>(3) }.Distribute();

            var traversed = new List<int> { 2, 3, 4 }.Traverse(x => new Id<int>(x + 1));

            var mapedAndDistributed = new List<int> { 2, 3, 4 }.Select(x => new Id<int>(x + 1)).Distribute();
        }

    }



}