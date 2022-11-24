using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisitorPattern_SimpleCalculator {
    public class SimpleCalcVisitor<Result> : ASTBaseVisitor<Result> {
        
        public Result VisitCompileUnit(IASTNode node) {
            CompileUnit cu = node as CompileUnit;
            if (cu != null) {
                return VisitChildren(cu.GetChildren());
            }
            else {
                throw new InvalidCastException("parameter must be CompileUnit");
            }
        }

        public Result VisitAddition(IASTNode node) {
            Addition cu = node as Addition;
            if (cu != null) {
                return VisitChildren(cu.GetChildren());
            } else {
                throw new InvalidCastException("parameter must be Addition");
            }
        }

        public Result VisitSubtraction(IASTNode node) {
            Subtraction cu = node as Subtraction;
            if (cu != null) {
                return VisitChildren(cu.GetChildren());
            } else {
                throw new InvalidCastException("parameter must be Subtraction");
            }
        }

        public Result VisitMultiplication(IASTNode node) {
            Multiplication cu = node as Multiplication;
            if (cu != null) {
                return VisitChildren(cu.GetChildren());
            } else {
                throw new InvalidCastException("parameter must be Multiplication");
            }
        }

        public Result VisitDivision(IASTNode node) {
            Division cu = node as Division;
            if (cu != null) {
                return VisitChildren(cu.GetChildren());
            } else {
                throw new InvalidCastException("parameter must be Multiplication");
            }
        }

        public Result VisitAssignment(IASTNode node) {
            Assignment cu = node as Assignment;
            if (cu != null) {
                return VisitChildren(cu.GetChildren());
            } else {
                throw new InvalidCastException("parameter must be Multiplication");
            }
        }

        public Result VisitIDENTIFIER(IASTNode node) {
            IDENTIFIER cu = node as IDENTIFIER;
            return default(Result);
        }

        public Result VisitNUMBER(IASTNode node) {
            NUMBER cu = node as NUMBER;
            return default(Result);
        }
    }
}
