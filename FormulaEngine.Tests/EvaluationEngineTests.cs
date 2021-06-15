using System;
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
            var result = EvaluationEngine.Evaluate(expression);


            //Then
            Assert.Equal(7, result);
        }

        [Fact]
        public void EvaluationTestFP001()
        {
            var expression = "1 + 1e5";
            //When
            var result = EvaluationEngine.Evaluate(expression);


            //Then
            Assert.Equal(100001, result);
        }

        [Fact]
        public void EvaluationTest_SubExpression001()
        {
            var expression = "(1 + 2)*3";
            //When
            var result = EvaluationEngine.Evaluate(expression);


            //Then
            Assert.Equal(9, result);
        }

        [Fact]
        public void EvaluationTest_SubExpression002()
        {
            var expression = "5*(3+1)";
            //When
            var result = EvaluationEngine.Evaluate(expression);


            //Then
            Assert.Equal(20, result);
        }

        [Fact]
        public void EvaluationTest_SubExpression003()
        {
            var expression = "(1 + 2)*3*4";
            //When
            var result = EvaluationEngine.Evaluate(expression);


            //Then
            Assert.Equal(36, result);
        }

        [Fact]
        public void EvaluationTest_Factorial001()
        {
            var expression = "5!";
            //When
            var result = EvaluationEngine.Evaluate(expression);


            //Then
            Assert.Equal(120, result);

        }
        [Fact]
        public void EvaluationTest_Factorial002()
        {
            var expression = "-5!";
            //When
            var result = EvaluationEngine.Evaluate(expression);


            //Then
            Assert.Equal(-120, result);
        }

        [Fact]
        public void EvaluationTest_Factorial003()
        {
            var expression = "-1*(1 + 2)!";
            //When
            var result = EvaluationEngine.Evaluate(expression);


            //Then
            Assert.Equal(-6, result);
        }

    }
}