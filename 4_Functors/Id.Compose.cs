using System;
using System.Threading.Tasks;

namespace Functors.Id.Compose
{
    public interface IFunctor<T>
    {
        IFunctor<T1> Map<T1>(Func<T, T1> f);
    }
    public static class FunctionalExtensions
    {
        public static Id<Task<T1>> MapT<T, T1>(this Id<Task<T>> @this, Func<T, T1> f)
            => @this.Map(t => t.Map(f));
    
        //this is not possible because C# does not support Higher Kinded Types
        //public static F<G<T1>> MapT<T, T1>(this F<G<T>> @this, Func<T, T1> f)
        //    where F:IFunctor<G>
        //    where G:IFunctor<T>
        //  => @this.Map(t => t.Map(f));


        public static Task<Id<T1>> MapT<T, T1>(this Task<Id<T>> @this, Func<T, T1> f)
           => @this.Map(t => t.Map(f));

        public static Task<T1> Map<T, T1>(this Task<T> task, Func<T, T1> mapping) =>
                   task.ContinueWith(t => mapping(t.Result));

    }
    public class Demo
    {
        public static void Run()
        {
            var functorComposition = new Id<Task<int>>(Task<int>.Run(() => 4)).Map(task => task.Map(x => x + 2));
        }
    }

    public class Id<T>
    {
        public T Value { get; set; }
        public Id(T value) => Value = value;
        public Id<T1> Map<T1>(Func<T, T1> f) => new Id<T1>(f(Value));

    }

}