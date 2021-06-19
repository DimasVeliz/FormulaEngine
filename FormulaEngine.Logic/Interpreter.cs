using System;
using System.Collections.Generic;
using System.Linq;

namespace FormulaEngine.Logic
{
    public class InterpreterMPrograms
    {
        private SymbolTable _symbolTable = new SymbolTable();

        public void Execute(MProgram program)
        {
            foreach (var statement in program.Statements)
            {
                if (statement is PrintStatement)
                {
                    System.Console.WriteLine(Execute(statement as PrintStatement));
                }
                else
                    Execute(statement as dynamic);
            }
        }

        public void Execute(LetStatement statement)
        {
            if (_symbolTable.IsVariableDefinedInCurrentScope(statement.Variable.Name))
                throw new Exception($"A variable with the same name was already defined");
            _symbolTable.DefineVariable(statement.Variable.Name, statement.Expression);
        }
        public void Execute(SetStatement statement)
        {
            if (!_symbolTable.IsVariableDefinedInCurrentScope(statement.Variable.Name))
                throw new Exception($"The variable does not exist in the current context");

            var engine = new EvaluationEngine(_symbolTable);

            var newValue = engine.Evaluate(statement.Expression);

            var previous = statement.Expression.Root.Token;

            _symbolTable.UpdateVariable(statement.Variable.Name, new Expression
            {
                Root = new NumberExpressionNode(
                    new Token(TokenType.Number, previous.LinePosition, previous.LineNumber, newValue.ToString())
                    )
                { }
            });

        }
        public void Execute(EvalStatement statement)
        {
            if (!_symbolTable.IsVariableDefinedInCurrentScope(statement.Variable.Name))
                throw new Exception($"The variable does not exist in the current context");

            var engine = new EvaluationEngine(_symbolTable);

            var newValue = engine.Evaluate(statement.Expression);

            var previous = statement.Expression.Root.Token;

            _symbolTable.UpdateVariable(statement.Variable.Name, new Expression
            {
                Root = new NumberExpressionNode(
                    new Token(TokenType.Number, previous.LinePosition, previous.LineNumber, newValue.ToString())
                    )
                { }
            });
        }
        public string Execute(PrintStatement statement)
        {
            var engine = new EvaluationEngine(_symbolTable);

            var newValue = engine.Evaluate(statement.Body);

            return newValue.ToString();
        }

        public void Execute(FuncDefStatement statement)
        {
            if (_symbolTable.IsFunctionDefined(statement.Function.Name))
            {
                throw new Exception($"A function with the same name was already defined");
            }

            _symbolTable.DefineFunction(statement);
        }
    }
}