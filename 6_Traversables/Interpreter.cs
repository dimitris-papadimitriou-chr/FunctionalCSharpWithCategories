using System;


namespace Interpreter
{
    public abstract class Expr
    {
        public abstract void Eval(Context context);
    }
    public class Context
    {
        public string Log { get; set; }
    }
    public class ValExpr : Expr
    {
        public ValExpr(int x) { X = x; }
        public int X { get; }

        public override void Eval(Context context)
        {
            context.Log += $"{X}";
        }

    }
    public class AddExpr : Expr
    {
        public AddExpr(Expr x, Expr y)
        {
            X = x;
            Y = y;
        }

        public Expr X { get; }
        public Expr Y { get; }

        public override void Eval(Context context)
        {
            context.Log += $"Adding(";
            X.Eval(context);
            context.Log += $",";
            Y.Eval(context);
            context.Log += $")";
        }
    }
    public class Demo
    {
        public static void Run()
        {
            var expression = new AddExpr(new AddExpr(new ValExpr(2), new ValExpr(2)), new ValExpr(2));

            var logContext = new Context { Log = "" }; //"Adding(Adding(2,2),2)"
            expression.Eval(logContext);

        }
    }



}