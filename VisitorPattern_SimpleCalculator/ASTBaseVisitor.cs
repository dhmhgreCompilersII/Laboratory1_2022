using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisitorPattern_SimpleCalculator {
    

    public abstract class ASTBaseVisitor<Return,Params> : IASTBaseVisitor<Return,Params> {

        // Visit a specific node and send a variable number of
        // arguments. The responsibility of the type and sequence
        // of arguments is on the user. ( box/unboxing for scalars)
        public virtual Return Visit(IASTVisitableNode node, params Params[] info) {
            return node.Accept<Return,Params>(this);
        }

        // Visit the children of a specific node and summarize the 
        // results by the visiting each child 
        public virtual Return VisitChildren(IASTComposite node, params Params[] info) {
            Return result = default(Return);
            Return iResult;
            foreach (IASTVisitableNode astNode in node) {
                iResult= astNode.Accept<Return,Params>(this,info);
                result = Summarize(iResult,result);
            }
            return result;
        }
        
        public virtual Return Summarize(Return iresult, Return result) {
            return iresult;
        }
    }
}
