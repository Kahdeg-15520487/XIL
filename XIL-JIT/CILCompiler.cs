using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using XIL.Assembler;
using XIL.LangDef;
using XIL.VM;

namespace XILtoCIL
{
    /// <summary>
    /// only support the CoreInstruction and ForeignFunctionInstruction
    /// </summary>
    public class CilCodeGenerator : ICodeGenerator
    {
        private List<(string label, int target)> labels;
        private List<Instruction> program;
        private List<string> stringTable;
        private Dictionary<string, int> libs;

        public CilCodeGenerator()
        {
            labels = new List<(string label, int target)>();
            program = new List<Instruction>();
            stringTable = new List<string>();
            libs = new Dictionary<string, int>();
        }

        public void AddInstruction(Instruction instruction)
        {
            program.Add(instruction);
        }

        public void AddInstruction(int op, int op1, int op2, int lnb)
        {
            program.Add(new Instruction(op, op1, op2, lnb));
        }

        public void AddJumpLabel(string label, int line)
        {
            if (GetJumpLabel(label) == -1)
            {
                labels.Add((label, line));
            }
        }

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

        public int GetJumpLabel(string label)
        {
            try
            {
                var jump = labels.First(j => j.label.Equals(label));
                return jump.target;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public int GetString(string str)
        {
            return stringTable.FindIndex(s => s.Equals(str));
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

        public Program Emit()
        {
            //serialize program's bytecode
            int[] instrs = Instruction.Serialize(program);
            //serialize program's stringtable
            string[] strs = this.stringTable.ToArray();

            return new Program(instrs, strs);
        }

        public Func<int> EmitDynamicMethod()
        {
            DynamicMethod method = new DynamicMethod("xil_cil", typeof(int), null);
            ILGenerator ilgen = method.GetILGenerator();

            var stringTable = new string[this.stringTable.Count];
            for (int i = 0; i < stringTable.Length; i++)
            {
                stringTable[i] = stringTable[i];
            }

            var jumpTable = new Dictionary<int, Label>();
            var lookUpJumpTable = new Dictionary<int, int>();
            for (int i = 0; i < this.labels.Count; i++)
            {
                jumpTable.Add(labels[i].target, ilgen.DefineLabel());
                lookUpJumpTable.Add(i, labels[i].target);
            }

            //gen cil instruction based on given instruction list, jump table and string constant
            for (int i = 0; i < program.Count; i++)
            {
                var instr = program[i];
                ilgen.EmitWriteLine(instr.ToString());

                if (jumpTable.ContainsKey(i))
                {
                    ilgen.MarkLabel(jumpTable[i]);
                    Console.WriteLine(i);
                }

                var a = ilgen.DeclareLocal(typeof(Int32));
                var b = ilgen.DeclareLocal(typeof(Int32));
                var c = ilgen.DeclareLocal(typeof(bool));

                switch ((InstructionOPCode)instr.OpCode)
                {
                    case InstructionOPCode.jmp:
                        {
                            var target = jumpTable[lookUpJumpTable[instr.FirstOperand]];
                            ilgen.Emit(OpCodes.Br_S, target);
                            //Console.WriteLine("jmp {0}", lookUpJumpTable[instr.param]);
                        }
                        break;
                    case InstructionOPCode.je:
                        {
                            ilgen.Emit(OpCodes.Stloc_0);
                            ilgen.Emit(OpCodes.Stloc_1);

                            ilgen.EmitWriteLine(a);
                            ilgen.EmitWriteLine(b);

                            ilgen.Emit(OpCodes.Ldloc_1);
                            ilgen.Emit(OpCodes.Ldloc_0);

                            var target = jumpTable[instr.FirstOperand];
                            ilgen.Emit(OpCodes.Ceq);

                            ilgen.Emit(OpCodes.Dup);
                            ilgen.Emit(OpCodes.Stloc_0);

                            ilgen.EmitWriteLine(c);
                            ilgen.Emit(OpCodes.Brtrue_S, target);
                        }
                        break;
                    case InstructionOPCode.j1:
                        {
                            var target = jumpTable[lookUpJumpTable[instr.FirstOperand]];
                            ilgen.Emit(OpCodes.Brtrue_S, target);
                            //Console.WriteLine("jmp {0}", lookUpJumpTable[instr.param]);
                        }
                        break;

                    case InstructionOPCode.yeet:
                        {
                            ilgen.Emit(OpCodes.Ldc_I4, instr.FirstOperand);
                        }
                        break;
                    case InstructionOPCode.dup:
                        {
                            ilgen.Emit(OpCodes.Dup);
                        }
                        break;
                    case InstructionOPCode.pop:
                        {
                            ilgen.Emit(OpCodes.Pop);
                        }
                        break;

                    case InstructionOPCode.add:
                        {
                            ilgen.Emit(OpCodes.Add);
                        }
                        break;

                    //case InstructionOPCode.ceq:
                    //    {
                    //        ilgen.Emit(OpCodes.Ceq);
                    //    }
                    //    break;
                    //case InstructionOPCode.cgt:
                    //    {
                    //        ilgen.Emit(OpCodes.Cgt);
                    //    }
                    //    break;
                    //case InstructionOPCode.clt:
                    //    {
                    //        ilgen.Emit(OpCodes.Clt);
                    //    }
                    //    break;

                    //case InstructionOPCode.callhost:
                    //    {
                    //        var hostFuncName = stringTable[instr.param];
                    //        var hostFunc = hostFunctions[hostFuncName];
                    //        ilgen.EmitCall(OpCodes.Call, hostFunc, null);
                    //    }
                    //    break;

                    case InstructionOPCode.exit:
                        ilgen.Emit(OpCodes.Ret);
                        break;
                }
            }

            return (Func<int>)method.CreateDelegate(typeof(Func<int>));
        }
    }
}
