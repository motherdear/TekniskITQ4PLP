using System;
using System.Collections.Generic;
using System.IO;

namespace FEvaluator.Scheme
{
    public class ScriptResolver
    {

        public static readonly string SCRIPTS_PATH = @"..\..\Scripts";

        public static void Load(ISchemeHandler handler, List<string> scripts)
        {
            foreach (string script in scripts)
            {
                string Path = GetScriptPath(script);

                if (System.IO.File.Exists(Path))
                {
                    string text = System.IO.File.ReadAllText(Path);
                    if (text.Length > 0) handler.Eval(text);
                }
            }
        }

        private static string GetScriptPath(string scriptName)
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            return Path.GetFullPath(Path.Combine(baseDirectory, SCRIPTS_PATH, scriptName));
        }
    }
}
