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
            return TestCircle(Size.X / 2) && TestLine(Size.X, Size.Y) && TestRectangle(Size.X, Size.Y) && TestFill(Size.X, Size.Y);
        }
        private bool TestCircle(int Radius)
        {
            // Randomize radius of the test circle, bounded by radius
            var Randomizer = new Random();
            int CircleRadius = Randomizer.Next(1, Radius);
            string CommandString = string.Format("(DRAW \"Red\" (CIRCLE '({0} {0}) {1}))", 2 * CircleRadius, CircleRadius);
            // Construct test circle
            CommandResult Result = _command.run(CommandString);
            var CenterPoint = new Pixel(2 * CircleRadius, 2 * CircleRadius);
            bool bIsRadiusCheckAlwaysTrue = true;
            var CleanedPixels = RemoveDuplicatePixels(Result.Pixels); 
            var UnprocessedPixels = new List<Pixel>(CleanedPixels);
            foreach (var CurrentPixel in Result.Pixels)
            {
                // get the distance between the pixels
                double Distance = GetDistance(CenterPoint, CurrentPixel);
                // Check if the pixel is the correct distance away from the center, no time to do a proper float conversion, so we just check +-1 too
                bool bIsOnRadius = Convert.ToInt32(Distance) == CircleRadius || Convert.ToInt32(Distance) == CircleRadius + 1 || Convert.ToInt32(Distance) == CircleRadius - 1;
                if (bIsRadiusCheckAlwaysTrue && !bIsOnRadius)
                {
                    bIsRadiusCheckAlwaysTrue = false;
                }
                //Console.WriteLine("X:" +item.X.ToString() + " Y:" +item.Y.ToString() + "Test Result: " + bIsOnRadius.ToString());
            }
            bool bAllPixelsHaveNeighbors = true;
            foreach (var CurrentPixel in CleanedPixels)
            {
                //UnprocessedPixels.RemoveAll(x => x == CurrentPixel);
                if (UnprocessedPixels.Count != 0)
                {
                    bAllPixelsHaveNeighbors = PixelHasNeighbor(CurrentPixel, UnprocessedPixels);
                }
                else // If the list is empty then we are at the last pixel (which does not have a neighbor by design 
                {
                    bAllPixelsHaveNeighbors = true;
                }
                // if a pixel did not have a neighbor then fail early
                if (!bAllPixelsHaveNeighbors)
                {
                    break;
                }
            }

            // Print the result
            Console.WriteLine("Circle Radius Test Was Successful: " + bIsRadiusCheckAlwaysTrue.ToString());
            Console.WriteLine("Circle Neighbor Test Was Successful: " + bAllPixelsHaveNeighbors.ToString());

            return bIsRadiusCheckAlwaysTrue;
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
            // Clean the output of duplicates
            var CleanedPixels = RemoveDuplicatePixels(Result.Pixels);
            var UnprocessedPixels = new List<Pixel>(CleanedPixels);
            bool bAllPixelsHaveNeighbors= true;
            foreach (var CurrentPixel in Result.Pixels)
            {
                // Remove the current pixel from the test set
                UnprocessedPixels.RemoveAll(x => x==CurrentPixel);
                if (UnprocessedPixels.Count != 0)
                {
                    bAllPixelsHaveNeighbors = PixelHasNeighbor(CurrentPixel, UnprocessedPixels);
                }
                else // If the list is empty then we are at the last pixel (which does not have a neighbor by design 
                {
                    bAllPixelsHaveNeighbors = true;
                }
                // if a pixel did not have a neighbor then fail early
                if (!bAllPixelsHaveNeighbors) { break; }
            }
            Console.WriteLine("Line Neighbor Test Was Successful: " + bAllPixelsHaveNeighbors.ToString());
            return bAllPixelsHaveNeighbors;
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
            string CommandString = string.Format("(DRAW \"Red\"(RECTANGLE '({0} {1}) '({2} {3})))", StartX, StartY, EndX, EndY);
            CommandResult Result = _command.run(CommandString);
            // Test the pixels for neighbors
            var ResultPixels = new List<Pixel>(Result.Pixels);
            var UnprocessedPixels = new List<Pixel>(ResultPixels);
            var CleanedPixels = RemoveDuplicatePixels(Result.Pixels);

            // Test if all pixels in the rectangle have valid neighbors            
            bool bAllPixelsHaveNeighbors = AllPixelsHaveNeighbors(UnprocessedPixels, ResultPixels);
            bool bAllPixelsAreOnRectangleEdge = false;

            // Test the location of all the pixels by removing pixels if they are on a line
            Pixel UpperLeft = new Pixel(StartX, StartY);
            Pixel LowerLeft = new Pixel(StartX, EndY);
            Pixel UpperRight = new Pixel(EndX, StartY);
            Pixel LowerRight = new Pixel(EndX, EndY);
            
            // Test the upper line
            CommandString = string.Format("(DRAW \"Red\" (LINE '({0} {1}) '({2} {3})))", UpperLeft.X, UpperLeft.Y, UpperRight.X, UpperRight.Y);
            CommandResult UpperResult = _command.run(CommandString);
            // Test the lower line
            CommandString = string.Format("(DRAW \"Red\" (LINE '({0} {1}) '({2} {3})))", LowerLeft.X, LowerLeft.Y, LowerRight.X, LowerRight.Y);
            CommandResult LowerResult = _command.run(CommandString);
            // Test the left line
            CommandString = string.Format("(DRAW \"Red\" (LINE '({0} {1}) '({2} {3})))", UpperLeft.X, UpperLeft.Y, LowerLeft.X, LowerLeft.Y);
            CommandResult LeftResult = _command.run(CommandString);
            // Test the right line
            CommandString = string.Format("(DRAW \"Red\" (LINE '({0} {1}) '({2} {3})))", UpperRight.X, UpperRight.Y, LowerRight.X,LowerRight.Y);
            CommandResult RightResult = _command.run(CommandString);

            List<Pixel>TestPixels = new List<Pixel>();
            TestPixels.AddRange(UpperResult.Pixels);
            TestPixels.AddRange(LowerResult.Pixels);
            TestPixels.AddRange(LeftResult.Pixels);
            TestPixels.AddRange(RightResult.Pixels);
            foreach (var CurrentPixel in TestPixels)
            {
                CleanedPixels.RemoveAll(x => x.X == CurrentPixel.X && x.Y == CurrentPixel.Y);
            }
            if(CleanedPixels.Count == 0)
            {
                bAllPixelsAreOnRectangleEdge = true;
            }
            Console.WriteLine("Rectangle Neighbor Test Was Successful: " + bAllPixelsHaveNeighbors.ToString());
            Console.WriteLine("Rectangle Neighbor Test Was Successful: " + bAllPixelsAreOnRectangleEdge.ToString());
            return bAllPixelsHaveNeighbors && bAllPixelsAreOnRectangleEdge;
        }
        private bool TestFill(int MaxX, int MaxY)
        {
            // Create randomized objects
            var Randomizer = new Random();
            // Try the fill command on all created objects and test the implementation
            return false;
        }

        #region Helper Functions
        private double GetDistance (Pixel Start, Pixel End)
        {
            return Math.Sqrt(Convert.ToDouble(Math.Pow(End.X - Start.X, 2)) + Convert.ToDouble(Math.Pow(End.Y - Start.Y, 2)));
        }

        private bool PixelHasNeighbor(Pixel TestPixel,List<Pixel> TestList)
        {
            // Init values
            bool bNeighborFound = false;
            int Distance = 0;
            // Check all pixels in the list if they are neighbors
            for (int i = 0; i < TestList.Count; i++)
            {
                Pixel CurrentPixel = TestList[i];
                if (CurrentPixel == TestPixel || i == TestList.Count) { bNeighborFound = true; }
                // Check if the pixel was 1 pixel away
                Distance = Convert.ToInt32(GetDistance(TestPixel, CurrentPixel));
                if (Distance == 1)
                {
                    bNeighborFound = true;
                }
            }
            // If no neighbor was found then return false;
            if (!bNeighborFound)
            {
                Console.WriteLine(Distance.ToString());
                return false;
            }
            return true;
        }

        private bool AllPixelsHaveNeighbors(List<Pixel> UnprocessedPixels, List<Pixel> ResultPixels)
        {
            bool bAllPixelsHaveNeighbors = true;

            foreach (var CurrentPixel in ResultPixels)
            {
                // Remove the current pixel from the test set
                UnprocessedPixels.RemoveAll(x => x == CurrentPixel);
                if (UnprocessedPixels.Count != 0)
                {
                    bAllPixelsHaveNeighbors = PixelHasNeighbor(CurrentPixel, UnprocessedPixels);
                }
                else // If the list is empty then we are at the last pixel (which does not have a neighbor by design 
                {
                    bAllPixelsHaveNeighbors = true;
                }
                // if a pixel did not have a neighbor then fail early
                if (!bAllPixelsHaveNeighbors) { break; }
            }
            return bAllPixelsHaveNeighbors;
        }
        private List<Pixel> RemoveDuplicatePixels(List<Pixel> UncleanPixels)
        {
            List<Pixel> CleanedPixels = new List<Pixel>(UncleanPixels);

            foreach (var CurrentPixel in UncleanPixels)
            {
                CleanedPixels.RemoveAll(x => x.X == CurrentPixel.X && x.Y == CurrentPixel.Y);
                CleanedPixels.Add(CurrentPixel);
            }
            return CleanedPixels;
        }
        #endregion
    }
}
