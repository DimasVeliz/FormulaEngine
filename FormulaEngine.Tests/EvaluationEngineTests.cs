using System;
using System.Collections.Generic;
using FormulaEngine.Logic;
using Xunit;

namespace FormulaEngine.Tests
{

    public class EvaluationEngineTests
    {
        [Fact]
        public void EvaluationTest()
        {
            //Given
            var expression = "1 + 2 * 3";
            //When
            var result = new EvaluationEngine().Evaluate(expression);


            //Then
            Assert.Equal(7, result);
        }

        [Fact]
        public void EvaluationTestFP001()
        {
            var expression = "1 + 1e5";
            //When
            var result = new EvaluationEngine().Evaluate(expression);


            //Then
            Assert.Equal(100001, result);
        }

        [Fact]
        public void EvaluationTest_SubExpression001()
        {
            var expression = "(1 + 2)*3";
            //When
            var result = new EvaluationEngine().Evaluate(expression);


            //Then
            Assert.Equal(9, result);
        }

        [Fact]
        public void EvaluationTest_SubExpression002()
        {
            var expression = "5*(3+1)";
            //When
            var result = new EvaluationEngine().Evaluate(expression);


            //Then
            Assert.Equal(20, result);
        }

        [Fact]
        public void EvaluationTest_SubExpression003()
        {
            var expression = "(1 + 2)*3*4";
            //When
            var result = new EvaluationEngine().Evaluate(expression);


            //Then
            Assert.Equal(36, result);
        }

        [Fact]
        public void EvaluationTest_Factorial001()
        {
            var expression = "5!";
            //When
            var result = new EvaluationEngine().Evaluate(expression);


            //Then
            Assert.Equal(120, result);

        }
        [Fact]
        public void EvaluationTest_Factorial002()
        {
            var expression = "-5!";
            //When
            var result = new EvaluationEngine().Evaluate(expression);


            //Then
            Assert.Equal(-120, result);
        }

        [Fact]
        public void EvaluationTest_Factorial003()
        {
            var expression = "-1*(1 + 2)!";
            //When
            var result = new EvaluationEngine().Evaluate(expression);


            //Then
            Assert.Equal(-6, result);
        }

        [Fact]
        public void EvaluationTest_Factorial004()
        {
            var expression = "-(3 + 2)!";
            //When
            var result = new EvaluationEngine().Evaluate(expression);


            //Then
            Assert.Equal(-120, result);
        }


        [Fact]
        public void CallingAndEvaluatingFunction()
        {
            var expression = "(1 + sqrt(64))*6";
            var engine = new EvaluationEngine();
            engine.AddBuiltInFunction<FunctionFactory>();

            var result = engine.Evaluate(expression);
            Assert.Equal(54, result);
        }

         [Fact]
        public void CallingAndEvaluatingMultiArgFunction()
        {
            var expression = "(1 + max(sqrt(64),19))*5";
            var engine = new EvaluationEngine();
            engine.AddBuiltInFunction<FunctionFactory>();

            var result = engine.Evaluate(expression);
            Assert.Equal(100, result);
        }

    }
}