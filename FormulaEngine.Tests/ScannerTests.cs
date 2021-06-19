using System;
using System.Collections.Generic;
using FormulaEngine.Logic;
using Xunit;

namespace FormulaEngine.Tests
{

    public class ScannerTests
    {
        [Fact]
        public void PeekTest()
        {
            var scanner = new SourceScanner("./simpleProgram.ml");

            var peek=scanner.Peek();
            int counterChars=0;
            while (peek.HasValue)
            {
                scanner.Read();
                peek= scanner.Peek();
                counterChars++;
            }

            Assert.Equal(18,counterChars);

        }
    }
}