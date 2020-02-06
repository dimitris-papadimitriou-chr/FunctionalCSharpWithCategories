using System;

namespace Functors.Tree
{

    public static partial class funcEtxnesion
    {
        public static string Show<T>(this Tree<T> @this) =>
            @this.MatchWith<string>(pattern: (
                Leaf: v => $"{v}",
                Node: (l,v, r) => $"({l.Show()},{v},{r.Show()})"
            ));
    }
    public abstract class Tree<T>
    {
        public abstract Tree<T1> Map<T1>(Func<T, T1> f);
        public abstract T1 MatchWith<T1>((Func<T, T1> Leaf, Func<Tree<T>, T, Tree<T>, T1> Node) pattern);
    }

    public class Node<T> : Tree<T>
    {
        public Tree<T> Left { get; set; }
        public T Value { get; set; }
        public Tree<T> Right { get; set; }

        public Node(Tree<T> left, T value, Tree<T> right)
        {
            this.Left = left;
            Value = value;
            this.Right = right;
        }

        public override Tree<T1> Map<T1>(Func<T, T1> f) => new Node<T1>(Left.Map(f), f(Value), Right.Map(f));
        public override T1 MatchWith<T1>((Func<T, T1> Leaf, Func<Tree<T>, T, Tree<T>, T1> Node) pattern)
                                                                     => pattern.Node(Left, Value, Right);
    }

    public class Leaf<T> : Tree<T>
    {
        public T Value { get; }
        public Leaf(T value) => Value = value;
        public override Tree<T1> Map<T1>(Func<T, T1> f) => new Leaf<T1>(f(Value));
        public override T1 MatchWith<T1>((Func<T, T1> Leaf, Func<Tree<T>, T, Tree<T>, T1> Node) pattern)
                                                                   => pattern.Leaf(Value);
    }

    public class Demo
    {
        public static void Run()
        {
        }
    }
}