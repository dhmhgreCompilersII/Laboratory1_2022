using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisitorPattern_SimpleCalculator {

    public enum ContainerType {
        NT_NA = -1,
        CT_FILE,
        CT_FUNDEF,
        CT_COMPOUNDSTATEMENT
    }

    public class CCFile : CComboContainer {
        public const int PREPROCESSOR_DIRECTIVES = 0, GLOBAL_VARIABLES = 1, FUNCTION_DEFINITIONS = 2;

        public readonly string[] mc_contextNames =
            { "PreprocessorDirectives", "GlobalVariables", "FunctionDefinitions" };

        private HashSet<string> m_globalVarSymbolTable = new HashSet<string>();
        private HashSet<string> m_FunctionsSymbolTable = new HashSet<string>();

        private CCFunctionDefinition m_mainDefinition = null;
        public CCFunctionDefinition MainDefinition => m_mainDefinition;

        public void DeclareGlobalVariable(string varname) {
            CodeContainer rep;
            if (!m_globalVarSymbolTable.Contains(varname)) {
                m_globalVarSymbolTable.Add(varname);
                rep = new CodeContainer(-1);
                rep.AddCode("float " + varname + ";\n", GLOBAL_VARIABLES);
                AddCode(rep, GLOBAL_VARIABLES);
            }
        }

        public void DeclareFunction(string funname, string funheader) {
            CodeContainer rep;
            if (!m_FunctionsSymbolTable.Contains(funname)) {
                rep = new CodeContainer(-1);
                m_globalVarSymbolTable.Add(funname);
                rep.AddCode(funheader + ";\n", GLOBAL_VARIABLES);
                AddCode(rep, GLOBAL_VARIABLES);
            }
        }

        public CCFile(bool withStartUpFunction) : 
            base(3, (int)ContainerType.CT_FILE) {

            if (withStartUpFunction) {
                m_mainDefinition = new CMainFunctionDefinition(this);
                AddCode(m_mainDefinition, FUNCTION_DEFINITIONS);
            }
        }

        public override CodeContainer AssemblyCodeContainer() {
            CodeContainer rep = new CodeContainer(-1);

            rep.AddCode(AssemblyContext(PREPROCESSOR_DIRECTIVES), -1);
            rep.AddCode(AssemblyContext(GLOBAL_VARIABLES), -1);
            rep.AddCode(AssemblyContext(FUNCTION_DEFINITIONS), -1);
            return rep;
        }

        public override void PrintStructure(StreamWriter m_ostream) {

            m_ostream.WriteLine("digraph {");

            ExtractSubgraphs(m_ostream, GLOBAL_VARIABLES, mc_contextNames[GLOBAL_VARIABLES]);
            ExtractSubgraphs(m_ostream, PREPROCESSOR_DIRECTIVES, mc_contextNames[PREPROCESSOR_DIRECTIVES]);
            ExtractSubgraphs(m_ostream, FUNCTION_DEFINITIONS, mc_contextNames[FUNCTION_DEFINITIONS]);

            foreach (CEmmitableCodeContainer child in MChildren) {
                child.PrintStructure(m_ostream);
            }

            m_ostream.WriteLine("}");
            m_ostream.Close();

            // Prepare the process to run
            ProcessStartInfo start = new ProcessStartInfo();
            // Enter in the command line arguments, everything you would enter after the executable name itself
            start.Arguments = "-Tgif CodeStructure.dot " + " -o" + " CodeStructure.gif";
            // Enter the executable to run, including the complete path
            start.FileName = "dot";
            // Do you want to show a console window?
            start.WindowStyle = ProcessWindowStyle.Hidden;
            start.CreateNoWindow = true;
            int exitCode;

            // Run the external process & wait for it to finish
            using (Process proc = Process.Start(start)) {
                proc.WaitForExit();

                // Retrieve the app's exit code
                exitCode = proc.ExitCode;
            }
        }
    }
    public class CCFunctionDefinition : CComboContainer {
        public const int HEADER = 0, BODY = 1;

        public readonly string[] mc_contextNames =
            { "FunctionDefinition_Header", "FunctionDefinition_Body" };

        private HashSet<string> m_localSymbolTable = new HashSet<string>();
        private CCFile m_file;

        public virtual void DeclareVariable(string varname, bool isread) {
            CodeContainer rep;
            if (!m_localSymbolTable.Contains(varname)) {
                if (isread) {
                    m_file.DeclareGlobalVariable(varname);
                } else {
                    rep = new CodeContainer(-1);
                    m_localSymbolTable.Add(varname);

                    rep.AddCode("float " + varname + ";\n", -1);
                    CCompoundStatement compoundst = GetChild(BODY, 0) as CCompoundStatement;
                    compoundst.AddCode(rep, CCompoundStatement.DECLARATIONS);
                }
            }
        }

        public void AddVariableToLocalSymbolTable(string varname) {
            if (!m_localSymbolTable.Contains(varname)) {
                m_localSymbolTable.Add(varname);
            }
        }

        public CCFunctionDefinition(CComboContainer parent) :
            base(2, (int)ContainerType.CT_FUNDEF) {
            m_file = parent as CCFile;
        }

        public override CodeContainer AssemblyCodeContainer() {
            CodeContainer rep = new CodeContainer(-1);
            // 1. Emmit Header
            rep.AddCode(AssemblyContext(HEADER), -1);

            // 4. Emmit Code Body
            rep.AddCode(AssemblyContext(BODY), -1);
            rep.AddNewLine(-1);

            return rep;
        }

        public override void PrintStructure(StreamWriter m_ostream) {
            ExtractSubgraphs(m_ostream, BODY, mc_contextNames[BODY]);
            ExtractSubgraphs(m_ostream, HEADER, mc_contextNames[HEADER]);
            
            foreach (CEmmitableCodeContainer child in MChildren) {
                child.PrintStructure(m_ostream);
            }
            m_ostream.WriteLine("\"{0}\"->\"{1}\"", MParentMNodeName, MNodeName);
        }
    }

    public class CMainFunctionDefinition : CCFunctionDefinition {
        public CMainFunctionDefinition(CComboContainer parent) : base(parent) {
            CCompoundStatement cmpst = new CCompoundStatement(this);
            string mainheader = "void main(int argc, char* argv[])";
            AddCode(mainheader, HEADER);
            AddCode(cmpst, BODY);
        }

        public override void DeclareVariable(string varname, bool isread) {
            CCFile file = MParent as CCFile;
            file.DeclareGlobalVariable(varname);
        }
    }

    public class CCompoundStatement : CComboContainer {
        public const int DECLARATIONS = 0, BODY = 1;

        public readonly string[] mc_contextNames =
            { "CompoundStatement_Declarations", "CompoundStatement_Body"};
        public CCompoundStatement(CComboContainer parent) :
            base(2, (int)ContainerType.CT_COMPOUNDSTATEMENT) {
        }

        public override CodeContainer AssemblyCodeContainer() {
            CodeContainer rep = new CodeContainer(-1 );
            rep.AddCode("{",-1);
            rep.EnterScope();
            rep.AddCode("//  ***** Local declarations *****",-1);
            rep.AddNewLine(-1);
            rep.AddCode(AssemblyContext(DECLARATIONS), -1);
            rep.AddCode("//  ***** Code Body *****",-1);
            rep.AddNewLine(-1);
            rep.AddCode(AssemblyContext(BODY),-1);
            rep.LeaveScope();
            rep.AddCode("}", -1);
            return rep;
        }

        public override void PrintStructure(StreamWriter m_ostream) {
            
            ExtractSubgraphs(m_ostream, BODY, mc_contextNames[BODY]);
            ExtractSubgraphs(m_ostream, DECLARATIONS, mc_contextNames[DECLARATIONS]);
            foreach (CEmmitableCodeContainer child in MChildren) {
                child.PrintStructure(m_ostream);
            }
            m_ostream.WriteLine("\"{0}\"->\"{1}\"", MParentMNodeName, MNodeName);
        }
    }
}
