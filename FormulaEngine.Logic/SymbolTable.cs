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
        private readonly Dictionary<string, SymbolTableEntry> Entries = new Dictionary<string, SymbolTableEntry>();

        public void AddOrUpdate(List<VNameValue> variables)
        {
            foreach (var item in variables)
            {
                AddOrUpdate(item.Name, item.Value);
            }
        }
        public void AddOrUpdate(string identifier, double value)
        {
            string key = identifier.ToLower();

            if (!Entries.ContainsKey(key))
            {
                Entries.Add(key, new VariableTableEntry(identifier, value));
            }
            else
            {
                var entry = Entries[key];
                if (entry.Type != EntryType.Variable)
                {
                    throw new System.Exception($"Indentifier {identifier} type mismatch");

                }

                (entry as VariableTableEntry).Value = value;

            }
        }

        public SymbolTableEntry Get(string identifier) =>
         Entries.ContainsKey(identifier.ToLower()) ? Entries[identifier.ToLower()] : null;



        public void AddFunction<T>()
        {
            //Getting the static functions taking any number of double parameters and return double
            //and integrating them into the symbols table

            var methods = typeof(T)
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(mi => typeof(double).IsAssignableFrom(mi.ReturnType))
            .Where(mi => !mi.GetParameters().Any(p => !p.ParameterType.IsAssignableFrom(typeof(double))));

            foreach (var mInfo in methods)
            {
                Entries.Add(mInfo.Name,new FunctionTableEntry(mInfo));
            }
        }

    }
}