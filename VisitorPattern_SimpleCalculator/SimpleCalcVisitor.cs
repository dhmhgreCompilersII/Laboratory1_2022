using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisitorPattern_SimpleCalculator {
    public class SimpleCalcVisitor<Result,Params> : ASTBaseVisitor<Result,Params> {
        
        public Result VisitCompileUnit(IASTNode node, params Params[] args) {
            CompileUnit cu = node as CompileUnit;
            if (cu != null) {
                return VisitChildren(cu.GetChildren(),args);
            }
            else {
                throw new InvalidCastException("parameter must be CompileUnit");
            }
        }

        public Result VisitAddition(IASTNode node, params Params[] args) {
            Addition cu = node as Addition;
            if (cu != null) {
                return VisitChildren(cu.GetChildren(),args);
            } else {
                throw new InvalidCastException("parameter must be Addition");
            }
        }

        public Result VisitSubtraction(IASTNode node,params Params[] args) {
            Subtraction cu = node as Subtraction;
            if (cu != null) {
                return VisitChildren(cu.GetChildren(),args);
            } else {
                throw new InvalidCastException("parameter must be Subtraction");
            }
        }

        public Result VisitMultiplication(IASTNode node, params Params[] args) {
            Multiplication cu = node as Multiplication;
            if (cu != null) {
                return VisitChildren(cu.GetChildren(),args);
            } else {
                throw new InvalidCastException("parameter must be Multiplication");
            }
        }

        public Result VisitDivision(IASTNode node, params Params[] args) {
            Division cu = node as Division;
            if (cu != null) {
                return VisitChildren(cu.GetChildren(),args);
            } else {
                throw new InvalidCastException("parameter must be Multiplication");
            }
        }

        public Result VisitAssignment(IASTNode node, params Params[] args) {
            Assignment cu = node as Assignment;
            if (cu != null) {
                return VisitChildren(cu.GetChildren(),args);
            } else {
                throw new InvalidCastException("parameter must be Multiplication");
            }
        }

        public Result VisitIDENTIFIER(IASTNode node, params Params[] args) {
            IDENTIFIER cu = node as IDENTIFIER;
            return default(Result);
        }

        public Result VisitNUMBER(IASTNode node, params Params[] args) {
            NUMBER cu = node as NUMBER;
            return default(Result);
        }
    }
}
