using System;

namespace FormulaEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            var expression = "1+2 -4 * 78/4";
            var lexer = new Lexer(new SourceScanner(expression));

            while (lexer.Peek().Type!=TokenType.EOE)
            {
                var token = lexer.ReadNext();
                System.Console.WriteLine($"Token {token.Value} of Type {token.Type}, found at position{token.Position}");
            }

        }
    }
}
