using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XIL.LangDef;

namespace XIL.Assembler
{
    internal class Lexer
    {
        public string source_code { get; private set; }
        public int pos { get; private set; }
        public char current_char { get; private set; }
        public int current_line { get; private set; }

        public Lexer(string source_code)
        {
            this.source_code = source_code;
            Reset();
        }

        public Lexer(Lexer other)
        {
            source_code = other.source_code;
            source_code += '\n';
            Reset();
        }

        void Error()
        {
            throw new Exception(string.Format("Error tokenzing: {0} at line {1}", current_char, current_line));
        }

        void Advance()
        {
            pos++;
            if (pos > source_code.Length - 1)
            {
                current_char = '\0';
            }
            else
            {
                current_char = source_code[pos];
            }
        }

        char Peek()
        {
            if (pos == source_code.Length - 1)
            {
                return '\0';
            }
            else
            {
                return source_code[pos + 1];
            }
        }

        void SkipWhiteSpaceAndTab()
        {
            while (current_char != '\0' && current_char.IsWhiteSpace())
            {
                Advance();
            }
        }

        void SkipComment()
        {
            while (current_char != '\0' && current_char != '\n')
            {
                Advance();
            }
        }

        Token Number()
        {
            string result = "";
            TokenType tokentype = TokenType.INT;
            if (current_char == '-')
            {
                result += '-';
                Advance();
            }
            while (current_char != '\0' && current_char.IsNumeric())
            {
                result += current_char;
                Advance();
            }
            return new Token(tokentype, result);
        }

        Token HexNumber()
        {
            string hex = "";
            TokenType tokentype = TokenType.INT;
            Advance(); //skip 0
            Advance(); //skip x
            while (current_char != '\0' && current_char.IsHexNumeric())
            {
                hex += current_char;
                Advance();
            }
            hex = Int32.Parse(hex, System.Globalization.NumberStyles.HexNumber).ToString();
            return new Token(tokentype, hex);
        }

        Token BinaryNumber()
        {
            string bin = "";
            TokenType tokentype = TokenType.INT;
            Advance(); //skip 0
            Advance(); //skip b
            while (current_char != '\0' && (current_char == '0' || current_char == '1'))
            {
                bin += current_char;
                Advance();
            }
            bin = Convert.ToInt32(bin, 2).ToString();
            return new Token(tokentype, bin);
        }

        Token Char()
        {
            Advance();
            char result = current_char;
            TokenType tokentype = TokenType.INT;
            Advance();
            if (current_char != '\'')
            {
                this.Error();
            }
            Advance();
            return new Token(tokentype, ((int)result).ToString());
        }

        Token Ident()
        {
            string result = "";
            while (current_char != '\0' && current_char.IsIdent())
            {
                result += current_char;
                Advance();
            }

            //check if Ident is a variable
            if (result.Contains('$'))
            {
                return new Token(TokenType.VAR, result);
            }

            //check if Ident is a line label
            if (result.Contains(':'))
            {
                return new Token(TokenType.LABEL, result);
            }

            //check if Ident is a bool literal
            if (bool.TryParse(result, out bool temp))
            {
                return new Token(TokenType.BOOL, result);
            }

            //then Ident must be something
            return new Token(TokenType.IDENT, result);
        }

        Token String()
        {
            string result = "";
            Advance();
            while (current_char != '\0' && current_char != '"')
            {
                result += current_char;
                Advance();
            }
            if (current_char != '"')
            {
                Error();
            }
            Advance();
            return new Token(TokenType.STRING, result);
        }

        public Token GetNextToken()
        {
            while (current_char != '\0')
            {
                if (current_char.IsWhiteSpace())
                {
                    SkipWhiteSpaceAndTab();
                    continue;
                }

                if (current_char == ';')
                {
                    SkipComment();
                    continue;
                }

                if (current_char == '0' && Peek() == 'x')
                {
                    return HexNumber();
                }

                if (current_char == '0' && Peek() == 'b')
                {
                    return BinaryNumber();
                }

                if (current_char.IsNumeric())
                {
                    return Number();
                }

                if (current_char == '-' && Peek().IsNumeric())
                {
                    return Number();
                }

                if (current_char == '\'')
                {
                    return Char();
                }

                if (current_char == '"')
                {
                    return String();
                }

                if (current_char.IsIdent())
                {
                    return Ident();
                }

                if (current_char == '\n')
                {
                    Advance();
                    current_line++;
                    return new Token(TokenType.NEWLINE, "\n");
                }

                Error();
                return new Token(TokenType.INVALID);
            }

            return new Token(TokenType.EOF);
        }

        public Token PeekNextToken()
        {
            Lexer peeker = new Lexer(this);
            return peeker.GetNextToken();
        }

        public void Reset()
        {
            pos = 0;
            current_char = source_code[pos];
            current_line = 0;
        }

        public List<Token> GetAllToken()
        {
            List<Token> result = new List<Token>();
            Token token = GetNextToken();
            while (token.tokenType != TokenType.EOF)
            {
                result.Add(token);
                token = GetNextToken();
            }
            return result;
        }
    }
}
