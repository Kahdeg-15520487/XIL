using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using XIL.LangDef;

namespace XIL.VM
{
    public class VirtualMachine
    {
        List<Thread> threads;
        public static Dictionary<int, InstructionAction> instructionMap = null;
        public static List<string> loadedLibrary = null;
        public static Random randomNumberGenerator = null;
        int currentThread = 0;
        public List<int> exitcodes;
        public int LastStep { get; private set; } = 0;
        public int CurrentStep { get; private set; } = 0;

        public VirtualMachine(params IInstructionImplementation[] instructionImplementations)
        {
            if (instructionMap == null)
            {
                InitInstructionMap(instructionImplementations);
            }
            Init();
        }

        private void InitInstructionMap(IInstructionImplementation[] instructionImplementations)
        {
            instructionMap = new Dictionary<int, InstructionAction>();
            loadedLibrary = new List<string>();
            foreach (var instrImplm in instructionImplementations)
            {
                loadedLibrary.Add(instrImplm.GetType().Name);

                var methods = instrImplm.GetType().GetTypeInfo().GetMethods()
                  .Where(m => m.GetCustomAttributes(typeof(InstructionAttribute), false).Count() > 0);

                foreach (var method in methods)
                {
                    //get instruction info
                    var attr = method.GetCustomAttribute<InstructionAttribute>();
                    //get delegate from method
                    var action = (InstructionAction)method.CreateDelegate(typeof(InstructionAction), instrImplm);
                    //map said delegate to instruction
                    instructionMap.Add(attr.OpCode, action);
                }
            }
        }

        public static bool ContainLibrary(string libname)
        {
            return loadedLibrary.Contains(libname);
        }

        private void Init()
        {
            threads = new List<Thread>();

            if (randomNumberGenerator != null)
            {
                randomNumberGenerator = new Random();
            }

            exitcodes = new List<int>();
            CurrentStep = 0;
            LastStep = 0;
        }

        public bool LoadProgram(Instruction[] instrs, string[] strs)
        {
            if (!ValidateProgram(instrs))
            {
                Console.WriteLine("Program contain undefined opcode");
                return false;
            }
            threads.Add(new Thread(instrs, strs));
            exitcodes.Add(0);
            return true;
        }

        private bool ValidateProgram(Instruction[] instrs)
        {
            bool isOK = true;
            foreach (var instr in instrs)
            {
                if (!instructionMap.ContainsKey(instr.opCode))
                {
                    isOK = false;
                    Console.WriteLine("Undefined opcode: {0:X}", instr.opCode);
                }
            }
            return isOK;
        }

        private Thread GetNextThread()
        {
            currentThread++;
            if (currentThread == threads.Count)
            {
                currentThread = 0;
            }
            return threads[currentThread];
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

        public void Run()
        {
            Thread thread = GetNextThread();
            //if (thread != null)
            //todo non blocking thread execution
            //either offset thread execution to another thread
            //or emulate multithreading using timeslice
            while (!thread.IsDoneExecuting)
            {
                Instruction currentInstruction = FetchInstruction(thread);
                //Console.WriteLine(currentInstruction);
                instructionMap[currentInstruction.opCode].Invoke(thread, currentInstruction.firstOperand, currentInstruction.secondOperand);
            }
        }
    }
}