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
        public static void Main(string[] args)
        {
            Functors.Tree.Switch.Demo.Run();
            Console.ReadKey();
        }
    }
}
