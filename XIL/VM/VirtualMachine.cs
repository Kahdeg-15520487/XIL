using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using XIL.LangDef;

namespace XIL.VM
{
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

        public int TickElapsedSinceLastTimeSlice;
        public int Tick()
        {
            return ++TickElapsedSinceLastTimeSlice;
        }

        public VirtualMachine(params IInstructionImplementation[] instructionImplementations)
        {
            if (InstructionMap == null)
            {
                InitInstructionMap(instructionImplementations);
            }
            Init();
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
            threads = new List<Thread>();

            if (RandomNumberGenerator != null)
            {
                RandomNumberGenerator = new Random();
            }

            Exitcodes = new List<int>();
            TickElapsedSinceLastTimeSlice = 0;
        }

        public bool LoadProgram(Instruction[] instrs, string[] strs)
        {
            if (!ValidateProgram(instrs))
            {
                Console.WriteLine("Program contain undefined opcode");
                return false;
            }
            threads.Add(new Thread(instrs, strs));
            Exitcodes.Add(0);
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
                    Console.WriteLine("Undefined opcode: {0:X}", instr.OpCode);
                }
            }
            return isOK;
        }

        private bool GetNextThread()
        {
            var ct = currentThread;
            do
            {
                currentThread++;
                currentThread = Wrap(currentThread, 0, threads.Count);
                if (currentThread == ct)
                {
                    return false;
                }
            } while (threads[currentThread].State != ThreadState.Running);
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
            return threads.All(t => t.State == ThreadState.Done);
        }

        private bool IsCurrentThreadDoneOrTimeOut()
        {
            return threads[currentThread].State == ThreadState.Done
                || TickElapsedSinceLastTimeSlice >= (int)threads[currentThread].Priority;
        }

        public void Run()
        {
            //Thread thread = GetNextThread();

            while (true)
            {
                if (IsAllThreadDone())
                {
                    break;
                }

                if (IsCurrentThreadDoneOrTimeOut())
                {
                    TickElapsedSinceLastTimeSlice = 0;
                    GetNextThread();
                }

                var thread = threads[currentThread];
                Console.WriteLine("thread {0}, priority {1}, tick {2}", currentThread, thread.Priority, TickElapsedSinceLastTimeSlice);
                //if (thread != null)
                //todo non blocking thread execution
                //either offset thread execution to another thread
                //or emulate multithreading using timeslice
                //while (!thread.IsDoneExecuting)
                {
                    Instruction currentInstruction = FetchInstruction(thread);
                    //Console.WriteLine("{0} {1} {2}", instructionMap[currentInstruction.opCode].Method.Name, currentInstruction.firstOperand, currentInstruction.secondOperand);
                    InstructionMap[currentInstruction.OpCode].Invoke(thread, currentInstruction.FirstOperand, currentInstruction.SecondOperand);

                    if (thread.IsRuntimeError)
                    {
                        RuntimeError(currentInstruction, thread.RuntimeErrorMessage);
                    }
                }
                Exitcodes[currentThread] = thread.ExitCode;
            }
        }

        public void RuntimeError(Instruction instruction, string errormsg)
        {
            Console.WriteLine();
            Console.WriteLine("Runtime error: " + errormsg);
            Console.WriteLine("at line {0}: {1} {2} {3}", instruction.LineNumber, InstructionMap[instruction.OpCode].Method.Name, instruction.FirstOperand, instruction.SecondOperand);
            Console.WriteLine();
        }
    }
}