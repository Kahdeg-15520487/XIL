using System;
using System.Collections.Generic;
using System.Text;

namespace XIL.LangDef
{
    /// <summary>
    /// instruction
    /// </summary>
    public struct Instruction
    {
        /// <summary>
        /// default byte scheme for an instruction <para/>
        /// opcode | op1 | op2 | line_number
        /// </summary>
        public const int InstructionByteLength = 4;
        /// <summary>
        /// Decode a bytecodes array to a instruction list
        /// </summary>
        /// <param name="program"></param>
        /// <returns></returns>
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
        /// <summary>
        /// serialize an instruction list to a byte code array
        /// </summary>
        /// <param name="program"></param>
        /// <returns></returns>
        public static int[] Serialize(List<Instruction> program)
        {
            int length = program.Count;
            int[] result = new int[length * Instruction.InstructionByteLength];

            Instruction instr;
            for (int i = 0; i < length; i++)
            {
                instr = program[i];
                result[i * Instruction.InstructionByteLength] = instr.OpCode;
                result[i * Instruction.InstructionByteLength + 1] = instr.FirstOperand;
                result[i * Instruction.InstructionByteLength + 2] = instr.SecondOperand;
                result[i * Instruction.InstructionByteLength + 3] = instr.LineNumber;
            }

            return result;
        }

        /// <summary>
        /// Operation Code
        /// </summary>
        public int OpCode { get; set; }
        /// <summary>
        /// operand 1
        /// </summary>
        public int FirstOperand { get; set; }
        /// <summary>
        /// operand 2
        /// </summary>
        public int SecondOperand { get; set; }
        /// <summary>
        /// line number for debug purpose
        /// </summary>
        public int LineNumber { get; set; }
        /// <summary>
        /// library metadata for debug purpose
        /// </summary>
        public int LibraryIndex { get; set; }
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="oc"></param>
        /// <param name="op1"></param>
        /// <param name="op2"></param>
        /// <param name="lnb"></param>
        public Instruction(InstructionOPCode oc, int op1 = 0, int op2 = 0, int lnb = -1)
        {
            OpCode = (int)oc;
            FirstOperand = op1;
            SecondOperand = op2;
            LineNumber = lnb;
            LibraryIndex = 0;
        }
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="oc"></param>
        /// <param name="op1"></param>
        /// <param name="op2"></param>
        /// <param name="lnb"></param>
        /// <param name="libindex"></param>
        public Instruction(int oc, int op1 = 0, int op2 = 0, int lnb = 0, int libindex = 0)
        {
            OpCode = oc;
            FirstOperand = op1;
            SecondOperand = op2;
            LineNumber = lnb;
            LibraryIndex = libindex;
        }

        /// <summary>
        /// nop
        /// </summary>
        public static Instruction Nop => new Instruction(InstructionOPCode.nop);
        /// <summary>
        /// exit
        /// </summary>
        public static Instruction Exit => new Instruction(InstructionOPCode.exit);

        public override string ToString()
        {
            return string.Format("{3}: 0x{0:X4} {1} {2}", OpCode, FirstOperand, SecondOperand, LineNumber);
        }
    }
}
