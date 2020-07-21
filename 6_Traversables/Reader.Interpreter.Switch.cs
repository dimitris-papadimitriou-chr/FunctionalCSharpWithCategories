//using System;
//using System.Collections.Generic;

//namespace Traversables.Reader.Interpreter.Switch
//{
//    public static class FunctionalExtensions
//    {
//        public static T1 Cata<T, T1>(this Expr<T> @this,
//    (Func<T1, T1, T1> add, Func<T1, T1, T1> sub, Func<T, T1> val) algebra) =>
//                @this switch
//                {
//                    ValExpr<T> { X: var v } => algebra.val(v),
//                    AddExpr<T> { X: var x, Y: var y } => algebra.add(x.Cata(algebra), y.Cata(algebra)),
//                    _ => throw new NotImplementedException(),

//                };


//        public static Reader<Env, T1> App<Env, T, T1>(this Reader<Env, Func<T, T1>> @this, Reader<Env, T> fa)
//            => new Reader<Env, T1>(env => { return @this.Map(f => fa.Map(f).Run(env)).Run(env); });

//        //public static int Eval<TEnv>(this Expr<string> @this, IDictionary<string, int> context) =>
//        //    @this switch
//        //    {
//        //        ValExpr<string> { X: var v } => context[v],
//        //        AddExpr<string> { X: var x, Y: var y } =>
//        //            new Reader<IDictionary<string, int>, Func<int, Func<int, int>>>(g => x1 => y1 => x1 + y1)
//        //                        .App(new Reader<IDictionary<string, int>, int>(g => x.Eval<IDictionary<string, int>>(g)))
//        //                        .App(new Reader<IDictionary<string, int>, int>(g => y.Eval<IDictionary<string, int>>(g)))
//        //                        .Run(context)
//        //    };

//        public static T1 Eval<TEnv, T, T1>(this Expr<T> @this,
//      (Func<T1, T1, T1> add, Func<T, T1> val) algebra) =>
//             @this switch
//             {
//                 ValExpr<T> { X: var v } => algebra.val(v),
//                 AddExpr<T> { X: var x, Y: var y } =>
//                     new Reader<TEnv, Func<T1, Func<T1, T1>>>(g => x1 => y1 => algebra.add(x1, y1))
//                                 .App(new Reader<TEnv, T1>(g => x.Eval(g, algebra)))
//                                 .App(new Reader<TEnv, T1>(g => y.Eval(g, algebra)))
//                                 .Run(context)
//             };
//        //public static T1 Eval<TEnv, T, T1>(this Expr<T> @this, TEnv context,
//        //     (Func<T1, T1, T1> add, Func<T, T1> val) algebra) =>
//        //    @this switch
//        //    {
//        //        ValExpr<T> { X: var v } => algebra.val(v),
//        //        AddExpr<T> { X: var x, Y: var y } =>
//        //            new Reader<TEnv, Func<T1, Func<T1, T1>>>(g => x1 => y1 => algebra.add(x1, y1))
//        //                        .App(new Reader<TEnv, T1>(g => x.Eval(g, algebra)))
//        //                        .App(new Reader<TEnv, T1>(g => y.Eval(g, algebra)))
//        //                        .Run(context)
//        //    };
//        //add: (x, y) => new Reader<object, Func<int, Func<int, int>>>(g => x1 => y1 => x1 + y1)
//        //            .App(new Reader<object, int>(g => x.Eval()))
//        //            .App(new Reader<object, int>(g => y.Eval()))
//        //            .Run(default),
//        //val: v => v));


//        //public static int Eval(this Expr<int> @this, Context context)
//        //       => @this.MatchWith(pattern: (
//        //           add: (x, y) => new Reader<object, Func<int, Func<int, int>>>(g => x1 => y1 => x1 + y1)
//        //                       .App(new Reader<object, int>(g => x.Eval(context)))
//        //                       .App(new Reader<object, int>(g => y.Eval(context)))
//        //                       .Run(context),
//        //           val: v => v));


//        //public static int EvalInt(this Expr<int> @this) =>
//        //    @this.MatchWith<int>(pattern: (
//        //           add: (x, y) => x.EvalInt() + y.EvalInt(),
//        //           val: v => v
//        //    ));

//    }
//    public abstract class Expr<T>
//    {
//    }
//    public class ValExpr<T> : Expr<T>
//    {
//        public ValExpr(T x)
//        {
//            X = x;
//        }

//        public T X { get; }

//    }
//    public class AddExpr<T> : Expr<T>
//    {
//        public AddExpr(Expr<T> x, Expr<T> y)
//        {
//            X = x;
//            Y = y;
//        }
//        public Expr<T> X { get; }
//        public Expr<T> Y { get; }
//    }
//    public class Reader<Env, T>
//    {
//        public Func<Env, T> Fn { get; set; }
//        public Reader(Func<Env, T> fn) => Fn = fn;
//        public Reader<Env, T1> Map<T1>(Func<T, T1> f) => new Reader<Env, T1>((env) => f(Fn(env)));
//        public T Run(Env env) => Fn(env);
//    }
//    public class Config
//    {
//        public string Name { get; set; }
//    }

//    public class Demo
//    {
//        public static void Run()
//        {
//            var getName = new Reader<Config, string>(g => g.Name).
//                Map(name => $"Name: {name }").
//                Run(new Config { Name = "Sql" });

//            var getName2 = new Reader<(int, int), Func<int, int>>(g => (r) => g.Item1 + r)
//                .App(new Reader<(int, int), int>(g => 1))
//                    .Run((1, 2));

//            var t = new Reader<object, Func<int, Func<int, int>>>(g => x1 => y1 => x1 + y1)
//                               .App(new Reader<object, int>(g => 1))
//                               .App(new Reader<object, int>(g => 1))
//                               .Run(default);

//            var expression = new AddExpr<string>(new AddExpr<string>(new ValExpr<string>("a"), new ValExpr<string>("b")),
//                new ValExpr<string>("c"));
//            var context = new Dictionary<string, int> { {"a",1 },
//            {"b",1 } ,
//            {"c",1 } };

//            var result = expression.Eval(
//                algebra: (
//                    add: (x, y) => x + y,
//                    val: v => context[v]
//                ));

//        }
//    }



//}