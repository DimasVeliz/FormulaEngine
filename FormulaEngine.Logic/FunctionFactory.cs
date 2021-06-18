
using System;
using System.Collections.Generic;

namespace FormulaEngine.Logic
{
    public class FunctionFactory
    {
        public static Dictionary<TokenType, Func<Token, ExpressionNode, ExpressionNode, BinaryOperatorExpressionNode>> Operations = new Dictionary<TokenType, Func<Token, ExpressionNode, ExpressionNode, BinaryOperatorExpressionNode>>()
        {
            {TokenType.Addition,(t,l,r)=>new AdditionBinaryOperatorExpressionNode(t,l,r)},
            {TokenType.Minus,(t,l,r)=>new SubstractionBinaryOperatorExpressionNode(t,l,r)},
            {TokenType.Multiplication,(t,l,r)=>new MultiplicationBinaryOperatorExpressionNode(t,l,r)},
            {TokenType.Division,(t,l,r)=>new DivisionBinaryOperatorExpressionNode(t,l,r)},

        };


        //built-in functions (unary)
        public static double sin(double nodeValue) => Math.Sin(nodeValue);
        public static double con(double nodeValue) => Math.Cos(nodeValue);
        public static double tan(double nodeValue) => Math.Tan(nodeValue);
        public static double sqrt(double nodeValue) => Math.Sqrt(nodeValue);
        public static double log(double arg) => Math.Log(arg);



        //built-int functions (binary)
        public static double min(double left, double right) => Math.Min(left, right);
        public static double max(double left, double right) => Math.Max(left, right);

        //built-int functions (N -ary)
        public static double dimk(double one, double two, double three) => one + two + three;



    }
}