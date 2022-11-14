using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisitorPattern_SimpleCalculator {
    public abstract class ASTNode {
        private int m_type;
        private int m_serialNumber;
        private string m_nodeName;
        private ASTComposite m_parent;
        private static int ms_serialCounter;
        
        public int MType => m_type;

        public int MSerialNumber => m_serialNumber;

        public string MNodeName => m_nodeName;

        public ASTComposite MParent => m_parent;

        public static int MsSerialCounter => ms_serialCounter;

        public ASTNode(int mType, ASTComposite mParent) {
            m_type = mType;
            m_parent = mParent;
        }
    }

    public abstract class ASTComposite : ASTNode {

        List<ASTNode> []m_children;

        public int MContexts => m_children.Length;

        public ASTComposite(int contexts,int mType, ASTComposite mParent) :
            base(mType, mParent) {
            m_children = new List<ASTNode>[contexts];
        }

        public ASTNode GetChild(int context, int index = 0) {
            return m_children[context][index];
        }

        public void AddChild(int context, ASTNode child) {
            m_children[context].Add(child);
        }
    }

    public abstract class ASTLeaf : ASTNode {
        private string m_stringLiteral;

        public string MStringLiteral => m_stringLiteral;

        public ASTLeaf(string leafLiteral,int mType, ASTComposite mParent) :
            base(mType, mParent) {
            m_stringLiteral=leafLiteral;
        }
    }
}
