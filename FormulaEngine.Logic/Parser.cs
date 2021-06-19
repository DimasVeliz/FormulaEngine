
using System;
using System.Collections.Generic;
using System.Linq;

namespace FormulaEngine.Logic
{
    /// <summary>
    /// Implements the following Production Rules
    ///         PROGRAM: STATEMENT*
    ///       STATEMENT: (LET_STATEMENT | DEF_STATEMENT | SET_STATEMENT | EVAL_STATEMENT | PRINT_STATEMENT)
    ///   LET_STATEMENT: 'let' VARIABLE '=>' EXPRESSION NEWLINE
    ///   DEF_STATEMENT: 'def' LITERAL '(' VARIABLE [, VARIABLE ]*')' '=>' EXPRESSION NEWLINE
    ///   SET_STATEMENT: 'set' VARIABLE '=>' EXPRESSION NEWLINE
    ///  EVAL_STATEMENT: 'eval' VARIABLE '=>' EXPRESSION NEWLINE
    /// PRINT_STATEMENT: 'print' '(' IDENTIFIER ')' NEWLINE
    ///         NEWLINE: '\n'
    ///      EXPRESSION: TERM [('+'|'-') TERM]*
    ///            TERM: FACTOR [('*'|'/') FACTOR]*
    ///          FACTOR: '-'? EXPONENT
    ///        EXPONENT: FACTORIAL_FACTOR [ '^' EXPONENT]*
    ///FACTORIAL_FACTOR: PRIMARY '!'?
    ///         PRIMARY 
    ///                : IDENTIFIER 
    ///                | SUB_EXPRESSION 
    ///                | NUMBER
    ///      IDENTIFIER: VARIABLE | FUNCTION
    ///        FUNCTION: FUNCTION_NAME '( FUNC_ARGS ')'
    ///       FUNC_ARGS: EXPRESSION [, EXPRESION ]*
    ///  SUB_EXPRESSION: '(' EXPRESSION ')'
    /// </summary>
    public class Parser
    {
        public Dictionary<TokenType, Func<Statement>> mapperFactory = new Dictionary<TokenType, Func<Statement>>();

        static readonly TokenType[] TERM_OPERATORS = new TokenType[] { TokenType.Addition, TokenType.Minus };
        static readonly TokenType[] FACTOR_OPERATORS = new TokenType[] { TokenType.Multiplication, TokenType.Division };

        private readonly Lexer lexer;
        private readonly SymbolTable _symbolTable;

        public Parser(Lexer lexer, SymbolTable symbolTable)
        {
            this.lexer = lexer;
            this._symbolTable = symbolTable;

            mapperFactory.Add(TokenType.Let, () => ParseLetStatement());
            mapperFactory.Add(TokenType.Set, () => ParseSetStatement());
            mapperFactory.Add(TokenType.Eval, () => ParseEvalStatement());
            mapperFactory.Add(TokenType.Print, () => ParsePrintStatement());
            mapperFactory.Add(TokenType.Def, () => ParseDefStatement());

        }
        public MProgram ParseProgram()
        {
            MProgram program = new MProgram();
            while (lexer.Peek().Type != TokenType.EOF)
            {
                Token next = lexer.Peek();

                if (next.Type == TokenType.New_Line)
                {
                    Accept();
                }

                program.Statements.Add(mapperFactory[next.Type]());
            }

            return program;
        }
        private FuncDefStatement ParseDefStatement()
        {
            Expect(TokenType.Def);
            Accept();
            Expect(TokenType.Identifier);
            var functionName = Accept();
            Expect(TokenType.OpenParen);
            Accept();

            var functionDefinition = new FuncDefStatement { Function = new FunctionExpressionNode(functionName) };

            do
            {
                Expect(TokenType.Identifier);
                functionDefinition.ParameterNames.Add(new VariableIdentifierExpressionNode(Accept()));
                if (isNext(TokenType.Arg_Separator))
                {
                    Accept();
                }
                else
                { break; }

            } while (true);

            return functionDefinition;
        }

        private PrintStatement ParsePrintStatement()
        {
            Expect(TokenType.Print);
            Accept();
            Expect(TokenType.OpenParen);
            Accept();

            var expression = Parse();
            Expect(TokenType.CloseParen);
            Accept();

            return new PrintStatement { Body = expression };
        }

        private EvalStatement ParseEvalStatement()
        {
            Expect(TokenType.Eval);
            Accept();
            Expect(TokenType.Identifier);
            var variableName = Accept();
            Expect(TokenType.Goes_To);
            Accept();
            var expression = Parse();

            return new EvalStatement { Variable = new VariableIdentifierExpressionNode(variableName), Expression = expression };
        }

        private SetStatement ParseSetStatement()
        {
            Expect(TokenType.Set);
            Accept();
            Expect(TokenType.Identifier);
            var variableName = Accept();
            Expect(TokenType.Goes_To);
            Accept();
            var expression = Parse();

            return new SetStatement { Variable = new VariableIdentifierExpressionNode(variableName), Expression = expression };
        }


        public LetStatement ParseLetStatement()
        {
            Expect(TokenType.Let);
            Accept();
            Expect(TokenType.Identifier);
            var variableName = Accept();
            Expect(TokenType.Goes_To);
            Accept();
            var expression = Parse();

            return new LetStatement() { Variable = new VariableIdentifierExpressionNode(variableName), Expression = expression };
        }
        public Expression Parse()
        {
            Expression expression = new Expression();
            expression.Root = ParseExpression();

            return expression;
        }
        public ExpressionNode ParseExpression()
        {
            ExpressionNode node;


            if (TryParseExpression(out node))
            {

                return node;
            }
            else
            {
                throw new Exception("Unable to parse the expression");
            }
        }

        private bool TryParseExpression(out ExpressionNode node)
        {
            if (TryParseTerm(out node))
            {
                while (isNext(TERM_OPERATORS))
                {
                    var op = Accept();

                    if (TryParseTerm(out var rightSide))
                    {
                        node = CreateBinaryOperator(op, node, rightSide);

                    }
                    else
                    {

                        throw new Exception($"Failed to parse rule Term ");
                    }
                }
            }




            return node != null;
        }

        ///TERM: FACTOR [('*'|'/') FACTOR]*
        private bool TryParseTerm(out ExpressionNode node)
        {
            if (TryParseFactor(out node))
            {
                while (isNext(FACTOR_OPERATORS))
                {
                    var op = Accept();

                    if (TryParseExponent(out var rightSide))
                    {
                        node = CreateBinaryOperator(op, node, rightSide);

                    }
                    else
                    {

                        throw new Exception($"Failed to parse rule Term ");
                    }
                }
            }




            return node != null;
        }


        ///   FACTOR: '-'? EXPONENT
        private bool TryParseFactor(out ExpressionNode node)
        {
            node = null;
            if (isNext(TokenType.Minus))
            {

                var op = Accept();
                if (TryParseExponent(out node))
                {
                    node = new NegationUnaryOperatorExpressionNode(op, node);
                }
                else
                {
                    throw new Exception($"Unable to parse the factor rule ");

                }
            }
            else
            {
                TryParseExponent(out node);
            }
            return node != null;
        }
        ///   EXPONENT: FACTORIAL_FACTOR [ '^' EXPONENT]*
        private bool TryParseExponent(out ExpressionNode node)
        {
            if (TryParseFactorialFactor(out node))
            {
                if (isNext(TokenType.Exponent))
                {
                    var op = Accept(); //consuming the ^ to call the factory later

                    if (TryParseExponent(out var rightSide))
                    {

                        node = new ExponentBinaryOperatorExpressionNode(op, node, rightSide);
                    }
                }
            }

            return node != null;
        }

        ///     FACTORIAL_FACTOR: PRIMARY '!'?
        private bool TryParseFactorialFactor(out ExpressionNode node)
        {
            if (TryParsePrimary(out node))
            {
                if (isNext(TokenType.Factorial))
                {
                    node = new FactorialUnaryOperatorExpressionNode(Accept(), node);
                }
            }

            return node != null;
        }

        ///         PRIMARY 
        ///                : IDENTIFIER 
        ///                | SUB_EXPRESSION 
        ///                | NUMBER
        private bool TryParsePrimary(out ExpressionNode node)
        {
            node = null;

            if (TryParseIdentifier(out node))
                return true;
            if (TryParseNumber(out node))
                return true;

            if (TryParseSubExpression(out node))
                return true;

            return false;
        }

        ///      IDENTIFIER
        ///                : VARIABLE 
        ///                | FUNCTION
        ///        FUNCTION: FUNCTION_NAME '( FUNC_ARGS ')'
        ///       FUNC_ARGS: EXPRESSION [, EXPRESION ]*
        private bool TryParseIdentifier(out ExpressionNode node)
        {
            if (TryParseVariable(out node))
                return true;
            if (TryParseFunction(out node))
                return true;

            return false;
        }
        private bool TryParseVariable(out ExpressionNode node)
        {
            node = null;
            if (isNext(TokenType.Identifier))
            {
                var token = lexer.Peek();
                //var entry = _symbolTable.Get(token.Value);
                //if (entry == null)
                //{
                //throw new Exception($"Undefined Identifier {token.Value} at line {lexer.LineNumber}, col : {lexer.LinePosition}");
                //
                //}
                //if (entry.Type == EntryType.Variable)
                //{
                node = new VariableIdentifierExpressionNode(Accept());
                // }

            }
            return node != null;

        }
        ///        FUNCTION: FUNCTION_NAME '( FUNC_ARGS ')'
        ///       FUNC_ARGS: EXPRESSION [, EXPRESION ]*
        private bool TryParseFunction(out ExpressionNode node)
        {
            node = null;
            if (isNext(TokenType.Identifier))
            {
                var token = lexer.Peek();
                var entry = _symbolTable.Get(token.Value);
                if (entry == null)
                {
                    throw new Exception($"Undefined Identifier {token.Value} ");

                }
                if (entry.Type == EntryType.Function)
                {
                    node = new FunctionExpressionNode(Accept());
                    Expect(TokenType.OpenParen);
                    Accept();
                    if (TryParseFuncArgs(out var listOfArgs))
                    {
                        (node as FunctionExpressionNode).ArgumentsNodes.AddRange(listOfArgs);
                    }
                    Expect(TokenType.CloseParen);
                    Accept();

                }

            }
            return node != null;
        }

        ///       FUNC_ARGS: EXPRESSION [, EXPRESION ]*
        private bool TryParseFuncArgs(out List<ExpressionNode> argumentNodes)
        {
            argumentNodes = new List<ExpressionNode>();

            if (TryParseExpression(out var expNode))
            {
                argumentNodes.Add(expNode);
                while (isNext(TokenType.Arg_Separator))
                {
                    Accept();  //consuming the , element
                    if (TryParseExpression(out expNode))
                        argumentNodes.Add(expNode);
                    else
                    {
                        throw new Exception($"Couldnt parse function arguments ");
                    }

                }
            }

            return argumentNodes.Any();
        }

        ///NUMBER: [0-9]+
        private bool TryParseNumber(out ExpressionNode node)
        {
            node = null;
            if (isNext(TokenType.Number))
            {
                node = new NumberExpressionNode(Accept());
            }

            return node != null;

        }

        private bool TryParseSubExpression(out ExpressionNode node)
        {
            node = null;
            if (isNext(TokenType.OpenParen))
            {
                Accept(); //consumes the open parent ( 

                if (TryParseExpression(out node))
                {

                    Expect(TokenType.CloseParen);
                    Accept(); //consumes the close parent )
                }
            }
            return node != null;

        }

        private bool isNext(params TokenType[] possibleTokens)
        {
            return isNext(x => possibleTokens.Any(pT => pT == x));
        }
        private bool isNext(Predicate<TokenType> match) => match(lexer.Peek().Type);
        private Token Accept() => lexer.ReadNext();

        private ExpressionNode CreateBinaryOperator(Token op, ExpressionNode left, ExpressionNode right)
        {
            return FunctionFactory.Operations[op.Type](op, left, right);
        }
        private void Expect(TokenType expected)
        {
            if (!isNext(expected))
            {
                throw new System.Exception($"Unxpected: {lexer.Peek().Value} ");
            }
        }
    }
}