using System;
using System.Collections.Generic;
 
namespace Functors.Maybe 
{
    public static class FunctionalExtensions
    {
        public static T1 MatchWith<T, T1>(this List<T> @this, (Func<T1> Empty, Func<T, List<T>, T1> Cons) algebra) =>
        (@this.Count == 0) ?
        algebra.Empty() :
        algebra.Cons(@this[0], @this.GetRange(1, @this.Count - 1)); 

        public static Maybe<T> FirstOrNone<T>(this List<T> @this, Func<T, bool> predicate) =>
          @this.MatchWith<T, Maybe<T>>(algebra: (
                 Empty: () => new None<T>(),
                 Cons: (v, r) => predicate(v) ?
                     new Some<T>(v) :
                     r.FirstOrNone(predicate)
           ));

    }
    public abstract class Maybe<T>
    {
        public abstract Maybe<T1> Map<T1>(Func<T, T1> f);
        public abstract TResult MatchWith<TResult>((Func<TResult> None, Func<T, TResult> Some) pattern);
    }
    public class None<T> : Maybe<T>
    {
        public None() { }
        public override TResult MatchWith<TResult>((Func<TResult> None, Func<T, TResult> Some) pattern) => pattern.None();
        public override Maybe<T1> Map<T1>(Func<T, T1> f) => new None<T1>();
    }

    public class Some<T> : Maybe<T>
    {
        private readonly T value;
        public Some(T value) => this.value = value;
        public override TResult MatchWith<TResult>((Func<TResult> None, Func<T, TResult> Some) pattern) => pattern.Some(value);
        public override Maybe<T1> Map<T1>(Func<T, T1> f) => new Some<T1>(f(value));

    }
    public class Demo
    {
        public static void Run()
        {
            var result = new None<int>()
                .Map(v => $"number is  : {v}")
                .MatchWith<string>(pattern: (
                    None: () => "Not found",
                    Some: v => $"Answer - {v}"
            ));
        }
    }

}
