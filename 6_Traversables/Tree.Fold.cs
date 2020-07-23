using System;
using System.Collections.Generic;
using Functors.Tree;

namespace Tree.Fold
{
    public static partial class FunctionalExt
    {
        public static List<T> Concat<T>(this List<T> @this, List<T> range)
        {
            @this.AddRange(range);
            return @this;
        }
        public static List<T> AsList<T>(this T @this) => new List<T>() { @this };

        public static T Fold<T>(this Tree<T> @this, (Func<T> empty, Func<T, T, T> concat) monoid) =>
            @this.MatchWith(pattern: (
            Leaf: v => monoid.concat(monoid.empty(), v),
            Node: (l, v, r) => monoid.concat(v, monoid.concat(l.Fold(monoid), r.Fold(monoid)))
            )); 
    }
     
    public class Demo
    {
        public static void Run()
        {
            var tree = new Node<int>(new Leaf<int>(2), 2, new Node<int>(new Leaf<int>(2), 4, new Leaf<int>(4)));
            var r = tree.Fold(monoid: (() => 0, (x, y) => x + y));
            var tolist = tree.Map(x => x.AsList()).Fold(monoid: (() => new List<int>(), (x, y) => x.Concat(y)));
        }

    }
}