using FPCSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Monads
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            await Traversables.Task.Example.Demo.Run();
            Console.ReadKey();
        }
    }
}
