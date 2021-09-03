using System;
using System.Collections.Generic;
using System.Text;

using XIL.LangDef;
using XIL.VM;

namespace XIL.StandardLibrary
{
    public class StringInstruction : IInstructionImplementation
    {
        const string lib = "str";

        /// <summary>
        /// pushs &lt;string literal&gt; <para/>
        /// push a string literal onto the stack
        /// </summary>
        [Instruction(0x70, "pushs", lib)]
        public void PushString(Thread thread, int operand1, int operand2)
        {
            Util.PushStringToStack(thread, thread.GetString(operand1));
        }

        /// <summary>
        /// pops <para/>
        /// pop a string off of the stack
        /// </summary>
        [Instruction(0x71, "pops", lib)]
        public void PopString(Thread thread, int operand1, int operand2)
        {
            Util.PopStringFromStack(thread);
        }

        /// <summary>
        /// concat <para/>
        /// concatenate 2 string on the stack
        /// </summary>
        [Instruction(0x72, "concat", lib)]
        public void ConcatString(Thread thread, int operand1, int operand2)
        {
            string s2 = Util.PopStringFromStack(thread);
            string s1 = Util.PopStringFromStack(thread);
            Util.PushStringToStack(thread, s1 + s2);
        }

        /// <summary>
        /// substr <para/>
        /// substring with startIndex and length on the stack
        /// </summary>
        [Instruction(0x73, "substr", lib)]
        public void SubString(Thread thread, int oprand1, int operand2)
        {
            int length = thread.Pop();
            int startIndex = thread.Pop();
            string s = Util.PopStringFromStack(thread);
            Util.PushStringToStack(thread, s);
            Util.PushStringToStack(thread, s.Substring(startIndex, length));
        }


        /// <summary>
        /// cmpstr &lt;string literal&gt; <para/>
        /// compare string on stack with string literal
        /// </summary>
        [Instruction(0x84, "cmpstr", lib)]
        public void CompareString(Thread thread, int op1, int op2)
        {
            string s1 = Util.PopStringFromStack(thread);
            Util.PushStringToStack(thread, s1);
            string s2 = thread.GetString(op1);
            thread.Push(s1.Equals(s2, StringComparison.InvariantCulture) ? 1 : 0);
        }
    }
}
