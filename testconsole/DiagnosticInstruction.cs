using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using XIL.LangDef;
using XIL.VM;

namespace testconsole {
	public class DiagnosticInstruction : IInstructionImplementation {
		Stopwatch stopwatch = new Stopwatch();

		/// <summary>
		/// 
		/// </summary>
		[Instruction(0x40, "startclock")]
		public void StartClock(Thread thread, int op1, int op2) {
			stopwatch.Reset();
			stopwatch.Start();
		}

		/// <summary>
		/// 
		/// </summary>
		[Instruction(0x41, "stopclock")]
		public void StopClock(Thread thread, int op1, int op2) {
			stopwatch.Stop();
			int result = stopwatch.Elapsed.Milliseconds;
			thread.Push(result);
		}
	}
}
