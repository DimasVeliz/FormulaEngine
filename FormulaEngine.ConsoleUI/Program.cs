using System;
using FormulaEngine.Logic;

namespace FormulaEngine.ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            var st = new SymbolTable();
            st.AddFunction<FunctionFactory>();


            var entry = (st.Get("sqrt")as FunctionTableEntry)
            .MethodInfo
            .Invoke(null,new object[]{64});

            System.Console.WriteLine(entry);
        }
    }
}
