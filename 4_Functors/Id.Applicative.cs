using System;

namespace Functors.Id.Applicative
{
    public static class FunctionalExtensions
    { 
        public static Id<T1> Ap<T, T1>(this Id<Func<T, T1>> @this, Id<T> fa) => @this.Map(f => fa.Cata(f));

    }
    public class Demo
    {
        public static void Run()
        {
            var chain = new Id<Func<int, Func<int, int>>>(x => y => x + y).Ap(new Id<int>(1)).Ap(new Id<int>(1));
            chain.Cata(Console.WriteLine);
        }
    }

    public class Id<T>
    {
        public T Value { get; set; }
        public Id(T value) => Value = value;
        public Id<T1> Map<T1>(Func<T, T1> f) => new Id<T1>(f(Value));
        public T1 Cata<T1>(Func<T, T1> callback) => callback(Value);
        public void Cata(Action<T> callback) => callback(Value);
    }

}