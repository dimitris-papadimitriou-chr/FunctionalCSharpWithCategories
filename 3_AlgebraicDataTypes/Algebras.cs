using System;
using System.Collections.Generic;
 
namespace Algebras
{ 
    public class Demo
    {
        public class Product
        {
            public int A { get; set; }
            public int B { get; set; }
        }

        public static void GetDiscount(int A, int B) { }
        public static void GetDiscount((int A, int B) product) { } 
        public static void GetDiscount(Product product) { }

        public static void Run()
        {
            {
                (int A, int B) product = (A: 10, B: 20);

                Func<(int A, int B), int> first = p => p.A;
                Func<(int A, int B), int> second = p => p.B;
            }
            {
                Product product = new Product { A = 10, B = 20 };
                Func<Product, int> first = p => p.A;
                Func<Product, int> second = p => p.B; 
            }
  
            {

                var product = new List<int> { 10, 20 };
                Func<List<int>, int> first = p => p[0];
                Func<List<int>, int> second = p => p[1];
            }
        }
    }

}
