using System;
using FormulaEngine.Logic;
using Xunit;

namespace FormulaEngine.Tests
{


    public class LexerTests
    {
        [Fact]
        public void CountingTokens()
        {
            var scanner = new SourceScanner("./simpleProgram.ml");

            var lexer = new Lexer(scanner);
            var token = lexer.Peek();
            var tokenCounter = 0;
            while (token.Type != TokenType.EOF)
            {
                tokenCounter++;
                token = lexer.ReadNext();

            }

            Assert.Equal(54,tokenCounter);

        }


    }
}
