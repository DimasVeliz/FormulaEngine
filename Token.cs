using System;
using System.Collections.Generic;

namespace FormulaEngine
{
    public enum TokenType
        {
            EOE,
            Addition,
            Substraction,
            Multiplication,
            Division
        }

    public class Token
    {
        public int Position {get;}
        public TokenType Type {get;}
        public string Value {get;}

        public Token(TokenType type, int position, string value)
        {
            Type = type;
            Position = position;
            Value = value;
        }
    }
}