using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catamorphisms.List.BaseFunctor
{ 
    public class Product<T, E>
    {
        public T Value { get; set; }
        public E Rest { get; set; }
    }
     
    public class SomeF<T, E>  //where E: SomeF<T, E>
    {
        private readonly Product<T, E> value;
        public SomeF(Product<T, E> value) => this.value = value;
    }

    public class SomeFixed<T, E> where E : SomeFixed<T, E>
    {
        private readonly Product<T, E> value;
        public SomeFixed(Product<T, E> value) => this.value = value;
    }

    public class Some<T>
    {
        private readonly Product<T, Some<T>> value;
        public Some(Product<T, Some<T>> value) => this.value = value;
    }

    public class Demo
    {
        public static void Run()
        {
         
            SomeF<int, int> s = new SomeF<int, int>(new Product<int, int>() { Value = 1, Rest = 1 });

            SomeF<int, SomeF<int, int>> s1 = new SomeF<int, SomeF<int, int>>(new Product<int, SomeF<int, int>>() { Value = 1, Rest = s });

            SomeF<int, SomeF<int, SomeF<int, int>>> s2 = new SomeF<int, SomeF<int, SomeF<int, int>>>(new Product<int, SomeF<int, SomeF<int, int>>>() { Value = 1, Rest = s1 });
 

        }
    }

}
