
using System;
using System.Collections.Generic;

namespace FormulaEngine.Logic
{
    public static class FunctionFactory
    {
        public static Dictionary <TokenType,Func<Token,ASTNode,ASTNode,BinaryOperatorASTNode>> Operations = new Dictionary<TokenType,Func<Token,ASTNode,ASTNode,BinaryOperatorASTNode>>()
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
    ///     FACTOR: NUMBER
    ///     NUMBER: [0-9]+
    /// 
    /// An expression expanded: => (Factor *|/ Factor) (+|-) (Factor *|/ Factor)
    /// </summary>
    public static class Parser
    {
        
        public static ASTNode Parse(string expression)
        {
            var lexer = new Lexer(new SourceScanner(expression));

            return ParseExpression(lexer);
        }

        private static ASTNode ParseExpression(Lexer lexer)
        {
            var left = ParseTerm(lexer);

            var lookahead = lexer.Peek();
            while (lookahead.Type==TokenType.Addition || lookahead.Type==TokenType.Substraction)
            {
                var op = lexer.ReadNext();

                var right = ParseTerm(lexer);
                
                left = CreateBinaryOperator(op,left,right);

                lookahead= lexer.Peek();
            }


            return left;
        }

        ///TERM: FACTOR [('*'|'/') FACTOR]*
        private static ASTNode ParseTerm(Lexer lexer)
        {
            var left =ParseFactor(lexer);

            var lookahead = lexer.Peek();
            while (lookahead.Type==TokenType.Multiplication || lookahead.Type==TokenType.Division)
            {
                var op = lexer.ReadNext();

                var right = ParseFactor(lexer);
                
                left = CreateBinaryOperator(op,left,right);

                lookahead= lexer.Peek();
            }
            
            return left;
        }
        ///FACTOR: NUMBER
        private static ASTNode ParseFactor(Lexer lexer)
        {
            return ParseNumber(lexer);
        }

        ///NUMBER: [0-9]+
        private static ASTNode ParseNumber(Lexer lexer)
        {
            var token =lexer.Peek();
            if (token.Type!=TokenType.Number)
            {
                throw new Exception($"Invalid Expression at position:{lexer.Position}");
            }

            Accept(lexer);

            return new NumberASTNode(token);

        }
        private static Token Accept(Lexer lexer) => lexer.ReadNext();

        private static ASTNode CreateBinaryOperator(Token op, ASTNode left, ASTNode right)
        {
            return FunctionFactory.Operations[op.Type](op,left,right);
        }
        private static void Expect(Lexer lexer, TokenType expected)
        {
            if (lexer.Peek().Type != expected)
            {
                throw new System.Exception($"Expected: {expected} at Position {lexer.Position}");
            }
        }
    }
}