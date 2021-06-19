using System;
using System.Collections.Generic;
using System.Linq;

namespace FormulaEngine.Logic
{
    public class EvaluationEngine
    {
        private readonly SymbolTable _symbolTable;

        public void AddBuiltInFunction<T>() =>_symbolTable.AddFunctionALanguageSymbol<T>();
        public void AddBuiltInGlobalVariables( List<VNameValue> variables) =>_symbolTable.AddOrUpdateALanguageSymbol(variables);

        public  EvaluationEngine(SymbolTable symbolTable)
        {
            _symbolTable = symbolTable;
        }

        public double Evaluate(Expression expression)
        {
            return Evaluate(expression.Root as dynamic);
        }

        public double Evaluate(ExpressionNode root)=>Evaluate(root as dynamic);
        public double Evaluate(NumberExpressionNode node) => node.Value;
        public double Evaluate(AdditionBinaryOperatorExpressionNode node) =>
        Evaluate(node.Left as dynamic) + Evaluate(node.Right as dynamic);

        public double Evaluate(ExponentBinaryOperatorExpressionNode node) =>
        Math.Pow(Evaluate(node.Left as dynamic), Evaluate(node.Right as dynamic));

        public double Evaluate(SubstractionBinaryOperatorExpressionNode node) =>
        Evaluate(node.Left as dynamic) - Evaluate(node.Right as dynamic);
        public double Evaluate(MultiplicationBinaryOperatorExpressionNode node) =>
        Evaluate(node.Left as dynamic) * Evaluate(node.Right as dynamic);
        public double Evaluate(DivisionBinaryOperatorExpressionNode node) =>
        Evaluate(node.Left as dynamic) / Evaluate(node.Right as dynamic);

        public double Evaluate(FactorialUnaryOperatorExpressionNode node)
        {
            int fact(int x) => x == 0 ? 1 : x * fact(x - 1);
            int value = (int)Evaluate(node.Target as dynamic);
            if (value < 0)
            {
                throw new System.Exception("Factorial supported only for Non Negative numbers");
            }
            return fact(value);
        }
        public double Evaluate(NegationUnaryOperatorExpressionNode node) =>
        -1 * Evaluate(node.Target as dynamic);


        public double Evaluate(VariableIdentifierExpressionNode node)
        {
            var variable = _symbolTable.Get(node.Name);
            if (variable == null || variable.Type != EntryType.Variable)
            {
                throw new Exception($"Error evaluating the variable {variable.IdentifierName}");
            }
            return (variable as VariableTableEntry).Value;
        }

        public double Evaluate(FunctionExpressionNode node)
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