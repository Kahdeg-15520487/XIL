using System;
using System.Collections.Generic;
using System.Text;
using XIL.LangDef;

namespace XIL.Assembler {
	public class CodeGenerator {
		private Dictionary<string, int> labels;
		public List<Instruction> program;

		public CodeGenerator() {
			labels = new Dictionary<string, int>();
			program = new List<Instruction>();
		}

		internal void AddInstruction(Instruction instruction) {
			program.Add(instruction);
		}

		/// <summary>
		/// get a jump label's target
		/// </summary>
		internal int GetJumpTarget(string label) {
			return labels[label];
		}

		/// <summary>
		/// add a jump label
		/// </summary>
		internal void AddJumpLabel(string label, int linecount) {
			labels.Add(label, linecount);
		}
	}
}
