using System;

namespace Functors.IO
{
    public static class Unit { }
    public static class FunctionalExtensions
    {
        public static IO<T> ToIO<T>(this T @this) => new IO<T>(() => @this);
        public static IO<T> ToIO<T>(this Func<T> @this) => new IO<T>(@this);
        public static Lazy<T1> Map<T, T1>(this Lazy<T> @this, Func<T, T1> f) => new Lazy<T1>(() => f(@this.Value));
        public static Func<T1> Map<T, T1>(this Func<T> @this, Func<T, T1> f) => new Func<T1>(() => f(@this()));

    }
    public class IO<T>
    {
        public Func<T> Fn { get; set; }
        public IO(Func<T> fn) => Fn = fn;
        public IO<T1> Map<T1>(Func<T, T1> f) => new IO<T1>(() => f(Fn()));
        public T1 MatchWith<T1>(Func<T, T1> callback) => callback(Fn());
        public void MatchWith(Action<T> callback) => callback(Fn());
        public T Run() => Fn();
    }

    public class Demo
    {
        public static void Run()
        {
            var readKeyInstruction = new IO<ConsoleKeyInfo>(() => Console.ReadKey());
             
        }
    }
}