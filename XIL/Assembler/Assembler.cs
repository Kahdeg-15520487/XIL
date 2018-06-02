using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XIL.LangDef;

namespace XIL.Assembler
{
    public class Assembler
    {
        public struct CompileResult
        {
            public bool Success;
            public string Message;
            public CodeGenerator CodeGenerator;
            public CompileResult(bool success, string msg,CodeGenerator codegen)
            {
                Success = success;
                Message = msg;
                CodeGenerator = codegen;
            }
        }
        IInstructionImplementation[] instructionImplementations;
        public Assembler(params IInstructionImplementation[] instructionImplementations)
        {
            this.instructionImplementations = instructionImplementations;
        }

        public CompileResult Compile(string sourcecode)
        {
            Lexer lexer = new Lexer(sourcecode);
            CodeGenerator codegen = new CodeGenerator();
            Parser parser = new Parser(lexer, codegen, instructionImplementations);
            try
            {
                parser.Parse();
            }
            catch (Exception e)
            {
                return new CompileResult(false, e.Message, codegen);
            }

            return new CompileResult(true, "Success.", codegen);
        }
    }
}
