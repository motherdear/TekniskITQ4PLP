namespace FEvaluator.Scheme
{
    public interface ISchemeHandler
    {
        object Eval(string expr, params object[] args);
        object Eval(string expr, string importspec, params object[] args);
        string EvalToString(string expr);
        string EvalToString(string expr, string importspec);
    }
}