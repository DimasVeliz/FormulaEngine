using System;
using FormulaEngine.Logic;

namespace FormulaEngine.ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            var expression ="1+2/4 -8";

            var lexer = new Lexer(new SourceScanner(expression));
            
            while (lexer.Peek().Type!=TokenType.EOE)
            {
                var currentToken = lexer.ReadNext();
                System.Console.WriteLine($"Token of type: {currentToken.Type} with value: {currentToken.Value} at position {currentToken.Position}");
            }
        }
    }
}
