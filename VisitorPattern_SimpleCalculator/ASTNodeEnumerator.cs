using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisitorPattern_SimpleCalculator {
    internal class ASTCompositeEnumerator : IEnumerator<IASTNode> {
        private ASTComposite m_sourceNode;

        private int mi_curContext;
        private int mi_curNode;
        private ASTNode m_curNode;

        public ASTCompositeEnumerator(ASTComposite sourceNode) {
            m_sourceNode = sourceNode;
            mi_curNode = -1;
        }

        public IASTNode Current => m_curNode;

        public bool MoveNext() {
            mi_curNode++;
            if (mi_curNode < m_sourceNode.GetNumberOfContextNode(mi_curContext)) {
                m_curNode =  m_sourceNode.GetChild(mi_curContext,mi_curNode);
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

        public void Reset() {
            mi_curContext = -1;
            mi_curNode = -1;
        }

        object IEnumerator.Current => Current;
        
        public void Dispose() { }
    }
}
