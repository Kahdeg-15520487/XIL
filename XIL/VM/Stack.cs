using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace XIL.VM
{
    internal class Stack : ICollection
    {
        int[] stack;
        public readonly int Size;
        public int Top { get; private set; }

        public int Count => Top;

        public bool IsSynchronized => false;

        public object SyncRoot => false;

        public bool IsReadOnly => false;

        public int this[int index] {
            get => stack[index];
            set { stack[index] = value; }
        }

        public Stack(int stacksize = 1000)
        {
            stack = new int[stacksize];
            Size = stacksize;
            Top = 0;
        }

        public int Pop()
        {
            if (Top == 0)
            {
                throw new InvalidOperationException("Stack Empty");
            }
            return stack[--Top];
        }

        public void Push(int value)
        {
            if (Top == Size - 1)
            {
                throw new InvalidOperationException("Stack Full");
            }
            stack[Top++] = value;
        }

        public int Peek()
        {
            if (Top == 0)
            {
                throw new InvalidOperationException("Stack Empty");
            }
            return stack[Top - 1];
        }

        public void Set(int index, int value)
        {
            if (index > Top)
            {
                throw new IndexOutOfRangeException();
            }
            stack[index] = value;
        }

        public int Get(int index)
        {
            if (index > Top)
            {
                throw new IndexOutOfRangeException();
            }
            return stack[index];
        }

        public void PushArray(int[] array)
        {
            int arraySize = array.GetLength(0);

            if (Top + arraySize >= Size)
            {
                throw new InvalidOperationException("Stack full");
            }

            Array.Copy(array, 0, stack, Top, arraySize);
            Top += arraySize;
        }

        public int[] PopArray(int arrayIndex, int arraySize)
        {
            if (arrayIndex > Top)
            {
                throw new IndexOutOfRangeException();
            }

            if (arrayIndex + arraySize - 1 > Top)
            {
                throw new IndexOutOfRangeException();
            }

            int[] result = new int[arraySize];
            Array.Copy(stack, arrayIndex, result, 0, arraySize);
            Top -= arraySize;
            return result;
        }

        public void Clear()
        {
            Top = 0;
        }

        public void CopyTo(Array array, int index)
        {
            stack.CopyTo(array, index);
        }

        public IEnumerator GetEnumerator()
        {
            return stack.GetEnumerator();
        }

        public void Add(int item)
        {

        }

        public bool Contains(int item)
        {
            for (int i = 0; i < Top; i++)
            {
                if (stack[i] == item)
                {
                    return true;
                }
            }
            return false;
        }

        public void CopyTo(int[] array, int arrayIndex)
        {
            //Array.Copy(_stack,)
            stack.CopyTo(array, arrayIndex);
        }

        public bool Remove(int item)
        {
            return false;
        }

        /// <summary>
        /// get a snapshot of the stack
        /// </summary>
        public int[] SnapShot()
        {
            int[] snapshot = new int[Top];
            for (int i = 0; i < Top; i++)
            {
                snapshot[i] = stack[i];
            }
            return snapshot;
        }
    }
}
