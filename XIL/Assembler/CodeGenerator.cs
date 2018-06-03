using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XIL.LangDef;

namespace XIL.Assembler
{
    public class CodeGenerator
    {
        private Dictionary<string, int> labels;
        public List<Instruction> program;
        public List<string> stringTable;

        public CodeGenerator()
        {
            labels = new Dictionary<string, int>();
            program = new List<Instruction>();
            stringTable = new List<string>();
        }

        internal void AddInstruction(Instruction instruction)
        {
            program.Add(instruction);
        }
        internal void AddInstruction(int opcode, int op1 = 0, int op2 = 0, int lnb = 0)
        {
            program.Add(new Instruction(opcode, op1, op2, lnb));
        }

        /// <summary>
        /// get a jump label's target
        /// </summary>
        internal int GetJumpTarget(string label)
        {
            return labels[label];
        }

        /// <summary>
        /// add a jump label
        /// </summary>
        internal void AddJumpLabel(string label, int linecount)
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
        internal int GetString(string str)
        {
            return stringTable.FindIndex(s => s.Equals(str));
        }

        /// <summary>
        /// add a string constant <para/>
        /// return the index of the added string constant
        /// </summary>
        internal int AddString(string str)
        {
            int index = GetString(str);
            if (index == -1)
            {
                stringTable.Add(str);
                index = stringTable.Count - 1;
            }
            return index;
        }

        //todo make codegen
        public VM.Program Serialize()
        {
            //serialize program's bytecode
            int[] instrs = Instruction.Serialize(program);
            string[] strs = this.stringTable.ToArray();
            //serialize program's stringtable
            return new VM.Program(instrs, strs);
        }
    }
}
