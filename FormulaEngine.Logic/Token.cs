using System;
using System.Collections.Generic;

namespace FormulaEngine.Logic
{
    public enum TokenType
        {
            EOE,
            Number,
            Addition,
            Substraction,
            Multiplication,
            Division,
            Decimal_Separator
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