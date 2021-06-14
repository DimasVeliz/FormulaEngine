
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
    public  class Parser
    {
        private readonly Lexer lexer;

        public Parser(Lexer lexer)
        {
            this.lexer = lexer;
        }
        
        public  ASTNode Parse(string expression)
        {
            
            return ParseExpression();
        }

        private  ASTNode ParseExpression()
        {
            var left = ParseTerm();

            var lookahead = lexer.Peek();
            while (lookahead.Type==TokenType.Addition || lookahead.Type==TokenType.Substraction)
            {
                var op = lexer.ReadNext();

                var right = ParseTerm();
                
                left = CreateBinaryOperator(op,left,right);

                lookahead= lexer.Peek();
            }


            return left;
        }

        ///TERM: FACTOR [('*'|'/') FACTOR]*
        private  ASTNode ParseTerm()
        {
            var left =ParseFactor();

            var lookahead = lexer.Peek();
            while (lookahead.Type==TokenType.Multiplication || lookahead.Type==TokenType.Division)
            {
                var op = lexer.ReadNext();

                var right = ParseFactor();
                
                left = CreateBinaryOperator(op,left,right);

                lookahead= lexer.Peek();
            }
            
            return left;
        }
        ///FACTOR: NUMBER
        private  ASTNode ParseFactor()
        {
            return ParseNumber();
        }

        ///NUMBER: [0-9]+
        private  ASTNode ParseNumber()
        {
            var token =lexer.Peek();
            if (token.Type!=TokenType.Number)
            {
                throw new Exception($"Invalid Expression at position:{lexer.Position}");
            }

            Accept();

            return new NumberASTNode(token);

        }
        private  Token Accept() => lexer.ReadNext();

        private  ASTNode CreateBinaryOperator(Token op, ASTNode left, ASTNode right)
        {
            return FunctionFactory.Operations[op.Type](op,left,right);
        }
        private  void Expect(TokenType expected)
        {
            if (lexer.Peek().Type != expected)
            {
                throw new System.Exception($"Expected: {expected} at Position {lexer.Position}");
            }
        }
    }
}