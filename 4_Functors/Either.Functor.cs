
using System;

namespace Functors.Either
{
    
    public interface IFunctor<T>
    {
        IFunctor<T1> Map<T1>(Func<T, T1> f);
    }
    public abstract class Either<TL, TR> : IFunctor<TR>
    {
        public IFunctor<T1> Map<T1>(Func<TR, T1> f) =>
        this.MatchWith<Either<TL, T1>>(pattern: (
            right: r => new Right<TL, T1>(f(r)),
            left: l => new Left<TL, T1>(l)
         ));
        public abstract T1 MatchWith<T1>((Func<TR, T1> right, Func<TL, T1> left) pattern);
        public abstract void MatchWith((Action<TR> right, Action<TL> left) pattern);
    }


    public class Left<TL, TR> : Either<TL, TR>
    {
        public TL Value { get; }
        public Left(TL value) => Value = value;
        public Either<TL, T1> Bind<T1>(Func<TR, Either<TL, T1>> f) => new Left<TL, T1>(Value);
        public override T1 MatchWith<T1>((Func<TR, T1> right, Func<TL, T1> left) pattern) => pattern.left(Value);
        public override void MatchWith((Action<TR> right, Action<TL> left) pattern) => pattern.left(Value);
    }

    public class Right<TL, TR> : Either<TL, TR>
    {
        public TR Value { get; }
        public Right(TR value) => Value = value;
        public Either<TL, T1> Bind<T1>(Func<TR, Either<TL, T1>> f) => f(Value);
        public override T1 MatchWith<T1>((Func<TR, T1> right, Func<TL, T1> left) pattern) => pattern.right(Value);
        public override void MatchWith((Action<TR> right, Action<TL> left) pattern) => pattern.right(Value);

    }

    class Demo
    { 
        public static void Run()
        {

        } 
    }  
}
