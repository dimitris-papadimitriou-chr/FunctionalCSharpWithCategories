using System;

namespace Catamorphisms.List.Ana
{
    public static class FunctionalExtensions
    {
        public static int Multiply(this Maybe<Product<int>> @this) =>
            @this.MatchWith(pattern: (
                   None: () => 1,
                   Some: (product) => product.Value * product.Rest.Multiply()
         ));

        public static Maybe<Product<int>> AnaToListBase(this int @n) =>
            @n == 0 ? (Maybe<Product<int>>)new None<Product<int>>() :
                       new Some<Product<int>>(new Product<int>()
                       {
                           Value = @n,
                           Rest = (@n - 1).AnaToListBase()
                       });
    }

    public class Product<T>
    {
        public T Value { get; set; }
        public Maybe<Product<T>> Rest { get; set; }
    }
    public abstract class Maybe<T>
    {
        public abstract Maybe<T1> Select<T1>(Func<T, T1> f);
        public abstract TResult MatchWith<TResult>((Func<TResult> None, Func<T, TResult> Some) pattern);
    }
    public class None<T> : Maybe<T>
    {
        public None() { }
        public override TResult MatchWith<TResult>((Func<TResult> None, Func<T, TResult> Some) pattern) => pattern.None();
        public override Maybe<T1> Select<T1>(Func<T, T1> f) => new None<T1>();
    }

    public class Some<T> : Maybe<T>
    {
        private readonly T value;
        public Some(T value) => this.value = value;
        public override TResult MatchWith<TResult>((Func<TResult> None, Func<T, TResult> Some) pattern) => pattern.Some(value);
        public override Maybe<T1> Select<T1>(Func<T, T1> f) => new Some<T1>(f(value));

    }
    public class Demo
    {
        public static void Run()
        {
            var result = 10.AnaToListBase().Multiply();
        }
    }

}
