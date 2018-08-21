namespace XIL.Assembler
{
    internal class Token
    {
        public TokenType tokenType { get; set; }
        public string lexeme { get; set; }

        public Token(TokenType token_type, string lexeme = null)
        {
            tokenType = token_type;
            this.lexeme = lexeme;
        }

        public override string ToString()
        {
            return string.Format("{0} : {1}", tokenType, lexeme);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 91;
                hash = hash * 71 + lexeme.GetHashCode();
                hash = hash * 71 + tokenType.GetHashCode();
                return hash;
            }
        }
    }
}