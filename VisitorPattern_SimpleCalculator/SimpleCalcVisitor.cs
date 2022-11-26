using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisitorPattern_SimpleCalculator {
    public class SimpleCalcVisitor<Result,Params> : ASTBaseVisitor<Result,Params> {
        
        public virtual Result VisitCompileUnit(ASTNode node, params Params[] args) {
            CompileUnit cu = node as CompileUnit;
            if (cu != null) {
                return VisitChildren(cu.GetChildren(),args);
            }
            else {
                throw new InvalidCastException("parameter must be CompileUnit");
            }
        }

        public virtual Result VisitAddition(ASTNode node, params Params[] args) {
            Addition cu = node as Addition;
            if (cu != null) {
                return VisitChildren(cu.GetChildren(),args);
            } else {
                throw new InvalidCastException("parameter must be Addition");
            }
        }

        public virtual Result VisitSubtraction(ASTNode node,params Params[] args) {
            Subtraction cu = node as Subtraction;
            if (cu != null) {
                return VisitChildren(cu.GetChildren(),args);
            } else {
                throw new InvalidCastException("parameter must be Subtraction");
            }
        }

        public virtual Result VisitMultiplication(ASTNode node, params Params[] args) {
            Multiplication cu = node as Multiplication;
            if (cu != null) {
                return VisitChildren(cu.GetChildren(),args);
            } else {
                throw new InvalidCastException("parameter must be Multiplication");
            }
        }

        public virtual Result VisitDivision(ASTNode node, params Params[] args) {
            Division cu = node as Division;
            if (cu != null) {
                return VisitChildren(cu.GetChildren(),args);
            } else {
                throw new InvalidCastException("parameter must be Multiplication");
            }
        }

        public virtual Result VisitAssignment(ASTNode node, params Params[] args) {
            Assignment cu = node as Assignment;
            if (cu != null) {
                return VisitChildren(cu.GetChildren(),args);
            } else {
                throw new InvalidCastException("parameter must be Multiplication");
            }
        }

        public virtual Result VisitIDENTIFIER(ASTNode node, params Params[] args) {
            IDENTIFIER cu = node as IDENTIFIER;
            return default(Result);
        }

        public virtual Result VisitNUMBER(ASTNode node, params Params[] args) {
            NUMBER cu = node as NUMBER;
            return default(Result);
        }
    }
}
