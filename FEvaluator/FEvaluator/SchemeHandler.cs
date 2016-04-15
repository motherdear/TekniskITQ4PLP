using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheme
{
    using IronScheme; // the extension methods are exported from this namespace
    public interface IIronScheme
    {
        string EvalToString(string expr);
        string EvalToString(string expr, string importspec);
        object Eval(string expr, params object[] args);
        object Eval(string expr, string importspec, params object[] args);
    }
    public class SchemeHandler : IIronScheme
    {
        public SchemeHandler()
        {
            // Hacky Hack HACK
            // Evaluate a non defining statement in scheme to bootstrap the loading of the underlying evaluater, otherwise the first calculation will be slow.
            Eval("(+)");
        }
        public object Eval(string expr, params object[] args)
        {
            return expr.Eval(); // calls IronScheme.RuntimeExtensions.Eval(string)
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
