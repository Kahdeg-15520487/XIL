using System;
using System.Collections.Generic;
using System.Text;

namespace XIL.Assembler.Preprocessor
{
    static class ExtensionMethod
    {
        public static string Replace(this string s, int start, int length, string replace)
        {
            return s.Remove(start, length).Insert(start, replace.PadRight(length));
        }
    }
}
