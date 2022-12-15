using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisitorPattern_SimpleCalculator {


    public abstract class CEmmitableCodeContainer {

        protected int m_nestingLevel = 0;

        public int NestingLevel {
            get => m_nestingLevel;
            set => m_nestingLevel = value;
        }

        public abstract ASTNode M_ASTNode { get; }

        protected string MNodeName => M_ASTNode.MNodeName;
        protected string MParentMNodeName => M_ASTNode.MParent.MNodeName;

        public CEmmitableCodeContainer() {
            
        }

        public abstract CodeContainer AssemblyCodeContainer();
        public abstract void PrintStructure(StreamWriter m_ostream);
        public abstract void AddCode(String code,int context);
        public abstract void AddCode(CEmmitableCodeContainer code, int context );
        public abstract string EmmitStdout();
        public abstract void EmmitToFile(StreamWriter f);
        public virtual void EnterScope() {
            m_nestingLevel++;
        }
        public virtual void LeaveScope() {
            if (m_nestingLevel > 0) {
                m_nestingLevel--;
            } else {
                throw new Exception("Non-matched nesting");
            }
        }
        public abstract void AddNewLine(int context);
    }

    public abstract class CComboContainer : CEmmitableCodeContainer {
        private ASTComposite m_treeNode;
        private static int ms_clusterSerial = 0;

        public override ASTNode M_ASTNode => m_treeNode;

        public CEmmitableCodeContainer MParent {
            get {
                return M_ASTNode.MParent.HierarchyBridgeLink as CEmmitableCodeContainer;
            }
        }

        public IEnumerable<CEmmitableCodeContainer> MChildren  {
            get{
                foreach (ASTNode child in m_treeNode) {
                    yield return child.HierarchyBridgeLink as CEmmitableCodeContainer;
                }
            }
        }

        public CComboContainer(int contexts,int type) {
            m_treeNode = new ASTComposite(contexts, type);
            m_treeNode.HierarchyBridgeLink = this;
        }

        protected virtual CodeContainer AssemblyContext(int ct) {
            CodeContainer rep = new CodeContainer(-1);
            for (int i = 0; i < m_treeNode.GetNumberOfContextNodes(ct); i++) {
                CEmmitableCodeContainer x = GetChild(ct, i); ;
                rep.AddCode(x.AssemblyCodeContainer(),ct);
            }
            return rep;
        }

        public virtual CEmmitableCodeContainer GetChild(int context, int index=0) {
            return m_treeNode.GetChild(context, index).HierarchyBridgeLink as CEmmitableCodeContainer;
        }

        public override void AddCode(string code, int context) {
            CodeContainer container = new CodeContainer(-1);
            container.AddCode(code, -1);
            m_treeNode.AddChild(context,container.TreeNode);
        }

        public override void AddCode(CEmmitableCodeContainer code, int context=1) {
            m_treeNode.AddChild(context, code.M_ASTNode);
        }

        public override void AddNewLine(int context=-1) {
            CodeContainer container = new CodeContainer(-1);
            container.AddNewLine(-1);
            m_treeNode.AddChild(context, container.TreeNode);
        }

        public override string EmmitStdout() {
            string s = AssemblyCodeContainer().ToString();
            Console.WriteLine(s);
            return s;
        }
        public override string ToString() {
            string s = AssemblyCodeContainer().ToString();
            return s;
        }

        public override void EmmitToFile(StreamWriter f) {
            string s = AssemblyCodeContainer().ToString();
            f.WriteLine(s);
        }

        protected void ExtractSubgraphs(StreamWriter m_ostream, int context,string contextName) {
            int contextNodes = m_treeNode.GetNumberOfContextNodes(context);
            ASTNode child;
            if (contextNodes!=0) {
                m_ostream.WriteLine("\tsubgraph cluster" + ms_clusterSerial++ + "{");
                m_ostream.WriteLine("\t\tnode [style=filled,color=white];");
                m_ostream.WriteLine("\t\tstyle=filled;");
                m_ostream.WriteLine("\t\tcolor=lightgrey;");
                m_ostream.Write("\t\t");
                for (int i = 0; i < contextNodes; i++) {
                    child = m_treeNode.GetChild(context, i);
                    m_ostream.Write(child.MNodeName + ";");
                }

                m_ostream.WriteLine("\n\t\tlabel=" + contextName + ";");
                m_ostream.WriteLine("\t}");
            }
        }
    }

    public class CodeContainer :CEmmitableCodeContainer {
        private ASTLeaf m_treeNode;
        StringBuilder m_repository = new StringBuilder();

        public ASTLeaf TreeNode => m_treeNode;

        public override ASTNode M_ASTNode =>m_treeNode;

        public CodeContainer(int type) {
            m_treeNode = new ASTLeaf(String.Empty,type);
            m_treeNode.HierarchyBridgeLink = this;
        }

        public override void AddCode(string code, int context) {
            string[] lines = code.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines) {
                m_repository.Append(line);
                if (code.Contains('\n')) {
                    m_repository.Append("\r\n");
                    m_repository.Append(new string('\t', m_nestingLevel));
                }
            }
        }

        public override void AddCode(CEmmitableCodeContainer code, int context) {
            string str = code.ToString();
            AddCode(str, context);
        }

        public override void AddNewLine(int context) {
            m_repository.Append("\r\n");
            m_repository.Append(new string('\t', m_nestingLevel));
        }

        public override CodeContainer AssemblyCodeContainer() {
            return this;
        }

        public override string EmmitStdout() {
            System.Console.WriteLine(m_repository.ToString());
            return m_repository.ToString();
        }

        public override void EmmitToFile(StreamWriter f) {
            f.WriteLine(m_repository.ToString());
        }

        public override void EnterScope() {
            base.EnterScope();
            AddNewLine(-1);
        }

        public override void LeaveScope() {
            base.LeaveScope();
            AddNewLine(-1);
        }
        public override string ToString() {
            return m_repository.ToString();
        }
        public override void PrintStructure(StreamWriter m_ostream) {
            m_ostream.WriteLine("\"{0}\"->\"{1}\"", M_ASTNode.MParent.MNodeName, M_ASTNode.MNodeName);
        }
    }
}
