using System;
using System.Collections.Generic;
using FormulaEngine.Logic;
using Xunit;

namespace FormulaEngine.Tests
{

    public class BuiltInFunctionEvaluationTests
    {
        [Fact]
        public void SqrtTest()
        {
            var functionAST = new FunctionASTNode(new Token(TokenType.Identifier,0,"sqrt"));

            functionAST.ArgumentsNodes.Add(new NumberASTNode(new Token(TokenType.Number,1,"81")));

            var engine = new EvaluationEngine();
            engine.AddBuiltInFunction<FunctionFactory>();
            var result =engine.Evaluate(functionAST);

            Assert.Equal(9,result);
        }

         [Fact]
        public void MaxTest()
        {
            var functionAST = new FunctionASTNode(new Token(TokenType.Identifier,0,"max"));

            functionAST.ArgumentsNodes.Add(new NumberASTNode(new Token(TokenType.Number,1,"81")));
            functionAST.ArgumentsNodes.Add(new NumberASTNode(new Token(TokenType.Number,1,"90")));

            var engine = new EvaluationEngine();
            engine.AddBuiltInFunction<FunctionFactory>();
            var result =engine.Evaluate(functionAST);

            Assert.Equal(90,result);
        }

         [Fact]
        public void CustomBuiltInDimTest()
        {
            var functionAST = new FunctionASTNode(new Token(TokenType.Identifier,0,"dimk"));

            functionAST.ArgumentsNodes.Add(new NumberASTNode(new Token(TokenType.Number,1,"81")));
            functionAST.ArgumentsNodes.Add(new NumberASTNode(new Token(TokenType.Number,1,"20")));
            functionAST.ArgumentsNodes.Add(new NumberASTNode(new Token(TokenType.Number,1,"2")));

            var engine = new EvaluationEngine();
            engine.AddBuiltInFunction<FunctionFactory>();
            var result =engine.Evaluate(functionAST);

            Assert.Equal(103,result);
        }

        

    }
}