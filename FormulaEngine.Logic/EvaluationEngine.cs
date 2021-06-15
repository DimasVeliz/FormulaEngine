using System.Linq;

namespace FormulaEngine.Logic
{
    public static class EvaluationEngine
    {
        public static double Evaluate(string expression)
        {
            var astRoot = new Parser(new Lexer(new SourceScanner(expression))).Parse(expression);

            return Evaluate(astRoot as dynamic);
        }
        public static double Evaluate(NumberASTNode node) => node.Value;
        public static double Evaluate(AdditionBinaryOperatorASTNode node) =>
        Evaluate(node.Left as dynamic) + Evaluate(node.Right as dynamic);

        public static double Evaluate(SubstractionBinaryOperatorASTNode node) =>
        Evaluate(node.Left as dynamic) - Evaluate(node.Right as dynamic);
        public static double Evaluate(MultiplicationBinaryOperatorASTNode node) =>
        Evaluate(node.Left as dynamic) * Evaluate(node.Right as dynamic);
        public static double Evaluate(DivisionBinaryOperatorASTNode node) =>
        Evaluate(node.Left as dynamic) / Evaluate(node.Right as dynamic);

        public static double Evaluate(FactorialUnaryOperatorASTNode node) 
        {
            int fact(int x) => x==0? 1: x*fact(x-1);
            int value = (int)Evaluate(node.Target as dynamic);
            if (value<0)
            {
                throw new System.Exception("Factorial supported only for Non Negative numbers");
            }
            return fact(value);
        }
        public static double Evaluate(NegationUnaryOperatorASTNode node) =>
        -1 * Evaluate(node.Target as dynamic);

    }
}