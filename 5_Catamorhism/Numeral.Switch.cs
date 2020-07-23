using System;
using System.Diagnostics;

namespace Catamorphisms.Numeral.Switch
{
    public static partial class FunctionalExt
    {
        public static string Show(this Numeral @this) =>
                @this switch
                {
                    Zero { } => $"Zero( )",
                    Succ { Rest: var rest } => $"Succ({rest.Show()})",
                };


        public static T1 Cata<T1>(this Numeral @this, (Func<T1> Zero, Func<T1, T1> Succ) algebra) =>
                @this switch
                {
                    Zero { } => algebra.Zero(),
                    Succ { Rest: var rest } => algebra.Succ(rest.Cata<T1>(algebra))
                };
    }
    public abstract class Numeral { }
    public class Zero : Numeral { }
    public class Succ : Numeral
    {
        public Numeral Rest { get; set; }
        public Succ(Numeral rest) => this.Rest = rest;
    }


    public class Demo
    {
        public static void Run()
        {
            //Succ(Succ(Succ(Succ(Zero( )))))
            var fourNumeral = new Succ(rest: new Succ(rest: new Succ(rest: new Succ(rest: new Zero()))));

            (Func<int> Zero, Func<int, int> Succ) algebraInt = (
                Zero: () => 0,
                Succ: (n) => n + 1
            );

            (Func<string> Zero, Func<string, string> Succ) algebraString = (
                 Zero: () => $"Zero( )",
                 Succ: (Rest) => $"Add one -({Rest })"
            );

            (Func<string> Zero, Func<string, string> Succ) algebraQuestionMarks = (
                          Zero: () => $"",
                          Succ: (Rest) => $"!{Rest }"
                     );

            Func<int, string> f = null;
            f = n => n == 0 ? "" : $"!{ f(n - 1)}";

            (Func<string> E, Func<string, string> F) algebra = (
                     E: () => $"",
                     F: (Rest) => $"!{Rest }"
                );

            Func<int, string> recursion = null;
            recursion = n => n == 0 ? algebra.E() : $"!{ algebra.F(recursion(n - 1))}";

            var four = fourNumeral.Cata<int>(algebraInt);

            Console.WriteLine(four);
        }

    }

}
