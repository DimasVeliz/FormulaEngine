using System;
using FormulaEngine.Logic;
using Xunit;

namespace FormulaEngine.Tests
{

    public class ParserTests
    {
        [Fact]
        public void ParseTest()
        {
            var expression = "1 + 2";

            var ast = (BinaryOperatorASTNode)new Parser(new Lexer(new SourceScanner(expression))).Parse(expression);
;
            Assert.NotNull(ast);

            Assert.Equal(TokenType.Addition, ast.Token.Type);
            Assert.Equal(TokenType.Number, ast.Left.Token.Type);
            Assert.Equal(TokenType.Number, ast.Right.Token.Type);

            Assert.Equal(1, (ast.Left as NumberASTNode).Value);
            Assert.Equal(2, (ast.Right as NumberASTNode).Value);

        }

        [Fact]
        public void ParseTest02()
        {
            var expression = "1 + 2*3";

            var ast = (BinaryOperatorASTNode)new Parser(new Lexer(new SourceScanner(expression))).Parse(expression);


            Assert.NotNull(ast);

            Assert.Equal(TokenType.Addition, ast.Token.Type);
            Assert.Equal(TokenType.Number, ast.Left.Token.Type);

            Assert.Equal(TokenType.Multiplication, ast.Right.Token.Type);
            Assert.Equal(TokenType.Number, (ast.Right as BinaryOperatorASTNode).Left.Token.Type);
            Assert.Equal(TokenType.Number, (ast.Right as BinaryOperatorASTNode).Right.Token.Type);


            Assert.Equal(1, (ast.Left as NumberASTNode).Value);
            Assert.Equal(2, ((ast.Right as BinaryOperatorASTNode).Left as NumberASTNode).Value);
            Assert.Equal(3, ((ast.Right as BinaryOperatorASTNode).Right as NumberASTNode).Value);

        }
        [Fact]        
        public void ParserTest03()
        {
            var expression = "1 +";

           

            Assert.Throws<Exception>(()=>new Parser(new Lexer(new SourceScanner(expression))).Parse(expression));


        }
    }
}