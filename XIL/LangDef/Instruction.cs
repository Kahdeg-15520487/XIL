using System;
using System.Collections.Generic;
using System.Text;

namespace XIL.LangDef
{
    public struct Instruction
    {
        public const int InstructionByteLength = 4;
        public static IEnumerable<Instruction> Deserialize(int[] program)
        {
            //todo define program's binary layout
            //todo define program's word's length
            int length = program.GetLength(0) / Instruction.InstructionByteLength;
            for (int i = 0; i < length; i++)
            {
                yield return new Instruction(program[i * Instruction.InstructionByteLength], program[i * Instruction.InstructionByteLength + 1], program[i * Instruction.InstructionByteLength + 2], program[i * Instruction.InstructionByteLength + 3]);
            }
        }
        public static int[] Serialize(List<Instruction> program)
        {
            int length = program.Count;
            int[] result = new int[length * Instruction.InstructionByteLength];

            Instruction instr;
            for (int i = 0; i < length; i++)
            {
                instr = program[i];
                result[i * Instruction.InstructionByteLength] = instr.opCode;
                result[i * Instruction.InstructionByteLength + 1] = instr.firstOperand;
                result[i * Instruction.InstructionByteLength + 2] = instr.secondOperand;
                result[i * Instruction.InstructionByteLength + 3] = instr.lineNumber;
            }

            return result;
        }

        public int opCode;
        public int firstOperand;
        public int secondOperand;
        public int lineNumber;
        public Instruction(InstructionOPCode oc, int op1 = 0, int op2 = 0, int lnb = 0)
        {
            opCode = (int)oc;
            firstOperand = op1;
            secondOperand = op2;
            lineNumber = lnb;
        }
        public Instruction(int oc, int op1 = 0, int op2 = 0, int lnb = 0)
        {
            opCode = oc;
            firstOperand = op1;
            secondOperand = op2;
            lineNumber = lnb;
        }

        /// <summary>
        /// nop
        /// </summary>
        public static Instruction Nop => new Instruction(InstructionOPCode.nop);
        public static Instruction Exit => new Instruction(InstructionOPCode.exit);

        public override string ToString()
        {
            return string.Format("{3}: 0x{0:X4} {1} {2}", opCode, firstOperand, secondOperand, lineNumber);
        }
    }
}
