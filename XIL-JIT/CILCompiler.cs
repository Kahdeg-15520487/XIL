﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using XIL.Assembler;
using XIL.LangDef;
using XIL.VM;
using XIL.JIT;

namespace XILtoCIL
{
    /// <summary>
    /// only support the CoreInstruction and ForeignFunctionInstruction
    /// </summary>
    public class CilCodeGenerator : ICodeGenerator
    {
        private readonly List<(string label, int target)> labels;
        private readonly List<Instruction> program;
        private readonly List<string> stringTable;
        private readonly Dictionary<string, int> libs;

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
                (string label, int target) jump = this.labels.First(j => j.label.Equals(label));
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

        public void EmitAssembly()
        {
            //AssemblyName assembly = new AssemblyName("DynamicAssembly");
            //AssemblyBuilder assemblyBuilder =
            //    AppDomain.CurrentDomain.DefineDynamicAssembly(
            //        assembly,
            //        AssemblyBuilderAccess.Save);
            //ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(assembly.Name, assembly.Name + ".exe");
            //TypeBuilder typeBuilder = moduleBuilder.DefineType("Program", TypeAttributes.Public);
            //typeBuilder.DefineField("aaaa", typeof(HostFunctionCollection), FieldAttributes.Static | FieldAttributes.Private);
            ////Type[] parameters = new Type[] { typeof(string[]) };
            //MethodBuilder methodBuilder = typeBuilder.DefineMethod("Main", MethodAttributes.Public | MethodAttributes.Static, typeof(int), null);
            //ILGenerator ilgen = methodBuilder.GetILGenerator();

            //this.EmitMethod(ilgen);

            //typeBuilder.CreateType();
            //assemblyBuilder.SetEntryPoint(methodBuilder.GetBaseDefinition(), PEFileKinds.ConsoleApplication);
            //assemblyBuilder.Save(assembly.Name + ".exe");
        }
        public Func<int> EmitDynamicMethod()
        {
            DynamicMethod method = new DynamicMethod("xil_cil", typeof(int), null);
            ILGenerator ilgen = method.GetILGenerator();

            this.EmitMethod(ilgen);

            return (Func<int>)method.CreateDelegate(typeof(Func<int>));
        }

        private void EmitMethod(ILGenerator ilgen)
        {
            StringBuilder asmSource = new StringBuilder();

            string[] stringTable = new string[this.stringTable.Count];
            for (int i = 0; i < stringTable.Length; i++)
            {
                stringTable[i] = stringTable[i];
            }

            Dictionary<int, Label> jumpTable = new Dictionary<int, Label>();
            Dictionary<int, int> lookUpJumpTable = new Dictionary<int, int>();
            for (int i = 0; i < this.labels.Count; i++)
            {
                jumpTable.Add(this.labels[i].target, ilgen.DefineLabel());
                lookUpJumpTable.Add(i, this.labels[i].target);
            }

            //gen cil instruction based on given instruction list, jump table and string constant
            int instructionCount = this.program.Count;
            //some local register for easier thing
            LocalBuilder a = ilgen.DeclareLocal(typeof(int));
            //LocalBuilder b = ilgen.DeclareLocal(typeof(int));
            //LocalBuilder c = ilgen.DeclareLocal(typeof(bool));
            for (int i = 0; i < this.program.Count; i++)
            {
                Instruction instr = this.program[i];
                //ilgen.EmitWriteLine(instr.ToString());

                if (jumpTable.ContainsKey(i))
                {
                    ilgen.MarkLabel(jumpTable[i]);
                    Console.WriteLine("jump label defined: {0}", i);
                }


                switch ((InstructionOPCode)instr.OpCode)
                {
                    case InstructionOPCode.jmp:
                        {
                            //var target = jumpTable[lookUpJumpTable[instr.FirstOperand]];
                            Label target = jumpTable[instr.FirstOperand];
                            ilgen.Emit(OpCodes.Br_S, target);
                            asmSource.AppendFormat("{1}| jmp {0}", instr.FirstOperand, i.ToString().PadLeft(3, '0'));
                            //program.
                            //Console.WriteLine("jmp {0}", lookUpJumpTable[instr.param]);
                        }
                        break;
                    case InstructionOPCode.je:
                        {
                            Label target = jumpTable[instr.FirstOperand];
                            ilgen.Emit(OpCodes.Ceq);
                            ilgen.Emit(OpCodes.Brtrue_S, target);
                            asmSource.AppendFormat("{1}| j= {0}", instr.FirstOperand, i.ToString().PadLeft(3, '0'));
                        }
                        break;
                    case InstructionOPCode.j1:
                        {
                            //var target = jumpTable[lookUpJumpTable[instr.FirstOperand]];
                            Label target = jumpTable[instr.FirstOperand];
                            ilgen.Emit(OpCodes.Brtrue_S, target);
                            asmSource.AppendFormat("{1}| j1 {0}", instr.FirstOperand, i.ToString().PadLeft(3, '0'));
                            //Console.WriteLine("jmp {0}", lookUpJumpTable[instr.param]);
                        }
                        break;

                    case InstructionOPCode.yeet:
                        {
                            ilgen.Emit(OpCodes.Ldc_I4, instr.FirstOperand);
                            asmSource.AppendFormat("{1}| push {0}", instr.FirstOperand, i.ToString().PadLeft(3, '0'));
                        }
                        break;
                    case InstructionOPCode.dup:
                        {
                            ilgen.Emit(OpCodes.Dup);
                            asmSource.AppendFormat("{0}| dup", i.ToString().PadLeft(3, '0'));
                        }
                        break;
                    case InstructionOPCode.pop:
                        {
                            ilgen.Emit(OpCodes.Pop);
                            asmSource.AppendFormat("{0}| pop", i.ToString().PadLeft(3, '0'));
                        }
                        break;

                    case InstructionOPCode.add:
                        {
                            ilgen.Emit(OpCodes.Add);
                            asmSource.AppendFormat("{0}| add", i.ToString().PadLeft(3, '0'));
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
                        {
                            //ilgen.Emit(OpCodes.Stloc_0);
                            //ilgen.EmitWriteLine(a);
                            ilgen.Emit(OpCodes.Ret);
                            asmSource.AppendFormat("{0}| exit", i.ToString().PadLeft(3, '0'));
                            break;
                        }
                }
                asmSource.AppendLine();
            }
            ilgen.Emit(OpCodes.Ret);
            //foreach (var label in jumpTable.Where(kvp => kvp.Key >) {

            //}

            Console.WriteLine("===asm===");
            Console.WriteLine(asmSource.ToString());
            Console.WriteLine("===asm===");
        }
    }
}
