using System;
using System.Collections.Generic;

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

    public class SymbolTable
    {
        private readonly Dictionary<string, SymbolTableEntry> Entries = new Dictionary<string, SymbolTableEntry>();

        public void AddOrUpdate(List<VNameValue> variables)
        {
            foreach (var item in variables)
            {
                AddOrUpdate(item.Name,item.Value);
            }
        }
        public void AddOrUpdate(string identifier, double value)
        {
            if (!Entries.ContainsKey(identifier))
            {
                Entries.Add(identifier, new VariableTableEntry(identifier, value));
            }
            else
            {
                var entry = Entries[identifier];
                if (entry.Type != EntryType.Variable)
                {
                    throw new System.Exception($"Indentifier {identifier} type mismatch");

                }

                (entry as VariableTableEntry).Value = value;

            }
        }

        public SymbolTableEntry Get(string identifier) =>Entries.ContainsKey(identifier)? Entries[identifier]:null;

    }
}