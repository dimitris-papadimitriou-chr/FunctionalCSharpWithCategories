using System;
 
namespace Functors.Reader.Applicative
{
    public static class FunctionalExtensions
    {
        public static Reader<Env, T1> App<Env, T, T1>(this Reader<Env, Func<T, T1>> @this, Reader<Env, T> fa)
            => new Reader<Env, T1>(env => { return @this.Map(f => fa.Map(f).Run(env)).Run(env); });

    }

    //public class Reader<Env, T>
    //{
    //    public Func<Env, T> Fn { get; set; }
    //    public Reader(Func<Env, T> fn) => Fn = fn;
    //    public Reader<Env, T1> Map<T1>(Func<T, T1> f) => new Reader<Env, T1>((env) => f(Fn(env)));
    //    public T Run(Env env) => Fn(env);
    //}

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

        }
    } 
}