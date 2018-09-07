using System;
using System.Collections.Generic;
using System.Text;

namespace XIL.Assembler
{
    /// <summary>
    /// result for a compilation
    /// </summary>
    public struct CompileResult
    {
        /// <summary>
        /// success?
        /// </summary>
        public bool Success;
        /// <summary>
        /// error message
        /// </summary>
        public string Message;
        /// <summary>
        /// the code generator, to be replaced with an interface
        /// </summary>
        public ICodeGenerator CodeGenerator;
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="success"></param>
        /// <param name="msg"></param>
        /// <param name="codegen"></param>
        public CompileResult(bool success, string msg, ICodeGenerator codegen)
        {
            Success = success;
            Message = msg;
            CodeGenerator = codegen;
        }
    }
}
