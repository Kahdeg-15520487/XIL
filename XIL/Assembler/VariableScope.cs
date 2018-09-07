using System;
using System.Collections.Generic;
using System.Text;

namespace XIL.Assembler {
	internal class VariableScope {
		public Dictionary<string, int> vars;
		public int CurrentStackslot { get; private set; }

		public int this[string index] {
			get { return vars[index]; }
		}

		public VariableScope childScope;
		public void AddChildScope(VariableScope child) {
			childScope = child;
		}

		public VariableScope() {
			vars = new Dictionary<string, int>();
			CurrentStackslot = 0;
		}

		public void AddSymbol(string symbol) {
			vars.Add(symbol, CurrentStackslot);
		}
	}
}
