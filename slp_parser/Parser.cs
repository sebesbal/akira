using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using akira;
using System.IO;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace slp_parser
{
    class Parser: Rule
    {
        slpLexer lexer;
        slpParser parser;
        public override bool Apply(XElement node)
        {
            if (node.Name != "slp") return false;

            string code = node.Value;
            AntlrInputStream input = new AntlrInputStream(code);
            lexer = new slpLexer(input);
            CommonTokenStream tokens = new CommonTokenStream(lexer);
            parser = new slpParser(tokens);
            IParseTree tree = parser.program();
            XElement elem = TreeToNode(null, tree);
            node.ReplaceWith(elem);

            return true;
        }

        XElement TreeToNode(XElement parent, IParseTree tree)
        {
            if (tree is ParserRuleContext)
            {
                string name = parser.RuleNames[(tree as ParserRuleContext).RuleIndex];
                XElement result = new XElement(name);
                if (parent != null)
                {
                    parent.Add(result);
                }
                for (int i = 0; i < tree.ChildCount; ++i)
                {
                    var child = tree.GetChild(i);
                    TreeToNode(result, child);
                }
                return result;
            }
            else if (tree is TerminalNodeImpl)
            {
                string txt = (tree as TerminalNodeImpl).Payload.Text;
                string name = lexer.RuleNames[(tree as TerminalNodeImpl).Payload.Type - 1];
                parent.Value = txt;
                parent.Name = name;
                return null;
            }
            else
            {
                return null;
            }
        }
    }
}
