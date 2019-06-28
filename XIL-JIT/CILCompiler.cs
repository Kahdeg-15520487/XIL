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
            this.labels = new List<(string label, int target)>();
            this.program = new List<Instruction>();
            this.stringTable = new List<string>();
            this.libs = new Dictionary<string, int>();
        }

        public void AddInstruction(Instruction instruction)
        {
            this.program.Add(instruction);
        }

        public void AddInstruction(int op, int op1, int op2, int lnb)
        {
            this.program.Add(new Instruction(op, op1, op2, lnb));
        }

        public void AddJumpLabel(string label, int line)
        {
            if (this.GetJumpLabel(label) == -1)
            {
                this.labels.Add((label, line));
            }
        }

        public int AddString(string str)
        {
            int index = this.GetString(str);
            if (index == -1)
            {
                this.stringTable.Add(str);
                index = this.stringTable.Count - 1;
            }
            return index;
        }

        public int GetJumpLabel(string label)
        {
            try
            {
                var jump = this.labels.First(j => j.label.Equals(label));
                return jump.target;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public int GetString(string str)
        {
            return this.stringTable.FindIndex(s => s.Equals(str));
        }

        /// <summary>
        /// add a library metadata
        /// </summary>
        /// <param name="lib"></param>
        public int AddLibrary(string lib)
        {
            if (!this.libs.ContainsKey(lib))
            {
                this.libs.Add(lib, this.labels.Count);
            }
            return this.libs.Count - 1;
        }

        /// <summary>
        /// get a library index
        /// </summary>
        /// <param name="lib"></param>
        /// <returns></returns>
        public int GetLibrary(string lib)
        {
            return this.libs[lib];
        }

        public Program Emit()
        {
            //serialize program's bytecode
            int[] instrs = Instruction.Serialize(this.program);
            //serialize program's stringtable
            string[] strs = this.stringTable.ToArray();

            return new Program(instrs, strs);
        }

        public Func<int> EmitDynamicMethod()
        {
            DynamicMethod method = new DynamicMethod("xil_cil", typeof(int), null);
            ILGenerator ilgen = method.GetILGenerator();
            StringBuilder asmSource = new StringBuilder();

            var stringTable = new string[this.stringTable.Count];
            for (int i = 0; i < stringTable.Length; i++)
            {
                stringTable[i] = stringTable[i];
            }

            var jumpTable = new Dictionary<int, Label>();
            var lookUpJumpTable = new Dictionary<int, int>();
            for (int i = 0; i < this.labels.Count; i++)
            {
                jumpTable.Add(this.labels[i].target, ilgen.DefineLabel());
                lookUpJumpTable.Add(i, this.labels[i].target);
            }

            //gen cil instruction based on given instruction list, jump table and string constant
            int instructionCount = this.program.Count;
            for (int i = 0; i < this.program.Count; i++)
            {
                var instr = this.program[i];
                ilgen.EmitWriteLine(instr.ToString());

                if (jumpTable.ContainsKey(i))
                {
                    ilgen.MarkLabel(jumpTable[i]);
                    Console.WriteLine(i);
                }

                //some local register for easier thing
                var a = ilgen.DeclareLocal(typeof(int));
                var b = ilgen.DeclareLocal(typeof(int));
                var c = ilgen.DeclareLocal(typeof(bool));

                switch ((InstructionOPCode)instr.OpCode)
                {
                    case InstructionOPCode.jmp:
                        {
                            //var target = jumpTable[lookUpJumpTable[instr.FirstOperand]];
                            var target = jumpTable[instr.FirstOperand];
                            ilgen.Emit(OpCodes.Br_S, target);
                            //asmSource.AppendFormat("{1}| jmp {0}", instr.FirstOperand, i.ToString().PadLeft();
                            //program.
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
                            //var target = jumpTable[lookUpJumpTable[instr.FirstOperand]];
                            var target = jumpTable[instr.FirstOperand];
                            ilgen.Emit(OpCodes.Brtrue_S, target);
                            asmSource.AppendFormat("j1 {0}", instr.FirstOperand);
                            //Console.WriteLine("jmp {0}", lookUpJumpTable[instr.param]);
                        }
                        break;

                    case InstructionOPCode.yeet:
                        {
                            ilgen.Emit(OpCodes.Ldc_I4, instr.FirstOperand);
                            asmSource.AppendFormat("push {0}", instr.FirstOperand);
                        }
                        break;
                    case InstructionOPCode.dup:
                        {
                            ilgen.Emit(OpCodes.Dup);
                            asmSource.Append("dup");
                        }
                        break;
                    case InstructionOPCode.pop:
                        {
                            ilgen.Emit(OpCodes.Pop);
                            asmSource.Append("pop");
                        }
                        break;

                    case InstructionOPCode.add:
                        {
                            ilgen.Emit(OpCodes.Add);
                            asmSource.Append("add");
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
                asmSource.AppendLine();
            }
            Console.WriteLine("===asm===");
            Console.WriteLine(asmSource.ToString());
            Console.WriteLine("===asm===");
            return (Func<int>)method.CreateDelegate(typeof(Func<int>));
        }
    }
}
