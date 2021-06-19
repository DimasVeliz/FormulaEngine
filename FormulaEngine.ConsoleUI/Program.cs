using System;
using FormulaEngine.Logic;

namespace FormulaEngine.ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            var lexer = new Lexer(new SourceScanner("./simpleProgram.ml"));

            var sourceCodeParsed = new Parser(lexer, new SymbolTable());

            var interpreter = new InterpreterMPrograms();

            interpreter.Execute(sourceCodeParsed.ParseProgram());
        }
    }
}
