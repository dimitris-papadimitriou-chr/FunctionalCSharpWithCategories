using System;

namespace Functors.Id
{ 
    public class Id<T>
    {
        public T Value { get; set; }
        public Id(T value) => Value = value;
        public Id<T1> Map<T1>(Func<T, T1> f) => new Id<T1>(f(Value));
        public T1 Cata<T1>(Func<T, T1> callback) => callback(Value);
        public void Cata(Action<T> callback) => callback(Value);
    }

}