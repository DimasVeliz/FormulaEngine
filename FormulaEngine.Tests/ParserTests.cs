using System;
using System.Collections.Generic;
using FormulaEngine.Logic;
using Xunit;

namespace FormulaEngine.Tests
{

    public class ParserTests
    {
        [Fact]
        public void ParseTest()
        {
            var expression = "1 + 2";

            var ast = (BinaryOperatorASTNode)new Parser(new Lexer(new SourceScanner(expression)), new SymbolTable()).Parse();
            
            Assert.NotNull(ast);

            Assert.Equal(TokenType.Addition, ast.Token.Type);
            Assert.Equal(TokenType.Number, ast.Left.Token.Type);
            Assert.Equal(TokenType.Number, ast.Right.Token.Type);

            Assert.Equal(1, (ast.Left as NumberASTNode).Value);
            Assert.Equal(2, (ast.Right as NumberASTNode).Value);

        }

        [Fact]
        public void ParseTest02()
        {
            var expression = "1 + 2*3";

            var ast = (BinaryOperatorASTNode)new Parser(new Lexer(new SourceScanner(expression)), new SymbolTable()).Parse();


            Assert.NotNull(ast);

            Assert.Equal(TokenType.Addition, ast.Token.Type);
            Assert.Equal(TokenType.Number, ast.Left.Token.Type);

            Assert.Equal(TokenType.Multiplication, ast.Right.Token.Type);
            Assert.Equal(TokenType.Number, (ast.Right as BinaryOperatorASTNode).Left.Token.Type);
            Assert.Equal(TokenType.Number, (ast.Right as BinaryOperatorASTNode).Right.Token.Type);


            Assert.Equal(1, (ast.Left as NumberASTNode).Value);
            Assert.Equal(2, ((ast.Right as BinaryOperatorASTNode).Left as NumberASTNode).Value);
            Assert.Equal(3, ((ast.Right as BinaryOperatorASTNode).Right as NumberASTNode).Value);

        }
        [Fact]
        public void ParserTest03()
        {
            var expression = "1 +";



            Assert.Throws<Exception>(() => new Parser(new Lexer(new SourceScanner(expression)), new SymbolTable()).Parse());


        }
        [Fact]
        public void ParserTestUnbalanced()
        {
            var expression = "1 +(2+3))";



            Assert.Throws<Exception>(() => new Parser(new Lexer(new SourceScanner(expression)), new SymbolTable()).Parse());


        }


        [Fact]
        public void ParserTestVariables()
        {
            var expression = "1 + x*2";

            var variablesInvolved = new List<VNameValue>()
            {
                new VNameValue{Name="x", Value=5}
            };
            
            var engine = new EvaluationEngine();
            engine.AddBuiltInGlobalVariables(variablesInvolved);
            var result = engine.Evaluate(expression);

            Assert.Equal(11, result);

        }

        [Fact]
        public void ParserTestVariables2()
        {
            var expression = "x! + x*2";

            var variablesInvolved = new List<VNameValue>()
            {
                new VNameValue{Name="x", Value=5}
            };
            
            var engine = new EvaluationEngine();
            engine.AddBuiltInGlobalVariables(variablesInvolved);
            var result = engine.Evaluate(expression);

            Assert.Equal(130, result);

        }
    }
}