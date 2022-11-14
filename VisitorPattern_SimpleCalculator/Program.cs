// See https://aka.ms/new-console-template for more information
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using VisitorPattern_SimpleCalculator;

StreamReader aStream = new StreamReader("input.txt");
AntlrInputStream ANTRStream = new AntlrInputStream(aStream);
SimpleCalcLexer lexer  = new SimpleCalcLexer(ANTRStream);
CommonTokenStream tokens = new CommonTokenStream(lexer);
SimpleCalcParser parser = new SimpleCalcParser(tokens);
IParseTree tree =parser.compileUnit();
STPrinterVisitor visitor = new STPrinterVisitor();
visitor.Visit(tree);
