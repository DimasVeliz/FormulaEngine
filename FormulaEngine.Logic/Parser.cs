
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
            {TokenType.Substraction,(t,l,r)=>new SubstractionBinaryOperatorASTNode(t,l,r)},
            {TokenType.Multiplication,(t,l,r)=>new MultiplicationBinaryOperatorASTNode(t,l,r)},
            {TokenType.Division,(t,l,r)=>new DivisionBinaryOperatorASTNode(t,l,r)},

        };
    }
    /// <summary>
    /// Implements the following Production Rules
    /// EXPRESSION: TERM [('+'|'-') TERM]*
    ///     TERM: FACTOR [('*'|'/') FACTOR]*
    ///     FACTOR: '(' EXPRESSION ')' | NUMBER
    /// </summary>
    public class Parser
    {
        static readonly TokenType[] TERM_OPERATORS = new TokenType[] { TokenType.Addition, TokenType.Substraction };
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
        ///FACTOR: NUMBER
        private ASTNode ParseFactor()
        {
            return ParseNumber();
        }

        ///NUMBER: [0-9]+
        private ASTNode ParseNumber()
        {
            Expect(TokenType.Number);


            return new NumberASTNode(Accept());

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