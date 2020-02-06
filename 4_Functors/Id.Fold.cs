using System;
using System.Collections.Generic;

namespace Functors.Id.Fold
{
    public static class FunctionalExtensions
    {
        public static List<T> ToList<T>(this T @this) => new List<T>() { @this }; 
        public static List<T> Concat<T>(this List<T> @this, List<T> range)
        {
            @this.AddRange(range);
            return @this;
        } 
    }
    public class Demo
    {
        public static void Run()
        {
            var toArray = new Id<int>(4).Fold<List<int>>(new List<int>(), (e, a) => a.Concat(e.ToList()));
            var multiply = new Id<int>(4).Fold<int>(1, (e, a) => a * e);
        }
    }

    public class Id<T>
    {
        public T Value { get; set; }
        public Id(T value) => Value = value;
        public Id<T1> Map<T1>(Func<T, T1> f) => new Id<T1>(f(Value));
        public TM Fold<TM>(TM state, Func<T, TM, TM> reducer) => reducer(Value, state); 
    }

}