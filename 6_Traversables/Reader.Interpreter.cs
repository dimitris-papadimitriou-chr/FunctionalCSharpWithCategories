using System;

namespace Traversables.Reader.Interpreter
{
    public static class FunctionalExtensions
    {
        public class Context
        {

        } 

        public static Reader<Env, T1> App<Env, T, T1>(this Reader<Env, Func<T, T1>> @this, Reader<Env, T> fa)
            => new Reader<Env, T1>(env => { return @this.Map(f => fa.Map(f).Run(env)).Run(env); });

        public static int Eval(this Expr<int> @this)
       => @this.MatchWith(pattern: (
           add: (x, y) => new Reader<object, Func<int, Func<int, int>>>(g => x1 => y1 => x1 + y1)
                       .App(new Reader<object, int>(g => x.Eval()))
                       .App(new Reader<object, int>(g => y.Eval()))
                       .Run(default),
           val: v => v));


        public static int Eval(this Expr<int> @this, Context context)
               => @this.MatchWith(pattern: (
                   add: (x, y) => new Reader<object, Func<int, Func<int, int>>>(g => x1 => y1 => x1 + y1)
                               .App(new Reader<object, int>(g => x.Eval(context)))
                               .App(new Reader<object, int>(g => y.Eval(context)))
                               .Run(context),
                   val: v => v));


        public static int EvalInt(this Expr<int> @this) =>
            @this.MatchWith<int>(pattern: (
                   add: (x, y) => x.EvalInt() + y.EvalInt(),
                   val: v => v
            ));

    }
    public abstract class Expr<T>
    {
        public abstract T1 MatchWith<T1>((Func<Expr<T>, Expr<T>, T1> add, Func<T, T1> val) pattern);
    }
    public class ValExpr<T> : Expr<T>
    {
        public ValExpr(T x)
        {
            X = x;
        }

        public T X { get; }

        public override T1 MatchWith<T1>((Func<Expr<T>, Expr<T>, T1> add, Func<T, T1> val) pattern)
                 => pattern.val(X);
    }
    public class AddExpr<T> : Expr<T>
    {
        public AddExpr(Expr<T> x, Expr<T> y)
        {
            X = x;
            Y = y;
        }

        public Expr<T> X { get; }
        public Expr<T> Y { get; }

        public override T1 MatchWith<T1>((Func<Expr<T>, Expr<T>, T1> add, Func<T, T1> val) pattern)
             => pattern.add(X, Y);
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
            var getName = new Reader<Config, string>(g => g.Name).
                Map(name => $"Name: {name }").
                Run(new Config { Name = "Sql" });

            var getName2 = new Reader<(int, int), Func<int, int>>(g => (r) => g.Item1 + r)
                .App(new Reader<(int, int), int>(g => 1))
                    .Run((1, 2));

            var t = new Reader<object, Func<int, Func<int, int>>>(g => x1 => y1 => x1 + y1)
                               .App(new Reader<object, int>(g => 1))
                               .App(new Reader<object, int>(g => 1))
                               .Run(default);

            var expression = new AddExpr<int>(new AddExpr<int>(new ValExpr<int>(2), new ValExpr<int>(2)), new ValExpr<int>(2));
            var result = expression.Eval();

        }
    }



}