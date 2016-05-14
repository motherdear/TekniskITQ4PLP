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
using System.Drawing;
using Scheme;


namespace FEvaluator
{
    using System.Windows;

    public partial class Window1 : Window
    {
        private IIronScheme SchemeScript;
        static string[] SchemePaths = new string[] 
                                      { @"C:\Users\motherdear\Documents\PLP\TekniskITQ4PLP\FEvaluator\FEvaluator\SchemeRootCode.txt", 
                                        @"C:\Users\motherdear\Documents\PLP\TekniskITQ4PLP\FEvaluator\FEvaluator\SchemeRootCode2.txt" };
        public Window1()
        {
            InitializeComponent();
            // Instantiate scheme
            SchemeScript = new SchemeHandler();        
        }

        private void Evaluate_Click(object sender, RoutedEventArgs e)
        {
            LoadSchemeResources();
            if (Input.Text.Length != 0)
            {
                DisplayArea.Text = SchemeScript.EvalToString(Input.Text);
            }
        }
        /** Load all needed scheme resources*/
        virtual public void LoadSchemeResources() 
        {
            // Read all the needed scheme files
            foreach (string Path in SchemePaths)
            {
                if (!System.IO.File.Exists(Path))
                {
                    DisplayArea.Text = "\"" + Path + "\" Does not exist\n";
                    break;
                }
                // Do the reading
                string SchemeCode = System.IO.File.ReadAllText(Path);
                // only execute the code if it exists
                if (SchemeCode.Length != 0)
                {
                    SchemeScript.Eval(SchemeCode);
                }
            }
        }
        virtual public void GetPointsFromScheme(string SchemeCode) 
        {
            string SchemeResults = "";
            if (SchemeCode.Length != 0)
            {
                SchemeResults += SchemeScript.EvalToString(SchemeCode) + "\n";
            }
            //string TestString = "1,2;2,4;5,1";
            string InString = ParseSchemePointString(SchemeResults);
            List<Point> SchemePointList = ConvertStringToPixelArray(InString);

            var ResultString = ConvertPointArrayToString(SchemePointList);

            DisplayArea.Text = ResultString;
        }
        private string ParseSchemePointString(string InString) 
        {
            return InString.Trim(new char[] { '{', '}' });
        }

        private List<Point> ConvertStringToPixelArray(string InString) 
        {
            var ReturnList = new List<Point>();

            ReturnList = InString.Split(';').Select(s => s.Split(',')).Select(a => new Point(x: Int32.Parse(a[0]),y: Int32.Parse(a[1]))).ToList<Point>();
            return ReturnList;
        }
        private String ConvertPointArrayToString(List<Point> PointList) 
        {
            var Builder = new StringBuilder();
            for (int i = 0; i < PointList.Count; ++i)
            {
                var Coordinate = PointList[i];
                Builder.Append("{");
                Builder.Append(Coordinate.ToString());
                Builder.Append("}");
                if (i < PointList.Count - 1) { Builder.Append(","); }
            }
            return Builder.ToString();
        }
    }
}
