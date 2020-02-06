using System;
using Functors.Tree;

namespace  Traversables.Tree.Cata
{ 
    public static partial class funcEtxnesion
    { 
        public static T Cata<T>(this Tree<T> @this, (Func<T, T> Leaf, Func<T, T, T, T> Node) algebra) =>
                @this.MatchWith(pattern: (
                  Leaf: v => algebra.Leaf(v),
                  Node: (l, v, r) => algebra.Node(r.Cata<T>(algebra), v, l.Cata<T>(algebra))
              ));
    }
     
    public class Demo
    { 
        public static void Run()
        {
            var tree = new Node<int>(new Node<int>(new Leaf<int>(1), 2, new Leaf<int>(3)),
                4,
                new Node<int>(new Leaf<int>(5), 6, new Leaf<int>(7)));
            //(Func<T, T> Leaf, Func<T, T, T, T> Node) algebra
            Console.WriteLine(
                tree.Cata(algebra: (
                Leaf: (v) => v,
                Node: (l, v, r) => l + v + r))
            );

            Console.WriteLine(tree.Show()); 
        }

    } 
}