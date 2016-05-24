using System;
using FEvaluator.Scheme;

namespace FEvaluator
{
    using System.Windows;

    public partial class MainWindow : Window
    {
        private CommandRunner _command;
        
        public MainWindow()
        {
            InitializeComponent();
            _command = new CommandRunner();
        }

        private void Evaluate_Click(object sender, RoutedEventArgs e)
        {
            
            if (Input.Text.Length != 0)
            {
                _command.run(Input.Text);
                DisplayArea.Text = "Command ran.";
            }
        }
    }
}
