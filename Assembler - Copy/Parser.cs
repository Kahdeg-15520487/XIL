using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using XIL.LangDef;
using System.Reflection;

namespace XIL.Assembler {
	public partial class Parser {
		public Lexer Lexer { get; private set; }
		public VariableScope GlobalScope { get; private set; }
		public CodeGenerator CodeGen { get; private set; }
		public Token CurrentToken { get; private set; }
		public int InstructionCounter { get; private set; }

		private VariableScope currentScope;
		private static Dictionary<string, int> instructionMap = null;

		public Parser(Lexer lexer, CodeGenerator codegen, params IInstructionImplementation[] instructionImplementations) {
			if (instructionMap == null) {
				InitInstructionMap(instructionImplementations);
			}

			Lexer = lexer;
			GlobalScope = new VariableScope();
			currentScope = GlobalScope;
			CodeGen = codegen;
			CurrentToken = Lexer.GetNextToken();
			InstructionCounter = 0;
		}

		private void InitInstructionMap(IInstructionImplementation[] instructionImplementations) {
			instructionMap = new Dictionary<string, int>();
			foreach (var instrImplm in instructionImplementations) {
				var methods = instrImplm.GetType().GetTypeInfo().GetMethods()
				  .Where(m => m.GetCustomAttributes(typeof(InstructionAttribute), false).Count() > 0);

				foreach (var method in methods) {
					//get instruction info
					var attr = method.GetCustomAttribute<InstructionAttribute>();
					//map said delegate to instruction
					instructionMap.Add(attr.OpName, attr.OpCode);
				}
			}
		}

		void Error(TokenType expecting) {
			throw new Exception(string.Format("Invalid syntax: unexpected {0} at line {1}, expecting {2}", CurrentToken, Lexer.current_line + 1, expecting));
		}

		void Eat(TokenType token_type) {
			if (CurrentToken.tokenType == token_type) {
				Console.WriteLine(CurrentToken);
				CurrentToken = Lexer.GetNextToken();
			}
			else {
				Error(token_type);
			}
		}

		/// <summary>
		/// label = IDENT COLON NEWLINE
		/// </summary>
		string label() {
			StringBuilder output = new StringBuilder();

			var token = CurrentToken;
			Eat(TokenType.LABEL);
			var label = token.lexeme.Remove(token.lexeme.IndexOf(':'));
			CodeGen.AddJumpLabel(label, InstructionCounter);
			output.Append(token.lexeme);

			Eat(TokenType.NEWLINE);

			return output.ToString();
		}

		string instruction() {
			StringBuilder output = new StringBuilder();
			int opcode = 0;
			int op1 = 0;
			int op2 = 0;

			var token = CurrentToken;
			output.Append(token.lexeme);
			Eat(TokenType.IDENT);

			opcode = instructionMap[token.lexeme];

			if (CurrentToken.tokenType != TokenType.EOF) {
				//todo typecheck
				//output.Append(type_check(instr.opcode, instr));
			}

			//do operands
			//parse op1
			if (CurrentToken.tokenType != TokenType.NEWLINE) {
				switch (CurrentToken.tokenType) {
					case TokenType.INT:
						op1 = int.Parse(CurrentToken.lexeme);
						Eat(TokenType.INT);
						break;
					case TokenType.STRING:
						//todo parse string constant
						Eat(TokenType.STRING);
						break;
					case TokenType.BOOL:
						op1 = bool.Parse(CurrentToken.lexeme) ? 1 : 0;
						Eat(TokenType.BOOL);
						break;
					case TokenType.VAR:
						op1 = currentScope[CurrentToken.lexeme];
						Eat(TokenType.VAR);
						break;
					case TokenType.IDENT:
						op1 = CodeGen.GetJumpTarget(CurrentToken.lexeme);
						Eat(TokenType.IDENT);
						break;
				}
				output.AppendFormat(" : {0}", op1);
			}
			//parse op2
			if (CurrentToken.tokenType != TokenType.NEWLINE) {
				switch (CurrentToken.tokenType) {
					case TokenType.INT:
						op2 = int.Parse(CurrentToken.lexeme);
						Eat(TokenType.INT);
						break;
					case TokenType.STRING:
						//todo parse string constant
						Eat(TokenType.STRING);
						break;
					case TokenType.BOOL:
						op2 = bool.Parse(CurrentToken.lexeme) ? 1 : 0;
						Eat(TokenType.BOOL);
						break;
					case TokenType.VAR:
						op2 = currentScope[CurrentToken.lexeme];
						Eat(TokenType.VAR);
						break;
					case TokenType.IDENT:
						op2 = CodeGen.GetJumpTarget(CurrentToken.lexeme);
						Eat(TokenType.IDENT);
						break;
				}
				output.AppendFormat(" {0}", op2);
			}

			Eat(TokenType.NEWLINE);

			CodeGen.AddInstruction(new Instruction(opcode, op1, op2));
			return output.ToString();
		}

		string XIL() {
			StringBuilder output = new StringBuilder();

			while (CurrentToken.tokenType != TokenType.EOF) {
				switch (CurrentToken.tokenType) {
					case TokenType.LABEL:
						output.AppendFormat("{1} : {0}", label(), InstructionCounter);
						output.AppendLine();
						break;

					case TokenType.IDENT:
						output.AppendFormat("{1} : {0}", instruction(), InstructionCounter);
						output.AppendLine();
						InstructionCounter++;
						break;

					case TokenType.NEWLINE:
						Eat(TokenType.NEWLINE);
						output.AppendLine();
						break;
				}
			}

			return output.ToString();
		}

		public string Parse() {
			return XIL();
		}
	}
}
