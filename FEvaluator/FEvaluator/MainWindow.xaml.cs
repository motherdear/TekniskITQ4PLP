using System;
using FEvaluator.Scheme;

namespace FEvaluator
{
    using System.Windows;

    public partial class MainWindow : Window
    {
        private ISchemeHandler _handler;

        
        public MainWindow()
        {
            InitializeComponent();
            _handler = new SchemeHandler();
        }

        private void Evaluate_Click(object sender, RoutedEventArgs e)
        {
            
            if (Input.Text.Length != 0)
            {
                DisplayArea.Text = _handler.EvalToString(Input.Text);
            }
        }
        

        /** Get the actual path of each script */

    }
}
