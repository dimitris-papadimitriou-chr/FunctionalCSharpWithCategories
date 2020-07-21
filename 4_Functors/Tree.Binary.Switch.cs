using System;

namespace Functors.Tree.Binary.Switch
{
    public static partial class funcEtxnesion
    {
        public static T1 Cata<T, T1>(this Tree<T> @this,
            (Func<T, T1> Leaf, Func<T1, T1, T1> Node) algebra) =>
                @this switch
                {
                    Leaf<T> { Value: var v } => algebra.Leaf(v),
                    Node<T> { Left: var l, Right: var r } => algebra.Node(l.Cata(algebra), r.Cata(algebra)),
                    _ => throw new NotImplementedException()
                };

        public static string Show<T>(this Tree<T> @this) =>
                   @this.Cata<T, string>(algebra: (
                       Leaf: v => $"{v}",
                       Node: (l, r) => $"({l},{r})"
                   ));

        public static Tree<T1> Map<T, T1>(this Tree<T> @this, Func<T, T1> f) =>
            @this.Cata<T, Tree<T1>>(
           (
               Leaf: (v) => new Leaf<T1>(f(v)),
               Node: (l, r) => new Node<T1>(l, r)
            ));

        public static Tree<T1> Map1<T, T1>(this Tree<T> @this, Func<T, T1> f) =>
              @this switch
              {
                  Leaf<T> { Value: var v } => new Leaf<T1>(f(v)),
                  Node<T> { Left: var l, Right: var r } => new Node<T1>(l.Map1(f), r.Map1(f)),
                  _ => throw new NotImplementedException()
              };


        public static Tree<T> Reverse<T>(this Tree<T> @this) =>
              @this switch
              {
                  Leaf<T> { Value: var v } => new Leaf<T>(v),
                  Node<T> { Left: var l, Right: var r } => new Node<T>(r, l),
                  _ => throw new NotImplementedException()
              };

    }

    public class Tree<T> { }

    public class Node<T> : Tree<T>
    {
        public Tree<T> Left { get; set; }
        public Tree<T> Right { get; set; }
        public Node(Tree<T> left, Tree<T> right)
        {
            this.Left = left;
            this.Right = right;
        }
    }

    public class Leaf<T> : Tree<T>
    {
        public T Value { get; }
        public Leaf(T value) => Value = value;
    }

    public class Demo
    {
        public static void Run()
        {
        }
    }
}