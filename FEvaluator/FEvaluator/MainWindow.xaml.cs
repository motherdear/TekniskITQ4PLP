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
        private WriteableBitmap _bitmap;
        
        public MainWindow()
        {
            InitializeComponent();
            _command = new CommandRunner();
            _bitmap = BitmapFactory.New(512, 512);
            imgCanvas.Source = _bitmap;
            
        }

        private void handleTextAt(string command)
        {
            // Sigh, should have learned regex...

            string coordTokenOpen = "'(";
            string coordTokenClose = ")";

            int idxOpen = command.IndexOf(coordTokenOpen, StringComparison.OrdinalIgnoreCase);
            int idxClose = command.IndexOf(coordTokenClose, StringComparison.OrdinalIgnoreCase);

            string[] coords = (command.Substring(idxOpen + coordTokenOpen.Length, (idxClose - idxOpen) - coordTokenOpen.Length)).Split(' ');

            string textToken = "\"";

            idxOpen = command.IndexOf(textToken, StringComparison.OrdinalIgnoreCase);
            idxClose = command.IndexOf(textToken, idxOpen + 1, StringComparison.OrdinalIgnoreCase);

            string text = command.Substring(idxOpen + textToken.Length, (idxClose - idxOpen) - textToken.Length);
        }

        private void Evaluate_Click(object sender, RoutedEventArgs e)
        {
            using (_bitmap.GetBitmapContext())
            {
                if (Input.Text.Length != 0)
                {
                    string[] commands = Input.Text.Split(new string[] { Environment.NewLine}, StringSplitOptions.None);

                    foreach (string command in commands)
                    {
                        DisplayArea.Text += $"run {command} { Environment.NewLine }";

                        int idx = command.IndexOf("TEXT-AT", StringComparison.OrdinalIgnoreCase);
                        if (idx >= 0)
                        {
                            handleTextAt(command);        
                        }
                        else
                        {
                            CommandResult result = _command.run(command);

                            foreach (Pixel pixel in result.Pixels)
                            {
                                _bitmap.SetPixel(pixel.X, pixel.Y, (Color)ColorConverter.ConvertFromString(result.Color));
                            }
                        
                        }
                    }
                }
            }

        }
    }
}
