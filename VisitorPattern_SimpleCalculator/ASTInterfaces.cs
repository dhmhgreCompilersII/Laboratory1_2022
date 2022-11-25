using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisitorPattern_SimpleCalculator {

    public interface IASTNode {
        Return Accept<Return, Params>(IASTBaseVisitor<Return, Params> v, params Params[] info);
    }

    public interface IASTBaseVisitor<Return, Params> {
        Return Visit(IASTNode node, params Params[] info);
        Return VisitChildren(IEnumerable<IASTNode> children, params Params[] info);
    }

}
