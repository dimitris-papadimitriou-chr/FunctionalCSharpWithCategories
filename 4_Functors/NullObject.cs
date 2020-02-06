using System;

namespace Functors.NullObject
{
    public abstract class AbstractObject<T>
    {
        public abstract void Operation();
    }
    public class NullObject<T> : AbstractObject<T>
    {
        public NullObject() { }

        public override void Operation()
        {
            //Do nothing here
        }
    }

    public class RealObject<T> : AbstractObject<T>
    {
        private readonly T value;
        public RealObject(T value) => this.value = value;//  injection

        public override void Operation()
        {
            // do something with
            Console.WriteLine($"{value}");
        }
    }
    public class Demo
    {
        public static void Run()
        {
            new RealObject<int>(5).Operation();
        }
    } 
}
