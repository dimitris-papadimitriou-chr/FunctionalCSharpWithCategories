using System;


namespace Interpreter.Cata
{
    public static class FunctionalExtensions
    { 
        public static Reader<Env, T1> App<Env, T, T1>(this Reader<Env, Func<T, T1>> @this, Reader<Env, T> fa)
            => new Reader<Env, T1>(env => { return @this.Map(f => fa.Map(f).Run(env)).Run(env); });
         
        public static T1 Eval<T, T1>(this Expr<T> @this,
            (Func<T1, T1, T1> add, Func<T1, T1, T1> sub, Func<T, T1> val) algebra)
            =>
            @this.MatchWith(pattern: (
                   add: (x, y) => algebra.add(x.Eval(algebra), y.Eval(algebra)),
                   sub: (x, y) => algebra.sub(x.Eval(algebra), y.Eval(algebra)),
                   val: v => algebra.val(v)
            ));

    }
    public abstract class Expr<T>
    {
        public abstract T1 MatchWith<T1>((Func<Expr<T>, Expr<T>, T1> add, Func<Expr<T>, Expr<T>, T1> sub, Func<T, T1> val) pattern);
    }
    public class ValExpr<T> : Expr<T>
    {
        public ValExpr(T x) { X = x; }
        public T X { get; }
        public override T1 MatchWith<T1>((Func<Expr<T>, Expr<T>, T1> add, Func<Expr<T>, Expr<T>, T1> sub, Func<T, T1> val) pattern)
                 => pattern.val(X);
    }

    public class AddExpr<T> : Expr<T>
    {
        public AddExpr(Expr<T> x, Expr<T> y) { X = x; Y = y; }

        public Expr<T> X { get; }
        public Expr<T> Y { get; }

        public override T1 MatchWith<T1>(
            (Func<Expr<T>, Expr<T>, T1> add,
            Func<Expr<T>, Expr<T>, T1> sub,
            Func<T, T1> val) pattern)
             => pattern.add(X, Y);
    }
    public class SubExpr<T> : Expr<T>
    {
        public SubExpr(Expr<T> x, Expr<T> y) { X = x; Y = y; }

        public Expr<T> X { get; }
        public Expr<T> Y { get; }

        public override T1 MatchWith<T1>((Func<Expr<T>, Expr<T>, T1> add,
             Func<Expr<T>, Expr<T>, T1> sub,
            Func<T, T1> val) pattern)
             => pattern.sub(X, Y);
    }
    public class Reader<Env, T>
    {
        public Func<Env, T> Fn { get; set; }
        public Reader(Func<Env, T> fn) => Fn = fn;
        public Reader<Env, T1> Map<T1>(Func<T, T1> f) => new Reader<Env, T1>((env) => f(Fn(env)));
        public T Run(Env env) => Fn(env);
    }
    public class Config
    {
        public string Name { get; set; }
    }

    public class Demo
    {
        public static void Run()
        {
            var expression = new SubExpr<int>(new AddExpr<int>(new ValExpr<int>(2), new ValExpr<int>(2)), new ValExpr<int>(2));

            var result = expression.Eval(algebra: (
                    add: (x, y) => x + y,
                    sub: (x, y) => x + y,
                    val: v => v)
                );

        }
    }



}