using System;
using System.Collections.Generic;
using System.IO;
using XIL.Assembler.Preprocessor.AST;

namespace XIL.Assembler.Preprocessor
{
    public class Preprocessor
    {
        public bool IsSuccess = true;
        struct MacroDefinition
        {
            public int Start { get; set; }
            public int End { get; set; }
            public string Macro { get; set; }

            public MacroDefinition(int start, int end, string macro)
            {
                Start = start;
                End = end;
                Macro = macro;
            }

            public override string ToString()
            {
                return $"{Start}->{End} : {Macro}";
            }
        }
        public string Process(string source)
        {
            //search for #* macro *#
            List<MacroDefinition> macros = new List<MacroDefinition>();
            using (StringReader sr = new StringReader(source))
            {
                bool isInMacro = false;
                int pos = 0;
                int start = 0;
                int end = 0;
                while (sr.Peek() != -1)
                {
                    var c = (char)sr.Read();
                    pos++;

                    if (c == '#' && sr.Peek() == '*')
                    {
                        start = pos;
                        if (isInMacro)
                        {
                            //throw error
                        }
                        sr.Read();
                        pos++;
                        isInMacro = true;
                    }
                    else if (c == '*' && sr.Peek() == '#')
                    {
                        if (!isInMacro)
                        {
                            //throw error
                        }
                        sr.Read();
                        pos++;
                        end = pos;
                        isInMacro = false;

                        macros.Add(new MacroDefinition(start, end, source.Substring(start + 1, end - (start + 1) - 2).Trim()));
                    }
                }
            }

            //solve all macro and put it back
            string processedSource = source;
            Dictionary<string, int> variables = new Dictionary<string, int>();
            Interpreter interpreter = new Interpreter(variables);
            foreach (var macro in macros)
            {
                Lexer lexer = new Lexer(macro.Macro);
                Parser parser = new Parser(lexer);
                try
                {
                    var result = parser.Parse();
                    result.Accept(interpreter);
                    var output = interpreter.Output;
                    if (result is Assignment)
                    {

                        //ignore output
                        var asgm = result as Assignment;
                        Console.WriteLine("{0} -> {1} = {2}", macro.Macro, asgm.ident.token.lexeme, output);

                        //replace macro with empty space
                        //processedSource.Replace()
                        processedSource = processedSource.Replace(macro.Start - 1, macro.End - macro.Start + 1, "");
                    }
                    else
                    {
                        Console.WriteLine("{0} -> {1}", macro.Macro, output);
                        //replace macro with output
                        processedSource = processedSource.Replace(macro.Start - 1, macro.End - macro.Start + 1, output.ToString());
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    IsSuccess = false;
                }
            }

            return processedSource;
        }
    }
}