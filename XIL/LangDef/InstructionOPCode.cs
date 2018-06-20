namespace XIL.LangDef
{
    public enum InstructionOPCode
    {
        /// <summary>
        /// jump lable <para />
        /// jump to the given label
        /// </summary>
        jmp,
        /// <summary>
        /// je lable <para />
        /// jump to the given label if a = b
        /// </summary>
        je,
        /// <summary>
        /// jne lable <para />
        /// jump to the given label if a != b
        /// </summary>
        jne,
        /// <summary>
        /// jg lable <para />
        /// jump to the given label if a > b
        /// </summary>
        jg,
        /// <summary>
        /// jge lable <para />
        /// jump to the given label if a >= b
        /// </summary>
        jge,
        /// <summary>
        /// jl lable <para />
        /// jump to the given label if a &lt; b
        /// </summary>
        jl,
        /// <summary>
        /// jle lable <para />
        /// jump to the given label if a &lt;= b
        /// </summary>
        jle,
        /// <summary>
        /// j1 lable <para />
        /// jump to the given label if tots is 1
        /// </summary>
        j1,
		/// <summary>
		/// j0 lable <para />
		/// jump to the given label if tots is 0
		/// </summary>
		j0,
        /// <summary>
        /// jel lable literal <para />
        /// jump to the given label if a == literal
        /// </summary>
        jel,
        /// <summary>
        /// jnel lable literal <para />
        /// jump to the given label if a != literal
        /// </summary>
        jnel,

        /// <summary>
        /// exit <para/>
        /// stop executing, set exit code to tots and clear the stack 
        /// </summary>
        exit,
        /// <summary>
        /// pause <para/>
        /// wait for the &lt;tots&gt; ms
        /// </summary>
        pause,
        /// <summary>
        /// halt <para/>
        /// halt execution, can only be unhalt by another thread or by the vm
        /// </summary>
        halt,
        /// <summary>
        /// unhalt <para/>
        /// unhalt another thread indicate by &lt;tots&gt;
        /// </summary>
        unhalt,
        /// <summary>
        /// nop <para/>
        /// do nothing
        /// </summary>
        nop,

        /// <summary>
        /// add <para />
        /// add 2 tots and push the result
        /// </summary>
        add,
        /// <summary>
        /// sub <para />
        /// subtract 2 tots and push the result
        /// </summary>
        sub,
        /// <summary>
        /// mul <para />
        /// multiply 2 tots and push the result
        /// </summary>
        mul,
        /// <summary>
        /// div <para />
        /// divide 2 tots and push the result
        /// </summary>
        div,
        /// <summary>
        /// mod <para />
        /// modulus 2 tots and push the result
        /// </summary>
        mod,

        /// <summary>
        /// dec <para />
        /// decrement tots
        /// </summary>
        dec,
        /// <summary>
        /// inc <para />
        /// increment tots
        /// </summary>
        inc,
        /// <summary>
        /// neg <para />
        /// negate tots
        /// </summary>
        neg,

        /// <summary>
        /// cmp <para />
        /// cmp 2 tots and push 1, 0 or -1
        /// </summary>
        cmp,

        /// <summary>
        /// push &lt;var&gt; <para />
        /// push the value of &lt;var&gt; on tots
        /// </summary>
        push,
        /// <summary>
        /// yeet &lt;literal&gt; <para />
        /// yeet &lt;literal&gt; on tots
        /// </summary>
        yeet,
        /// <summary>
        /// pop &lt;var&gt; <para />
        /// pop the value of tots into &lt;var&gt;
        /// </summary>
        pop,
        /// <summary>
        /// load &lt;var&gt; &lt;literal&gt; <para />
        /// load &lt;literal&gt; into &lt;var&gt;
        /// </summary>
        load,
        /// <summary>
        /// gets <para />
        /// push the content of the value at the stack index <para/>
        /// which is on tots onto the tots
        /// </summary>
        getstack,
        /// <summary>
        /// sets <para />
        /// set the content of the value at the stack index <para/>
        /// which is on second tots to tots
        /// </summary>
        setstack,
        /// <summary>
        /// copy &lt;var1&gt; &lt;var2&gt; <para />
        /// copy value of &lt;var2&gt; into &lt;var1&gt;
        /// </summary>
        copy,
        /// <summary>
        /// dup <para/>
        /// duplicate tots
        /// </summary>
        dup,
        /// <summary>
        /// swap <para/>
        /// swap 2 tots
        /// </summary>
        swap,
        /// <summary>
        /// remove <para/>
        /// remove tots
        /// </summary>
        remove,

        /// <summary>
        /// rand <para />
        /// push a random value on tots
        /// </summary>
        rand,
		/// <summary>
		/// rand <para />
		/// push a random value on tots
		/// </summary>
		randmax,

		/// <summary>
		/// call &lt;label&gt; <para />
		/// store the current ic into return target and jump to the given label
		/// </summary>
		call,
        /// <summary>
        /// ret <para />
        /// jump to the return target
        /// </summary>
        ret,

		/// <summary>
		/// brp <para />
		/// break execution
		/// </summary>
		brp,

        /// <summary>
        /// req &lt;library's name&gt; <para />
        /// check VM for certain library
        /// </summary>
        req
    }
}