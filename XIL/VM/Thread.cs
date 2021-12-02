using System;
using System.Collections.Generic;
using System.Text;

using XIL.LangDef;

namespace XIL.VM
{
    /// <summary>
    /// Thread's priority
    /// </summary>
    public enum Priority
    {
        /// <summary>
        /// 1 timeslice
        /// </summary>
        Low = 1,
        /// <summary>
        /// 4 timeslice
        /// </summary>
        Normal = 4,
        /// <summary>
        /// 8 timeslice
        /// </summary>
        High = 8,
        /// <summary>
        /// run till done
        /// </summary>
        Exclusive = int.MaxValue
    }

    /// <summary>
    /// Thread's state
    /// </summary>
    public enum ThreadState
    {
        /// <summary>
        /// thread is running
        /// </summary>
        Running,
        /// <summary>
        /// thread is paused
        /// </summary>
        Pause,
        /// <summary>
        /// thread has done executing
        /// </summary>
        Done
    }

    /// <summary>
    /// An execution thread
    /// </summary>
    public class Thread
    {
        #region data and code
        internal Stack stack;
        Stack<int> fstack;
        Instruction[] instructions = null;
        string[] stringTable = null;
        /// <summary>
        /// instruction pointer
        /// </summary>
        public int currentInstruction = 0;
        /// <summary>
        /// program length
        /// </summary>
        public int InstructionCount { get; private set; } = 0;
        /// <summary>
        /// exit code
        /// </summary>
        public int ExitCode = 0;
        /// <summary>
        /// where to return jump
        /// </summary>
        public int ReturnJump = 0;
        /// <summary>
        /// where to return function
        /// </summary>
        public int FunctionReturn = 0;
        #endregion

        #region runtime info
        /// <summary>
        /// thread's priority
        /// </summary>
        public readonly Priority Priority = Priority.Normal;
        /// <summary>
        /// thread's state
        /// </summary>
        public ThreadState State = ThreadState.Running;
        /// <summary>
        /// has the thread been loaded with a program
        /// </summary>
        public bool IsLoaded => this.instructions != null;
        /// <summary>
        /// is the thread running
        /// </summary>
        public bool IsRunning { get; private set; } = false;
        /// <summary>
        /// has the thread done running
        /// </summary>
        public bool IsDoneExecuting { get; private set; } = false;
        /// <summary>
        /// does the thread has a runtime error
        /// </summary>
        public bool IsRuntimeError = false;
        /// <summary>
        /// runtime error's message
        /// </summary>
        public string RuntimeErrorMessage = null;
        #endregion

        /// <summary>
        /// init an empty thread
        /// </summary>
        /// <param name="priority"></param>
        public Thread(Priority priority = Priority.Normal)
        {
            this.Priority = priority;
            this.Init();
        }

        /// <summary>
        /// init a thread and load it with a program
        /// </summary>
        /// <param name="instrs"></param>
        /// <param name="strs"></param>
        /// <param name="priority"></param>
        public Thread(Instruction[] instrs, string[] strs, Priority priority = Priority.Normal)
        {
            this.Priority = priority;
            this.LoadInstructions(instrs, strs);
            this.Init();
        }

        private void Init()
        {
            this.stack = new Stack();
            this.fstack = new Stack<int>();
        }

        /// <summary>
        /// load a program into a thread
        /// </summary>
        /// <param name="instrs"></param>
        /// <param name="strs"></param>
        public void LoadInstructions(Instruction[] instrs, string[] strs)
        {
            this.instructions = new Instruction[instrs.Length];
            Array.Copy(instrs, this.instructions, instrs.Length);

            this.stringTable = new string[strs.Length];
            Array.Copy(strs, this.stringTable, strs.Length);

            this.currentInstruction = 0;
            this.InstructionCount = this.instructions.Length;
            this.IsRunning = true;
            this.IsDoneExecuting = false;
        }

        /// <summary>
        /// get a string from the thread's string table
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string GetString(int index)
        {
            return index < this.stringTable.Length ? this.stringTable[index] : null;
        }

        #region stack interface
        /// <summary>
        /// Check if the stack is empty
        /// </summary>
        public bool IsStackEmpty => this.stack.Top == 0;

        /// <summary>
        /// stack's top index
        /// </summary>
        public int StackTopIndex => this.stack.Top;

        /// <summary>
        /// Pop off tots and return it
        /// </summary>
        /// <returns>the poped value</returns>
        public int Pop()
        {
            if (this.stack.Top == 0)
            {
                throw new InvalidOperationException("Stack Empty");
            }
            return this.stack.Pop();
        }

        /// <summary>
        /// push a value on tots
        /// </summary>
        /// <param name="value">the value to push</param>
        /// <returns>the pushed value</returns>
        public int Push(int value)
        {
            if (this.stack.Top == this.stack.Size - 1)
            {
                throw new InvalidOperationException("Stack Full");
            }
            this.stack.Push(value);
            return this.stack.Peek();
        }

        /// <summary>
        /// peek the value on tots
        /// </summary>
        /// <returns>the value on tots</returns>
        public int Peek()
        {
            if (this.stack.Top == 0)
            {
                throw new InvalidOperationException("Stack Empty");
            }
            return this.stack.Peek();
        }

        /// <summary>
        /// Set the stack element at index to the given value
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void Set(int index, int value)
        {
            if (index >= 0)
            {
                this.stack.Set(index, value);
            }
            else
            {
                //todo relative stack index

            }
        }

        /// <summary>
        /// Get the value of the stack element at index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public int Get(int index)
        {
            if (index >= 0)
            {
                return this.stack.Get(index);
            }
            else
            {
                //todo relative stack index
                return this.stack.Get(this.stack.Top + index);
            }
        }

        /// <summary>
        /// push an array on tots
        /// </summary>
        /// <param name="array">the array to be pushed</param>
        public void PushArray(int[] array)
        {
            this.stack.PushArray(array);
        }

        // todo implement array access 
        /// <summary>
        /// pop an array from tots
        /// </summary>
        /// <param name="arrayIndex">where does the array start</param>
        /// <param name="arraySize">what is the array's size</param>        
        /// <returns></returns>
        public int[] PopArray(int arrayIndex, int arraySize)
        {
            return this.stack.PopArray(arrayIndex, arraySize);
        }

        /// <summary>
        /// clear the stack
        /// </summary>
        public void Clear()
        {
            this.stack.Clear();
        }

        /// <summary>
        /// grow the stack
        /// </summary>
        /// <param name="newSize">new stack size</param>
        /// <returns>current stack size</returns>
        public int Grow(int newSize)
        {
            if (this.stack.Size < newSize)
            {
                this.stack.Size = newSize;
            }
            return this.stack.Size;
        }
        #endregion

        #region fstack interface
        /// <summary>
        /// Current function stack
        /// </summary>
        public int CurrentFStack => this.fstack.Count;
        /// <summary>
        /// Pop a function return address from stack
        /// </summary>
        /// <returns></returns>
        public int PopF()
        {
            return this.fstack.Pop();
        }
        /// <summary>
        /// Peek the top function return on stack
        /// </summary>
        public int PeekF()
        {
            return this.fstack.Peek();
        }
        /// <summary>
        /// Push a function return address on stack
        /// </summary>
        public void PushF(int f)
        {
            this.fstack.Push(f);
        }
        #endregion

        #region instruction manipulation
        /// <summary>
        /// get an instruction at <paramref name="index"/>, will throw exception if using negative or out of range index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Instruction this[int index] => this.instructions[index];

        /// <summary>
        /// fetch the next instruction and advance the instruction pointer
        /// </summary>
        /// <returns>the next instruction</returns>
        public Instruction FetchInstruction()
        {
            if (this.currentInstruction == this.InstructionCount)
            {
                this.EndExecution();
                return Instruction.Nop;
            }
            else
            {
                return this.instructions[this.currentInstruction++];
            }
        }

        /// <summary>
        /// append an instruction into the thread
        /// Has not been implemented
        /// for runtime modifying capability
        /// </summary>
        /// <param name="instr"></param>
        public void AppendInstruction(Instruction instr)
        {
            throw new NotImplementedException(nameof(AppendInstruction));
        }

        /// <summary>
        /// modify an instruction
        /// for runtime modifying capability
        /// </summary>
        /// <param name="index"></param>
        /// <param name="instr"></param>
        public void SetInstruction(int index, Instruction instr)
        {
            if (index > this.InstructionCount - 1)
            {
                throw new IndexOutOfRangeException();
            }
            this.instructions[index] = instr;
        }
        #endregion

        /// <summary>
        /// pause this thread
        /// </summary>
        public void PauseExecution()
        {
            this.IsRunning = false;
        }

        /// <summary>
        /// end this thread
        /// </summary>
        public void EndExecution()
        {
            this.IsRunning = false;
            this.IsDoneExecuting = true;
            this.State = ThreadState.Done;
        }

        /// <summary>
        /// raise a runtime error on this thread
        /// </summary>
        public void RuntimeError(string errmsg)
        {
            this.IsRuntimeError = true;
            this.RuntimeErrorMessage = errmsg;
            this.IsRunning = false;
            this.IsDoneExecuting = true;
        }
    }
}
