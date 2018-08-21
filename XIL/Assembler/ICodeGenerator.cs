using System;
using System.Collections.Generic;
using System.Text;
using XIL.LangDef;

namespace XIL.Assembler
{
    /// <summary>
    /// ICodeGenerator interface
    /// </summary>
    public interface ICodeGenerator
    {
        /// <summary>
        /// add a library metadata
        /// </summary>
        /// <param name="lib"></param>
        int AddLibrary(string lib);
        /// <summary>
        /// get a library index
        /// </summary>
        /// <param name="lib"></param>
        /// <returns></returns>
        int GetLibrary(string lib);
        /// <summary>
        /// add an instruction
        /// </summary>
        /// <param name="instruction"></param>
        void AddInstruction(Instruction instruction);
        /// <summary>
        /// add an instruction
        /// </summary>
        /// <param name="op"></param>
        /// <param name="op1"></param>
        /// <param name="op2"></param>
        /// <param name="lnb"></param>
        void AddInstruction(int op, int op1, int op2, int lnb);
        /// <summary>
        /// add a string constant <para/>
        /// return the index of the added string constant
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        int AddString(string s);
        /// <summary>
        /// retrieve a string constant <para/>
        /// return -1 if string constant is not exist
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        int GetString(string s);
        /// <summary>
        /// add a jump label
        /// </summary>
        /// <param name="label"></param>
        /// <param name="line"></param>
        void AddJumpLabel(string label, int line);
        /// <summary>
        /// get a jump label's target
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        int GetJumpLabel(string s);

        /// <summary>
        /// emit the program
        /// </summary>
        /// <returns></returns>
        VM.Program Emit();
    }
}
