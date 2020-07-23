using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Functors.Tree.Binary;
using Functors.Id;


namespace Traversables.Tree.Fold.Compose
{
    public static partial class FunctionalExt
    { 
        public static T Fold<T>(this Tree<T> @this, (Func<T> empty, Func<T, T, T> concat) monoid) =>
            @this.MatchWith(pattern: (
            Leaf: v => monoid.concat(monoid.empty(), v),
            Node: (l, r) => monoid.concat(l.Fold(monoid), r.Fold(monoid))
            ));

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
        public static T FoldT<T>(this List<Tree<T>> @this, (Func<T> empty, Func<T, T, T> concat) monoid)
            => @this.Select(x => x.Fold(monoid)).ToList().Fold(monoid);

        public static T FoldT<T>(this Tree<List<T>> @this, (Func<T> empty, Func<T, T, T> concat) monoid)
               => @this.Map(x => x.Fold(monoid)).Fold(monoid);
        public static T FoldT<T>(this Tree<Tree<T>> @this, (Func<T> empty, Func<T, T, T> concat) monoid)
          => @this.Map(x => x.Fold(monoid)).Fold(monoid);

        public static Id<T1> Ap<T, T1>(this Id<Func<T, T1>> @this, Id<T> fa) => @this.Map(f => fa.Map(f)).Value;
    }


    public class Demo
    {
        public static void Run()
        {

            var tree = new Node<List<int>>(new Leaf<List<int>>(new List<int>() { 1, 2 })
                , new Node<List<int>>(new Leaf<List<int>>(new List<int>() { 3, 4 }),
                new Leaf<List<int>>(new List<int>() { 5, 6 })));

            var compositeFold = tree.FoldT(monoid: (() => 0, (x, y) => x + y));
        }

    } 
}