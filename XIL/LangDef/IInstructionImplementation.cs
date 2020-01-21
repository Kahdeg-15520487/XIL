using System;

namespace XIL.LangDef
{
    /// <summary>
    /// 
    /// </summary>
	[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class InstructionAttribute : Attribute
    {
        readonly int opcode;
        /// <summary>
        /// operation code
        /// </summary>
		public int OpCode {
            get { return opcode; }
        }

        readonly string opname;
        /// <summary>
        /// operation code's mnemonic
        /// </summary>
		public string OpName {
            get { return opname; }
        }

        readonly string library;
        /// <summary>
        /// the library that this is instruction belong to
        /// </summary>
        public string Library {
            get { return library; }
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="opname"></param>
        /// <param name="lib"></param>
        public InstructionAttribute(int opcode, string opname = "", string lib = null) {
            this.opcode = opcode;
            this.opname = opname;
            this.library = lib;
        }
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="opname"></param>
		public InstructionAttribute(InstructionOPCode opcode, string opname = "") {
            this.opcode = (int)opcode;
            this.opname = opname;
            this.library = "core";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            return string.Format("0x{0:X} : {1}", opcode, opname);
        }
    }

    //marker interface
    /// <summary>
    /// IInstructionImplementation interface
    /// </summary>
    public interface IInstructionImplementation { }
}