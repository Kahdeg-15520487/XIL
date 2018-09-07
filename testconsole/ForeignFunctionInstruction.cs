using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XIL.LangDef;
using XIL.VM;


namespace testconsole
{
    public class ForeignFunctionInstruction : IInstructionImplementation
    {
        const string lib = "ffi";

        public static Dictionary<string, Action<Thread>> ForeignFunctionMap = null;

        public static void InitForeignFunctionMap(params (string name, Action<Thread> func)[] foreignfuncs)
        {
            ForeignFunctionMap = new Dictionary<string, Action<Thread>>();
            foreach (var (name, func) in foreignfuncs)
            {
                ForeignFunctionMap.Add(name, func);
            }
        }
        public static void AddForeignFunction(string name, Action<Thread> func)
        {
            ForeignFunctionMap.Add(name, func);
        }

        /// <summary>
		/// host &lt;foreign function name&gt; <para/>
		/// call the host function
		/// </summary>
		[Instruction(0x50, "host", lib)]
        public void HostCall(Thread thread, int operand1, int operand2)
        {
            if (ForeignFunctionMap is null)
            {
                ForeignFunctionMap = new Dictionary<string, Action<Thread>>();
            }

            var hostFuncName = thread.GetString(operand1);

            if (!ForeignFunctionMap.ContainsKey(hostFuncName))
            {
                //todo throw host function unknown
                Console.WriteLine("unknown host function \"{0}\"", hostFuncName);
                Console.WriteLine("execution terminated");
                thread.EndExecution();
                return;
            }
            ForeignFunctionMap[hostFuncName].Invoke(thread);
        }
    }
}
