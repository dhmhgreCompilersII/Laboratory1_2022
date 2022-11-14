using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Tree;

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

            // Step 3: Add to parents
            m_parentsStack.Push(newNode);
            m_contextsStack.Push(CompileUnit.EXPRESSIONS);

            // Step 4: Visit children
            base.VisitCompileUnit(context);

            m_parentsStack.Pop();
            m_contextsStack.Pop();
            return 0;
        }

        public override int VisitAssignment(SimpleCalcParser.AssignmentContext context) {
            ASTComposite parent = m_parentsStack.Peek();
            int parentContext = m_contextsStack.Peek();

            // Step 1 : Create Node
            Assignment newNode= new Assignment(parent);

            // Step 2: Add to parent
            parent.AddChild(parentContext,newNode);
            
            // Step 3: Add to parents
            m_parentsStack.Push(newNode);
            
            // Step 4: Visit children
            m_contextsStack.Push(Assignment.IDENTIFIER);
            Visit(context.IDENTIFIER());
            m_contextsStack.Pop();

            m_contextsStack.Push(Assignment.EXPRESSION);
            Visit(context.expr());
            m_contextsStack.Pop();
           
            m_parentsStack.Pop();
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

                    // Step 3: Add to parents
                    m_parentsStack.Push(newNode);
                    // Step 4: Visit children
                    m_contextsStack.Push(Addition.LEFT);
                    Visit(context.expr(0));
                    m_contextsStack.Pop();

                    m_contextsStack.Push(Addition.RIGHT);
                    Visit(context.expr(1));
                    m_contextsStack.Pop();

                    m_parentsStack.Pop();
                    break;
                case SimpleCalcLexer.MINUS:
                    // Step 1 : Create Node
                    newNode = new Subtraction(parent);
                    break;
            }
            
            

            
            return 0;
        }

        public override int VisitMulDiv(SimpleCalcParser.MulDivContext context) {
            return base.VisitMulDiv(context);
        }

        public override int VisitTerminal(ITerminalNode node) {
            return base.VisitTerminal(node);
        }
    }
}
