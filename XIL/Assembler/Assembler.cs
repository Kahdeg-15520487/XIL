using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XIL.LangDef;

namespace XIL.Assembler
{
    /// <summary>
    /// XIL Assembler
    /// </summary>
    public class Assembler
    {
        readonly IInstructionImplementation[] instructionImplementations;
        /// <summary>
        /// initialize the assembler with instruction implementations
        /// </summary>
        /// <param name="instructionImplementations"></param>
        public Assembler(params IInstructionImplementation[] instructionImplementations) {
            this.instructionImplementations = instructionImplementations;
        }

        /// <summary>
        /// compile a xil script
        /// </summary>
        /// <param name="sourcecode"></param>
        /// <returns></returns>
        public CompileResult Compile(string sourcecode) {
            Lexer lexer = new Lexer(sourcecode);
            CodeGenerator codegen = new CodeGenerator();
            Parser parser = new Parser(lexer, codegen, instructionImplementations);
            //try
            {
                parser.Parse();
            }
            //catch (Exception e)
            //{
            //    return new CompileResult(false, e.Message, codegen);
            //}

            return new CompileResult(true, "Success.", codegen);
        }

        /// <summary>
        /// compile a xil script
        /// </summary>
        /// <param name="sourcecode"></param>
        /// <param name="codegen"></param>
        /// <returns></returns>
        public CompileResult Compile(string sourcecode, ICodeGenerator codegen) {
            Lexer lexer = new Lexer(sourcecode);
            Parser parser = new Parser(lexer, codegen, instructionImplementations);
            try {
                parser.Parse();
            }
            catch (Exception e) {
                return new CompileResult(false, e.Message, codegen);
            }

            return new CompileResult(true, "Success.", codegen);
        }
    }
}
