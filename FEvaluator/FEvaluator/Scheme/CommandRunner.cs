﻿using System.Linq;
using IronScheme.Runtime;

namespace FEvaluator.Scheme
{
    public class CommandRunner
    {
        private ISchemeHandler _handler;

        public CommandRunner()
        {
            _handler = new SchemeHandler();
        }

        public CommandResult run(string command)
        {
            CommandResult result = new CommandResult();

            var _eval = (_handler.Eval($"(invoke 'draw {command})") as Cons);
            var color = (_eval.car as string);
            var pixels = _eval.cdr as Cons;

            result.Color = color;
            foreach (Cons pixel in pixels)
            {
                int[] _pixels = pixel.Cast<int>().ToArray();
                result.Pixels.Add(new Pixel(_pixels[0], _pixels[1]));
            }

            return result;
        }

    }
}
