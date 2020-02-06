using System;
using System.Diagnostics;

namespace ListBase.MatchWith
{
    public static partial class funcEtxnesion
    {
        public static T Multiply<T>(this ListBase<T> @this, (Func<T, T, T> concat, T empty) monoid) =>
        @this.MatchWith(pattern: (
            Empty: () => monoid.empty,
            Cons: (value, rest) => monoid.concat(value, rest.Multiply(monoid))
        ));

        public static ListBase<T1> Map<T, T1>(this ListBase<T> @this, Func<T, T1> f) =>
      @this.MatchWith<ListBase<T1>>(pattern: (
          Empty: () => new Empty<T1>(),
          Cons: (value, rest) => new Cons<T1>(f(value), rest.Map(f))
      ));
    }
    public abstract class ListBase<T>
    {
         public T Multiply((Func<T, T, T> concat, T empty) monoid) =>
         this.MatchWith(pattern: (
             Empty: () => monoid.empty,
             Cons: (value, rest) => monoid.concat(value, rest.Multiply(monoid))
         ));

        public abstract T1 MatchWith<T1>((Func<T1> Empty, Func<T, ListBase<T>, T1> Cons) pattern);

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
        public override T1 MatchWith<T1>((Func<T1> Empty, Func<T, ListBase<T>, T1> Cons) pattern) => pattern.Cons(Value, Rest);
    }
    public class Empty<T> : ListBase<T>
    { 
        public override T1 MatchWith<T1>((Func<T1> Empty, Func<T, ListBase<T>, T1> Cons) pattern) => pattern.Empty(); 
    }
     
    public class Demo
    {
        public static void Run()
        {
            var list = new Cons<int>(value: 2, rest: new Cons<int>(value: 5, rest: new Empty<int>()));
            Debug.WriteLine(list.ToString());
            var list2 = list.Map(x => x + 2);
            Debug.WriteLine(list2.ToString()); 
        } 
    }

}
