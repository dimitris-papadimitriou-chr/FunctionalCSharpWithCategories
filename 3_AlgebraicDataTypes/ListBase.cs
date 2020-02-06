using System;
using System.Diagnostics;

namespace FPCSharp
{
    public abstract class ListBase<T>
    {
        public abstract ListBase<T1> Map<T1>(Func<T, T1> f);
         public abstract T Multiply((Func<T, T, T> concat, T empty) monoid);
    }

    public class Cons<T> : ListBase<T>
    {
        public ListBase<T> Rest { get; set; }
        public T Value { get; set; }
        public Cons(T value, ListBase<T> rest)
        {
            this.Value = value;
            this.Rest = rest;
        }

        public override ListBase<T1> Map<T1>(Func<T, T1> f) => new Cons<T1>(f(Value), Rest.Map(f));
        public override string ToString() => $"new Cons<T1>({Value}, {Rest.ToString()})";
        public override T Multiply((Func<T, T, T> concat, T empty) monoid) => monoid.concat(Value, Rest.Multiply(monoid));

    }
    public class Empty<T> : ListBase<T>
    {
        public override ListBase<T1> Map<T1>(Func<T, T1> f) => new Empty<T1>();
        public override T Multiply((Func<T, T, T> concat, T empty) monoid) => monoid.empty;
        public override string ToString() => "Empty()";
    }


    public class Demo
    {
        public static void Run()
        {
            var list = new Cons<int>(value: 2, rest: new Cons<int>(value: 5, rest: new Empty<int>()));
            Debug.WriteLine(list.ToString());
            var list2 = list.Map(x => x + 2);
            Debug.WriteLine(list2.ToString());
            var multiply = list.Multiply(monoid: (concat: (x, y) => x * y, empty: 1)); 
        }
         
    }

}
