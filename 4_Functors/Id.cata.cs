using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Functors.Id.Cata
{
    public static class FunctionalExtensions
    {
        public static Func<T, T2> Compose<T, T1, T2>(this Func<T, T1> func1, Func<T1, T2> func2)
        {
            return x => func2(func1(x));
        }

    }
    public class Demo
    {
        public static void Run()
        {
            new Id<int>(3).Cata(Console.WriteLine);
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