namespace XIL.Assembler
{
    internal enum TokenType
    {
        /// <summary>
        /// signed int
        /// </summary>
		INT,
        /// <summary>
        /// will get converted to intarr string
        /// </summary>
        STRING,
        /// <summary>
        /// true = 1, false = 0
        /// </summary>
        BOOL,

        /// <summary>
        /// variable
        /// </summary>
        VAR,
        /// <summary>
        /// label
        /// </summary>
        LABEL,
        /// <summary>
        /// an instruction
        /// </summary>
        IDENT,

        /// <summary>
        /// newline
        /// </summary>
        NEWLINE,

        /// <summary>
        /// invalid token
        /// </summary>
        INVALID,

        /// <summary>
        /// end of file
        /// </summary>
        EOF,

        /// <summary>
        /// match any token
        /// </summary>
        ANY
    }
}
