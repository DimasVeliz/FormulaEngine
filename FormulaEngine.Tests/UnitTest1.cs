using System;
using FormulaEngine.Logic;
using Xunit;

namespace FormulaEngine.Tests
{

    public class UnitTest1
    {
        [Fact]
        public void LexerTest()
        {
            var expression = "1 + 2";
            (TokenType, int, string)[] expectedResults = new (TokenType, int, string)[]{
                (TokenType.Number,0,"1"),
                (TokenType.Addition,2,"+"),
                (TokenType.Number,4,"2")

            };

            var lexer = new Lexer(new SourceScanner(expression));

            foreach (var (t, p, v) in expectedResults)
            {
                var token = lexer.ReadNext();
                Assert.Equal(t, token.Type);
                Assert.Equal(p, token.Position);
                Assert.Equal(v, token.Value);

            }

            Assert.Equal(TokenType.EOE, lexer.ReadNext().Type);
        }


        [Fact]
        public void ParseTest()
        {
            var expression = "1 + 2";

            var ast = (BinaryOperatorASTNode)Parser.Parse(expression);
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

            var ast = (BinaryOperatorASTNode)Parser.Parse(expression);

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

           

            Assert.Throws<Exception>(()=>Parser.Parse(expression));

        }
    }
}
