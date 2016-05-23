using System;
using FEvaluator.Scheme;

namespace FEvaluator
{
    using System.Windows;

    public partial class MainWindow : Window
    {
        private CommandWrapper _command;
        
        public MainWindow()
        {
            InitializeComponent();
            _command = new CommandWrapper();
        }

        private void Evaluate_Click(object sender, RoutedEventArgs e)
        {
            
            if (Input.Text.Length != 0)
            {
                DisplayArea.Text = _command.exec(Input.Text);
            }
        }
    }
}
