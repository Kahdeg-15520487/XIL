﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace XIL.VM
{
    class Stack : ICollection
    {
        int[] _stack;
        public readonly int Size;
        public int Top { get; private set; }

        public int Count => Top;

        public bool IsSynchronized => false;

        public object SyncRoot => false;

        public int this[int index]
        {
            get => _stack[index];
            set { _stack[index] = value; }
        }

        public Stack(int stacksize = 1000)
        {
            _stack = new int[stacksize];
            Size = stacksize;
            Top = 0;
        }

        public int Pop()
        {
            if (Top == 0)
            {
                throw new InvalidOperationException("Stack Empty");
            }
            return _stack[--Top];
        }

        public void Push(int value)
        {
            if (Top == Size - 1)
            {
                throw new InvalidOperationException("Stack Full");
            }
            _stack[Top++] = value;
        }

        public int Peek()
        {
            if (Top == 0)
            {
                throw new InvalidOperationException("Stack Empty");
            }
            return _stack[Top-1];
        }

        public void Set(int index, int value)
        {
            if (index > Top)
            {
                throw new IndexOutOfRangeException();
            }
            _stack[index] = value;
        }

        public int Get(int index)
        {
            if (index>Top)
            {
                throw new IndexOutOfRangeException();
            }
            return _stack[index];
        }

        public void PushArray(int[] array)
        {
            int arraySize = array.GetLength(0);

            if (Top + arraySize >= Size)
            {
                throw new InvalidOperationException("Stack full");
            }

            Array.Copy(array, 0, _stack, Top, arraySize);
            Top += arraySize;
        }

        public int[] PopArray(int arrayIndex,int arraySize)
        {
            if (arrayIndex > Top)
            {
                throw new IndexOutOfRangeException();
            }

            if (arrayIndex + arraySize -1 > Top)
            {
                throw new IndexOutOfRangeException();
            }

            int[] result = new int[arraySize];
            Array.Copy(_stack, arrayIndex, result, 0, arraySize);
            return result;
        }

        public void Clear()
        {
            Top = 0;
        }

        public void CopyTo(Array array, int index)
        {
            _stack.CopyTo(array, index);
        }

        public IEnumerator GetEnumerator()
        {
            return _stack.GetEnumerator();
        }
    }
}
