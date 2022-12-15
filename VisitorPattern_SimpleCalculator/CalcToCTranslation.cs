using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisitorPattern_SimpleCalculator {
    public class TranslationParameters {
        // Provides access to the ancestor container object. Provides 
        // access to properties of the parent during construction of the
        // current element
        private CComboContainer m_parent = null;
        // Provides access to the FunctionDefinition that
        // hosts the current element. Provides access to the API
        // of the container function i.e to Declare a variable etc
        private CCFunctionDefinition m_containerFunction = null;
        // Provides the context of the parent container object
        // where the current container shouldbe placed. Necessary 
        // when the current object is created and placed in the parent
        // at a specific context
        private int m_parentContextType = -1;

        public CComboContainer M_Parent {
            get => m_parent;
            set => m_parent = value;
        }
        public int M_ParentContextType {
            get => m_parentContextType;
            set => m_parentContextType = value;
        }

        public CCFunctionDefinition M_ContainerFunction {
            get => m_containerFunction;
            set => m_containerFunction = value;
        }
    }
    public class CalcToCTranslation :
        SimpleCalcVisitor<CEmmitableCodeContainer, TranslationParameters> {

        private CCFile m_translatedFile;
        public CCFile M_TranslatedFile => m_translatedFile;


        public override CEmmitableCodeContainer VisitCompileUnit(CompileUnit node,
            params TranslationParameters[] args) {

            CCFunctionDefinition mainf;
            //1. Create Output File
            m_translatedFile = new CCFile(true);
            mainf = m_translatedFile.MainDefinition;

            m_translatedFile.AddCode("#include <stdio.h>\n", CCFile.PREPROCESSOR_DIRECTIVES);
            m_translatedFile.AddCode("#include <stdlib.h>\n", CCFile.PREPROCESSOR_DIRECTIVES);
            
            CCompoundStatement cmpst = mainf.GetChild(CCFunctionDefinition.BODY, 0) as CCompoundStatement;
            foreach (ASTNode child in node.GetContextChildren(CompileUnit.EXPRESSIONS)) {
                CEmmitableCodeContainer dep = Visit(child, new TranslationParameters() {
                    M_Parent = cmpst,
                    M_ContainerFunction = mainf,
                    M_ParentContextType = CCompoundStatement.BODY
                });
                cmpst.AddCode(dep, CCompoundStatement.BODY);
            }
            return m_translatedFile;
        }

        public override CEmmitableCodeContainer VisitAddition(Addition node, params TranslationParameters[] args) {
            CodeContainer rep = new CodeContainer(-1);
            args[0].M_Parent?.AddCode(rep, args[0].M_ParentContextType);
            rep.AddCode(Visit(node.GetChild(Addition.LEFT, 0), new TranslationParameters() {
                M_ContainerFunction = args[0].M_ContainerFunction,
                M_Parent = null,
                M_ParentContextType = -1
            }).AssemblyCodeContainer(), -1);
            rep.AddCode("+", -1);
            rep.AddCode(Visit(node.GetChild(Addition.RIGHT, 0), new TranslationParameters() {
                M_ContainerFunction = args[0].M_ContainerFunction,
                M_Parent = null,
                M_ParentContextType = -1
            }).AssemblyCodeContainer(), -1);
            return rep;
        }

        public override CEmmitableCodeContainer VisitSubtraction(Subtraction node, params TranslationParameters[] args) {
            CodeContainer rep = new CodeContainer(-1);
            args[0].M_Parent?.AddCode(rep, args[0].M_ParentContextType);
            rep.AddCode(Visit(node.GetChild(Subtraction.LEFT, 0), new TranslationParameters() {
                M_ContainerFunction = args[0].M_ContainerFunction,
                M_Parent = null,
                M_ParentContextType = -1
            }).AssemblyCodeContainer(), -1);
            rep.AddCode("-", -1);
            rep.AddCode(Visit(node.GetChild(Subtraction.RIGHT, 0), new TranslationParameters() {
                M_ContainerFunction = args[0].M_ContainerFunction,
                M_Parent = null,
                M_ParentContextType = -1
            }).AssemblyCodeContainer(), -1);
            return rep;
        }

        public override CEmmitableCodeContainer VisitMultiplication(Multiplication node, params TranslationParameters[] args) {
            CodeContainer rep = new CodeContainer(-1);
            args[0].M_Parent?.AddCode(rep, args[0].M_ParentContextType);
            rep.AddCode(Visit(node.GetChild(Multiplication.LEFT, 0), new TranslationParameters() {
                M_ContainerFunction = args[0].M_ContainerFunction,
                M_Parent = null,
                M_ParentContextType = -1
            }).AssemblyCodeContainer(), -1);
            rep.AddCode("*", -1);
            rep.AddCode(Visit(node.GetChild(Multiplication.RIGHT, 0), new TranslationParameters() {
                M_ContainerFunction = args[0].M_ContainerFunction,
                M_Parent = null,
                M_ParentContextType = -1
            }).AssemblyCodeContainer(), -1);
            return rep;
        }

        public override CEmmitableCodeContainer VisitDivision(Division node, params TranslationParameters[] args) {
            CodeContainer rep = new CodeContainer(-1);
            args[0].M_Parent?.AddCode(rep, args[0].M_ParentContextType);
            rep.AddCode(Visit(node.GetChild(Division.LEFT, 0), new TranslationParameters() {
                M_ContainerFunction = args[0].M_ContainerFunction,
                M_Parent = null,
                M_ParentContextType = -1
            }).AssemblyCodeContainer(), -1);
            rep.AddCode("/", -1);
            rep.AddCode(Visit(node.GetChild(Division.RIGHT, 0), new TranslationParameters() {
                M_ContainerFunction = args[0].M_ContainerFunction,
                M_Parent = null,
                M_ParentContextType = -1
            }).AssemblyCodeContainer(), -1);
            return rep;
        }

        public override CEmmitableCodeContainer VisitAssignment(Assignment node, params TranslationParameters[] args) {
            CCFunctionDefinition fun = args[0].M_ContainerFunction as CCFunctionDefinition;

            CodeContainer rep = new CodeContainer(-1);
            args[0].M_Parent?.AddCode(rep, args[0].M_ParentContextType);
            IDENTIFIER id = node.GetChild(Assignment.IDENTIFIER, 0) as IDENTIFIER;
            fun.DeclareVariable(id.M_Text, false);
            rep.AddCode(id.M_Text, -1);
            rep.AddCode("=", -1);
            rep.AddCode(Visit(node.GetChild(Assignment.EXPRESSION, 0), new TranslationParameters() {
                M_ContainerFunction = args[0].M_ContainerFunction,
                M_Parent = null,
                M_ParentContextType = -1
            }).AssemblyCodeContainer(), -1);
            return rep;
        }

        public override CEmmitableCodeContainer VisitIDENTIFIER(IDENTIFIER node, params TranslationParameters[] args) {
            CodeContainer rep = new CodeContainer(-1);
            args[0].M_ContainerFunction.DeclareVariable(node.M_Text, true);
            rep.AddCode(node.M_Text, -1);
            args[0].M_Parent?.AddCode(rep, args[0].M_ParentContextType);
            return rep;
        }

        public override CEmmitableCodeContainer VisitNUMBER(NUMBER node, params TranslationParameters[] args) {
            CodeContainer rep = new CodeContainer(-1);
            rep.AddCode(node.M_Text, -1);
            args[0].M_Parent?.AddCode(rep, args[0].M_ParentContextType);
            return rep;
        }
    }
}
