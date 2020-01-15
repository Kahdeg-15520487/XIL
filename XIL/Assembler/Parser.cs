using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using XIL.LangDef;
using System.Reflection;

namespace XIL.Assembler
{
    internal class Parser
    {
        internal Lexer Lexer { get; private set; }
        internal VariableScope GlobalScope { get; private set; }
        internal ICodeGenerator CodeGen { get; private set; }
        internal Token CurrentToken { get; private set; }
        internal int InstructionCounter { get; private set; }

        private VariableScope currentScope;
        private static Dictionary<string, int> instructionMap = null;
        private static Dictionary<string, InstructionAttribute> instructionMetaDataMap;

        internal Parser(Lexer lexer, ICodeGenerator codegen, params IInstructionImplementation[] instructionImplementations)
        {
            if (instructionMap == null)
            {
                this.InitInstructionMap(instructionImplementations);
            }

            this.Lexer = lexer;
            this.GlobalScope = new VariableScope();
            this.currentScope = this.GlobalScope;
            this.CodeGen = codegen;
            this.CurrentToken = this.Lexer.GetNextToken();
            this.InstructionCounter = 0;

            this.linePrettyPrint = new StringBuilder();
            this.linePrettyPrint.AppendFormat("line {0}: ", (this.Lexer.current_line + 1).ToString().PadLeft(5));
        }

        private void InitInstructionMap(params IInstructionImplementation[] instructionImplementations)
        {
            instructionMap = new Dictionary<string, int>();
            instructionMetaDataMap = new Dictionary<string, InstructionAttribute>();
            foreach (IInstructionImplementation instrImplm in instructionImplementations)
            {
                IEnumerable<MethodInfo> methods = instrImplm.GetType().GetTypeInfo().GetMethods()
                  .Where(m => m.GetCustomAttributes(typeof(InstructionAttribute), false).Count() > 0);

                foreach (MethodInfo method in methods)
                {
                    //get instruction info
                    InstructionAttribute attr = method.GetCustomAttribute<InstructionAttribute>();
                    //map said delegate to instruction
                    instructionMap.Add(attr.OpName, attr.OpCode);
                    instructionMetaDataMap.Add(attr.OpName, attr);
                }
            }

            foreach (KeyValuePair<string, int> mappedInstruction in instructionMap)
            {
                InstructionAttribute instrMetaData = instructionMetaDataMap[mappedInstruction.Key];
                Console.WriteLine("loaded \"{0}\" from {1} lib", instrMetaData, instrMetaData.Library);
            }
        }

        void Error(TokenType expecting)
        {
            throw new Exception(string.Format("Invalid syntax: unexpected {0} at line {1}, expecting {2}", this.CurrentToken, this.Lexer.current_line + 1, expecting));
        }

        StringBuilder linePrettyPrint;
        private bool IsPrettyPrint = false;

        void Eat(TokenType token_type)
        {
            if (token_type == TokenType.ANY || this.CurrentToken.tokenType == token_type)
            {
                if (this.IsPrettyPrint)
                {
                    if (token_type == TokenType.NEWLINE)
                    {
                        Console.WriteLine(this.linePrettyPrint.ToString());
                        this.linePrettyPrint.Clear();
                        this.linePrettyPrint.AppendFormat("line {0}: ", (this.Lexer.current_line + 1).ToString().PadLeft(5));
                    }
                    else
                    {
                        this.linePrettyPrint.Append(this.CurrentToken + " ");
                    }
                }

                this.CurrentToken = this.Lexer.GetNextToken();
            }
            else
            {
                this.Error(token_type);
            }
        }

        /// <summary>
        /// label = IDENT COLON NEWLINE
        /// </summary>
        string label()
        {
            StringBuilder output = new StringBuilder();

            Token token = this.CurrentToken;
            this.Eat(TokenType.LABEL);
            string label = token.lexeme.Remove(token.lexeme.IndexOf(':'));
            this.CodeGen.AddJumpLabel(label, this.InstructionCounter);
            output.Append(token.lexeme);

            this.Eat(TokenType.NEWLINE);

            return output.ToString();
        }

        /// <summary>
        /// instruction = IDENT 
        /// </summary>
        string instruction()
        {
            StringBuilder output = new StringBuilder();
            int opcode = 0;
            int op1 = 0;
            int op2 = 0;

            Token token = this.CurrentToken;
            output.Append(token.lexeme);
            this.Eat(TokenType.IDENT);

            //todo throw syntax error on unknown opcode
            opcode = instructionMap[token.lexeme];

            if (this.CurrentToken.tokenType != TokenType.EOF)
            {
                //todo typecheck
                //output.Append(type_check(instr.opcode, instr));
            }

            //parse operands
            //parse op1
            if (this.CurrentToken.tokenType != TokenType.NEWLINE)
            {
                switch (this.CurrentToken.tokenType)
                {
                    case TokenType.INT:
                        op1 = int.Parse(this.CurrentToken.lexeme);
                        this.Eat(TokenType.INT);
                        break;
                    case TokenType.STRING:
                        op1 = this.CodeGen.AddString(this.CurrentToken.lexeme);
                        this.Eat(TokenType.STRING);
                        break;
                    case TokenType.BOOL:
                        op1 = bool.Parse(this.CurrentToken.lexeme) ? 1 : 0;
                        this.Eat(TokenType.BOOL);
                        break;
                    case TokenType.VAR:
                        op1 = this.currentScope[this.CurrentToken.lexeme];
                        this.Eat(TokenType.VAR);
                        break;
                    case TokenType.IDENT:
                        op1 = this.CodeGen.GetJumpLabel(this.CurrentToken.lexeme);
                        this.Eat(TokenType.IDENT);
                        break;
                }
                output.AppendFormat(" : {0}", op1);
            }
            //parse op2
            if (this.CurrentToken.tokenType != TokenType.NEWLINE)
            {
                switch (this.CurrentToken.tokenType)
                {
                    case TokenType.INT:
                        op2 = int.Parse(this.CurrentToken.lexeme);
                        this.Eat(TokenType.INT);
                        break;
                    case TokenType.STRING:
                        op2 = this.CodeGen.AddString(this.CurrentToken.lexeme);
                        this.Eat(TokenType.STRING);
                        break;
                    case TokenType.BOOL:
                        op2 = bool.Parse(this.CurrentToken.lexeme) ? 1 : 0;
                        this.Eat(TokenType.BOOL);
                        break;
                    case TokenType.VAR:
                        op2 = this.currentScope[this.CurrentToken.lexeme];
                        this.Eat(TokenType.VAR);
                        break;
                    case TokenType.IDENT:
                        op2 = this.CodeGen.GetJumpLabel(this.CurrentToken.lexeme);
                        this.Eat(TokenType.IDENT);
                        break;
                }
                output.AppendFormat(" {0}", op2);
            }

            this.Eat(TokenType.NEWLINE);

            this.CodeGen.AddInstruction(opcode, op1, op2, this.Lexer.current_line);
            return output.ToString();
        }

        string XIL()
        {
            StringBuilder output = new StringBuilder();

            while (this.CurrentToken.tokenType != TokenType.EOF)
            {
                switch (this.CurrentToken.tokenType)
                {
                    case TokenType.LABEL:
                        output.AppendFormat("{1} : {0}", this.label(), this.InstructionCounter);
                        output.AppendLine();
                        break;

                    case TokenType.VAR:
                        //todo parse variable declartion
                        break;

                    case TokenType.IDENT:
                        output.AppendFormat("{1} : {0}", this.instruction(), this.InstructionCounter);
                        output.AppendLine();
                        this.InstructionCounter++;
                        break;

                    case TokenType.NEWLINE:
                        this.Eat(TokenType.NEWLINE);
                        output.AppendLine();
                        break;
                }
            }

            return output.ToString();
        }

        public void ParseAllLabel()
        {
            while (this.CurrentToken.tokenType != TokenType.EOF)
            {
                switch (this.CurrentToken.tokenType)
                {
                    case TokenType.LABEL:
                        this.label();
                        break;

                    case TokenType.IDENT:
                        while (this.CurrentToken.tokenType != TokenType.NEWLINE)
                        {
                            this.Eat(TokenType.ANY);
                        }
                        this.Eat(TokenType.NEWLINE);
                        this.InstructionCounter++;
                        break;

                    case TokenType.NEWLINE:
                        this.Eat(TokenType.NEWLINE);
                        break;
                }
            }
        }

        public string Parse()
        {
            this.IsPrettyPrint = false;
            this.ParseAllLabel();
            this.Lexer.Reset();
            this.IsPrettyPrint = true;
            this.CurrentToken = this.Lexer.GetNextToken();
            this.InstructionCounter = 0;
            return this.XIL();
        }
    }
}
