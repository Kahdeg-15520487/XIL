using System.Collections.Generic;

using XIL.LangDef;
using XIL.VM;

namespace XIL.StandardLibrary
{
    public class ObjectInstruction : IInstructionImplementation
    {
        const string lib = "obj";

        /// <summary>
        /// pusho <para/>
        /// push an empty object on stack
        /// </summary>
        [Instruction(0x90, "pusho", lib)]
        public void PushObject(Thread thread, int op1, int op2)
        {
            Util.PushObject(thread, new Dictionary<string, string>());
        }

        /// <summary>
        /// pusho <para/>
        /// push an empty object on stack
        /// </summary>
        [Instruction(0x91, "popo", lib)]
        public void PopObject(Thread thread, int op1, int op2)
        {
            Util.PopObject(thread);
        }

        /// <summary>
        /// setobj <para/>
        /// set a field of an object
        /// </summary>
        [Instruction(0x92, "setobj", lib)]
        public void SetObjectField(Thread thread, int op1, int op2)
        {
            string fieldValue = Util.PopStringFromStack(thread);
            string fieldName = Util.PopStringFromStack(thread);
            Dictionary<string, string> obj = Util.PopObject(thread);
            if (obj.ContainsKey(fieldName))
            {
                obj[fieldName] = fieldValue;
            }
            else
            {
                obj.Add(fieldName, fieldValue);
            }
            Util.PushObject(thread, obj);
        }

        /// <summary>
        /// getobj <para/>
        /// Get a field of an object
        /// </summary>
        [Instruction(0x93, "getobj", lib)]
        public void GetObjectField(Thread thread, int op1, int op2)
        {
            string fieldName = Util.PopStringFromStack(thread);
            string fieldValue = string.Empty;
            Dictionary<string, string> obj = Util.PopObject(thread);
            Util.PushObject(thread, obj);
            if (obj.ContainsKey(fieldName))
            {
                fieldValue = obj[fieldName];
            }
            else
            {
                thread.RuntimeError($"object doesnt contain field \"{fieldName}\"");
            }
            Util.PushStringToStack(thread, fieldValue);
        }
    }
}
