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
        public readonly string []mc_contextNames = { "Expressions" };
        
        public CompileUnit( ) : 
            base(1, (int)NodeType.NT_COMPILEUNIT) {
        }

        public override Return Accept<Return,Params>(IASTBaseVisitor<Return,Params> v,
            params Params[] info) {
            SimpleCalcVisitor<Return,Params> visitor = v as SimpleCalcVisitor<Return,Params>;
            return visitor.VisitCompileUnit(this, info);
        }
    }

    public class Addition : ASTComposite {
        public const int LEFT = 0, RIGHT=1;
        public readonly string[] mc_contextNames = { "Left", "Right" };

        public Addition() :
            base(2, (int)NodeType.NT_ADDITION) {
        }

        public override Return Accept<Return, Params>(IASTBaseVisitor<Return, Params> v,
            params Params[] info) {
            SimpleCalcVisitor<Return, Params> visitor = v as SimpleCalcVisitor<Return, Params>;
            return visitor.VisitAddition(this, info);
        }
    }
    public class Subtraction : ASTComposite {
        public const int LEFT = 0, RIGHT = 1;
        public readonly string[] mc_contextNames = { "Left", "Right" };

        public Subtraction() :
            base(2, (int)NodeType.NT_SUBTRACTION) {
        }

        public override Return Accept<Return, Params>(IASTBaseVisitor<Return, Params> v,
            params Params[] info) {
            SimpleCalcVisitor<Return, Params> visitor = v as SimpleCalcVisitor<Return, Params>;
            return visitor.VisitSubtraction(this, info);
        }
    }
    public class Multiplication : ASTComposite {
        public const int LEFT = 0, RIGHT = 1;
        public readonly string[] mc_contextNames = { "Left", "Right" };

        public Multiplication() :
            base(2, (int)NodeType.NT_MULTIPLICATION) {
        }

        public override Return Accept<Return, Params>(IASTBaseVisitor<Return, Params> v,
            params Params[] info) {
            SimpleCalcVisitor<Return, Params> visitor = v as SimpleCalcVisitor<Return, Params>;
            return visitor.VisitMultiplication(this, info);
        }
    }
    public class Division : ASTComposite {
        public const int LEFT = 0, RIGHT = 1;
        public readonly string[] mc_contextNames = { "Left", "Right" };

        public Division() :
            base(2, (int)NodeType.NT_DIVISION) {
        }

        public override Return Accept<Return, Params>(IASTBaseVisitor<Return, Params> v,
            params Params[] info) {
            SimpleCalcVisitor<Return, Params> visitor = v as SimpleCalcVisitor<Return, Params>;
            return visitor.VisitDivision(this, info);
        }
    }
    public class Assignment : ASTComposite {
        public const int IDENTIFIER = 0, EXPRESSION = 1;
        public readonly string[] mc_contextNames = { "LValue", "Expressions" };

        public Assignment() :
            base(2, (int)NodeType.NT_ASSIGNMENT) {
        }

        public override Return Accept<Return, Params>(IASTBaseVisitor<Return, Params> v,
            params Params[] info) {
            SimpleCalcVisitor<Return, Params> visitor = v as SimpleCalcVisitor<Return, Params>;
            return visitor.VisitAssignment(this,info);
        }
    }

    public class IDENTIFIER : ASTLeaf {
        private string m_text;
        
        public string M_Text {
            get { return m_text; }
            set { m_text = value; }
        }

        public IDENTIFIER(string strliteral) :
            base(strliteral,(int)NodeType.NT_VARIABLE) {
            M_Text = strliteral;
        }

        public override Return Accept<Return, Params>(IASTBaseVisitor<Return, Params> v,
            params Params[] info) {
            SimpleCalcVisitor<Return, Params> visitor = v as SimpleCalcVisitor<Return, Params>;
            return visitor.VisitIDENTIFIER(this, info);
        }
    }
    public class  NUMBER : ASTLeaf {
        private string m_text;

        public string M_Text {
            get { return m_text; }
            set { m_text = value; }
        }
        public NUMBER(string strliteral) :
            base(strliteral, (int)NodeType.NT_NUMBER) {
            M_Text = strliteral;
        }

        public override Return Accept<Return, Params>(IASTBaseVisitor<Return, Params> v,
            params Params[] info) {
            SimpleCalcVisitor<Return, Params> visitor = v as SimpleCalcVisitor<Return, Params>;
            return visitor.VisitNUMBER(this, info);
        }
    }
}
