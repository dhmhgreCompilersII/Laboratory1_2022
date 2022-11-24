using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisitorPattern_SimpleCalculator {
    public interface IASTBaseVisitor<Return> {
        Return Visit(IASTNode node, params object[] info);
        Return VisitChildren(IEnumerable<IASTNode> children, params object[] info);
    }

    public abstract class ASTBaseVisitor<Return> : IASTBaseVisitor<Return> {

        // Visit a specific node and send a variable number of
        // arguments. The responsibility of the type and sequence
        // of arguments is on the user. ( box/unboxing for scalars)
        public virtual Return Visit(IASTNode node, params object[] info) {
            return node.Accept<Return>(this);
        }

        // Visit the children of a specific node and summarize the 
        // results by the visiting each child 
        public virtual Return VisitChildren(IEnumerable<IASTNode> children, params object[] info) {
            Return result = default(Return);
            Return iResult;
            foreach (IASTNode astNode in children) {
                iResult= astNode.Accept<Return>(this);
                result = Summarize(iResult,result);
            }
            return result;
        }
        
        public virtual Return Summarize(Return iresult, Return result) {
            return iresult;
        }
    }
}
