using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Catamorphisms.Mergesort.Hylo
{

    public static partial class FunctionalExt
    {
        public static string Show<T>(this Tree<T> @this) =>
        @this.MatchWith<string>(algebra: (
            Leaf: v => $"{v}",
            Node: (l, r) => $"({l.Show()},{r.Show()})"
        ));

        public static List<T> EmptyList<T>() => new List<T>();
        public static List<T> ToList<T>(this T @this) => new List<T>() { @this };
        public static List<T> ToList<T>(this ValueTuple<T, T> @this) => new List<T> { @this.Item1, @this.Item2 };
        public static List<T> ToList<T>(this ValueTuple<T, List<T>> @this) => @this.Item1.ToList().Concat(@this.Item2);

        public static Tree<T> ToTree<T>(this List<T> @this)
        {
            var lenght = @this.Length();
            return lenght == 1 ? (Tree<T>)
                new Leaf<T>(@this.FirstOrDefault()) :
                new Node<T>(@this.GetRange(0, lenght / 2).ToTree(),
                @this.GetRange(lenght / 2, (int)Math.Ceiling((decimal)lenght / 2)).ToTree()
                ); 
        }

        public static List<T> ZipOrdered<T>(this List<T> @this, List<T> @a2) where T : IComparable
      =>
         @this.MatchWith(algebra: (
             Empty: () => @a2,
             Cons: (x, xs) =>
                 @a2.MatchWith(algebra: (
                     Empty: () => xs,
                     Cons: (y, ys) => (x.CompareTo(y) > 0) ?
                     x.ToList().Concat(xs.ZipOrdered((y, ys).ToList()))
                         : y.ToList().Concat(ys.ZipOrdered((x, xs).ToList()))
                         )
                 )));

         
        public static List<T> Sort<T>(this Tree<T> @this) where T : IComparable =>
               @this.MatchWith(algebra: (
                   Leaf: v => v.ToList(),
                   Node: (l, r) => l.Sort().ZipOrdered(r.Sort())
               ));

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
            @this.MatchWith(algebra: (Empty: monoid.empty, Cons: (v, r) => monoid.concat(v, r.Fold(monoid))));

        public static TM FoldMap<T, TM>(this List<T> @this,
            (Func<TM> Empty, Func<TM, TM, TM> Concat) m,
            Func<T, TM> f) =>
               @this.MatchWith(algebra: (
                   Empty: () => m.Empty(),
                   Cons: (v, r) => m.Concat(f(v), r.FoldMap(m, f))
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


    public abstract class Tree<T>
    {
        public abstract T1 MatchWith<T1>((Func<T, T1> Leaf, Func<Tree<T>, Tree<T>, T1> Node) algebra);
    }


    public class Node<T> : Tree<T>
    {
        public Tree<T> Left { get; set; }
        public T Value { get; set; }
        public Tree<T> Right { get; set; }

        public Node(Tree<T> left, Tree<T> right)
        {
            this.Left = left;
            this.Right = right;
        }
        public override T1 MatchWith<T1>((Func<T, T1> Leaf, Func<Tree<T>, Tree<T>, T1> Node) algebra)
                                                                            => algebra.Node(Left, Right);

    }

    public class Leaf<T> : Tree<T>
    {
        public T V { get; }
        public Leaf(T v) => V = v;
        public override T1 MatchWith<T1>((Func<T, T1> Leaf, Func<Tree<T>, Tree<T>, T1> Node) algebra) => algebra.Leaf(V);
    }

    public class Demo
    {
        public static void Run()
        {
            var initial = new List<int> { 1, 2, 10, 6, 20, 11, 2, 3, 4 }.ToTree();
            Console.WriteLine(initial.Show());
            var sorted = initial.Sort();
            Console.WriteLine(sorted.ToTree().Show()); 
            var y = new List<int> { 10, 3, 1 }.ZipOrdered(new List<int> { 8, 4, 2 });

        }
    }



}