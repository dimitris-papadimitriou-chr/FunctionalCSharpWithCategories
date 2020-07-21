using System;

namespace Functors.Tree.Switch
{

    public static partial class FunctionalExt
    {
        public static string Show<T>(this Tree<T> @this) =>
              @this switch
              {
                  Leaf<T> { Value: var v } => $"({v})",
                  Node<T> { Left: var l, Value: var v, Right: var r } => $"({l.Show()},{v},{r.Show()})",
              };
         
        public static Tree<T> Reverse<T>(this Tree<T> @this) =>
            @this switch
            {
                Leaf<T> { Value: var v } => new Leaf<T>(v),
                Node<T> { Left: var l, Value: var v, Right: var r } =>
                                       new Node<T>(r.Reverse(), v, l.Reverse()),
            };

    }

    public abstract class Tree<T> { }

    public class Node<T> : Tree<T>
    {
        public Tree<T> Left { get; }
        public T Value { get; }
        public Tree<T> Right { get; }
        public Node(Tree<T> left, T value, Tree<T> right)
        {
            Left = left;
            Value = value;
            Right = right;
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
            var tr = new Node<int>(new Node<int>(new Leaf<int>(1), 2, new Leaf<int>(3)), 4, new Node<int>(new Leaf<int>(5), 6, new Leaf<int>(7)));
            Console.WriteLine(tr.Show());
            Console.WriteLine(tr.Reverse().Show());
        }
    }
}