using System;
 
namespace Functors.Reader 
{ 
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

        }
    }



}