using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Tree;

namespace VisitorPattern_SimpleCalculator {
    
    public  class ASTNode : IASTVisitableNode, ILabelled {
        private int m_type;
        private int m_serialNumber;
        // open for modification by subclasses
        protected string m_nodeName;
        private ASTComposite m_parent;
        private static int ms_serialCounter;
        // Either will use this member or there has to be
        // a static dictionary to map ASTNode to the corresponding
        // link hierarchy node. Trees have many applications 
        // most of the times they extend to an application domain
        // So this link is necessary.
        private object m_hierarchyBridgeLink;

        public object HierarchyBridgeLink {
            get => m_hierarchyBridgeLink;
            set => m_hierarchyBridgeLink = value ??
                throw new ArgumentNullException(nameof(value));
        }

        public int MType => m_type;

        // Node label is open for changes as is virtual
        public virtual int MSerialNumber => m_serialNumber;

        // Node label is open for changes as is virtual
        public virtual string MNodeName => m_nodeName;

        public ASTComposite MParent => m_parent;

        public static int MsSerialCounter => ms_serialCounter;

        public ASTNode(int mType, ASTComposite mParent) {
            m_type = mType;
            m_parent = mParent;
            m_serialNumber = ms_serialCounter++;
            m_nodeName = "Node" +GetType().Name +m_serialNumber;
        }

        public virtual Return Accept<Return, Params>(IASTBaseVisitor<Return, Params> v,
            params Params[] info) {
            return v.Visit(this);
        }
    }

    public class ASTComposite : ASTNode, IASTComposite {

        List<ASTNode> []m_children;
        
        public int MContexts => m_children.Length;

        public ASTComposite(int contexts,int mType, ASTComposite mParent) :
            base(mType, mParent) {
            m_children = new List<ASTNode>[contexts];
            for (int i = 0; i < contexts; i++) {
                m_children[i] = new List<ASTNode>();
            }
        }

        public int GetNumberOfContextNodes (int context) {
            if (context < m_children.Length) {
                return m_children[context].Count;
            }
            else {
                throw new ArgumentOutOfRangeException("context index out of range");
            }
        }

        public IEnumerable<ASTNode> GetContextChildren(int context) {
            if (context < m_children.Length) {
                foreach (ASTNode node in m_children[context]) {
                    yield return node;
                }
            } else {
                throw new ArgumentOutOfRangeException("node index out of range");
            }
        }

        public IEnumerable<ASTNode> GetChildren() {
            foreach (ASTNode node in this) {
                yield return node;
            }
        }

        public ASTNode GetChild(int context, int index = 0) {
            if (context < m_children.Length) {
                if (index < m_children[context].Count) {
                    return m_children[context][index];
                }
                else {
                    throw new ArgumentOutOfRangeException("node index out of range");
                }
            }
            else {
                throw new ArgumentOutOfRangeException("context index out of range");
            }
        }

        public void AddChild(int context, ASTNode child) {
            if (context < m_children.Length) {
                m_children[context].Add(child);
            } else {
                throw new ArgumentOutOfRangeException("context index out of range");
            }
        }

        public IEnumerator<IASTVisitableNode> GetEnumerator() {
            return new ASTCompositeEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public IASTIterator CreateIterator() {
            return new ASTChildrenIterator(this);
        }

        public IASTIterator CreateContextIterator(int context) {
            return new ASTContextIterator(this,context);
        }

        public override Return Accept<Return, Params>(IASTBaseVisitor<Return,
            Params> v, params Params[] info) {
            return v.Visit(this, info);
        }
    }
    
    public class ASTLeaf : ASTNode {
        private string m_stringLiteral;

        public string MStringLiteral => m_stringLiteral;

        public ASTLeaf(string leafLiteral,int mType, ASTComposite mParent) :
            base(mType, mParent) {
            m_stringLiteral=leafLiteral;
        }
        public override Return Accept<Return, Params>(IASTBaseVisitor<Return,
            Params> v, params Params[] info) {
            return v.Visit(this, info);
        }
    }
}
