using System;
using System.Collections.Generic;
using System.Linq;

namespace FormulaEngine.Logic
{
    public class EvaluationEngine
    {
        private readonly SymbolTable _symbolTable;

        public void AddBuiltInFunction<T>() => _symbolTable.AddFunctionALanguageSymbol<T>();
        public void AddBuiltInGlobalVariables(List<VNameValue> variables) => _symbolTable.AddOrUpdateALanguageSymbol(variables);

        public EvaluationEngine(SymbolTable symbolTable)
        {
            _symbolTable = symbolTable;
        }

        public double Evaluate(Expression expression)
        {
            return Evaluate(expression.Root as dynamic);
        }

        public double Evaluate(ExpressionNode root) => Evaluate(root as dynamic);
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
            var variable = _symbolTable.GetVariableCorrespondentExpression(node.Name);


            if (variable == null)
            {
                throw new Exception($"Error evaluating the variable ");
            }
            double result = Evaluate(variable.Root as dynamic);
            return result;
        }

        public double Evaluate(FunctionExpressionNode node)
        {
            var userDefinedFunctionExist = _symbolTable.IsFunctionDefined(node.Name);
            if (userDefinedFunctionExist)
            {
                var localFunction = _symbolTable.GetFunction(node.Name);

                if (localFunction.ParameterNames.Count != node.ArgumentsNodes.Count)
                {
                    throw new Exception($"Function {node.Name} is missing arguments");
                }

                _symbolTable.BeginScope();

                for (int i = 0; i < localFunction.ParameterNames.Count; i++)
                {
                    var argValue = Evaluate(node.ArgumentsNodes[i]);

                    var expression = new Expression
                    {
                        Root = new NumberExpressionNode(new Token(TokenType.Number, -1, -1, argValue.ToString()))
                    };
                    _symbolTable.DefineVariable(localFunction.ParameterNames[i].Name, expression);
                }

                var result = Evaluate(localFunction.Body as dynamic);
                _symbolTable.EndScope();


                return result;
            }

            // attempting to return a built-in function
            var variable = _symbolTable.Get(node.Name);
            if (variable == null || variable.Type != EntryType.Function)
            {
                throw new Exception($"Error evaluating the variable {variable.IdentifierName}");
            }
            return (double)(variable as FunctionTableEntry)
            .MethodInfo
            .Invoke(null, node.ArgumentsNodes.Select(arg => Evaluate(arg as dynamic)).ToArray());
        }
    }

    public class VNameValue
    {
        public string Name { get; set; }
        public double Value { get; set; }
    }
}