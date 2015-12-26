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
using slp_parser;

namespace akira
{
    class slp_ast: Rule
    {
        slpLexer lexer;
        slpParser parser;
        Listener listener;
        public override bool Apply(Context ctx, ref XElement node)
        {
            if (node.Name != "slp") return false;

            string code = node.Value;
            AntlrInputStream input = new AntlrInputStream(code);
            lexer = new slpLexer(input);
            CommonTokenStream tokens = new CommonTokenStream(lexer);

            parser = new slpParser(tokens);
            IParseTree tree = parser.program();

            ParseTreeWalker walker = new ParseTreeWalker();
            listener = new Listener();
            walker.Walk(listener, tree);

            //XElement elem = TreeToNode(null, tree);
            XElement elem = listener.program;

            node.ReplaceWith(elem);
            node = elem;

            return true;
        }

        XElement ListToNode(XElement parent, slpParser.ListContext tree)
        {
            var list = new LinkedList<Tuple<Operator, ParserRuleContext>>();
            var oprerators = listener.operators;

            foreach (var item in tree.children)
            {
                var text = item.GetText();
                Operator op = item.Payload as Operator;
                if (op != null)
                {
                }
            }

            return null;
        }



        XElement TreeToNode(XElement parent, IParseTree tree)
        {
            //if (tree is slpParser.ListContext)
            //{
            //    return ListToNode(parent, (slpParser.ListContext)tree);
            //}
            //else 
            if (tree is ParserRuleContext)
            {
                string name = parser.RuleNames[(tree as ParserRuleContext).RuleIndex];
                XElement result = new XElement(name);
                for (int i = 0; i < tree.ChildCount; ++i)
                {
                    var child = tree.GetChild(i);
                    TreeToNode(result, child);
                }
                if (parent != null)
                {
                    parent.Add(result);
                }
                return result;
            }
            else if (tree is TerminalNodeImpl)
            {
                string txt = (tree as TerminalNodeImpl).Payload.Text;

                int id = (tree as TerminalNodeImpl).Payload.Type - 1;
                // string name = id < lexer.RuleNames.Count() ? lexer.RuleNames[id] : "fos";
                string name = lexer.RuleIndexMap.FirstOrDefault(x => x.Value == id).Key;
                XElement result = new XElement(name);
                result.Value = txt;
                parent.Add(result);

                ////parent.Value = txt;
                //if (!name.StartsWith("T_"))
                //{
                //    parent.Name = name;
                //}
                return null;
            }
            else
            {
                return null;
            }
        }
    }
}
