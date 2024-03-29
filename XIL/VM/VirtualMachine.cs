﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using XIL.LangDef;

namespace XIL.VM
{
    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum VirtualMachineVerboseLevel
    {
        /// <summary>
        /// log nothing
        /// </summary>
        None = 0,
        /// <summary>
        /// log loadtime error
        /// </summary>
        LoadtimeError = 1,
        /// <summary>
        /// log runtime error
        /// </summary>
        RuntimeError = 2,
        /// <summary>
        /// log instruction detail
        /// </summary>
        InstructionInfo = 4,
        /// <summary>
        /// log thead information
        /// </summary>
        ThreadInfo = 8,
        /// <summary>
        /// log loadtime information
        /// </summary>
        LoadtimeInfo = 16,
    }

    /// <summary>
    /// instruction delegate
    /// </summary>
    public delegate void InstructionAction(Thread thread, int operand1, int operand2);

    /// <summary>
    /// A XIL virtual machine
    /// </summary>
    public class VirtualMachine
    {
        List<Thread> threads;
        /// <summary>
        /// a list of instruction's implementation
        /// </summary>
        public static Dictionary<int, InstructionAction> InstructionMap = null;

        /// <summary>
        /// a list of instruction's metadata
        /// </summary>
        public static Dictionary<int, InstructionAttribute> InstructionMetaDataMap = null;

        /// <summary>
        /// a list of loaded instruction library's name
        /// </summary>
        public static List<string> LoadedLibrary = null;

        /// <summary>
        /// random number generator
        /// </summary>
        public static Random RandomNumberGenerator = null;

        int currentThread = 0;

        /// <summary>
        /// exitcode of all thread
        /// </summary>
        public List<int> Exitcodes;

        /// <summary>
        /// log verbosity level
        /// </summary>
        public VirtualMachineVerboseLevel VerboseLevel { get; set; } = VirtualMachineVerboseLevel.LoadtimeError | VirtualMachineVerboseLevel.RuntimeError;

        /// <summary>
        /// time keeping
        /// </summary>
        private int TickElapsedSinceLastTimeSlice;

        /// <summary>
        /// tick an update
        /// </summary>
        /// <returns></returns>
        public int Tick()
        {
            return ++this.TickElapsedSinceLastTimeSlice;
        }

        /// <summary>
        /// init a vm with a list of instruction implementation
        /// </summary>
        /// <param name="instructionImplementations"></param>
        public VirtualMachine(VirtualMachineVerboseLevel verboseLevel = VirtualMachineVerboseLevel.LoadtimeError | VirtualMachineVerboseLevel.RuntimeError, params IInstructionImplementation[] instructionImplementations)
        {
            this.VerboseLevel = verboseLevel;
            if (InstructionMap == null)
            {
                this.InitInstructionMap(instructionImplementations);
            }
            this.Init();
        }

        private void InitInstructionMap(IInstructionImplementation[] instructionImplementations)
        {
            InstructionMap = new Dictionary<int, InstructionAction>();
            InstructionMetaDataMap = new Dictionary<int, InstructionAttribute>();
            LoadedLibrary = new List<string>();
            foreach (IInstructionImplementation instrImplm in instructionImplementations)
            {
                LoadedLibrary.Add(instrImplm.GetType().Name);

                IEnumerable<MethodInfo> methods = instrImplm.GetType().GetTypeInfo().GetMethods()
                  .Where(m => m.GetCustomAttributes(typeof(InstructionAttribute), false).Count() > 0);

                foreach (MethodInfo method in methods)
                {
                    //get instruction info
                    InstructionAttribute attr = method.GetCustomAttribute<InstructionAttribute>();
                    //get delegate from method
                    InstructionAction action = (InstructionAction)method.CreateDelegate(typeof(InstructionAction), instrImplm);
                    //map said delegate to instruction
                    InstructionMap.Add(attr.OpCode, action);
                    InstructionMetaDataMap.Add(attr.OpCode, attr);
                }
            }

            if (this.VerboseLevel.HasFlag(VirtualMachineVerboseLevel.LoadtimeInfo))
            {
                foreach (KeyValuePair<int, InstructionAction> mappedInstruction in InstructionMap.ToList().OrderBy(kvp => kvp.Key))
                {
                    InstructionAttribute instrMetaData = InstructionMetaDataMap[mappedInstruction.Key];
                    Console.WriteLine("loaded \"{0}\" from {1} lib", instrMetaData, instrMetaData.Library);
                }
            }
        }

        /// <summary>
        /// check if a library has been loaded
        /// </summary>
        /// <param name="libname"></param>
        /// <returns></returns>
        public static bool ContainLibrary(string libname)
        {
            return LoadedLibrary.Contains(libname);
        }

        private void Init()
        {
            this.threads = new List<Thread>();

            if (RandomNumberGenerator != null)
            {
                RandomNumberGenerator = new Random();
            }

            this.Exitcodes = new List<int>();
            this.TickElapsedSinceLastTimeSlice = 0;
        }

        /// <summary>
        /// load a program
        /// </summary>
        /// <param name="instrs"></param>
        /// <param name="strs"></param>
        /// <returns></returns>
        public bool LoadProgram(Instruction[] instrs, string[] strs)
        {
            if (!this.ValidateProgram(instrs))
            {
                return false;
            }
            this.threads.Add(new Thread(instrs, strs));
            this.Exitcodes.Add(0);
            return true;
        }

        private bool ValidateProgram(Instruction[] instrs)
        {
            bool isOK = true;
            foreach (Instruction instr in instrs)
            {
                if (!InstructionMap.ContainsKey(instr.OpCode))
                {
                    isOK = false;
                    this.LoadtimeErrorLog(instr, "Program contain undefined opcode");
                }
            }
            return isOK;
        }

        private bool GetNextThread()
        {
            int ct = this.currentThread;
            do
            {
                this.currentThread++;
                this.currentThread = Wrap(this.currentThread, 0, this.threads.Count);
                if (this.currentThread == ct)
                {
                    return false;
                }
            } while (this.threads[this.currentThread].State != ThreadState.Running);
            return true;
        }

        static int Wrap(int i, int min, int max)
        {
            return i % max + min;
        }


        private Instruction FetchInstruction(Thread thread)
        {
            if (thread.currentInstruction == thread.InstructionCount)
            {
                thread.EndExecution();
                return Instruction.Exit;
            }
            else
            {
                return thread[thread.currentInstruction++];
            }
        }

        private bool IsAllThreadDone()
        {
            return this.threads.All(t => t.State == ThreadState.Done);
        }

        private bool IsCurrentThreadDoneOrTimeOut()
        {
            return this.threads[this.currentThread].State == ThreadState.Done
                || this.TickElapsedSinceLastTimeSlice >= (int)this.threads[this.currentThread].Priority;
        }

        /// <summary>
        /// run the virtual machine synchronously
        /// </summary>
        public void Run()
        {
            //Thread thread = GetNextThread();

            while (true)
            {
                if (this.IsAllThreadDone())
                {
                    break;
                }

                if (this.IsCurrentThreadDoneOrTimeOut())
                {
                    this.TickElapsedSinceLastTimeSlice = 0;
                    this.GetNextThread();
                }

                Thread thread = this.threads[this.currentThread];
                this.ThreadInfoLog(thread);
                //if (thread != null)
                //todo non blocking thread execution
                //either offset thread execution to another thread
                //or emulate multithreading using timeslice
                //while (!thread.IsDoneExecuting)
                {
                    Instruction currentInstruction = this.FetchInstruction(thread);
                    this.InstructionInfoLog(currentInstruction);
                    InstructionMap[currentInstruction.OpCode].Invoke(thread, currentInstruction.FirstOperand, currentInstruction.SecondOperand);

                    if (thread.IsRuntimeError)
                    {
                        this.RuntimeErrorLog(currentInstruction, thread.RuntimeErrorMessage);
                    }
                }
                this.Exitcodes[this.currentThread] = thread.ExitCode;
            }
        }

        private void ThreadInfoLog(Thread thread)
        {
            if (this.VerboseLevel.HasFlag(VirtualMachineVerboseLevel.ThreadInfo))
            {
                Console.WriteLine("thread {0}, priority {1}, tick {2}", this.currentThread, thread.Priority, this.TickElapsedSinceLastTimeSlice);
            }
        }

        private void InstructionInfoLog(Instruction currentInstruction)
        {
            if (this.VerboseLevel.HasFlag(VirtualMachineVerboseLevel.InstructionInfo))
            {
                Console.WriteLine("{0} {1} {2}", InstructionMap[currentInstruction.OpCode].Method.Name, currentInstruction.FirstOperand, currentInstruction.SecondOperand);
            }
        }

        private void RuntimeErrorLog(Instruction instruction, string errormsg)
        {
            if (this.VerboseLevel.HasFlag(VirtualMachineVerboseLevel.RuntimeError))
            {
                Console.WriteLine();
                Console.WriteLine("Runtime error: " + errormsg);
                Console.WriteLine("at line {0}: {1} {2} {3}", instruction.LineNumber, InstructionMap[instruction.OpCode].Method.Name, instruction.FirstOperand, instruction.SecondOperand);
                Console.WriteLine();
            }
        }

        private void LoadtimeErrorLog(Instruction instr, string v)
        {
            if (this.VerboseLevel.HasFlag(VirtualMachineVerboseLevel.LoadtimeError))
            {
                Console.WriteLine();
                Console.Write(v);
                Console.Write(": ");
                Console.WriteLine("Undefined opcode: {0:X}", instr.OpCode);
            }
        }
    }
}