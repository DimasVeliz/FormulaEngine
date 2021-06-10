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
    }
}