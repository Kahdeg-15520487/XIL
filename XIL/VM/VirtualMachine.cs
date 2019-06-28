using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using XIL.LangDef;

namespace XIL.VM
{
    [Flags]
    public enum VirtualMachineVerboseLevel
    {
        None = 0,
        LoadtimeError = 1,
        RuntimeError = 2,
        InstructionInfo = 4,
        ThreadInfo = 8,
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
        public static Dictionary<int, InstructionAction> InstructionMap = null;
        public static List<string> LoadedLibrary = null;
        public static Random RandomNumberGenerator = null;
        int currentThread = 0;
        public List<int> Exitcodes;

        public VirtualMachineVerboseLevel VerboseLevel { get; set; } = VirtualMachineVerboseLevel.LoadtimeError | VirtualMachineVerboseLevel.RuntimeError;

        public int TickElapsedSinceLastTimeSlice;
        public int Tick()
        {
            return ++this.TickElapsedSinceLastTimeSlice;
        }

        public VirtualMachine(params IInstructionImplementation[] instructionImplementations)
        {
            if (InstructionMap == null)
            {
                this.InitInstructionMap(instructionImplementations);
            }
            this.Init();
        }

        private void InitInstructionMap(IInstructionImplementation[] instructionImplementations)
        {
            InstructionMap = new Dictionary<int, InstructionAction>();
            LoadedLibrary = new List<string>();
            foreach (var instrImplm in instructionImplementations)
            {
                LoadedLibrary.Add(instrImplm.GetType().Name);

                var methods = instrImplm.GetType().GetTypeInfo().GetMethods()
                  .Where(m => m.GetCustomAttributes(typeof(InstructionAttribute), false).Count() > 0);

                foreach (var method in methods)
                {
                    //get instruction info
                    var attr = method.GetCustomAttribute<InstructionAttribute>();
                    //get delegate from method
                    var action = (InstructionAction)method.CreateDelegate(typeof(InstructionAction), instrImplm);
                    //map said delegate to instruction
                    InstructionMap.Add(attr.OpCode, action);
                }
            }
        }

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
            foreach (var instr in instrs)
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
            var ct = this.currentThread;
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

        public void RuntimeErrorLog(Instruction instruction, string errormsg)
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