using System;
using System.Linq;
using System.Text;
using FEvaluator.Scheme;
using IronScheme;
using IronScheme.Runtime;

namespace FEvaluator.Scheme
{
    public class CommandWrapper
    {
        private ISchemeHandler _handler;

        public CommandWrapper()
        {
            _handler = new SchemeHandler();
        }

        public string exec(string command)
        {
            StringBuilder sb = new StringBuilder();
            var pixels = $"(invoke 'draw {command})".Eval<Cons>().cdr as Cons;

            foreach (Cons pixel in pixels)
            {
                int[] _pixels = pixel.Cast<int>().ToArray();
                sb.AppendLine($"x= {_pixels[0]} y= {_pixels[1]}");
            }

            return sb.ToString();
        }

    }
}
