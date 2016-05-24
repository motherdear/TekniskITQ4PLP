using System;
using FEvaluator.Scheme;

namespace FEvaluator
{
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

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
                string[] commands = Input.Text.Split(new string[] { Environment.NewLine}, StringSplitOptions.None);

                WriteableBitmap writeableBmp = BitmapFactory.New(512, 512);
                imgCanvas.Source = writeableBmp;

                foreach (string command in commands)
                {
                    CommandResult result = _command.run(command);
                    DisplayArea.Text += $"run {command} { Environment.NewLine }";

                    using (writeableBmp.GetBitmapContext())
                    {
                        foreach (Pixel pixel in result.pixels)
                        {
                            writeableBmp.SetPixel(pixel.X, pixel.Y, (Color) ColorConverter.ConvertFromString(result.color));
                        }
                    }
                }
            }
        }
    }
}
