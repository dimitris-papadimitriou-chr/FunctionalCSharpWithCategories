using System;

namespace Functors.Tree.Binary
{
    public static partial class funcEtxnesion
    {
        public static string Show<T>(this Tree<T> @this) =>
            @this.MatchWith<string>(pattern: (
                Leaf: v => $"{v}",
                Node: (l, r) => $"({l.Show()},{r.Show()})"
            ));
    }
    public abstract class Tree<T>
    {
        public abstract Tree<T1> Map<T1>(Func<T, T1> f);
        public abstract T1 MatchWith<T1>((Func<T, T1> Leaf, Func<Tree<T>, Tree<T>, T1> Node) pattern);
    }

    public class Node<T> : Tree<T>
    {
        public Tree<T> Left { get; set; }
        public Tree<T> Right { get; set; }
        public Node(Tree<T> left, Tree<T> right)
        {
            this.Left = left;
            this.Right = right;
        }

        public override Tree<T1> Map<T1>(Func<T, T1> f) => new Node<T1>(Left.Map(f), Right.Map(f));
        public override T1 MatchWith<T1>((Func<T, T1> Leaf, Func<Tree<T>, Tree<T>, T1> Node) pattern)
                                                                     => pattern.Node(Left, Right);
    }

    public class Leaf<T> : Tree<T>
    {
        public T Value { get; }
        public Leaf(T value) => Value = value;
        public override Tree<T1> Map<T1>(Func<T, T1> f) => new Leaf<T1>(f(Value));
        public override T1 MatchWith<T1>((Func<T, T1> Leaf, Func<Tree<T>, Tree<T>, T1> Node) pattern)
                                                                   => pattern.Leaf(Value);
    }

    public class Demo
    {
        public static void Run()
        {
        }
    }
}