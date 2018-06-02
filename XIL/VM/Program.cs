using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using XIL.LangDef;

namespace XIL.VM
{
    public class Program
    {
        public int[] Bytecode;
        public string[] StringTable;

        public Program(int[] instrs, string[] strs)
        {
            Bytecode = new int[instrs.Length];
            Array.Copy(instrs, Bytecode, instrs.Length);
            StringTable = new string[strs.Length];
            Array.Copy(strs, StringTable, strs.Length);
        }

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
