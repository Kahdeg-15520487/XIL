using System;
using System.Collections.Generic;
using System.Text;

using XIL.LangDef;

namespace XIL.VM
{
    public enum Priority
    {
        Low,
        Normal,
        High,
        Exclusive
    }

    public class Thread
    {
        #region data and code
        Stack _stack;
        Stack<int> _fstack;
        Instruction[] _instructions = null;
        /// <summary>
        /// instruction pointer
        /// </summary>
        public int currentInstruction = 0;
        /// <summary>
        /// program length
        /// </summary>
        public int InstructionCount { get; private set; } = 0;
        public int ExitCode = 0;
        public int returnJump = 0;
        public int FunctionReturn = 0;
        #endregion

        #region runtime info
        public readonly Priority priority = Priority.Normal;
        public bool IsLoaded => _instructions != null;
        public bool IsRunning { get; private set; } = false;
        public bool IsDoneExecuting { get; private set; } = false;
        #endregion

        public Thread(Priority priority = Priority.Normal)
        {
            this.priority = priority;
            Init();
        }

        public Thread(List<Instruction> instrs, Priority priority = Priority.Normal)
        {
            this.priority = priority;
            LoadInstructions(instrs);
            Init();
        }

        private void Init()
        {
            _stack = new Stack();
            _fstack = new Stack<int>();
        }

        public void LoadInstructions(List<Instruction> instrs)
        {
            _instructions = new Instruction[instrs.Count];
            instrs.CopyTo(_instructions);
            currentInstruction = 0;
            InstructionCount = _instructions.GetLength(0);
            IsRunning = true;
            IsDoneExecuting = false;
        }

        #region stack interface
        /// <summary>
        /// Check if the stack is empty
        /// </summary>
        public bool IsStackEmpty => _stack.Top == 0;

        /// <summary>
        /// stack's top index
        /// </summary>
        public int StackTopIndex => _stack.Top;

        /// <summary>
        /// Pop off tots and return it
        /// </summary>
        /// <returns>the poped value</returns>
        public int Pop()
        {
            if (_stack.Top == 0)
            {
                throw new InvalidOperationException("Stack Empty");
            }
            return _stack.Pop();
        }

        /// <summary>
        /// push a value on tots
        /// </summary>
        /// <param name="value">the value to push</param>
        /// <returns>the pushed value</returns>
        public int Push(int value)
        {
            if (_stack.Top == _stack.Size - 1)
            {
                throw new InvalidOperationException("Stack Full");
            }
            _stack.Push(value);
            return _stack.Peek();
        }

        /// <summary>
        /// peek the value on tots
        /// </summary>
        /// <returns>the value on tots</returns>
        public int Peek()
        {
            if (_stack.Top == 0)
            {
                throw new InvalidOperationException("Stack Empty");
            }
            return _stack.Peek();
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
                _stack.Set(index, value);
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
                return _stack.Get(index);
            }
            else
            {
                //todo relative stack index
                return 0;
            }
        }

        /// <summary>
        /// push an array on tots
        /// </summary>
        /// <param name="array">the array to be pushed</param>
        public void PushArray(int[] array)
        {
            _stack.PushArray(array);
        }

        /// <summary>
        /// pop an array from tots
        /// </summary>
        /// <param name="arrayIndex">where does the array start</param>
        /// <param name="arraySize">what is the array's size</param>
        //todo implement array access 
        /// <returns></returns>
        public int[] PopArray(int arrayIndex, int arraySize)
        {
            return _stack.PopArray(arrayIndex, arraySize);
        }

        /// <summary>
        /// clear the stack
        /// </summary>
        public void Clear()
        {
            _stack.Clear();
        }
        #endregion

        #region fstack interface
        public int CurrentFStack { get => _fstack.Count; }
        public int PopF()
        {
            return _fstack.Pop();
        }
        public int PeekF()
        {
            return _fstack.Peek();
        }
        public void PushF(int f)
        {
            _fstack.Push(f);
        }
        #endregion

        #region instruction manipulation
        /// <summary>
        /// get an instruction at <paramref name="index"/>, will throw exception if using negative or out of range index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Instruction this[int index] => _instructions[index];

        /// <summary>
        /// fetch the next instruction and advance the instruction pointer
        /// </summary>
        /// <returns>the next instruction</returns>
        public Instruction FetchInstruction()
        {
            if (currentInstruction == InstructionCount)
            {
                EndExecution();
                return Instruction.Nop;
            }
            else
            {
                return _instructions[currentInstruction++];
            }
        }

        public void AppendInstruction(Instruction instr)
        {

        }

        public void SetInstruction(int index, Instruction instr)
        {
            if (index > InstructionCount - 1)
            {
                throw new IndexOutOfRangeException();
            }
            _instructions[index] = instr;
        }
        #endregion

        /// <summary>
        /// end this thread
        /// </summary>
        public void EndExecution()
        {
            IsRunning = false;
            IsDoneExecuting = true;
        }
    }
}
