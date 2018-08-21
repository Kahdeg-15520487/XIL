using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using XIL.LangDef;
using XIL.VM;

namespace testconsole
{
    public class DiagnosticInstruction : IInstructionImplementation
    {
        const string lib = "diagnostic";

        Stopwatch stopwatch = new Stopwatch();

        /// <summary>
        /// 
        /// </summary>
        [Instruction(0x40, "startclock", lib)]
        public void StartClock(Thread thread, int op1, int op2)
        {
            stopwatch.Reset();
            stopwatch.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        [Instruction(0x41, "stopclock", lib)]
        public void StopClock(Thread thread, int op1, int op2)
        {
            stopwatch.Stop();
            int result = stopwatch.Elapsed.Milliseconds;
            thread.Push(result);
        }
    }
}
