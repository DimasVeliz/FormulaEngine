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
        //operations
        const char PLUS = '+';
        const char MINUS = '-';
        const char MULTIPLICATION = '*';
        const char DIVISION = '/';
        const char FACTORIAL = '!';
        const char EXPONENT = '^';



        //Grouping and other symbols

        const char DECIMAL_SEPARATORS = '.';

        const char OPEN_PAREN = '(';

        const char CLOSE_PAREN = ')';

        const char ARG_SEPARATOR = ',';

        const char NEW_LINE ='\n';


        //keywords
        const string LET = "let";
        const string SET = "set";
        const string EVAL = "eval";
        const string DEF = "def";
        const string PRINT = "print";

        const string GOES_TO = "=>";



        static readonly char[] E_NOTATION = new char[] { 'e', 'E' };
        static readonly char[] SIGN_OPERATORS = new char[] { PLUS, MINUS };

        public static Dictionary<string, TokenType> KeyWordMap = new Dictionary<string, TokenType>()
        {
            {LET,TokenType.Let},
            {SET,TokenType.Set},
            {PRINT,TokenType.Print},
            {DEF,TokenType.Def},
            {EVAL,TokenType.Eval},
            {GOES_TO,TokenType.Goes_To},

        };

        public static Dictionary<char, Func<int, int, char, Token>> SimpleTokenMap = new Dictionary<char, Func<int, int, char, Token>>()
        {
            {PLUS,(pL,lN,v)=>new Token(TokenType.Addition,pL,lN,v.ToString())},
            {MINUS,(pL,lN,v)=>new Token(TokenType.Minus,pL,lN,v.ToString())},
            {MULTIPLICATION,(pL,lN,v)=>new Token(TokenType.Multiplication,pL,lN,v.ToString())},
            {DIVISION,(pL,lN,v)=>new Token(TokenType.Division,pL,lN,v.ToString())},
            {OPEN_PAREN,(pL,lN,v)=>new Token(TokenType.OpenParen,pL,lN,v.ToString())},
            {CLOSE_PAREN,(pL,lN,v)=>new Token(TokenType.CloseParen,pL,lN,v.ToString())},
            {FACTORIAL,(pL,lN,v)=>new Token(TokenType.Factorial,pL,lN,v.ToString())},
            {EXPONENT,(pL,lN,v)=>new Token(TokenType.Exponent,pL,lN,v.ToString())},
            {ARG_SEPARATOR,(pL,lN,v)=>new Token(TokenType.Arg_Separator,pL,lN,v.ToString())},

        };
        readonly SourceScanner _scanner;
        public Lexer(SourceScanner scanner)
        {
            _scanner = scanner;
        }
        
        public int LineNumber =>_scanner.LineNumber;
        public int LinePosition =>_scanner.LinePosition;

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
                return new Token(TokenType.EOF, _scanner.LinePosition, _scanner.LineNumber, null);
            ConsumeWhiteSpace();

            Token token;
            if (TryTokenizeSimpleToken(out token))
                return token;

            if (TryTokenizeNumber(out token))
                return token;

            if (TryTokenizeIdentifier(out token))
                return token;
            if (TryParseGoesTo(out token))
            {
                return token;
            }

            throw new Exception($"Unexpected character{_scanner.Peek() } found at line: {_scanner.LineNumber}, col: {_scanner.LinePosition}");
        }

        private bool TryParseGoesTo(out Token token)
        {
            token = null;

            if (isNext(x => x == '='))
            {
                Accept();
                Expect(x => x == '>');
                Accept();
                token = new Token(TokenType.Goes_To, _scanner.LinePosition - 2, _scanner.LineNumber, "=>");
            }
            return token != null;
        }

        private bool TryTokenizeSimpleToken(out Token token)
        {
            token = null;

            if (isNext(SimpleTokenMap.ContainsKey))
            {
                var lPosition = _scanner.LinePosition;
                var lNumber = _scanner.LineNumber;

                var op = Accept();
                token = SimpleTokenMap[op](lPosition, lNumber, op);
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

            var lPosition = _scanner.LinePosition;
            var lNumber = _scanner.LineNumber;

            sb.Append(ReadDigits());  // \d+ 

            if (isNext(DECIMAL_SEPARATORS))
            {
                sb.Append(Accept());  //.? 
            }

            sb.Append(ReadDigits());   // \d+ 


            if (sb.Length != 0 && char.IsDigit(sb[sb.Length - 1]) && isNext(E_NOTATION))
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
                token = new Token(TokenType.Number, lPosition, lNumber, sb.ToString());
            }
            if (token != null && !double.TryParse(token.Value, out _))
            {
                throw new Exception($"Invalid numeric value/format found at line {token.LineNumber}, col {token.LinePosition}");
            }
            return token != null;
        }

        private void Expect(Func<char, bool> match)
        {
            if (!isNext(match))
            {
                throw new Exception($"Unexpected value at at line {_scanner.LineNumber}, col {_scanner.LinePosition}");
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


        /// identifiers => _? [a-zA-Z][a-zA-Z0-9_]*
        private bool TryTokenizeIdentifier(out Token token)
        {
            token = null;
            var sb = new StringBuilder();

            if (isNext('_'))
            {
                sb.Append(Accept());
                Expect(char.IsLetter);
            }
            if (isNext(char.IsLetter))
            {
                sb.Append(Accept());
                while (isNext(x => (char.IsLetterOrDigit(x)) || x == '_'))
                {
                    sb.Append(Accept());
                }
            }

            if (sb.Length > 0)
            {
                var lPosition = _scanner.LinePosition;
                var lNumber = _scanner.LineNumber;
                var value = sb.ToString();

                if (KeyWordMap.ContainsKey(value.ToLower()))
                {
                    token = new Token(KeyWordMap[value.ToLower()], lPosition, lNumber, value);

                }
                else
                    token = new Token(TokenType.Identifier, lPosition, lNumber, value);
            }


            return token != null;
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
            while (isNext(char.IsWhiteSpace)&&!isNext(NEW_LINE))
                Accept();
        }
    }
}