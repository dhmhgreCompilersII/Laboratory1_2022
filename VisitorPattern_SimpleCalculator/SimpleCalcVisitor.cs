using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisitorPattern_SimpleCalculator {
    public class SimpleCalcVisitor<Result,Params> : ASTBaseVisitor<Result,Params> {


        public virtual Result VisitContextChildren(ASTComposite node, int context,
            params Params[] info) {
            Result result = default(Result);
            Result iResult;
            IASTIterator it = node.CreateContextIterator(context);
            ASTNode astNode;
            for (it.Init(); it.End() == false; it.Next()) {
                astNode = it.MCurNode;
                iResult = astNode.Accept<Result, Params>(this, info);
                result = Summarize(iResult, result);
            }
            return result;
        }

        public override Result VisitChildren(IASTComposite node, params Params[] info) {
            ASTComposite n = node as ASTComposite;
            Result result = default(Result);
            Result iResult;
            IASTIterator it = n.CreateIterator();
            ASTNode astNode;
            for (it.Init(); it.End() == false; it.Next()) {
                astNode = it.MCurNode;
                iResult = astNode.Accept<Result, Params>(this, info);
                result = Summarize(iResult, result);
            }
            return result;
        }

        public virtual Result VisitCompileUnit(CompileUnit node, params Params[] args) {
            return VisitChildren(node,args);
        }

        public virtual Result VisitAddition(Addition node, params Params[] args) {
            return VisitChildren(node, args);
        }

        public virtual Result VisitSubtraction(Subtraction node,params Params[] args) {
            return VisitChildren(node, args);
        }

        public virtual Result VisitMultiplication(Multiplication node, params Params[] args) {
            return VisitChildren(node, args);
        }

        public virtual Result VisitDivision(Division node, params Params[] args) {
            return VisitChildren(node, args);
        }

        public virtual Result VisitAssignment(Assignment node, params Params[] args) {
            return VisitChildren(node, args);
        }

        public virtual Result VisitIDENTIFIER(IDENTIFIER node, params Params[] args) {
            return default(Result);
        }

        public virtual Result VisitNUMBER(NUMBER node, params Params[] args) {
            return default(Result);
        }
    }
}
