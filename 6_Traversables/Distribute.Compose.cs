using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using Functors.Id;

namespace Traversables.Distribute.Compose
{
    public static partial class funcEtxnesion
    {
        public static List<T> AsList<T>(this T @this) => new List<T> { @this };

        public static List<T> Concat<T>(this List<T> @this, List<T> range)
        {
            @this.AddRange(range);
            return @this;
        }
        public static T1 Cata<T, T1>(this List<T> @this, (Func<T1> Empty, Func<T, List<T>, T1> Cons) algebra) =>
                (@this.Count == 0) ?
                algebra.Empty() :
                algebra.Cons(@this[0], @this.GetRange(1, @this.Count - 1));

        public static Id<List<T>> Distribute<T>(this List<Id<T>> @this) =>
               @this.Cata(algebra: (
                   Empty: () => new Id<List<T>>(new List<T>()),
                   Cons: (v, r) =>
                       new Id<Func<T, Func<List<T>, List<T>>>>(x => y => x.AsList().Concat(y))
                       .App(v)
                       .App(r.Distribute())
             ));

        public static Id<List<List<T>>> DistributeT<T>(this List<List<Id<T>>> @this) =>
            @this.Select(v => v.Distribute()).ToList().Distribute();

        public static Id<T1> App<T, T1>(this Id<Func<T, T1>> @this, Id<T> fa) => @this.Map(f => fa.Map(f)).Value;
    }

    public class Demo
    { 
        public static void Run()
        { 
            var composite = new List<List<Id<int>>> {
                new List<Id<int>> {new Id<int>(2), new Id<int>(2), new Id<int>(2) } ,
                new List<Id<int>> { new Id<int>(2), new Id<int>(2), new Id<int>(2) } }.DistributeT(); 

        }

    }



}