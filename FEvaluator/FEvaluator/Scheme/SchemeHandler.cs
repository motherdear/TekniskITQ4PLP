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
            // Reset the environment, // HACK HACK, we really should be evalling with different interaction environments so that we could have seperate handlers
            "(interaction-environment (new-interaction-environment))".Eval();
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