

namespace Catamorphisms.Visitor
{
    public abstract class Visitor<T, T1>
    {
        public abstract T1 VisitNode(Node<T> node);
        public abstract T1 VisitLeaf(Leaf<T> leaf);
    }

    public class AlgebraSumVisitor : Visitor<int, int>
    {
        public override int VisitLeaf(Leaf<int> leaf) => leaf.V;
        public override int VisitNode(Node<int> node) =>
            node.Left.Accept(this) + node.Value + node.Right.Accept(this);
    }

    public abstract class Tree<T>
    {
        public abstract T1 Accept<T1>(Visitor<T, T1> visitor);
    }

    public class Node<T> : Tree<T>
    {
        public Tree<T> Left { get; set; }
        public T Value { get; set; }
        public Tree<T> Right { get; set; }
        public Node(Tree<T> left, T value, Tree<T> right)
        {
            this.Left = left;
            this.Value = value;
            this.Right = right;
        }
        public override T1 Accept<T1>(Visitor<T, T1> visitor) => visitor.VisitNode(this);
    }

    public class Leaf<T> : Tree<T>
    {
        public T V { get; }
        public Leaf(T v) => V = v;
        public override T1 Accept<T1>(Visitor<T, T1> visitor) => visitor.VisitLeaf(this);
    }


    public class Demo
    {
        public static void Run()
        {
            var tree = new Node<int>(new Node<int>(new Leaf<int>(1), 2, new Leaf<int>(3)), 
                4, new Node<int>(new Leaf<int>(5), 6, new Leaf<int>(7)));

            var sum = tree.Accept(new AlgebraSumVisitor());
        }

    }


}