using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using akira;

namespace slp_parser
{
    class Listener: slpBaseListener
    {
        public void EnterEveryRule(Antlr4.Runtime.ParserRuleContext ctx)
        {
            throw new NotImplementedException();
        }

        public void ExitEveryRule(Antlr4.Runtime.ParserRuleContext ctx)
        {
            throw new NotImplementedException();
        }

        public void VisitErrorNode(Antlr4.Runtime.Tree.IErrorNode node)
        {
            throw new NotImplementedException();
        }

        public void VisitTerminal(Antlr4.Runtime.Tree.ITerminalNode node)
        {
            throw new NotImplementedException();
        }
    }
}
