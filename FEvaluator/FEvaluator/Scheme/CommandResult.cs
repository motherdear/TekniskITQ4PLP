using System;
using System.Collections.Generic;

namespace FEvaluator.Scheme
{
    public class CommandResult
    {
        public String Color { get; set; }
        public List<Pixel> Pixels { get; set; }

        public CommandResult()
        {
            Pixels = new List<Pixel>();
        }
    }
}
