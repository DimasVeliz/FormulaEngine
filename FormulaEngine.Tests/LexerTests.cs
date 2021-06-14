using System;
using FormulaEngine.Logic;
using Xunit;

namespace FormulaEngine.Tests
{

    
    public class LexerTests
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
        public void Test_FP_001()
        {
            var expected = "100";

            Lexer lexer = new Lexer(new SourceScanner(expected));

            Assert.Equal(expected,lexer.ReadNext().Value);

        }

         [Fact]
        public void Test_FP_002()
        {
            var expected = "1.2";

            Lexer lexer = new Lexer(new SourceScanner(expected));

            Assert.Equal(expected,lexer.ReadNext().Value);

        }
         [Fact]
        public void Test_FP_003()
        {
            var expected = ".5";

            Lexer lexer = new Lexer(new SourceScanner(expected));

            Assert.Equal(expected,lexer.ReadNext().Value);

        } [Fact]
        public void Test_FP_004()
        {
            var expected = "1e5";

            Lexer lexer = new Lexer(new SourceScanner(expected));

            Assert.Equal(expected,lexer.ReadNext().Value);

        }


        
    }
}
