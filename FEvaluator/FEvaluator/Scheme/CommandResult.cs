using System;
using System.Collections.Generic;

namespace FEvaluator.Scheme
{
    public class CommandResult
    {
        public String color { get; set; }
        public List<Pixel> pixels { get; set; }

        public CommandResult()
        {
            pixels = new List<Pixel>();
        }
    }
}
