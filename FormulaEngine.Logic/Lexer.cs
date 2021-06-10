using System;
using System.Collections.Generic;
using System.Text;

namespace FormulaEngine.Logic
{
    //this class implements the following matching rules
    // OPERATOR L + | - | * | /
    // NUMBER: [0-9]+

    public class Lexer
    {
        public static Dictionary<char, Func<int, char, Token>> OperatorMap = new Dictionary<char, Func<int, char, Token>>()
        {
            {'+',(p,v)=>new Token(TokenType.Addition,p,v.ToString())},
            {'-',(p,v)=>new Token(TokenType.Substraction,p,v.ToString())},
            {'*',(p,v)=>new Token(TokenType.Multiplication,p,v.ToString())},
            {'/',(p,v)=>new Token(TokenType.Division,p,v.ToString())},

        };
        readonly SourceScanner _scanner;
        public Lexer(SourceScanner scanner)
        {
            _scanner = scanner;
        }

        public Token Peek()
        {
            _scanner.Push();
            var token = ReadNext();
            _scanner.Pop();
            return token;
        }

        public Token ReadNext()
        {
            if (_scanner.EndOfSource)
                return new Token(TokenType.EOE, _scanner.Position, null);
            ConsumeWhiteSpace();

            Token token;
            if (TryTokenizeOperator(out token))
                return token;

            if (TryTokenizeNumber(out token))
                return token;

            throw new Exception($"Unexpected character{_scanner.Peek() } found at possition {_scanner.Position}");
        }
        private bool TryTokenizeOperator(out Token token)
        {
            token = null;

            var lookahead = _scanner.Peek();
            if (lookahead.HasValue && OperatorMap.ContainsKey(lookahead.Value))
            {
                token= OperatorMap[lookahead.Value](_scanner.Position,_scanner.Read().Value);
            }
            return token != null;

        }
        private bool TryTokenizeNumber(out Token token)
        {
            token = null;

            if (IsDigit(_scanner.Peek()))
            {
                var position = _scanner.Position;
                var sBuilder= new StringBuilder();
                while (IsDigit(_scanner.Peek()))
                {
                    sBuilder.Append(_scanner.Read().Value);
                }
                token = new Token(TokenType.Number,position,sBuilder.ToString());
            }

            return token != null;
        }
        bool IsWhiteSpace(char? c) => c.HasValue && char.IsWhiteSpace(c.Value);
        bool IsDigit(char? c) => c.HasValue && char.IsDigit(c.Value);


        private void ConsumeWhiteSpace()
        {
            while (IsWhiteSpace(_scanner.Peek()))
            {
                _scanner.Read();
            }
        }
    }
}