using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisitorPattern_SimpleCalculator {
    
    public interface IASTIterator {
        ASTNode MCurNode { get; }
        void Init();
        bool End();
        void Next();
    }

    public interface ILabelled {
        string MNodeName { get; }
    }

    public interface IASTVisitableNode {
        Return Accept<Return, Params>(IASTBaseVisitor<Return, Params> v, params Params[] info);
    }

    public interface IASTComposite : IEnumerable<IASTVisitableNode> {

    }

    public interface IASTBaseVisitor<Return, Params> {
        Return Visit(IASTVisitableNode node, params Params[] info);
        Return VisitChildren(IASTComposite node, params Params[] info);
    }

}
