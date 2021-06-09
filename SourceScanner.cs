using System;
using System.Collections.Generic;

namespace FormulaEngine
{
    public class SourceScanner
    {
        readonly Stack<int> PositionStack = new Stack<int>();
        readonly string _buffer;
        public int Position { get; private set; }

        public bool EndOfSource => Position >= _buffer.Length; 

        
        public char? Read()
        {
            if (EndOfSource)
            {
                return (char?)null;
            }
            return _buffer[Position++];
        }

        public char? Peek()
        {
            Push();
            var next = Read();
            Pop();
            return next;
        }

        public void Push()
        {
            PositionStack.Push(Position);
        }
        public void Pop()
        {
            Position = PositionStack.Pop();
        }
    }
}