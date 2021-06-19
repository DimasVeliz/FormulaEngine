using System;
using System.Collections.Generic;
using System.IO;

namespace FormulaEngine.Logic
{
    class PositionMaker
    {
        public int LinePosition { get; set; }
        public int LineNumber { get; set; }
    }
    public class SourceScanner
    {
        const char NEW_LINE_CHAR='\n';
        public List<string> SourceCode { get; set; }
        private int _linePosition;
        private int _lineNumber;


        readonly Stack<PositionMaker> PositionStack = new Stack<PositionMaker>();
        public int LinePosition => _linePosition;
        public int LineNumber => _lineNumber;

        public bool EndOfSource => _lineNumber +1== SourceCode.Count && _linePosition+1 == SourceCode[_lineNumber].Length;


        public SourceScanner(string programPath)
        {
            List<string> linesOfCode = new List<string>();
            FileStream fs = new FileStream(programPath, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(fs);
            while (!reader.EndOfStream)
            {
                string newLine = reader.ReadLine()+NEW_LINE_CHAR;
                
                linesOfCode.Add(newLine);
            }

            _lineNumber = 0;
            _linePosition = -1;
            this.SourceCode = new List<string>(linesOfCode);
        }
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
                return Read();

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