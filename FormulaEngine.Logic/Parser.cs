
using System;
using System.Collections.Generic;
using System.Linq;

namespace FormulaEngine.Logic
{
    public static class FunctionFactory
    {
        public static Dictionary<TokenType, Func<Token, ASTNode, ASTNode, BinaryOperatorASTNode>> Operations = new Dictionary<TokenType, Func<Token, ASTNode, ASTNode, BinaryOperatorASTNode>>()
        {
            {TokenType.Addition,(t,l,r)=>new AdditionBinaryOperatorASTNode(t,l,r)},
            {TokenType.Minus,(t,l,r)=>new SubstractionBinaryOperatorASTNode(t,l,r)},
            {TokenType.Multiplication,(t,l,r)=>new MultiplicationBinaryOperatorASTNode(t,l,r)},
            {TokenType.Division,(t,l,r)=>new DivisionBinaryOperatorASTNode(t,l,r)},

        };
    }
    /// <summary>
    /// Implements the following Production Rules
    /// EXPRESSION: TERM [('+'|'-') TERM]*
    ///     TERM: FACTOR [('*'|'/') FACTOR]*
    ///     FACTOR: '-'? FACTORIAL_FACTOR
    ///     EXPONENT: 
    ///     FACTORIAL_FACTOR: PRIMARY '!'?
    ///     PRIMARY: SUB_EXPRESSION | NUMBER
    ///     SUB_EXPRESSION: '(' EXPRESSION ')'
    /// </summary>
    public class Parser
    {
        static readonly TokenType[] TERM_OPERATORS = new TokenType[] { TokenType.Addition, TokenType.Minus };
        static readonly TokenType[] FACTOR_OPERATORS = new TokenType[] { TokenType.Multiplication, TokenType.Division };

        private readonly Lexer lexer;

        public Parser(Lexer lexer)
        {
            this.lexer = lexer;
        }

        public ASTNode Parse(string expression)
        {

            return ParseExpression();
        }

        private ASTNode ParseExpression()
        {
            var left = ParseTerm();


            while (isNext(TERM_OPERATORS))
            {
                var op = Accept();
                var right = ParseTerm();
                left = CreateBinaryOperator(op, left, right);
            }

            return left;
        }

        ///TERM: FACTOR [('*'|'/') FACTOR]*
        private ASTNode ParseTerm()
        {
            var left = ParseFactor();


            while (isNext(FACTOR_OPERATORS))
            {
                var op = Accept();
                var right = ParseFactor();
                left = CreateBinaryOperator(op, left, right);
            }

            return left; ;
        }


        ///     FACTOR: '-'? FACTORIAL_FACTOR
       
        private ASTNode ParseFactor()
        {
            ASTNode node = default;
            if (isNext(TokenType.Minus))
            {
                node = new NegationUnaryOperatorASTNode(Accept(), ParseFactorialFactor());
            }
            else
            {
                node = ParseFactorialFactor();
            }
            return node;
        }

        ///     FACTORIAL_FACTOR: PRIMARY '!'?
        private ASTNode ParseFactorialFactor()
        {
            ASTNode node = ParsePrimary();
            if (isNext(TokenType.Factorial))
            {
                node = new FactorialUnaryOperatorASTNode(Accept(), node);
            }
            return node;
        }


        private ASTNode ParsePrimary()
        {
            ASTNode node;

            if (TryParseNumber(out node))
            {
                return node;
            }

            if (TryParseSubExpression(out node))
            {
                return node;
            }

            throw new Exception($"Invalid Expression, Expected number or open paren at {lexer.Position}");
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
                node = ParseExpression();
                Expect(TokenType.CloseParen);
                Accept(); //consumes the close parent )
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