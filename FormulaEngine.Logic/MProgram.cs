using System.Collections.Generic;

namespace FormulaEngine.Logic
{
    public class MProgram
    {
        public MProgram()
        {
            Statements = new List<Statement>();
        }
        public List<Statement> Statements { get; set; }
    }
}