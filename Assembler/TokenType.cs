namespace XIL.Assembler {
	public enum TokenType {
		INT,    //signed int
		STRING, //will get converted to intarr string
		BOOL,   //true = 1, false = 0

		VAR,  //variable
		LABEL,	//label
		IDENT,  //an instruction

		NEWLINE,//

		INVALID,//invalid token
		EOF
	}
}