

namespace FormulaEngine.Logic
{
    /// <summary>
    /// Implements the following Production Rules
    /// EXPRESSION: TERM [('+'|'-') TERM]*
    ///     TERM: FACTOR [('*'|'/') FACTOR]*
    ///     FACTOR: NUMBER
    ///     NUMBER: [0-9]+
    /// 
    /// An expression expanded: => (Factor *|/ Factor) (+|-) (Factor *|/ Factor)
    /// </summary>
    public static class Parser
    {
        public static ASTNode Parse(string expression)
        {

        }

        public static ASTNode ParseExpression(Lexer lexer)
        {

        }
        public static ASTNode ParseTerm(Lexer lexer)
        {

        }
        public static ASTNode ParseFactor(Lexer lexer)
        {

        }
        public static ASTNode ParseNumber(Lexer lexer)
        {

        }

    }
}