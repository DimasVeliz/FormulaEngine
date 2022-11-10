# FormulaEngine

The formula engine repository is a class library written in C# to lex, parse and interpret a simple mathematical programing language. It includes also a Console Application for showing the library's functionalities as well as the necessary testing.

This project is inspired and based in the works of Richard Weeks to whom I personally owe lots of knoweledge, check out his youtube channel and show some love -> : https://www.youtube.com/channel/UCUg_M6wvaS-DhHpFpW7aC6w/featured 

## the math language follows this production rules
```
         PROGRAM: STATEMENT*
       STATEMENT: (LET_STATEMENT | DEF_STATEMENT | SET_STATEMENT | EVAL_STATEMENT | PRINT_STATEMENT)
   LET_STATEMENT: 'let' VARIABLE '=>' EXPRESSION NEWLINE
   DEF_STATEMENT: 'def' LITERAL '(' VARIABLE [, VARIABLE ]*')' '=>' EXPRESSION NEWLINE
   SET_STATEMENT: 'set' VARIABLE '=>' EXPRESSION NEWLINE
  EVAL_STATEMENT: 'eval' VARIABLE '=>' EXPRESSION NEWLINE
 PRINT_STATEMENT: 'print' '(' IDENTIFIER ')' NEWLINE
         NEWLINE: '\n'
      EXPRESSION: TERM [('+'|'-') TERM]*
            TERM: FACTOR [('*'|'/') FACTOR]*
          FACTOR: '-'? EXPONENT
        EXPONENT: FACTORIAL_FACTOR [ '^' EXPONENT]*
FACTORIAL_FACTOR: PRIMARY '!'?
         PRIMARY 
                : IDENTIFIER 
                | SUB_EXPRESSION 
                | NUMBER
      IDENTIFIER: VARIABLE | FUNCTION
        FUNCTION: FUNCTION_NAME '( FUNC_ARGS ')'
       FUNC_ARGS: EXPRESSION [, EXPRESION ]*
  SUB_EXPRESSION: '(' EXPRESSION ')'
```

## Some code examples:
### Keywords
```
let set def print eval
```
### Operators available
```
 + - * / ! ^ 
 ```

### List of built in functions available 
```
//unary 
sin(x) cos(x) tan(x) sqrt(x) log(x)

//binary
min(x,y) max(x,y)

//n-ary built-in functions can be added as well
//this one returns the sum of the three args
dimk(x,y,z)

```

### variable declarations
```

let yourVariable = 5

let anotherVariable = yourVariable +1

//built in functions can be used in variable declarations

let a = 5
let b = (sqrt(64)+ max(a,80))/2

```


### simple program
```
def square(x) => x^2
def sum(x1,x2) => x1+x2


let a => sum(square(4),5) 

print(a)

set a => 5

print(a)

let b => 7

print(b)

let c => 0

print(c)


set c =>  (a + b)/2

print(c)

let k => sum(sqrt(64),1) ^ 3!

print(k)

```
