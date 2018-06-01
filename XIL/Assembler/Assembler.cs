using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XIL.LangDef;

namespace XIL.Assembler {
	public class Assembler {
		public struct CompileResult {
			public bool Success;
			public string Message;
			public List<Instruction> CompiledProgram;
			public CompileResult(bool success, string msg, List<Instruction> prog) {
				Success = success;
				Message = msg;
				CompiledProgram = prog;
			}
		}
		IInstructionImplementation[] instructionImplementations;
		public Assembler(params IInstructionImplementation[] instructionImplementations) {
			this.instructionImplementations = instructionImplementations;
		}

		public CompileResult Compile(string sourcecode) {
			Lexer lexer = new Lexer(sourcecode);
			CodeGenerator codegen = new CodeGenerator();
			Parser parser = new Parser(lexer, codegen, instructionImplementations);
            try
            {
                parser.Parse();
            }
            catch (Exception e)
            {
                return new CompileResult(false, e.Message, null);
            }

            return new CompileResult(true, "Success.", codegen.program);
		}
	}
}
