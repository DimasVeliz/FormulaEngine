using System;
using FormulaEngine.Logic;

namespace FormulaEngine.ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            
           
            var scanner= new SourceScanner(args[0]);
            var lexer = new Lexer(scanner);

            var sourceCodeParsed = new Parser(lexer, new SymbolTable());

            var interpreter = new InterpreterMPrograms();
            

            var result = interpreter.Execute(sourceCodeParsed.ParseProgram());

            foreach (var item in result.Outputs)
            {
                System.Console.WriteLine(item);
            }
        }
    }
}
