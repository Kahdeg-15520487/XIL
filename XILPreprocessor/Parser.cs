using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XIL.Assembler.Preprocessor.AST;

namespace XIL.Assembler.Preprocessor
{
    class Parser
    {
        Lexer lexer;

        Token current_token;

        public Parser(Lexer l)
        {
            lexer = l;
            current_token = lexer.GetNextToken();
        }

        void Error()
        {
            throw new Exception("Invalid syntax: " + current_token);
        }

        void Eat(TokenType t)
        {
            if (current_token.type == t)
            {
                current_token = lexer.GetNextToken();
            }
            else
            {
                Error();
            }
        }

        /// <summary>
        /// factor : INTERGER | IDENT | LPAREN EXPR RPAREN
        /// </summary>
        /// <returns></returns>
        ASTNode Factor()
        {
            var token = current_token;

            switch (token.type)
            {
                case TokenType.INTERGER:
                    Eat(TokenType.INTERGER);
                    return new Operand(token);
                case TokenType.IDENT:
                    Eat(TokenType.IDENT);
                    return new Operand(token);
                case TokenType.LPAREN:
                    Eat(TokenType.LPAREN);
                    var node = PrecendenceLevel3();
                    Eat(TokenType.RPAREN);
                    return node;
            }

            Error();
            return null;
        }

        /// <summary>
        /// precendencelevel1 : factor (EXP factor) *
        /// </summary>
        /// <returns></returns>
        ASTNode PrecendenceLevel1()
        {
            var node = Factor();

            while (current_token.type == TokenType.EXPONENT)
            {
                var token = current_token;
                switch (token.type)
                {
                    case TokenType.EXPONENT:
                        Eat(TokenType.EXPONENT);
                        break;
                }

                node = new BinaryOperation(node, token, Factor());
            }

            return node;
        }

        /// <summary>
        /// precendencelevel2 : precendencelevel1 ((MUL | DIV) precendencelevel1) *
        /// </summary>
        /// <returns></returns>
        ASTNode PrecendenceLevel2()
        {
            var node = PrecendenceLevel1();

            while (current_token.type == TokenType.MULTIPLY ||
                    current_token.type == TokenType.DIVIDE)
            {
                var token = current_token;
                switch (token.type)
                {
                    case TokenType.MULTIPLY:
                        Eat(TokenType.MULTIPLY);
                        break;
                    case TokenType.DIVIDE:
                        Eat(TokenType.DIVIDE);
                        break;
                }

                node = new BinaryOperation(node, token, Factor());
            }

            return node;
        }

        /// <summary>
        /// precendencelevel3 : precendencelevel1 ((PLUS | MINUS) precendencelevel1) *
        /// </summary>
        /// <returns></returns>
        ASTNode PrecendenceLevel3()
        {
            var node = PrecendenceLevel2();

            while (current_token.type == TokenType.PLUS ||
                    current_token.type == TokenType.MINUS)
            {
                var token = current_token;
                switch (token.type)
                {
                    case TokenType.PLUS:
                        Eat(TokenType.PLUS);
                        break;
                    case TokenType.MINUS:
                        Eat(TokenType.MINUS);
                        break;
                }
                node = new BinaryOperation(node, token, PrecendenceLevel2());
            }

            return node;
        }

        /// <summary>
        /// assignment : IDENT ASSIGN expr
        /// </summary>
        /// <returns></returns>
        ASTNode Assignment()
        {
            var token = current_token;
            Eat(TokenType.IDENT);
            var ident = new Operand(token);
            Eat(TokenType.ASSIGN);
            var expr = PrecendenceLevel3();
            return new Assignment(ident, expr);
        }

        /// <summary>
        /// expression : precendencelevel3 | assignment
        /// </summary>
        /// <returns></returns>
        ASTNode Expression()
        {
            if (lexer.PeekNextToken().type == TokenType.ASSIGN)
            {
                return Assignment();
            }
            else
            {
                return PrecendenceLevel3();
            }
        }

        public ASTNode Parse()
        {
            return Expression();
        }
    }
}
