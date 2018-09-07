using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using XIL.LangDef;
using XIL.VM;

namespace testconsole
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
            int arraySize = thread.Pop();
            int startIndex = thread.StackTopIndex - (arraySize - 1);
            var result = Decode(thread.PopArray(startIndex, arraySize));
            Console.WriteLine(result.ToString());
        }

        /// <summary>
        /// reads <para/>
        /// will read a string from console, encode it to an int array and push it on tots
        /// </summary>
        [Instruction(0x33, "reads", lib)]
        public void ReadString(Thread thread, int operand1, int operand2)
        {
            Console.Write("input string: ");
            var result = Encode(Console.ReadLine());
            thread.PushArray(result);
            thread.Push(result.GetLength(0));
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

        /// <summary>
        /// decode a int array to string
        /// </summary>
        /// <param name="a">the array to be decode</param>
        /// <returns>decoded string</returns>
        public static string Decode(int[] a)
        {
            StringBuilder result = new StringBuilder();
            foreach (var b in a)
            {
                result.Append(Encoding.ASCII.GetString(BitConverter.GetBytes(b)));
            }
            return result.ToString();
        }

        /// <summary>
        /// encode a string to its int-encoded array
        /// </summary>
        /// <param name="s">string to be encoded</param>
        /// <returns>encoded array</returns>
        public static int[] Encode(string s)
        {
            var strs = ChunksUpto(s, 4).ToArray();
            int[] result = new int[strs.GetLength(0)];
            int index = 0;
            foreach (var str in strs)
            {
                var bytes = Encoding.ASCII.GetBytes(str);
                result[index] = BitConverter.ToInt32(bytes, 0);
                index++;
            }
            return result;
        }

        /// <summary>
        /// chop string into <paramref name="maxChunkSize"/> sized string and will pad-right to make sure that the resulting substring always have that size
        /// </summary>
        /// <param name="str">the string to split</param>
        /// <param name="maxChunkSize">the size of the substring</param>
        /// <returns>the collection of splitted substring</returns>
        private static IEnumerable<string> ChunksUpto(string str, int maxChunkSize)
        {
            for (int i = 0; i < str.Length; i += maxChunkSize)
                yield return str.Substring(i, Math.Min(maxChunkSize, str.Length - i)).PadRight(maxChunkSize);
        }
    }
}