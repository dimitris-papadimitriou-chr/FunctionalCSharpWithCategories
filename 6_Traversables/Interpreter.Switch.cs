using System;

namespace Traversables.Interpreter.Switch
{
    public static class FunctionalExtensions
    {
   
        public static T1 Cata<T, T1>(this Expr<T> @this,
            (Func<T1, T1, T1> add, Func<T1, T1, T1> sub, Func<T, T1> val) algebra) =>
            @this switch
            {
                ValExpr<T> { X: var v } => algebra.val(v),
                AddExpr<T> { X: var x, Y: var y } => algebra.add(x.Cata(algebra), y.Cata(algebra)),
                SubExpr<T> { X: var x, Y: var y } => algebra.sub(x.Cata(algebra), y.Cata(algebra)),
                _ => throw new NotImplementedException(),
            };


    }
    public abstract class Expr<T>
    {
    }
    public class ValExpr<T> : Expr<T>
    {
        public ValExpr(T x) { X = x; }
        public T X { get; } 
    }

    public class AddExpr<T> : Expr<T>
    {
        public AddExpr(Expr<T> x, Expr<T> y) { X = x; Y = y; }

        public Expr<T> X { get; }
        public Expr<T> Y { get; } 

    }
    public class SubExpr<T> : Expr<T>
    {
        public SubExpr(Expr<T> x, Expr<T> y) { X = x; Y = y; }
        public Expr<T> X { get; }
        public Expr<T> Y { get; } 
    }
  
    public class Demo
    {
        public static void Run()
        {
            var expression = new SubExpr<int>(new AddExpr<int>(new ValExpr<int>(2),
                new ValExpr<int>(2)),
                new ValExpr<int>(2));

            var result = expression.Cata(algebra: (
                    add: (x, y) => x + y,
                    sub: (x, y) => x - y,
                    val: v => v)
                );

        }
    }



}
