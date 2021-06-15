
namespace FormulaEngine.Logic
{
    //this class implements the following matching rules
    // OPERATOR L + | - | * | /
    // NUMBER: [0-9]+

    public abstract class ASTNode
    {
        public Token Token { get; set; }
        public ASTNode(Token token)
        {
            Token = token;
        }

    }

    public class NumberASTNode : ASTNode
    {
        public double Value => double.Parse(Token.Value);
        public NumberASTNode(Token token) : base(token)
        {
        }

    }

    public abstract class OperatorASTNode : ASTNode
    {
        protected OperatorASTNode(Token token) : base(token)
        {
        }
    }

    public abstract class UnaryOperatorASTNode : ASTNode
    {

        public UnaryOperatorASTNode(Token token,ASTNode target) : base(token)
        {
            Target = target;
        }

        public ASTNode Target { get; }
    }

    public class FactorialUnaryOperatorASTNode : UnaryOperatorASTNode
    {
        public FactorialUnaryOperatorASTNode(Token token, ASTNode target) : base(token, target)
        {
        }
    }
    public class NegationUnaryOperatorASTNode : UnaryOperatorASTNode
    {
        public NegationUnaryOperatorASTNode(Token token, ASTNode target) : base(token, target)
        {
        }
    }

    public abstract class BinaryOperatorASTNode : OperatorASTNode
    {
        
        public ASTNode Left { get; }
        public ASTNode Right { get; }
        public BinaryOperatorASTNode(Token token, ASTNode left, ASTNode right) : base(token)
        {
            Token = token;
            Left = left;
            Right = right;
        }

    }

    public class AdditionBinaryOperatorASTNode : BinaryOperatorASTNode
    {
        public AdditionBinaryOperatorASTNode(Token token, ASTNode left, ASTNode right) : base(token, left, right)
        {
        }
    }

    public class SubstractionBinaryOperatorASTNode : BinaryOperatorASTNode
    {
        public SubstractionBinaryOperatorASTNode(Token token, ASTNode left, ASTNode right) : base(token, left, right)
        {
        }
    }

    public class MultiplicationBinaryOperatorASTNode : BinaryOperatorASTNode
    {
        public MultiplicationBinaryOperatorASTNode(Token token, ASTNode left, ASTNode right) : base(token, left, right)
        {
        }
    }

    public class DivisionBinaryOperatorASTNode : BinaryOperatorASTNode
    {
        public DivisionBinaryOperatorASTNode(Token token, ASTNode left, ASTNode right) : base(token, left, right)
        {
        }
    }
}