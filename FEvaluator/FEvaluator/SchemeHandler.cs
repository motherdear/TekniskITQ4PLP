using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheme
{
    using IronScheme; // the extension methods are exported from this namespace
    using IronScheme.Runtime;
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
            //var exp = "(car '((1 2 3) 4 5 6))";
            //var result = exp.Eval<Cons>(); // eval and cast => (1 2 3)
            //var list = result.Cast<int>().ToList();
            var result = expr.Eval<Cons>();
            //Console.WriteLine(result.PrettyPrint);
            Console.WriteLine(result.cdr.ToString());
            
            return result.ToPrettyString();
        }

        public string EvalToString(string expr, string importspec)
        {
            return expr.Eval().ToString();
        }
    }
    
}