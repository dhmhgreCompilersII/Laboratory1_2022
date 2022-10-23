grammar SimpleCalc;

/* Parser Rules */
compileUnit : (expr ';')+ ;

expr : NUMBER                   #NUMBER
     | IDENTIFIER               #IDENTIFIER
     | IDENTIFIER '=' expr      #Assignment
     | expr op=(PLUS|'-') expr  #AddSub
     | expr op=('*'|DIV) expr   #MulDiv
     | LP expr RP               #Paren
     ;
     
 /* Lexer Rules */
SEMI : ';';
ASSIGN : '=';
PLUS : '+';
MINUS : '-';
MULT  : '*';
DIV : '/';
LP : '(';
RP : ')';     
NUMBER : '0'|[1-9][0-9]*;
IDENTIFIER : [a-zA-Z][a-zA-Z0-9_]* ;

WS : [ \n\r\t]-> skip;
