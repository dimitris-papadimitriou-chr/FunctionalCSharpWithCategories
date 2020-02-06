using System;
using System.Collections.Generic;
using Functors.Tree.Binary;
using Functors.Id.Applicative;

namespace Traversables.Compose
{
    public static partial class funcEtxnesion
    { 
        public static List<T> Concat<T>(this List<T> @this, List<T> range)
        {
            @this.AddRange(range);
            return @this;
        }
        public static List<T> AsList<T>(this T @this) => new List<T>() { @this };
         
        public static T1 MatchWith<T, T1>(this List<T> @this, (Func<T1> Empty, Func<T, List<T>, T1> Cons) pattern) =>
                (@this.Count == 0) ?
                pattern.Empty() :
                pattern.Cons(@this[0], @this.GetRange(1, @this.Count - 1));

        public static Id<Tree<T>> Traverse<T>(this Tree<T> @this, Func<T, Id<T>> f) =>
        @this.MatchWith(pattern: (
       Leaf: v => new Id<Func<T, Tree<T>>>(x => new Leaf<T>(x)).Ap(f(v)),
       Node: (l, r) => new Id<Func<Tree<T>, Func<Tree<T>, Tree<T>>>>(x => y => new Node<T>(x, y))
               .Ap(l.Traverse(f))
               .Ap(r.Traverse(f))
        ));

        public static Id<List<T>> Traverse<T>(this List<T> @this, Func<T, Id<T>> f)
            =>
             @this.MatchWith(pattern: (
                 Empty: () => new Id<List<T>>(new List<T>()),
                 Cons: (v, r) =>
                    new Id<Func<T, Func<List<T>, List<T>>>>(x => y => x.AsList().Concat(y))
                        .Ap(f(v))
                        .Ap(r.Traverse(f))
           ));

        public static Id<List<Tree<T>>> TraverseT<T>(this List<Tree<T>> @this, Func<T, Id<T>> f)
            => @this.Traverse(x => x.Traverse(f));
        public static Id<Tree<List<T>>> TraverseT<T>(this Tree<List<T>> @this, Func<T, Id<T>> f)
           => @this.Traverse(x => x.Traverse(f));

    }


    public class Demo
    {

        public static void Run()
        {


            var treeOfLists = new Node<List<int>>(new Leaf<List<int>>(new List<int>() { 1, 2 })
                , new Node<List<int>>(new Leaf<List<int>>(new List<int>() { 3, 4 }),
                new Leaf<List<int>>(new List<int>() { 5, 6 })));

            var traversedTree = treeOfLists.TraverseT(x => new Id<int>(x + 1));


            var listOfTrees = new List<Tree<int>>() {
    new Node<int>(new Leaf<int>(2), new Node<int>(new Leaf<int>(2), new Leaf<int>(4)))
    ,  new Node<int>(new Leaf<int>(2), new Leaf<int>(4)) };

            var traversedList = listOfTrees.TraverseT(x => new Id<int>(x + 1));


        }

    }



}