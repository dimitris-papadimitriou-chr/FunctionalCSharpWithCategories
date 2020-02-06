using System;
using Functors.Tree.Binary;
using Functors.Id.Applicative;

namespace Traversables.Tree
{
    public static partial class funcEtxnesion
    {
        public static Id<Tree<T>> Distribute<T>(this Tree<Id<T>> @this) =>
            @this.MatchWith(pattern: (
           Leaf: v => new Id<Func<T, Tree<T>>>(x => new Leaf<T>(x)).Ap(v),
           Node: (l, r) => new Id<Func<Tree<T>, Func<Tree<T>, Tree<T>>>>(x => y => new Node<T>(x, y))
                   .Ap(l.Distribute())
                   .Ap(r.Distribute())
            ));

        public static Id<Tree<T1>> Traverse<T, T1>(this Tree<T> @this, Func<T, Id<T1>> f) =>
                @this.MatchWith(pattern: (
               Leaf: v => new Id<Func<T1, Tree<T1>>>(x => new Leaf<T1>(x)).Ap(f(v)),
               Node: (l, r) => new Id<Func<Tree<T1>, Func<Tree<T1>, Tree<T1>>>>(x => y => new Node<T1>(x, y))
                       .Ap(l.Traverse(f))
                       .Ap(r.Traverse(f))
                ));

    }
     

    public class Demo
    {
        public static void Run()
        {
            var tree = new Node<Id<int>>(new Node<Id<int>>(
                new Leaf<Id<int>>(new Id<int>(1)), new Leaf<Id<int>>(new Id<int>(3))),
             new Node<Id<int>>(new Leaf<Id<int>>(new Id<int>(3)), new Leaf<Id<int>>(new Id<int>(7))));

            var r = tree.Distribute();
            Console.Write(r.Value.Show());

            var traverse = new Node<int>(new Leaf<int>(2), new Leaf<int>(4)).Traverse(x => new Id<int>(x + 4));

        }
    }
}