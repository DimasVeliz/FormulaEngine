using System;
using FormulaEngine.Logic;

namespace FormulaEngine.ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Paste please the source file");
            string sourcePath = Console.ReadLine();
            var scanner= new SourceScanner(sourcePath);
            var lexer = new Lexer(scanner);

            var sourceCodeParsed = new Parser(lexer, new SymbolTable());

            var interpreter = new InterpreterMPrograms();
            

            interpreter.Execute(sourceCodeParsed.ParseProgram());
        }
    }
}
