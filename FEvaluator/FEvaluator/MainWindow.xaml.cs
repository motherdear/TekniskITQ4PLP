using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FEvaluator
{
    using IronScheme; // the extension methods are exported from this namespace
    public class SchemeHandler
    {
        public SchemeHandler()
        {
            // Hacky Hack HACK
            // Evaluate a non defining statement in scheme to bootstrap the loading of the underlying evaluater, otherwise the first calculation will be slow.
            Evaluate("(+)");
        }
        public object Evaluate(string input)
        {
            return input.Eval(); // calls IronScheme.RuntimeExtensions.Eval(string)
        }
    }
}
namespace FEvaluator
{
    using System.Windows;

    public partial class Window1 : Window
    {
        private SchemeHandler schemeHandler;
        public Window1()
        {
            InitializeComponent();
            // Instantiate scheme
            schemeHandler = new SchemeHandler();        }

        private void Evaluate_Click(object sender, RoutedEventArgs e)
        {
            DisplayArea.Text = schemeHandler.Evaluate(Input.Text).ToString();
        }

        private void DefinitionTests() 
        {
            var r1 = schemeHandler.Evaluate("(+ 1 2)");  // r1 is 3 

            var r2 = schemeHandler.Evaluate("(+ {0} {1})");  // r2 is 3

            var myproc = schemeHandler.Evaluate("(lambda (x y) (+ x y))");
            var r3 = schemeHandler.Evaluate("({0} {1} {2})"); // r3 is 3

            schemeHandler.Evaluate("(define foo 1000)");  // executes definition in interaction environment
            var r4 = schemeHandler.Evaluate("foo"); // r4 is 1000
        }
    }
}
