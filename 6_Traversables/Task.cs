using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Traversables.Task
{

    public static partial class FunctionalExt
    {
        public static List<T> AsList<T>(this T @this) => new List<T>() { @this };
         public static List<T> Concat<T>(this List<T> @this, List<T> range)
        {
            @this.AddRange(range);
            return @this;
        }

        public static Task<List<T>> Distribute<T>(this List<Task<T>> @this)
            =>
             @this.MatchWith(pattern: (
                 Empty: () => System.Threading.Tasks.Task.FromResult(new List<T>()),
                 Cons: (v, r) =>
                  System.Threading.Tasks.Task
                 .FromResult<Func<T, Func<List<T>, List<T>>>>(x => y => x.AsList().Concat(y))
                 .Ap(v)
                 .Ap(r.Distribute())
           ));

        public static Task<List<T1>> Traverse<T, T1>(this List<T> @this, Func<T, Task<T1>> f)
            =>
             @this.MatchWith(pattern: (
                 Empty: () => Task<List<T1>>.FromResult(new List<T1>()),
                 Cons: (v, r) => System.Threading.Tasks.Task
                .FromResult<Func<T1, Func<List<T1>, List<T1>>>>(x => y => x.AsList().Concat(y))
             .Ap(f(v))
             .Ap(r.Traverse(f))
           ));

        public static T1 MatchWith<T, T1>(this List<T> @this, (Func<T1> Empty, Func<T, List<T>, T1> Cons) pattern) =>
                (@this.Count == 0) ?
                pattern.Empty() :
                pattern.Cons(@this[0], @this.GetRange(1, @this.Count - 1));
          
        public static Task<T1> Map<T, T1>(this Task<T> @this, Func<T, T1> f)
           => @this.ContinueWith(task => f(task.Result));

        public static Task<T1> Ap<T, T1>(this Task<Func<T, T1>> fab, Task<T> fa)
        {
            var f = fab.GetAwaiter().GetResult();
            return Task<T1>.Run(() => f(fa.GetAwaiter().GetResult()));
        }
    }

 
    public class Demo
    {
        public static async System.Threading.Tasks.Task Run()
        {  
            Func<int, int, Task<int>> delay = (result, time) => Task<int>.Run(() =>
            {
                Console.WriteLine($"{result} started ");
                Thread.Sleep(time);
                Console.WriteLine($"{result} finished {time}");
                return result;
            });

            var t = await new List<Task<int>> { delay(20, 2000), delay(10, 3000) }.Distribute();


            var urlResults = await new List<Task<int>> { delay(20, 2000), delay(10, 3000) }.Distribute();

        }

    }



}