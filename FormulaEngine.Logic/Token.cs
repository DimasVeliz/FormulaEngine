using System;
using System.Collections.Generic;

namespace FormulaEngine.Logic
{
    public enum TokenType
    {

        //end of file
        EOF,

        //end of line
        New_Line,

        //keywords
        Let,
        Set,
        Def,
        Eval,
        Print,



        //operations
        Addition,
        Minus,
        Multiplication,
        Division,
        Factorial,
        Exponent,
        Goes_To,


        //Grouping and other symbols
        Decimal_Separator,
        OpenParen,
        CloseParen,
        Arg_Separator,


        //number or identifier tokens
        Number,
        Identifier,
    }

    public class Token
    {
        public int LinePosition { get; }
        public int LineNumber { get; }
        public TokenType Type { get; }
        public string Value { get; }

        public Token(TokenType type, int linePosition, int lineNumber, string value)
        {
            Type = type;
            LinePosition = linePosition;
            LineNumber = lineNumber;
            Value = value;
        }
    }
}