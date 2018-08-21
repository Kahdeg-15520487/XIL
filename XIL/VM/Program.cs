using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using XIL.LangDef;

namespace XIL.VM
{
    /// <summary>
    /// An in-memory program
    /// </summary>
    public class Program
    {
        /// <summary>
        /// bytecodes
        /// </summary>
        public int[] Bytecode;
        /// <summary>
        /// string constant table
        /// </summary>
        public string[] StringTable;

        /// <summary>
        /// Initialize a new program
        /// </summary>
        /// <param name="instrs"></param>
        /// <param name="strs"></param>
        public Program(int[] instrs, string[] strs)
        {
            Bytecode = new int[instrs.Length];
            Array.Copy(instrs, Bytecode, instrs.Length);
            StringTable = new string[strs.Length];
            Array.Copy(strs, StringTable, strs.Length);
        }

        /// <summary>
        /// serialize a program into a stream for saving
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="program"></param>
        public static void Serialize(Stream stream,Program program)
        {
            using (var bw = new BinaryWriter(stream))
            {
                bw.Write(program.Bytecode.Length);
                foreach (var b in program.Bytecode)
                {
                    bw.Write(b);
                }
                bw.Write(program.StringTable.Length);
                foreach (var str in program.StringTable)
                {
                    bw.Write(str);
                }
            }
        }

        /// <summary>
        /// serialize a program from a stream
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="instructions"></param>
        /// <param name="stringTable"></param>
        public static void Deserialize(Stream stream,out Instruction[] instructions,out string[] stringTable)
        {
            using (var br = new BinaryReader(stream))
            {
                int progLength = br.ReadInt32();
                int[] bytecode = new int[progLength];
                for (int i = 0; i < progLength; i++)
                {
                    bytecode[i] = br.ReadInt32();
                }
                instructions = Instruction.Deserialize(bytecode).ToArray();

                int strTableLength = br.ReadInt32();
                stringTable = new string[strTableLength];
                for (int i = 0; i < strTableLength; i++)
                {
                    stringTable[i] = br.ReadString();
                }
            }
        }
    }
}
