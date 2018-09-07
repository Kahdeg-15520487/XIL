using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XIL.LangDef;

namespace XIL.Assembler
{
    /// <summary>
    /// codegen
    /// </summary>
    public class CodeGenerator : ICodeGenerator
    {
        private Dictionary<string, int> labels;
        private Dictionary<string, int> libs;
        private List<Instruction> program;
        private List<string> stringTable;

        /// <summary>
        /// constructor
        /// </summary>
        public CodeGenerator()
        {
            labels = new Dictionary<string, int>();
            libs = new Dictionary<string, int>();
            program = new List<Instruction>();
            stringTable = new List<string>();
        }

        /// <summary>
        /// add an instruction
        /// </summary>
        /// <param name="instruction"></param>
        public void AddInstruction(Instruction instruction)
        {
            program.Add(instruction);
        }
        /// <summary>
        /// add an instruction
        /// </summary>
        /// <param name="opcode">opcode</param>
        /// <param name="op1">operand 1</param>
        /// <param name="op2">operand 2</param>
        /// <param name="lnb">line number for debug purpose, -1 for ignore</param>
        public void AddInstruction(int opcode, int op1 = 0, int op2 = 0, int lnb = 0)
        {
            program.Add(new Instruction(opcode, op1, op2, lnb));
        }

        /// <summary>
        /// get a jump label's target
        /// </summary>
        public int GetJumpLabel(string label)
        {
            return labels[label];
        }

        /// <summary>
        /// add a jump label
        /// </summary>
        public void AddJumpLabel(string label, int linecount)
        {
            if (!labels.ContainsKey(label))
            {
                labels.Add(label, linecount);
            }
        }

        /// <summary>
        /// retrieve a string constant <para/>
        /// return -1 if string constant is not exist
        /// </summary>
        public int GetString(string str)
        {
            return stringTable.FindIndex(s => s.Equals(str));
        }

        /// <summary>
        /// add a string constant <para/>
        /// return the index of the added string constant
        /// </summary>
        public int AddString(string str)
        {
            int index = GetString(str);
            if (index == -1)
            {
                stringTable.Add(str);
                index = stringTable.Count - 1;
            }
            return index;
        }

        /// <summary>
        /// emit a program
        /// </summary>
        /// <returns></returns>
        public VM.Program Emit()
        {
            //serialize program's bytecode
            int[] instrs = Instruction.Serialize(program);
            string[] strs = this.stringTable.ToArray();
            //serialize program's stringtable
            return new VM.Program(instrs, strs);
        }

        /// <summary>
        /// add a library metadata
        /// </summary>
        /// <param name="lib"></param>
        public int AddLibrary(string lib)
        {
            if (!libs.ContainsKey(lib))
            {
                libs.Add(lib, labels.Count);
            }
            return libs.Count - 1;
        }

        /// <summary>
        /// get a library index
        /// </summary>
        /// <param name="lib"></param>
        /// <returns></returns>
        public int GetLibrary(string lib)
        {
            return libs[lib];
        }
    }
}
