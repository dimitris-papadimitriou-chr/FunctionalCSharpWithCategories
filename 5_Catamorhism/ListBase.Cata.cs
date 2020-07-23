using System;
using System.Collections.Generic;
using System.Diagnostics;
 
namespace Catamorphisms.List
{
    public static partial class FunctionalExt
    {
        public static string Show<T>(this List<T> @this) => 
            @this.MatchWith(
               algebra: (
                   Empty: () => "",
                   Cons: (v, r) => $"{v}, {r.Show()}" 
             ));

        public static T1 MatchWith<T, T1>(this List<T> @this, (Func<T1> Empty, Func<T, List<T>, T1> Cons) algebra) =>
                (@this.Count == 0) ?
                algebra.Empty() :
                algebra.Cons(@this[0], @this.GetRange(1, @this.Count - 1));
        public static List<T> Concat<T>(this List<T> @this, List<T> range)
        {
            @this.AddRange(range);
            return @this;
        }

        public static int Mult(this List<int> @this) => @this.MatchWith(
            algebra: (
                Empty: () => 1,
                Cons: (v, r) => v * r.Mult()
          ));

        public static List<T1> Map2<T, T1>(this List<T> @this, Func<T, T1> f) => @this.MatchWith(
           algebra: (
               Empty: () => new List<T1> { },
               Cons: (v, r) => new List<T1> { f(v) }.Concat(r.Map2(f))
         ));

        public static List<T> Zip<T>(this List<T> @this, List<T> @a2) =>
            @this.MatchWith(algebra: (
                Empty: () => @a2,
                Cons: (x, xs) =>
                    @this.MatchWith(algebra: (
                        Empty: () => xs,
                        Cons: (y, ys) =>
                        new List<T> { x, y }.Concat(xs.Zip(ys))
                    ))));
         

    }

    public class ListBaseDemo
    {
        public static void Run()
        {
            Debug.WriteLine(new List<int> { 1, 3, 4, 6 }.Mult());
            Debug.WriteLine(new List<int> { 1, 3, 4, 6 }
            .Zip(new List<int> { 4, 6, 7, 8 }).Show()//[1, 1, 3, 3, 4, 4, 6, 6, ]
            );
        }


    }

}
