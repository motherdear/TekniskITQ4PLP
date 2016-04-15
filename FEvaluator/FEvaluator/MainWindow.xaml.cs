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
using Scheme;


namespace FEvaluator
{
    using System.Windows;
    public partial class Window1 : Window
    {
        private IIronScheme SchemeScript;
        public Window1()
        {
            InitializeComponent();
            // Instantiate scheme
            SchemeScript = new SchemeHandler();        
        }

        private void Evaluate_Click(object sender, RoutedEventArgs e)
        {
            DisplayArea.Text = SchemeScript.EvalToString(Input.Text);
        }

        private void DefinitionTests() 
        {
            var r1 = SchemeScript.Eval("(+ 1 2)");  // r1 is 3 

            var r2 = SchemeScript.Eval("(+ {0} {1})");  // r2 is 3

            var myproc = SchemeScript.Eval("(lambda (x y) (+ x y))");
            var r3 = SchemeScript.Eval("({0} {1} {2})"); // r3 is 3

            SchemeScript.Eval("(define foo 1000)");  // executes definition in interaction environment
            var r4 = SchemeScript.Eval("foo"); // r4 is 1000
        }
    }
}
