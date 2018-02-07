using System;

namespace XIL.LangDef {
	[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
	public sealed class InstructionAttribute : Attribute {
		readonly int opcode;
		public int OpCode {
			get { return opcode; }
		}

		readonly string opname;
		public string OpName {
			get { return opname; }
		}

		public InstructionAttribute(int opcode, string opname = "") {
			this.opcode = opcode;
			this.opname = opname;
		}
		public InstructionAttribute(InstructionOPCode opcode, string opname = "") {
			this.opcode = (int)opcode;
			this.opname = opname;
		}

		public override string ToString() {
			return string.Format("0x{0:X} : {1}", opcode, opname);
		}
	}

	//marker interface
	public interface IInstructionImplementation { }
}