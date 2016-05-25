using System.Collections.Generic;
using IronScheme;

namespace FEvaluator.Scheme
{
    public class SchemeHandler : ISchemeHandler
    {
        private static List<string> SCRIPTS = new List<string>()
        {
            "canvas.ss"
        };

        public SchemeHandler()
        {
            "(+)".Eval(); // Bootstrap loading of underlying evaluator to speed up first evaluation.
            ScriptResolver.Load(this, SCRIPTS);
        }
        public object Eval(string expr, params object[] args)
        {
                return expr.Eval();
        }

        public object Eval(string expr, string importspec, params object[] args)
        {
            return expr.Eval();
        }

        public string EvalToString(string expr)
        {
            return expr.Eval().ToString();
        }

        public string EvalToString(string expr, string importspec)
        {
            return expr.Eval().ToString();
        }
    }
    
}