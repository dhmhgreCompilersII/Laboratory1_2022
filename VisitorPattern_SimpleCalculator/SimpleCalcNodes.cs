using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace VisitorPattern_SimpleCalculator {

    public enum NodeType {
        NT_NA=-1, NT_COMPILEUNIT, NT_ADDITION, NT_SUBTRACTION,
        NT_MULTIPLICATION, NT_DIVISION, NT_VARIABLE, NT_NUMBER,
        NT_ASSIGNMENT
    }

    public class CompileUnit : ASTComposite {
        public const int EXPRESSIONS = 0;
        
        public CompileUnit( ) : 
            base(1, (int)NodeType.NT_COMPILEUNIT, null) {
        }

        public override Return Accept<Return>(IASTBaseVisitor<Return> v,
            params object[] info) {
            SimpleCalcVisitor<Return> visitor = v as SimpleCalcVisitor<Return>;
            return visitor.VisitCompileUnit(this);
        }
    }

    public class Addition : ASTComposite {
        public const int LEFT = 0, RIGHT=1;

        public Addition(ASTComposite mParent) :
            base(2, (int)NodeType.NT_ADDITION, mParent) {
        }

        public override Return Accept<Return>(IASTBaseVisitor<Return> v,
            params object[] info) {
            SimpleCalcVisitor<Return> visitor = v as SimpleCalcVisitor<Return>;
            return visitor.VisitAddition(this);
        }
    }
    public class Subtraction : ASTComposite {
        public const int LEFT = 0, RIGHT = 1;

        public Subtraction(ASTComposite mParent) :
            base(2, (int)NodeType.NT_SUBTRACTION, mParent) {
        }

        public override Return Accept<Return>(IASTBaseVisitor<Return> v,
            params object[] info) {
            SimpleCalcVisitor<Return> visitor = v as SimpleCalcVisitor<Return>;
            return visitor.VisitSubtraction(this);
        }
    }
    public class Multiplication : ASTComposite {
        public const int LEFT = 0, RIGHT = 1;

        public Multiplication(ASTComposite mParent) :
            base(2, (int)NodeType.NT_MULTIPLICATION, mParent) {
        }

        public override Return Accept<Return>(IASTBaseVisitor<Return> v,
            params object[] info) {
            SimpleCalcVisitor<Return> visitor = v as SimpleCalcVisitor<Return>;
            return visitor.VisitMultiplication(this);
        }
    }
    public class Division : ASTComposite {
        public const int LEFT = 0, RIGHT = 1;

        public Division(ASTComposite mParent) :
            base(2, (int)NodeType.NT_DIVISION, mParent) {
        }

        public override Return Accept<Return>(IASTBaseVisitor<Return> v,
            params object[] info) {
            SimpleCalcVisitor<Return> visitor = v as SimpleCalcVisitor<Return>;
            return visitor.VisitDivision(this);
        }
    }
    public class Assignment : ASTComposite {
        public const int IDENTIFIER = 0, EXPRESSION = 1;

        public Assignment(ASTComposite mParent) :
            base(2, (int)NodeType.NT_ASSIGNMENT, mParent) {
        }

        public override Return Accept<Return>(IASTBaseVisitor<Return> v,
            params object[] info) {
            SimpleCalcVisitor<Return> visitor = v as SimpleCalcVisitor<Return>;
            return visitor.VisitAssignment(this);
        }
    }

    public class IDENTIFIER : ASTLeaf {
        
        public IDENTIFIER(string strliteral,ASTComposite mParent) :
            base(strliteral,(int)NodeType.NT_VARIABLE, mParent) {
        }

        public override Return Accept<Return>(IASTBaseVisitor<Return> v,
            params object[] info) {
            SimpleCalcVisitor<Return> visitor = v as SimpleCalcVisitor<Return>;
            return visitor.VisitIDENTIFIER(this);
        }
    }
    public class  NUMBER : ASTLeaf {
        public NUMBER(string strliteral, ASTComposite mParent) :
            base(strliteral, (int)NodeType.NT_NUMBER, mParent) {
        }

        public override Return Accept<Return>(IASTBaseVisitor<Return> v,
            params object[] info) {
            SimpleCalcVisitor<Return> visitor = v as SimpleCalcVisitor<Return>;
            return visitor.VisitNUMBER(this);
        }
    }
}
