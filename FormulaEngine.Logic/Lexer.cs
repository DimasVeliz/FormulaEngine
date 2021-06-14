using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FormulaEngine.Logic
{
    //this class implements the following matching rules
    // OPERATOR L + | - | * | /
    // NUMBER: [0-9]+

    public class Lexer
    {
        const char PLUS = '+';
        const char MINUS = '-';
        const char MULTIPLICATION = '*';
        const char DIVISION = '/';
        const char DECIMAL_SEPARATORS = '.';

        static readonly char[] E_NOTATION = new char[] { 'e', 'E' };
        static readonly char[] SIGN_OPERATORS = new char[] { PLUS, MINUS };

        public int Position => _scanner.Position;
        public static Dictionary<char, Func<int, char, Token>> OperatorMap = new Dictionary<char, Func<int, char, Token>>()
        {
            {PLUS,(p,v)=>new Token(TokenType.Addition,p,v.ToString())},
            {MINUS,(p,v)=>new Token(TokenType.Substraction,p,v.ToString())},
            {MULTIPLICATION,(p,v)=>new Token(TokenType.Multiplication,p,v.ToString())},
            {DIVISION,(p,v)=>new Token(TokenType.Division,p,v.ToString())},
            {DECIMAL_SEPARATORS,(p,v)=>new Token(TokenType.Decimal_Separator,p,v.ToString())},


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
                token = OperatorMap[lookahead.Value](_scanner.Position, _scanner.Read().Value);
            }
            return token != null;

        }

        /// <summary>
        /// example: 1 110 1.5 .7 1e5 2e-7 
        /// regex : \d+ .? \d+ ([eE] [+ -]? \d+)?
        ///  
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private bool TryTokenizeNumber(out Token token)
        {
            token = null;
            var sb = new StringBuilder();
            var position = Position;

            sb.Append(ReadDigits());  // \d+ 

            if (isNext(DECIMAL_SEPARATORS))
            {
                sb.Append(Accept());  //.? 
            }

            sb.Append(ReadDigits());   // \d+ 


            if (sb.Length!=0 && char.IsDigit(sb[sb.Length-1])&& isNext(E_NOTATION))
            {
                sb.Append(Accept()); //[eE]

                if (isNext(SIGN_OPERATORS))
                {
                    sb.Append(Accept());  // [+ -]? 
                }

                Expect(char.IsDigit);

                sb.Append(ReadDigits()); // \d+
            }


            if (sb.Length > 0)
            {
                token = new Token(TokenType.Number, position, sb.ToString());
            }
            if (token != null && !double.TryParse(token.Value, out _))
            {
                throw new Exception($"Invalid numeric value/format found at position{token.Position}");
            }
            return token != null;
        }

        private void Expect(Func<char, bool> match)
        {
            if (!isNext(match))
            {
                throw new Exception($"Unexpected value at position {Position}");
            };
        }

        private string ReadDigits()
        {
            var sb = new StringBuilder();
            while (isNext(char.IsDigit))
            {
                sb.Append(Accept());
            }

            return sb.ToString();
        }

        private char Accept() => _scanner.Read().Value;

        private bool isNext(params char[] possibleValues)
        {
            return isNext(x => possibleValues.Any(pV => pV == x));
        }
        private bool isNext(Func<char, bool> match)
        {
            var lookahead = _scanner.Peek();
            return lookahead.HasValue && match(lookahead.Value);
        }

        private void ConsumeWhiteSpace()
        {
            while (isNext(char.IsWhiteSpace))
                Accept();
        }
    }
}