using System;
using System.Collections.Generic;

namespace FormulaEngine.Logic
{
    class PositionMaker
    {
        public int LinePosition { get; set; }
        public int LineNumber { get; set; }
    }
    public class SourceScanner
    {
        public List<string> SourceCode { get; set; }
        private int _linePosition;
        private int _lineNumber;


        readonly Stack<PositionMaker> PositionStack = new Stack<PositionMaker>();
        public int LinePosition => _linePosition;
        public int LineNumber => _lineNumber;


        public SourceScanner(List<string> source)
        {

            _lineNumber = 0;
            _linePosition = -1;
            SourceCode = source;
        }

        public char? Read()
        {

            if (MoreCharsAvailableInCurrentLine())
            {
                return SourceCode[_lineNumber][++_linePosition];
            }
            else if (MoreLinesAvailable())
            {
                AdvanceToNextLine();
            }
            else
            {
                return (char?)null;
            }

            throw new Exception("Unable to lex source code");
        }

        private void AdvanceToNextLine()
        {
            _lineNumber++;
            _linePosition = -1;

        }

        private bool MoreLinesAvailable() => _lineNumber < SourceCode.Count;

        private bool MoreCharsAvailableInCurrentLine()
        {
            if (_lineNumber < SourceCode.Count)
            {
                return _linePosition + 1 < SourceCode[_lineNumber].Length;
            }
            return false;
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
            PositionStack.Push(new PositionMaker { LineNumber = _lineNumber, LinePosition = _linePosition });
        }
        public void Pop()
        {
            var position = PositionStack.Pop();
            _lineNumber = position.LineNumber;
            _linePosition = position.LinePosition;
        }
    }
}