using System;
using Functors.Maybe;

namespace Catamorphisms.List
{
    public static class FunctionalExtensions
    {
        public static int Multiply(this Maybe<Product<int>> @this) =>
            @this.MatchWith(pattern: (
                   None: () => 1,
                   Some: (product) => product.Value * product.Rest.Multiply()
         )); 
    }

    public class Product<T>
    {
        public T Value { get; set; }
        public Maybe<Product<T>> Rest { get; set; }
    }

    public class Demo
    {
        public static void Run()
        {
            var listBase =
                new Some<Product<int>>(new Product<int>
                {
                    Value = 5,
                    Rest = new Some<Product<int>>(
                        new Product<int>
                        {
                            Value = 3,
                            Rest = new None<Product<int>>()
                        })
                });
            var r = listBase.Multiply();
        }
    }

}