using System;
using System.Collections.Generic;
using System.Text;

using XIL.LangDef;
using XIL.VM;

namespace testconsole {
	class RudeAssembler {
		public int[] Assemble(List<Instruction> prog) {
			List<int> result = new List<int>();
			Dictionary<string, int> labels = new Dictionary<string, int>();

			foreach (var instr in prog) {
				result.Add(instr.opCode);
			}

			return result.ToArray();
		}
	}
}
