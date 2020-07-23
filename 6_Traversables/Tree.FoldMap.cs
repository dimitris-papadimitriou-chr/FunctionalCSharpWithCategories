using System;
using Monoids;
using Functors.Tree.Binary;

namespace Traversables.Tree.FoldMap
{
    public static partial class FunctionalExt
    {
        public static T Fold<T>(this Tree<T> @this,
            (Func<T> empty, Func<T, T, T> concat) monoid) =>
            @this.MatchWith(pattern: (
            Leaf: v => monoid.concat(monoid.empty(), v),
            Node: (l, r) => monoid.concat(l.Fold(monoid), r.Fold(monoid))
            ));

        public static TM FoldMap<T, TM>(this Tree<T> @this,
            (Func<TM> empty, Func<TM, TM, TM> concat) monoid, Func<T, TM> f) =>
           @this.MatchWith(pattern: (
           Leaf: v => monoid.concat(monoid.empty(), f(v)),
           Node: (l, r) => monoid.concat(l.FoldMap(monoid, f), r.FoldMap(monoid, f))
         ));

        public static bool All<T>(this Tree<T> @this, Func<T, bool> f) =>
            @this.FoldMap((empty: () => true, concat: (x, y) => x && y), f);

        public static IMonoidAcc<TM> FoldMap<T, TM>(this Tree<T> @this,
            (Func<IMonoidAcc<TM>> empty, Func<T, IMonoidAcc<TM>> f) m) =>
             @this.MatchWith(pattern: (
                 Leaf: v => m.f(v),
                 Node: (l, r) => l.FoldMap(m).Concat(r.FoldMap(m))
             ));

    }


    public class Demo
    {
        public class And : IMonoidAcc<bool>
        {
            public And(bool value) => Identity = value;
            public bool Identity { get; set; }
            public IMonoidAcc<bool> Concat(IMonoidAcc<bool> m) => new And(Identity && m.Identity);
        }

        public static void Run()
        {
            var tree = new Node<int>(new Leaf<int>(2), new Node<int>(new Leaf<int>(2), new Leaf<int>(4)));

            var all2 = tree.FoldMap(m: (() => new And(true), x => new And(x > 4)));

            var allLargetThanThree = tree.FoldMap<int, bool>((empty: () => true, concat: (x, y) => x && y), i => i > 3);
        }

    }

}