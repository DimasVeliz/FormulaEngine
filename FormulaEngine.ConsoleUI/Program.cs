using System;
using FormulaEngine.Logic;

namespace FormulaEngine.ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            var sourceCode = new Parser(new Lexer(new SourceScanner("./simpleProgram.cs")),new SymbolTable());

            var interpreter = new InterpreterMPrograms();

            interpreter.Execute(sourceCode.ParseProgram());
        }
    }
}
