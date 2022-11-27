using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisitorPattern_SimpleCalculator {
    internal class ASTPrinterVisitor : SimpleCalcVisitor<int, ASTNode> {

        StreamWriter m_writer;
        private string m_outputFilename;
        private int m_clusterSerial;
        private int ms_clusterSerialCounter;

        public ASTPrinterVisitor(string outputFilename) {
            m_outputFilename = outputFilename;
            m_writer = new StreamWriter(m_outputFilename);
        }

        private void CreateContextSubgraph(ASTComposite node, int contextindex, string contextName) {

            m_writer.WriteLine($"\tsubgraph cluster{m_clusterSerial++} {{");
            m_writer.WriteLine("\t\tnode [style=filled,color=white];");
            m_writer.WriteLine("\t\tnode [style=filled,color=lightgrey];");
            bool first = true;
            foreach (ASTNode child in node.GetContextChildren(contextindex)) {
                if (first) {
                   m_writer.Write("\t\t");
                }
                first = false;
                m_writer.Write("\"" + child.MNodeName + "\"");
            }
            m_writer.WriteLine($";");
            m_writer.WriteLine($"\t\tlabel = \"{contextName}\";");
            m_writer.WriteLine($"\t}}");
        }


        public override int VisitCompileUnit(CompileUnit node, params ASTNode[] args) {
            CompileUnit n = node as CompileUnit;
            if (n == null) {
                throw new InvalidCastException("Expected CompileUnit type");
            }

            m_writer.WriteLine("digraph{");

            CreateContextSubgraph(n, CompileUnit.EXPRESSIONS,
                n.mc_contextNames[CompileUnit.EXPRESSIONS]);

            base.VisitCompileUnit(node, n);

            m_writer.WriteLine("}");
            m_writer.Close();

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "dot.exe";
            startInfo.Arguments = " -Tgif AST.dot -o AST.gif";

            Process proc = Process.Start(startInfo);

            return 0;
        }

        public override int VisitAssignment(Assignment node, params ASTNode[] args) {
            Assignment n = node as Assignment;
            if (n == null) {
                throw new InvalidCastException("Expected Assignment type");
            }

            m_writer.WriteLine($"\"{args[0].MNodeName}\"->\"{n.MNodeName}\";");

            CreateContextSubgraph(n, Assignment.IDENTIFIER,
                n.mc_contextNames[Assignment.IDENTIFIER]);

            CreateContextSubgraph(n, Assignment.EXPRESSION,
                n.mc_contextNames[Assignment.EXPRESSION]);

            base.VisitAssignment(node, n);

            return 0;
        }

        public override int VisitAddition(Addition node, params ASTNode[] args) {
            Addition n = node as Addition;
            if (n == null) {
                throw new InvalidCastException("Expected Addition type");
            }
            m_writer.WriteLine($"\"{args[0].MNodeName}\"->\"{n.MNodeName}\";");

            CreateContextSubgraph(n, Addition.LEFT,
                n.mc_contextNames[Addition.LEFT]);

            CreateContextSubgraph(n, Addition.RIGHT,
                n.mc_contextNames[Addition.RIGHT]);

            base.VisitAddition(node, n);

            return 0;
        }

        public override int VisitSubtraction(Subtraction node, params ASTNode[] args) {
            Subtraction n = node as Subtraction;
            if (n == null) {
                throw new InvalidCastException("Expected Subtraction type");
            }
            m_writer.WriteLine($"\"{args[0].MNodeName}\"->\"{n.MNodeName}\";");

            CreateContextSubgraph(n, Subtraction.LEFT,
                n.mc_contextNames[Subtraction.LEFT]);

            CreateContextSubgraph(n, Subtraction.RIGHT,
                n.mc_contextNames[Subtraction.RIGHT]);

            base.VisitSubtraction(node, n);

            return 0;
        }

        public override int VisitMultiplication(Multiplication node, params ASTNode[] args) {
            Multiplication n = node as Multiplication;
            if (n == null) {
                throw new InvalidCastException("Expected Multiplication type");
            }
            m_writer.WriteLine($"\"{args[0].MNodeName}\"->\"{n.MNodeName}\";");

            CreateContextSubgraph(n, Multiplication.LEFT,
                n.mc_contextNames[Multiplication.LEFT]);

            CreateContextSubgraph(n, Multiplication.RIGHT,
                n.mc_contextNames[Multiplication.RIGHT]);

            base.VisitMultiplication(node, n);

            return 0;
        }

        public override int VisitDivision(Division node, params ASTNode[] args) {
            Division n = node as Division;
            if (n == null) {
                throw new InvalidCastException("Expected Division type");
            }
            m_writer.WriteLine($"\"{args[0].MNodeName}\"->\"{n.MNodeName}\";");

            CreateContextSubgraph(n, Division.LEFT,
                n.mc_contextNames[Division.LEFT]);

            CreateContextSubgraph(n, Division.RIGHT,
                n.mc_contextNames[Division.RIGHT]);

            base.VisitDivision(node, n);

            return 0;
        }

        public override int VisitIDENTIFIER(IDENTIFIER node, params ASTNode[] args) {
            IDENTIFIER n = node as IDENTIFIER;
            if (n == null) {
                throw new InvalidCastException("Expected IDENTIFIER type");
            }
            m_writer.WriteLine($"\"{args[0].MNodeName}\"->\"{n.MNodeName}\";");

            return 0;
        }

        public override int VisitNUMBER(NUMBER node, params ASTNode[] args) {
            NUMBER n = node as NUMBER;
            if (n == null) {
                throw new InvalidCastException("Expected NUMBER type");
            }
            m_writer.WriteLine($"\"{args[0].MNodeName}\"->\"{n.MNodeName}\";");

            return 0;
        }
    }
}
