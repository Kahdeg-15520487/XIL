using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using XIL.LangDef;
using XIL.VM;

namespace XIL.StandardLibrary
{
    public class IOInstruction : IInstructionImplementation
    {
        const string lib = "io";

        /// <summary>
        /// print <para/>
        /// print the tots to the console
        /// </summary>
        [Instruction(0x30, "print", lib)]
        public void Print(Thread thread, int operand1, int operand2)
        {
            int op = thread.Pop();
            Console.WriteLine(op);
        }

        /// <summary>
        /// read <para/>
        /// read a number from console and push it on tots <para/>
        /// if the input is not a number, will push 0 on tots
        /// </summary>
        [Instruction(0x31, "read", lib)]
        public void ReadLine(Thread thread, int operand1, int operand2)
        {
            Console.Write("input a number: ");
            int.TryParse(Console.ReadLine(), out int result);
            thread.Push(result);
        }

        /// <summary>
        /// prints <para/>
        /// will print an int-encoded string on tots to console
        /// </summary>
        [Instruction(0x32, "prints", lib)]
        public void PrintString(Thread thread, int operand1, int operand2)
        {
            Console.WriteLine(Util.PopStringFromStack(thread));
        }

        /// <summary>
        /// reads <para/>
        /// will read a string from console, encode it to an int array and push it on tots
        /// </summary>
        [Instruction(0x33, "reads", lib)]
        public void ReadString(Thread thread, int operand1, int operand2)
        {
            Console.Write("input string: ");
            Util.PushStringToStack(thread, Console.ReadLine());
        }

        /// <summary>
        /// reads <para/>
        /// will read a string from console, encode it to an int array and push it on tots
        /// </summary>
        [Instruction(0x34, "printc", lib)]
        public void PrintChar(Thread thread, int operand1, int operand2)
        {
            int op = thread.Pop();
            char c = (char)op;
            if (Char.IsControl(c) && c != '\n')
            {
                c = ' ';
            }
            Console.Write(c);
        }

        /// <summary>
        /// reads <para/>
        /// will read a string from console, encode it to an int array and push it on tots
        /// </summary>
        [Instruction(0x35, "readc", lib)]
        public void ReadChar(Thread thread, int operand1, int operand2)
        {
            Console.Write("input char: ");
            var temp = Console.ReadLine();
            if (temp.Length == 0)
            {
                thread.Push('\0');
            }
            else
            {
                thread.Push(temp[0]);
            }
        }
    }
}