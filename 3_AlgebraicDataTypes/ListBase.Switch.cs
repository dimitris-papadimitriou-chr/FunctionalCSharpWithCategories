using System;
using System.Diagnostics;

namespace ListBase.MatchWith
{
    public static partial class funcEtxnesion
    {
        public static ListBase<T1> Map<T, T1>(this ListBase<T> @this, Func<T, T1> f) =>
             @this switch
             {
                 Empty<T> { } => new Empty<T1>(),
                 Cons<T> { Value: var value, Rest: var rest } => new Cons<T1>(f(value), rest.Map(f))
             };

        public static T Multiply<T>(this ListBase<T> @this, (Func<T, T, T> concat, T empty) monoid) =>
            @this switch
            {
                Empty<T> { } => monoid.empty,
                Cons<T> { Value: var value, Rest: var rest } =>
                        monoid.concat(value, rest.Multiply(monoid))
            };

        public static string Show<T>(this ListBase<T> @this) =>
          @this switch
          {
              Empty<T> { } => "",
              Cons<T> { Value: var value, Rest: var rest } => $"({value}, {rest.Show()})"
          };



    }
    public abstract class ListBase<T> { }
    public class Empty<T> : ListBase<T> { }
    public class Cons<T> : ListBase<T>
    {
        public ListBase<T> Rest { get; set; }
        public T Value { get; set; }
        public Cons(T value, ListBase<T> rest)
        {
            this.Value = value;
            this.Rest = rest;
        }
    }

    public class Demo
    {
        public static void Run()
        {
            var list = new Cons<int>(value: 2, rest: new Cons<int>(value: 5, rest: new Empty<int>()));
            Console.WriteLine(list.Show());
            var sum = list.Multiply(monoid: (concat: (x, y) => x + y, empty: 0));
            Console.WriteLine(sum);
            var sum1 = list.Multiply(((x, y) => x + y, 0));
            Console.WriteLine(sum1);
        }
    }

}
