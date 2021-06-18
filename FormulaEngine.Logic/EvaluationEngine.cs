using System;
using System.Collections.Generic;
using System.Linq;

namespace FormulaEngine.Logic
{
    public class EvaluationEngine
    {
        private readonly SymbolTable _symbolTable = new SymbolTable();

        public void AddBuiltInFunction<T>() =>_symbolTable.AddFunction<T>();

        public double Evaluate(string expression, List<VNameValue> variables)
        {
            _symbolTable.AddOrUpdate(variables); //filling it up
            var astRoot = new Parser(new Lexer(new SourceScanner(expression)), _symbolTable).Parse();


            return Evaluate(astRoot as dynamic);
        }

        public double Evaluate(ASTNode root)=>Evaluate(root as dynamic);
        public double Evaluate(NumberASTNode node) => node.Value;
        public double Evaluate(AdditionBinaryOperatorASTNode node) =>
        Evaluate(node.Left as dynamic) + Evaluate(node.Right as dynamic);

        public double Evaluate(ExponentBinaryOperatorASTNode node) =>
        Math.Pow(Evaluate(node.Left as dynamic), Evaluate(node.Right as dynamic));

        public double Evaluate(SubstractionBinaryOperatorASTNode node) =>
        Evaluate(node.Left as dynamic) - Evaluate(node.Right as dynamic);
        public double Evaluate(MultiplicationBinaryOperatorASTNode node) =>
        Evaluate(node.Left as dynamic) * Evaluate(node.Right as dynamic);
        public double Evaluate(DivisionBinaryOperatorASTNode node) =>
        Evaluate(node.Left as dynamic) / Evaluate(node.Right as dynamic);

        public double Evaluate(FactorialUnaryOperatorASTNode node)
        {
            int fact(int x) => x == 0 ? 1 : x * fact(x - 1);
            int value = (int)Evaluate(node.Target as dynamic);
            if (value < 0)
            {
                throw new System.Exception("Factorial supported only for Non Negative numbers");
            }
            return fact(value);
        }
        public double Evaluate(NegationUnaryOperatorASTNode node) =>
        -1 * Evaluate(node.Target as dynamic);


        public double Evaluate(VariableIdentifierASTNode node)
        {
            var variable = _symbolTable.Get(node.Name);
            if (variable == null || variable.Type != EntryType.Variable)
            {
                throw new Exception($"Error evaluating the variable {variable.IdentifierName}");
            }
            return (variable as VariableTableEntry).Value;
        }

        public double Evaluate(FunctionASTNode node)
        {
            var variable = _symbolTable.Get(node.Name);
            if (variable == null || variable.Type != EntryType.Function)
            {
                throw new Exception($"Error evaluating the variable {variable.IdentifierName}");
            }
            return (double)(variable as FunctionTableEntry)
            .MethodInfo
            .Invoke(null, node.ArgumentsNodes.Select(arg =>Evaluate(arg as dynamic)).ToArray());
        }
    }

    public class VNameValue
    {
        public string Name { get; set; }
        public double Value { get; set; }
    }
}