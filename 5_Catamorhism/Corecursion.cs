using System;
using Functors.Maybe;

namespace Catamorphisms.Corecursion
{
    public static class FunctionalExtensions
    {

        public static string Show(this Maybe<Product<int>> @this) =>
            @this.MatchWith(pattern: (
                   None: () => "{}",
                   Some: (product) => $"(Value:{product.Value.ToString()},Rest:{product.Rest.Show()})"
         ));

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
   
    public class Demo
    {
        public static void Run()
        {
             
            Func<int, Maybe<Product<int>>, Maybe<Product<int>>> Coana = null;
             
            Coana = (n, r) =>
            {
                Console.WriteLine(r.Show());
                return Coana(n + 1, new Some<Product<int>>(new Product<int>() { Value = n, Rest = r }));
            };

            var stream = Coana(0, new None<Product<int>>());// Will throw Exception 

            //(Value: 0, Rest:{ })
            //(Value: 1, Rest: (Value: 0, Rest:{ }))
            //(Value: 2, Rest: (Value: 1, Rest: (Value: 0, Rest:{ })))
            //(Value: 3, Rest: (Value: 2, Rest: (Value: 1, Rest: (Value: 0, Rest:{ }))))
            //(Value: 4, Rest: (Value: 3, Rest: (Value: 2, Rest: (Value: 1, Rest: (Value: 0, Rest:{ })))))
            //(Value: 5, Rest: (Value: 4, Rest: (Value: 3, Rest: (Value: 2, Rest: (Value: 1, Rest: (Value: 0, Rest:{ }))))))
                 
        }
    }

}
