

namespace FormulaEngine.Test
{
    //this class implements the following matching rules
    // OPERATOR L + | - | * | /
    // NUMBER: [0-9]+

    [TestClass()]
    public class LexerTest
    {
        [TestMethod()]
        public void ReadNextTest()
        {
            var expression = "1 + 2";
            (TokenType,int,string)[] expectedResults = new (TokenType,int,string)[]{
                (TokenType.Number,0,"1"),
                (TokenType.Addition,0,"+"),
                (TokenType.Number,0,"2")

            };

            var lexer = new Lexer(new SourceScanner(expression));

            foreach (var (t,p,v) in expectedResults)
            {
                var token = lexer.ReadNext();
                Assert.AreEqual(t,token.Type);
                Assert.AreEqual(p,token.Position);
                Assert.AreEqual(v,token.Value);

            }

            Assert.AreEqual(TokenType.EOE,lexer.ReadNext().Type);
        }
    }
}