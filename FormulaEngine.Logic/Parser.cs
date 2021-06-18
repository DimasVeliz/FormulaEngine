
using System;
using System.Linq;

namespace FormulaEngine.Logic
{
    /// <summary>
    /// Implements the following Production Rules
    ///      EXPRESSION: TERM [('+'|'-') TERM]*
    ///            TERM: FACTOR [('*'|'/') FACTOR]*
    ///          FACTOR: '-'? EXPONENT
    ///        EXPONENT: FACTORIAL_FACTOR [ '^' EXPONENT]*
    ///FACTORIAL_FACTOR: PRIMARY '!'?
    ///         PRIMARY 
    ///                : IDENTIFIER 
    ///                | SUB_EXPRESSION 
    ///                | NUMBER
    ///      IDENTIFIER: VARIABLE | FUNCTION
    ///        FUNCTION: FUNCTION_NAME '( FUNC_ARGS ')'
    ///       FUNC_ARGS: EXPRESSION [, EXPRESION ]*
    ///  SUB_EXPRESSION: '(' EXPRESSION ')'
    /// </summary>
    public class Parser
    {
        static readonly TokenType[] TERM_OPERATORS = new TokenType[] { TokenType.Addition, TokenType.Minus };
        static readonly TokenType[] FACTOR_OPERATORS = new TokenType[] { TokenType.Multiplication, TokenType.Division };

        private readonly Lexer lexer;
        private readonly SymbolTable _symbolTable;

        public Parser(Lexer lexer, SymbolTable symbolTable)
        {
            this.lexer = lexer;
            this._symbolTable = symbolTable;
        }

        public ASTNode Parse()
        {
            ASTNode node;


            if (TryParseExpression(out node))
            {
                Expect(TokenType.EOE);
                return node;
            }
            else
            {
                throw new Exception("Unable to parse the expression");
            }
        }

        private bool TryParseExpression(out ASTNode node)
        {
            if (TryParseTerm(out node))
            {
                while (isNext(TERM_OPERATORS))
                {
                    var op = Accept();

                    if (TryParseTerm(out var rightSide))
                    {
                        node = CreateBinaryOperator(op, node, rightSide);

                    }
                    else
                    {

                        throw new Exception($"Failed to parse rule Term at position {lexer.Position}");
                    }
                }
            }




            return node != null;
        }

        ///TERM: FACTOR [('*'|'/') FACTOR]*
        private bool TryParseTerm(out ASTNode node)
        {
            if (TryParseFactor(out node))
            {
                while (isNext(FACTOR_OPERATORS))
                {
                    var op = Accept();

                    if (TryParseExponent(out var rightSide))
                    {
                        node = CreateBinaryOperator(op, node, rightSide);

                    }
                    else
                    {

                        throw new Exception($"Failed to parse rule Term at position {lexer.Position}");
                    }
                }
            }




            return node != null;
        }


        ///   FACTOR: '-'? EXPONENT
        private bool TryParseFactor(out ASTNode node)
        {
            node = null;
            if (isNext(TokenType.Minus))
            {

                var op = Accept();
                if (TryParseExponent(out node))
                {
                    node = new NegationUnaryOperatorASTNode(op, node);
                }
                else
                {
                    throw new Exception($"Unable to parse the factor rule at position{lexer.Position}");

                }
            }
            else
            {
                TryParseExponent(out node);
            }
            return node != null;
        }
        ///   EXPONENT: FACTORIAL_FACTOR [ '^' EXPONENT]*
        private bool TryParseExponent(out ASTNode node)
        {
            if (TryParseFactorialFactor(out node))
            {
                if (isNext(TokenType.Exponent))
                {
                    var op = Accept(); //consuming the ^ to call the factory later

                    if (TryParseExponent(out var rightSide))
                    {

                        node = new ExponentBinaryOperatorASTNode(op, node, rightSide);
                    }
                }
            }

            return node != null;
        }

        ///     FACTORIAL_FACTOR: PRIMARY '!'?
        private bool TryParseFactorialFactor(out ASTNode node)
        {
            if (TryParsePrimary(out node))
            {
                if (isNext(TokenType.Factorial))
                {
                    node = new FactorialUnaryOperatorASTNode(Accept(), node);
                }
            }

            return node != null;
        }

        ///         PRIMARY 
        ///                : IDENTIFIER 
        ///                | SUB_EXPRESSION 
        ///                | NUMBER
        private bool TryParsePrimary(out ASTNode node)
        {
            node = null;

            if (!TryParseIdentifier(out node))
                if (!TryParseNumber(out node))
                    if (!TryParseSubExpression(out node))
                        throw new Exception($"Invalid Expression, Expected number or open paren at {lexer.Position}");

            return node != null;
        }

        ///      IDENTIFIER
        ///                : VARIABLE 
        ///                | FUNCTION
        ///        FUNCTION: FUNCTION_NAME '( FUNC_ARGS ')'
        ///       FUNC_ARGS: EXPRESSION [, EXPRESION ]*
        private bool TryParseIdentifier(out ASTNode node)
        {
            node = null;
            if (isNext(TokenType.Identifier))
            {
                var token = Accept();
                var entry = _symbolTable.Get(token.Value);
                if (entry == null)
                {
                    throw new Exception($"Undefined Identifier {token.Value} at position: {token.Position}");

                }
                if (entry.Type == EntryType.Variable)
                {
                    node = new VariableIdentifierASTNode(token);
                }

                else
                {
                    //TODO: is a function
                }
            }
            return node != null;
        }

        ///NUMBER: [0-9]+
        private bool TryParseNumber(out ASTNode node)
        {
            node = null;
            if (isNext(TokenType.Number))
            {
                node = new NumberASTNode(Accept());
            }

            return node != null;

        }

        private bool TryParseSubExpression(out ASTNode node)
        {
            node = null;
            if (isNext(TokenType.OpenParen))
            {
                Accept(); //consumes the open parent ( 

                if (TryParseExpression(out node))
                {

                    Expect(TokenType.CloseParen);
                    Accept(); //consumes the close parent )
                }
            }
            return node != null;

        }

        private bool isNext(params TokenType[] possibleTokens)
        {
            return isNext(x => possibleTokens.Any(pT => pT == x));
        }
        private bool isNext(Predicate<TokenType> match) => match(lexer.Peek().Type);
        private Token Accept() => lexer.ReadNext();

        private ASTNode CreateBinaryOperator(Token op, ASTNode left, ASTNode right)
        {
            return FunctionFactory.Operations[op.Type](op, left, right);
        }
        private void Expect(TokenType expected)
        {
            if (!isNext(expected))
            {
                throw new System.Exception($"Unxpected: {lexer.Peek()} at Position {lexer.Position}");
            }
        }
    }
}