grammar SimpleCalc;

/* Parser Rules */
compileUnit : (expr ';')+ ;

expr : NUMBER                   #NUMBER
     | IDENTIFIER               #IDENTIFIER
     | expr op=('*'|DIV) expr   #MulDiv
     | expr op=(PLUS|'-') expr  #AddSub
     | IDENTIFIER '=' expr      #Assignment       
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
