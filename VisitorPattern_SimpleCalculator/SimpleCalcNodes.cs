﻿using System;
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

    public static class SimpleCalcExtensions {
        public static NodeType GetNodeType(this ASTNode node) {
            return (NodeType)node.MType;
        }

        public static void VisitNode(this AbstractParseTreeVisitor<int> visitor,
            ParserRuleContext parent,int context) {

        }
    }

    public class CompileUnit : ASTComposite {
        public const int EXPRESSIONS = 0;
        
        public CompileUnit( ) : 
            base(1, (int)NodeType.NT_COMPILEUNIT, null) {
        }
    }

    public class Addition : ASTComposite {
        public const int LEFT = 0, RIGHT=1;

        public Addition(ASTComposite mParent) :
            base(2, (int)NodeType.NT_ADDITION, mParent) {
        }
    }
    public class Subtraction : ASTComposite {
        public const int LEFT = 0, RIGHT = 1;

        public Subtraction(ASTComposite mParent) :
            base(2, (int)NodeType.NT_SUBTRACTION, mParent) {
        }
    }
    public class Multiplication : ASTComposite {
        public const int LEFT = 0, RIGHT = 1;

        public Multiplication(ASTComposite mParent) :
            base(2, (int)NodeType.NT_MULTIPLICATION, mParent) {
        }
    }
    public class Division : ASTComposite {
        public const int LEFT = 0, RIGHT = 1;

        public Division(ASTComposite mParent) :
            base(2, (int)NodeType.NT_DIVISION, mParent) {
        }
    }
    public class Assignment : ASTComposite {
        public const int IDENTIFIER = 0, EXPRESSION = 1;

        public Assignment(ASTComposite mParent) :
            base(2, (int)NodeType.NT_ASSIGNMENT, mParent) {
        }
    }

    public class IDENTIFIER : ASTLeaf {
        
        public IDENTIFIER(string strliteral,ASTComposite mParent) :
            base(strliteral,(int)NodeType.NT_VARIABLE, mParent) {
        }
    }
    public class  NUMBER : ASTLeaf {
        public NUMBER(string strliteral, ASTComposite mParent) :
            base(strliteral, (int)NodeType.NT_NUMBER, mParent) {
        }
    }


}
