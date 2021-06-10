namespace FormulaEngine.Logic
{
    public static class EvaluationEngine
    {
        public static int Evaluate(string expression)
        {
            var astRoot = Parser.Parse(expression);

            return Evaluate(astRoot as dynamic);
        }
        public static int Evaluate(NumberASTNode node) => node.Value;
        public static int Evaluate(AdditionBinaryOperatorASTNode node) =>
        Evaluate(node.Left as dynamic) + Evaluate(node.Right as dynamic);

        public static int Evaluate(SubstractionBinaryOperatorASTNode node) =>
        Evaluate(node.Left as dynamic) - Evaluate(node.Right as dynamic);
        public static int Evaluate(MultiplicationBinaryOperatorASTNode node) =>
        Evaluate(node.Left as dynamic) * Evaluate(node.Right as dynamic);
        public static int Evaluate(DivisionBinaryOperatorASTNode node) =>
        Evaluate(node.Left as dynamic) / Evaluate(node.Right as dynamic);



    }
}