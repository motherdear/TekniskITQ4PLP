using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEvaluator.Scheme
{
    public class Pixel
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Pixel(int x, int y)
        {
            X = x;
            Y = y;
        }
    }   
}
