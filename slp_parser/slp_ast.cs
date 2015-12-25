﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using akira;
using System.IO;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace akira
{
    class slp_ast: Rule
    {
        slpLexer lexer;
        slpParser parser;
        public override bool Apply(Context ctx, ref XElement node)
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
            node = elem;

            return true;
        }

        //XElement TokensToNode(XElement parent, CommonTokenStream tree)
        //{

        //}

        XElement TreeToNode(XElement parent, IParseTree tree)
        {
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
