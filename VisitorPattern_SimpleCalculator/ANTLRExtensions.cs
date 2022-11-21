using Antlr4.Runtime.Tree;
using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisitorPattern_SimpleCalculator {
    internal static class ANTLRExtensions {


        private static ITerminalNode GetTerminalNode<Result>(this AbstractParseTreeVisitor<Result> t,
            ParserRuleContext node, IToken terminal) {

            for (int i = 0; i < node.ChildCount; i++) {
                ITerminalNode child = node.GetChild(i) as ITerminalNode;
                if (child != null) {
                    if (child.Symbol == terminal) {
                        return child;
                    }
                }
            }
            return null;
        }
        public static Result VisitElementInContext<Result>(this AbstractParseTreeVisitor<Result> t,
            ParserRuleContext node, int context, Stack<int> contextsStack) {
            Result res = default(Result);

            contextsStack.Push(context);
            res = t.Visit(node);     // Visits a particular element
            contextsStack.Pop();

            return res;
        }

        public static Result VisitElementsInContext<Result>(this AbstractParseTreeVisitor<Result> t,
            IEnumerable<IParseTree> nodeset, int context, Stack<int> contextsStack) {
            Result res = default(Result);

            contextsStack.Push(context);
            foreach (IParseTree node in nodeset) {
                res = t.Visit(node);
            }
            contextsStack.Pop();
            return res;
        }

        public static Result VisitTerminalInContext<Result>(this AbstractParseTreeVisitor<Result> t,
            ParserRuleContext tokenParent, IToken node, int context, Stack<int> s)  {
            s.Push(context);
            Result res = t.Visit(GetTerminalNode<Result>(t, tokenParent, node));
            s.Pop();
            return res;
        }
    }
}
