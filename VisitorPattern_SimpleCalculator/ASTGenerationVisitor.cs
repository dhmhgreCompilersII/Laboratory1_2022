using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Tree;
using static SimpleCalcParser;

namespace VisitorPattern_SimpleCalculator {
    internal class ASTGenerationVisitor : SimpleCalcBaseVisitor<int> {
        private ASTNode m_root;
        private Stack<ASTComposite> m_parentsStack = new Stack<ASTComposite>();
        private Stack<int> m_contextsStack = new Stack<int>();
        public override int VisitCompileUnit(SimpleCalcParser.CompileUnitContext context) {

            // Step 1 : Create Node
            CompileUnit newNode = new CompileUnit();

            // Step 2: Update root
            m_root = newNode;
            
            // Step 3: Visit children
            this.VisitElementsInContext(context.expr(), CompileUnit.EXPRESSIONS,
                m_contextsStack,newNode,m_parentsStack);
            
            return 0;
        }

        public override int VisitAssignment(SimpleCalcParser.AssignmentContext context) {
            ASTComposite parent = m_parentsStack.Peek();
            int parentContext = m_contextsStack.Peek();

            // Step 1 : Create Node
            Assignment newNode= new Assignment(parent);

            // Step 2: Add to parent
            parent.AddChild(parentContext,newNode);
           
            // Step 3: Visit children
            this.VisitTerminalInContext(context,context.IDENTIFIER().Symbol,
                Assignment.IDENTIFIER, m_contextsStack,newNode);

            this.VisitElementInContext(context.expr(), Assignment.EXPRESSION,
                m_contextsStack, newNode, m_parentsStack);
            
           return 0;
        }

        public override int VisitAddSub(SimpleCalcParser.AddSubContext context) {
            ASTComposite parent = m_parentsStack.Peek();
            int parentContext = m_contextsStack.Peek();
            ASTComposite newNode=null;
            switch (context.op.Type) {
                case SimpleCalcLexer.PLUS:
                    // Step 1 : Create Node
                    newNode = new Addition(parent);
                    // Step 2: Add to parent
                    parent.AddChild(parentContext, newNode);

                    // Step 3: Visit children
                    this.VisitElementInContext(context.expr(0),
                        Addition.LEFT, m_contextsStack,
                         newNode, m_parentsStack);

                    this.VisitElementInContext(context.expr(1),
                        Addition.RIGHT, m_contextsStack,
                        newNode, m_parentsStack);

                   
                    break;
                case SimpleCalcLexer.MINUS:
                    // Step 1 : Create Node
                    newNode = new Subtraction(parent);

                    // Step 2: Add to parent
                    parent.AddChild(parentContext, newNode);

                   // Step 3: Visit children
                    this.VisitElementInContext(context.expr(0),
                        Subtraction.LEFT, m_contextsStack,
                        newNode, m_parentsStack);

                    this.VisitElementInContext(context.expr(1),
                        Subtraction.RIGHT, m_contextsStack,
                        newNode, m_parentsStack);

                    
                    break;
            }
            return 0;
        }

        public override int VisitMulDiv(SimpleCalcParser.MulDivContext context) {
            ASTComposite parent = m_parentsStack.Peek();
            int parentContext = m_contextsStack.Peek();
            ASTComposite newNode = null;
            switch (context.op.Type) {
                case SimpleCalcLexer.MULT:
                    // Step 1 : Create Node
                    newNode = new Addition(parent);
                    // Step 2: Add to parent
                    parent.AddChild(parentContext, newNode);

                   // Step 3: Visit children
                    this.VisitElementInContext(context.expr(0),
                        Multiplication.LEFT, m_contextsStack,
                        newNode, m_parentsStack);

                    this.VisitElementInContext(context.expr(1),
                        Multiplication.RIGHT, m_contextsStack,
                        newNode, m_parentsStack);

                   
                    break;
                case SimpleCalcLexer.DIV:
                    // Step 1 : Create Node
                    newNode = new Subtraction(parent);

                    // Step 2: Add to parent
                    parent.AddChild(parentContext, newNode);
                    
                    // Step 3: Visit children
                    this.VisitElementInContext(context.expr(0),
                        Division.LEFT, m_contextsStack,
                        newNode, m_parentsStack);

                    this.VisitElementInContext(context.expr(1),
                        Division.RIGHT, m_contextsStack,
                        newNode, m_parentsStack);

                    
                    break;
            }

            return 0;
        }

        public override int VisitTerminal(ITerminalNode node) {
            ASTComposite parent=m_parentsStack.Peek();
            int parentContext = m_contextsStack.Peek();
            switch (node.Symbol.Type) {
                case SimpleCalcLexer.IDENTIFIER:
                    // Step 1 : Create Node
                    IDENTIFIER newnode1 = new IDENTIFIER(node.Symbol.Text,parent);

                    // Step 2: Add to parent
                    parent.AddChild(parentContext, newnode1);
                    
                    break;
                case SimpleCalcLexer.NUMBER:
                    // Step 1 : Create Node
                    NUMBER newnode2 = new NUMBER(node.Symbol.Text, parent);

                    // Step 2: Add to parent
                    parent.AddChild(parentContext, newnode2);
                    
                    break;
            }

            return 0;
        }
    }
}
