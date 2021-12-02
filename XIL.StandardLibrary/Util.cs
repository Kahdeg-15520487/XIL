using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

using XIL.VM;

namespace XIL.StandardLibrary
{
    public static class Util
    {
        /// <summary>
        /// Push an object to stack
        /// </summary>
        /// <param name="thread">thread.</param>
        /// <param name="obj">obj to be pushed.</param>
        public static void PushObject(Thread thread, Dictionary<string, string> obj)
        {
            Util.PushStringToStack(thread, JsonSerializer.Serialize(obj));
        }

        /// <summary>
        /// pop an object off stack
        /// </summary>
        /// <param name="thread">thread.</param>
        public static Dictionary<string, string> PopObject(Thread thread)
        {
            string s = Util.PopStringFromStack(thread);
            Dictionary<string, string> result = new Dictionary<string, string>(JsonSerializer.Deserialize<Dictionary<string, object>>(s).Select(kvp => new KeyValuePair<string, string>(kvp.Key, kvp.Value.ToString())));
            if (result == null)
            {
                Util.PushStringToStack(thread, s);
                thread.RuntimeError("there is no object to pop");
            }
            return result;
        }

        /// <summary>
        /// Push string to stack
        /// </summary>
        /// <param name="thread">thread.</param>
        /// <param name="s">string to be pushed.</param>
        public static void PushStringToStack(Thread thread, string s)
        {
            var result = EncodeString(s);
            thread.PushArray(result);
            thread.Push(result.GetLength(0));
        }

        /// <summary>
        /// Pop string from stack
        /// </summary>
        /// <param name="thread">thread.</param>
        /// <returns>poped string.</returns>
        public static string PopStringFromStack(Thread thread)
        {
            int arraySize = thread.Pop();
            //int startIndex = thread.StackTopIndex - (arraySize - 1);
            int startIndex = thread.StackTopIndex - arraySize;
            var ttt = thread.PopArray(startIndex, arraySize);
            return DecodeString(ttt);
        }

        /// <summary>
        /// decode a int array to string
        /// </summary>
        /// <param name="a">the array to be decode</param>
        /// <returns>decoded string</returns>
        public static string DecodeString(int[] a)
        {
            StringBuilder result = new StringBuilder();
            int length = a[0];
            foreach (var b in a.Skip(1))
            {
                result.Append(Encoding.ASCII.GetString(BitConverter.GetBytes(b)));
            }
            return result.ToString().Substring(0, length);
        }

        /// <summary>
        /// encode a string to its int-encoded array
        /// </summary>
        /// <param name="s">string to be encoded</param>
        /// <returns>encoded array</returns>
        public static int[] EncodeString(string s)
        {
            var strs = ChunksUpto(s, 4).ToArray();
            int[] result = new int[strs.GetLength(0) + 1];
            int index = 1;
            result[0] = s.Length;
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
