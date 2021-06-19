using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FormulaEngine.Logic
{
    public enum EntryType
    {
        Variable,
        Function
    }

    public class VariableScope
    {
        Dictionary<string, Expression> variables = new Dictionary<string, Expression>();

        public bool IsVariableDefined(string variableName) =>
        variables.ContainsKey(variableName);

        public void DefineVariable(string variableName, Expression expression) =>
        variables.Add(variableName, expression);

        public void UpdateVariable(string variableName, Expression expression) =>
        variables[variableName] = expression;

        public Expression GetVariableValue(string variableName) =>
        variables[variableName];

    }


    public class SymbolTableEntry
    {
        public SymbolTableEntry(string name, EntryType type)
        {
            IdentifierName = name;
            Type = type;
        }

        public string IdentifierName { get; }
        public EntryType Type { get; }
    }

    public class VariableTableEntry : SymbolTableEntry
    {
        public VariableTableEntry(string name, double value) : base(name, EntryType.Variable)
        {
            Value = value;
        }

        public double Value { get; set; }
    }

    public class FunctionTableEntry : SymbolTableEntry
    {
        public FunctionTableEntry(MethodInfo methodInfo) : base(methodInfo.Name, EntryType.Function)
        {
            MethodInfo = methodInfo;
        }

        public MethodInfo MethodInfo { get; }
    }

    public class SymbolTable
    {
        private readonly List<VariableScope> _symbolsScope = new List<VariableScope>();
        private readonly Dictionary<string, FuncDefStatement> _functions = new Dictionary<string, FuncDefStatement>();


        private readonly Dictionary<string, SymbolTableEntry> GlobalEntries = new Dictionary<string, SymbolTableEntry>();


        public VariableScope CurrentScope => _symbolsScope[_symbolsScope.Count - 1];

        public SymbolTable()
        {
            BeginScope();
        }

        private void BeginScope() => _symbolsScope.Add(new VariableScope());


        private void EndScope()
        {
            if (_symbolsScope.Count > 1)
            {
                _symbolsScope.RemoveAt(_symbolsScope.Count - 1);
            }
        }



        //support for user-defined variables
        public bool IsVariableDefinedInCurrentScope(string variableName) =>
        CurrentScope.IsVariableDefined(variableName);

        public void DefineVariable(string varName, Expression expression) =>
        CurrentScope.DefineVariable(varName, expression);
        public void UpdateVariable(string varName, Expression expression) =>
        CurrentScope.UpdateVariable(varName, expression);

        public Expression GetVariableCorrespondentExpression(string varName)
        {
            int currentScopeIndex = _symbolsScope.Count - 1;
            while (currentScopeIndex >= 0)
            {
                if (_symbolsScope[currentScopeIndex].IsVariableDefined(varName))
                {
                    return _symbolsScope[currentScopeIndex].GetVariableValue(varName);
                }
                currentScopeIndex--;
            }

            throw new Exception($"Variable {varName} undefined");
        }


       
       
        //support for user-defined functions
       
        public bool IsFunctionDefined(string functionName) => _functions.ContainsKey(functionName);
        public void DefineFunction(FuncDefStatement funcStatement) =>
        _functions.Add(funcStatement.Function.Name, funcStatement);
        public FuncDefStatement GetFunction(string functionName) => _functions[functionName];



        //support for built-in variables and function

        public void AddOrUpdateALanguageSymbol(List<VNameValue> variables)
        {
            foreach (var item in variables)
            {
                AddOrUpdateALanguageSymbol(item.Name, item.Value);
            }
        }
        public void AddOrUpdateALanguageSymbol(string identifier, double value)
        {
            string key = identifier.ToLower();

            if (!GlobalEntries.ContainsKey(key))
            {
                GlobalEntries.Add(key, new VariableTableEntry(identifier, value));
            }
            else
            {
                var entry = GlobalEntries[key];
                if (entry.Type != EntryType.Variable)
                {
                    throw new System.Exception($"Indentifier {identifier} type mismatch");

                }

                (entry as VariableTableEntry).Value = value;

            }
        }

        public SymbolTableEntry Get(string identifier) =>
         GlobalEntries.ContainsKey(identifier.ToLower()) ? GlobalEntries[identifier.ToLower()] : null;

        public void AddFunctionALanguageSymbol<T>()
        {
            //Getting the static functions taking any number of double parameters and return double
            //and integrating them into the symbols table

            var methods = typeof(T)
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(mi => typeof(double).IsAssignableFrom(mi.ReturnType))
            .Where(mi => !mi.GetParameters().Any(p => !p.ParameterType.IsAssignableFrom(typeof(double))));

            foreach (var mInfo in methods)
            {
                GlobalEntries.Add(mInfo.Name, new FunctionTableEntry(mInfo));
            }
        }

    }
}