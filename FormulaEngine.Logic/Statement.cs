using System.Collections.Generic;

namespace FormulaEngine.Logic
{
    public abstract class Statement
    {

    }

    public class BasicStatement : Statement
    {
        public VariableIdentifierExpressionNode Variable { get; set; }
        public Expression Expression { get; set; }

    }

    public class LetStatement : BasicStatement
    {

    }
    public class SetStatement : BasicStatement
    {

    }
    public class EvalStatement : BasicStatement
    {

    }
    public class FuncDefStatement : Statement
    {
        public FunctionExpressionNode Function { get; set; }
        public List<VariableIdentifierExpressionNode> ParameterNames { get; set; }

        public Expression Body { get; set; }
    }
    public class PrintStatement : Statement
    {
        public Expression Body { get; set; }

    }
}