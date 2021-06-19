
using System.Collections.Generic;

namespace FormulaEngine.Logic
{
    public class Expression
    {
        public ExpressionNode Root { get; set;}
    }

    

    public abstract class ExpressionNode
    {
        public Token Token { get; set; }
        public ExpressionNode(Token token)
        {
            Token = token;
        }

    }

    public abstract class IdentifierExpressionNode : ExpressionNode
    {

        protected IdentifierExpressionNode(Token token, string name) : base(token)
        {
            Name = name;
        }

        public string Name { get; }
    }
    public class VariableIdentifierExpressionNode : IdentifierExpressionNode
    {
        public VariableIdentifierExpressionNode(Token token) : base(token, token.Value)
        {
        }
    }
    public class FunctionExpressionNode : IdentifierExpressionNode
    {
        public List<ExpressionNode> ArgumentsNodes = new List<ExpressionNode>();
        public FunctionExpressionNode(Token token) : base(token, token.Value)
        {
        }
    }
    public class NumberExpressionNode : ExpressionNode
    {
        public double Value => double.Parse(Token.Value);
        public NumberExpressionNode(Token token) : base(token)
        {
        }
        

    }

    public abstract class OperatorExpressionNode : ExpressionNode
    {
        protected OperatorExpressionNode(Token token) : base(token)
        {
        }
    }

    public abstract class UnaryOperatorExpressionNode : ExpressionNode
    {

        public UnaryOperatorExpressionNode(Token token, ExpressionNode target) : base(token)
        {
            Target = target;
        }

        public ExpressionNode Target { get; }
    }

    public class FactorialUnaryOperatorExpressionNode : UnaryOperatorExpressionNode
    {
        public FactorialUnaryOperatorExpressionNode(Token token, ExpressionNode target) : base(token, target)
        {
        }
    }
    public class NegationUnaryOperatorExpressionNode : UnaryOperatorExpressionNode
    {
        public NegationUnaryOperatorExpressionNode(Token token, ExpressionNode target) : base(token, target)
        {
        }
    }

    public abstract class BinaryOperatorExpressionNode : OperatorExpressionNode
    {

        public ExpressionNode Left { get; }
        public ExpressionNode Right { get; }
        public BinaryOperatorExpressionNode(Token token, ExpressionNode left, ExpressionNode right) : base(token)
        {
            Token = token;
            Left = left;
            Right = right;
        }

    }

    public class ExponentBinaryOperatorExpressionNode : BinaryOperatorExpressionNode
    {
        public ExponentBinaryOperatorExpressionNode(Token token, ExpressionNode left, ExpressionNode right) : base(token, left, right)
        {
        }
    }

    public class AdditionBinaryOperatorExpressionNode : BinaryOperatorExpressionNode
    {
        public AdditionBinaryOperatorExpressionNode(Token token, ExpressionNode left, ExpressionNode right) : base(token, left, right)
        {
        }
    }

    public class SubstractionBinaryOperatorExpressionNode : BinaryOperatorExpressionNode
    {
        public SubstractionBinaryOperatorExpressionNode(Token token, ExpressionNode left, ExpressionNode right) : base(token, left, right)
        {
        }
    }

    public class MultiplicationBinaryOperatorExpressionNode : BinaryOperatorExpressionNode
    {
        public MultiplicationBinaryOperatorExpressionNode(Token token, ExpressionNode left, ExpressionNode right) : base(token, left, right)
        {
        }
    }

    public class DivisionBinaryOperatorExpressionNode : BinaryOperatorExpressionNode
    {
        public DivisionBinaryOperatorExpressionNode(Token token, ExpressionNode left, ExpressionNode right) : base(token, left, right)
        {
        }
    }
}