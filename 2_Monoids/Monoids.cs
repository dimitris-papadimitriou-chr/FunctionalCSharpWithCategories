using System;
using System.Collections.Generic;
using System.Linq;

namespace Monoids
{
    public interface IMonoid<T>
    {
        T Empty { get; }
        T Concat(T x, T y);
    }
    public interface IMonoidAcc<T>
    {
        T Identity { get; set; }
        IMonoidAcc<T> Concat(IMonoidAcc<T> m);
    }
    public class Sum : IMonoidAcc<int>
    { 
        public Sum(int value) => Identity = value;
        public int Identity { get; set; }
        public IMonoidAcc<int> Concat(IMonoidAcc<int> m) => new Sum(m.Identity + Identity);
    }

    public class DemoMonoidAcc
    {


        public async static void Run()
        {

            {
                var total = new Sum(0).Concat(new Sum(1)).Concat(new Sum(2)).Concat(new Sum(3));
            }
            {
                (int empty, Func<int, int, int> concat) sum = (0, (x, y) => x + y);
                var total = sum.concat(sum.concat(sum.empty, 2), 3);

            }

        }
    }


    public class Demo
    {
        public class Sum : IMonoid<int>
        {
            public int Concat(int x, int y) => x + y;
            public int Empty => 0;
        }
        public static void Run()
        {
            {
                var sum = new Sum();
                var total = sum.Concat(1, sum.Concat(3, 4));
                total = new[] { 1, 3, 4, 5 }.Aggregate(sum.Empty, sum.Concat);
            }
            //Multiply 
            {
                var list = new[] { 1, 2, 3, 4 };
                var total = 0;
                foreach (var item in list)
                    total = total + item;
            }

            //Multiply
            {
                var list = new[] { 1, 2, 3, 4 };
                var total = 1;
                foreach (var item in list)
                    total = total * item;
            }

            //Max
            {
                var list = new[] { 1, 2, 3, 4 };
                var total = int.MinValue;
                Func<int, int, int> max = (x, y) => x > y ? x : y;
                foreach (var item in list)
                    total = max(total, item); //total = total > item ? total : item;
            }

            {
                var sum = Fold(new[] { 1, 2, 3, 4 }, 0, (x, y) => x + y);
                var product = Fold(new[] { 1, 2, 3, 4 }, 1, (x, y) => x * y);
                var max = Fold(new[] { 1, 2, 3, 4 }, int.MinValue, (x, y) => x > y ? x : y);
            }


        }

        public static T Fold<T>(IEnumerable<T> list, T acumulate, Func<T, T, T> concat)
        {
            foreach (var item in list)
                acumulate = concat(acumulate, item);
            return acumulate;

        }
        public T Fold<T>(List<T> list, (T Empty, Func<T, T, T> Concat) monoid)
        {
            var accumulate = monoid.Empty;
            foreach (var item in list)
                accumulate = monoid.Concat(accumulate, item);
            return accumulate;
        }

        public T Fold<T>(List<T> list, IMonoid<T> monoid)
        {
            var accumulate = monoid.Empty;
            foreach (var item in list)
                accumulate = monoid.Concat(accumulate, item);
            return accumulate;
        }
    }



}