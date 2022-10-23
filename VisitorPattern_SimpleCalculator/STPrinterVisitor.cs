using System.Diagnostics;
using Antlr4.Runtime.Tree;

namespace VisitorPattern_SimpleCalculator; 


public class STPrinterVisitor : SimpleCalcBaseVisitor<int> {
    private StreamWriter dotFile = new StreamWriter("SyntaxTree.dot");
    private static int ms_serialCounter = 0;
    private Stack<string> m_labelsStack = new Stack<string>();
    public override int VisitCompileUnit(SimpleCalcParser.CompileUnitContext context) {
        
        // 1. Create a label
        string label = "CompileUnit_" + ms_serialCounter++ ;
        
        // 2. Push label for descentants
        m_labelsStack.Push(label);
        
        // 3. Print prolog
        dotFile.WriteLine("digraph G{");
        
        base.VisitCompileUnit(context);
        
        dotFile.WriteLine("}");
        dotFile.Close();
        
        // Prepare the process dot to run
        ProcessStartInfo start = new ProcessStartInfo();
        // Enter in the command line arguments, everything you would enter after the executable name itself
        start.Arguments = "-Tgif " +
                          Path.GetFileName("SyntaxTree.dot") + " -o " +
                          Path.GetFileNameWithoutExtension("SyntaxTree") + ".gif";
        // Enter the executable to run, including the complete path
        start.FileName = "dot";
        // Do you want to show a console window?
        start.WindowStyle = ProcessWindowStyle.Hidden;
        start.CreateNoWindow = true;
        int exitCode;

        // Run the external process & wait for it to finish
        using (Process proc = Process.Start(start)) {
            proc.WaitForExit();

            // Retrieve the app's exit code
            exitCode = proc.ExitCode;
        }
        return 0;
    }

    public override int VisitAssignment(SimpleCalcParser.AssignmentContext context) {
        string parent = m_labelsStack.Peek();
        // 1. Create a label
        string label = "Assignment_" + ms_serialCounter++;
        
        // 2. Push label for descentants
        m_labelsStack.Push(label);
        
        // 3. Link with parent
        dotFile.WriteLine("\""+parent+"\"->\""+label+"\";");
        
        base.VisitAssignment(context);
        
        // 4. Pop label
        m_labelsStack.Pop();

        return 0;
    }
   
    public override int VisitAddSub(SimpleCalcParser.AddSubContext context) {
        string parent = m_labelsStack.Peek();
        // 1. Create a label
        string label = "AddSub_" + ms_serialCounter++ ;
        
        // 2. Push label for descentants
        m_labelsStack.Push(label);
        
        // 3. Link with parent
        dotFile.WriteLine("\""+parent+"\"->\""+label+"\";");
        
        base.VisitAddSub(context);
        
        // 4. Pop label
        m_labelsStack.Pop();

        return 0;
    }

    public override int VisitMulDiv(SimpleCalcParser.MulDivContext context) {
        string parent = m_labelsStack.Peek();
        // 1. Create a label
        string label = "MulDiv_" + ms_serialCounter++ ;
        
        // 2. Push label for descentants
        m_labelsStack.Push(label);
        
        // 3. Link with parent
        dotFile.WriteLine("\""+parent+"\"->\""+label+"\";");
        
        base.VisitMulDiv(context);
        
        // 4. Pop label
        m_labelsStack.Pop();

        return 0;
    }
  

    public override int VisitParen(SimpleCalcParser.ParenContext context) {
        string parent = m_labelsStack.Peek();
        // 1. Create a label
        string label = "Parenthesis_" + ms_serialCounter++ ;
        
        // 2. Push label for descentants
        m_labelsStack.Push(label);
        
        // 3. Link with parent
        dotFile.WriteLine("\""+parent+"\"->\""+label+"\";");
        
        base.VisitParen(context);
        
        // 4. Pop label
        m_labelsStack.Pop();

        return 0;
    }

    public override int VisitTerminal(ITerminalNode node) {
        string parent = m_labelsStack.Peek();
        string label="";
        switch (node.Symbol.Type) {
            case SimpleCalcLexer.IDENTIFIER:
                // 1. Create a label
                label = "IDENTIFIER_" + ms_serialCounter++ ;
                // 2. Link with parent
                dotFile.WriteLine("\""+parent+"\"->\""+label+"\";");
                break;
            case SimpleCalcLexer.NUMBER:
                // 1. Create a label
                label = "NUMBER_" + ms_serialCounter++;
                // 2. Link with parent
                dotFile.WriteLine("\""+parent+"\"->\""+label+"\";");
                break;
        }
        
      
        return 0;
    }
}