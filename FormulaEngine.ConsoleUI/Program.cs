using System;
using FormulaEngine.Logic;

namespace FormulaEngine.ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            var scanner= new SourceScanner("./callingFunctions.ml");
            var lexer = new Lexer(scanner);

            var sourceCodeParsed = new Parser(lexer, new SymbolTable());

            var interpreter = new InterpreterMPrograms();

            interpreter.Execute(sourceCodeParsed.ParseProgram());
        }
    }
}
