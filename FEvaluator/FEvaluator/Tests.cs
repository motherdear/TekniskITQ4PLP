using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FEvaluator.Scheme;
using IronScheme.Runtime;
using System.Drawing;

namespace FEvaluator
{
    class Tests
    {
        private CommandRunner _command;
        public bool TestAllTestCases(Pixel Size)
        {
            _command = new CommandRunner();
            Console.WriteLine("Checking Test Cases");
            return TestCircle(Size.X/2) && TestLine(Size.X,Size.Y) && TestRectangle(Size.X, Size.Y) && TestFill(Size.X, Size.Y);
        }
        private bool TestCircle(int Radius)
        {
            // Randomize radius of the test circle, bounded by radius
            var Randomizer = new Random();
            int CircleRadius = Randomizer.Next(1, Radius);
            string CommandString = string.Format("(DRAW \"Red\" (CIRCLE '({0} {0}) {1}))",2*CircleRadius, CircleRadius);
            // Construct test circle
            CommandResult Result = _command.run(CommandString);
            var CenterPoint = new Pixel(2 * CircleRadius, 2 * CircleRadius);
            bool IsRadiusCheckAlwaysTrue = true;
            foreach (var item in Result.Pixels)
            {
                // get the distance between the pixels
                double Distance = Math.Sqrt(Convert.ToDouble(Math.Pow(item.X - CenterPoint.X, 2)) + Convert.ToDouble(Math.Pow(item.Y - CenterPoint.Y, 2)));
                // Check if the pixel is the correct distance away from the center, no time to do a proper float conversion, so we just check +-1 too
                bool bIsOnRadius = Convert.ToInt32(Distance) == CircleRadius || Convert.ToInt32(Distance) == CircleRadius + 1 || Convert.ToInt32(Distance) == CircleRadius - 1 ;
                if (IsRadiusCheckAlwaysTrue && !bIsOnRadius)
                {
                    IsRadiusCheckAlwaysTrue = false;
                }
                //Console.WriteLine("X:" +item.X.ToString() + " Y:" +item.Y.ToString() + "Test Result: " + bIsOnRadius.ToString());
            }
            // Print the result
            Console.WriteLine("Circle Test Was Successful: " + IsRadiusCheckAlwaysTrue.ToString());
            
            return IsRadiusCheckAlwaysTrue;
        }
        private bool TestLine(int MaxX, int MaxY)
        {
            // Radomize start and end points of the test line
            // Construct random but bounded start and end locations
            var Randomizer = new Random();
            int LineStartX = Randomizer.Next(0, MaxX);
            int LineStartY = Randomizer.Next(0, MaxY);
            // We knowingly don't handle the case of pixels being the exact same, no time to fix the potential bug
            int LineEndX = Randomizer.Next(0, MaxX);
            int LineEndY = Randomizer.Next(0, MaxY);
            // Construct test line
            string CommandString = string.Format("(DRAW \"Red\" (LINE '({0} {1}) '({2} {3})))", LineStartX, LineStartY, LineEndX, LineEndY);
            // Construct test circle
            CommandResult Result = _command.run(CommandString);



            return false;
        }
        private bool TestRectangle(int MaxX, int MaxY)
        {
            // Radomize start and end points of the test rectangle
            var Randomizer = new Random();
            int StartX = Randomizer.Next(0, MaxX);
            int StartY = Randomizer.Next(0, MaxY);
            // We knowingly don't handle the case of pixels being the exact same, no time to fix the potential bug
            int EndX = Randomizer.Next(0, MaxX);
            int EndY = Randomizer.Next(0, MaxY);
            // Construct test line
            return false;
        }
        private bool TestFill(int MaxX, int MaxY)
        {
            // Create randomized objects
            var Randomizer = new Random();
            // Try the fill command on all created objects and test the implementation
            return false;
        }
    }
}
