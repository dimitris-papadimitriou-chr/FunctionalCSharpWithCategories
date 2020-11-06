using System;
using System.Collections.Generic;

namespace Traversables.Interpreter.Minimal
{


    public class Demo
    {
        public static void Run()
        {
            {
                Func<string, Func<Dictionary<string, int>, int>> Var = (variable) => (context) => context[variable];

                Func<Func<Dictionary<string, int>, int>,
                    Func<Dictionary<string, int>, int>,
                    Func<Dictionary<string, int>, int>>
                  Add = (l, r) => (context) => l(context) + r(context);

                Func<Func<Dictionary<string, int>, int>,
                    Func<Dictionary<string, int>, int>,
                    Func<Dictionary<string, int>, int>>
                  Sub = (l, r) => (context) => l(context) + r(context);

                Func<Dictionary<string, int>, int> expr = Add(Add(Var("a"), Var("b")), Var("b"));
            }

            {
                Func<int, Func<int, int>> Var = (variable) => (context) => variable;

                Func<Func<int, int>,
               Func<int, int>,
               Func<int, int>>
                   Add = (l, r) => (context) => l(context) + r(context);

                Func<Func<int, int>,
                Func<int, int>,
                Func<int, int>>
                    Sub = (l, r) => (context) => l(context) + r(context);

                Func<int, int> expr = Add(Add(Var(2), Var(3)), Var(4));
            }
        }
    }



}
