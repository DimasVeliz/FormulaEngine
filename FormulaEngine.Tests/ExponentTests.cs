using System;
using System.Collections.Generic;
using FormulaEngine.Logic;
using Xunit;

namespace FormulaEngine.Tests
{

    public class ExponentTests
    {
        [Fact]
        public void Test001()
        {
            //Given
            var expression = "2^3";
            //When
            var result = new EvaluationEngine().Evaluate(expression,new List<VNameValue>());


            //Then
            Assert.Equal(8, result);
        }

        [Fact]
        public void Test002()
        {
            //Given
            var expression = "2^(2+3)";
            //When
            var result = new EvaluationEngine().Evaluate(expression,new List<VNameValue>());


            //Then
            Assert.Equal(32, result);
        }
        [Fact]
        public void Test003()
        {
            //Given
            var expression = "(1 + 2)^ 3";
            //When
            var result = new EvaluationEngine().Evaluate(expression,new List<VNameValue>());


            //Then
            Assert.Equal(27, result);
        }
        [Fact]
        public void Test004()
        {
            //Given
            var expression = "2 ^( 2^3)";
            //When
            var result = new EvaluationEngine().Evaluate(expression,new List<VNameValue>());


            //Then
            Assert.Equal(256, result);
        }

        [Fact]
        public void Test005()
        {
            //Given
            var expression = "( 2^3)^2";
            //When
            var result = new EvaluationEngine().Evaluate(expression,new List<VNameValue>());


            //Then
            Assert.Equal(64, result);
        }
        [Fact]
        public void Test006()
        {
            //Given
            var expression = "2^2^3";
            //When
            var result = new EvaluationEngine().Evaluate(expression,new List<VNameValue>());


            //Then
            Assert.Equal(256, result);
        }

        [Fact]
        public void Test007()
        {
            //Given
            var expression = "2 ^3!^2";
            //When
            var result = new EvaluationEngine().Evaluate(expression,new List<VNameValue>());


            //Then
            Assert.Equal(68719476736, result);
        }
    }
}