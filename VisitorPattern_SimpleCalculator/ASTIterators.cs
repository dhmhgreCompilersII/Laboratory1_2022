using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisitorPattern_SimpleCalculator {
    

    public class ASTChildrenIterator : IASTIterator {
        private ASTComposite m_sourceNode;

        private int mi_curContext;
        private int mi_curNode;
        private ASTNode m_curNode;
        private bool m_endFlag;

        public ASTNode MCurNode => m_curNode;

        public ASTChildrenIterator(ASTComposite sourceNode) {
            m_sourceNode = sourceNode;
            mi_curContext = 0;
            mi_curNode = 0;
            m_endFlag = false;
        }

        public void Init() {
            mi_curContext = 0;
            mi_curNode = 0;
            
            if (m_sourceNode.MContexts !=0 &&
                m_sourceNode.GetNumberOfContextNodes(mi_curContext) != 0) {
                m_curNode = m_sourceNode.GetChild(mi_curContext, mi_curNode);
                m_endFlag = false;
            }
            else {
                m_endFlag = true;
            }

        }

        public bool End() {
            return m_endFlag;
        }

        public void Next() {
            mi_curNode++;
            if (mi_curNode < m_sourceNode.GetNumberOfContextNodes(mi_curContext)) {
                m_curNode = m_sourceNode.GetChild(mi_curContext, mi_curNode);
            }
            else {
                mi_curContext++;
                if (mi_curContext < m_sourceNode.MContexts) {
                    mi_curNode = 0;
                    m_curNode = m_sourceNode.GetChild(mi_curContext, mi_curNode);
                }
                else {
                    m_endFlag = true;
                }
            }
        }
    }

    public class ASTContextIterator : IASTIterator {
        private ASTComposite m_sourceNode;
        private int m_Context;

        private int mi_curNode;
        private ASTNode m_curNode;
        private bool m_endFlag;

        public ASTNode MCurNode => m_curNode;

        public ASTContextIterator(ASTComposite sourceNode, int context) {
            m_sourceNode = sourceNode;
            m_Context = context;
            m_curNode = m_sourceNode.GetChild(m_Context, mi_curNode);
        }

        public void Init() {
            mi_curNode = 0;
            m_endFlag = false;
        }

        public bool End() {
            return m_endFlag;
        }

        public void Next() {
            mi_curNode++; // 
            if (mi_curNode < m_sourceNode.GetNumberOfContextNodes(m_Context)) {
                m_curNode = m_sourceNode.GetChild(m_Context, mi_curNode);
            } else {
                m_endFlag = true;
            }
        }
    }

    public class ASTCompositeEnumerator : IEnumerator<IASTVisitableNode> {
        private ASTComposite m_sourceNode;

        private int mi_curContext;
        private int mi_curNode;
        private ASTNode m_curNode;

        public ASTCompositeEnumerator(ASTComposite sourceNode) {
            m_sourceNode = sourceNode;
            mi_curNode = -1;  // must be initialized here
            mi_curContext = 0;
        }

        public IASTVisitableNode Current => m_curNode;

        public bool MoveNext() {
            mi_curNode++;
            if (mi_curNode < m_sourceNode.GetNumberOfContextNodes(mi_curContext)) {
                m_curNode = m_sourceNode.GetChild(mi_curContext, mi_curNode);
            } else {
                mi_curContext++;
                if (mi_curContext < m_sourceNode.MContexts) {
                    mi_curNode = 0;
                    m_curNode = m_sourceNode.GetChild(mi_curContext, mi_curNode);
                } else {
                    return false;
                }
            }
            return true;
        }

        public void Reset() {  // This is does not actually called by the foreach
                               // Thats why initialization is mandatory in the 
                               // constructor
            mi_curContext = 0;
            mi_curNode = -1;
        }

        object IEnumerator.Current => Current;

        public void Dispose() { }
    }
}
