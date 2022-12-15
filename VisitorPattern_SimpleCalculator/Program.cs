﻿// See https://aka.ms/new-console-template for more information
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
ASTGenerationVisitor visitor1= new ASTGenerationVisitor();
visitor1.Visit(tree);
ASTPrinterVisitor visitor2 = new ASTPrinterVisitor("ast.dot");
visitor2.Visit(visitor1.Root);
CalcToCTranslation cgen = new CalcToCTranslation();
cgen.Visit(visitor1.Root);
StreamWriter m_streamWriter = new StreamWriter("CodeStructure.dot");
cgen.M_TranslatedFile.PrintStructure(m_streamWriter);