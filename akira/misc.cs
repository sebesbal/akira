﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace akira
{
    public class rule : Rule
    {
        public override bool ApplyAfter(Context ctx, ref Node that)
        {
            if (!that.MatchHead("rule")) return false;
            
            var id = that.FindAttribute("id");
            string className = id == null ? ctx.GenName() : (id.Second as NString).Value;
            var code = GenerateCode(ctx, that, className);
            NCode n = new NCode(code);
            Node.Replace(ref that, n);
            return true;
        }

        protected string GenerateCode(Context ctx, Node node, string className)
        {
            bool after = node.FindAttribute("after") != null;
            CodeBuilder sb = new CodeBuilder();
            sb.Indent++;
            sb.AddLine("public class " + className + ": Rule");
            sb.BeginCurly();
            sb.AddLine(after ? "public override bool ApplyAfter(Context ctx, ref Node that)"
                                : "public override bool Apply(Context ctx, ref Node that)");

            sb.BeginCurly();
            sb.AddLine("Node cur = that;");
            var v = new List<Node>();
            node.GetTree(v);
            foreach (var item in v)
            {
                NCode code = item as NCode;
                if (code != null)
                {
                    sb.AddLine(code.Value);
                }
            }
            sb.AddLine("return false;");
            sb.End();
            sb.End();
            //sb.AddLine("}}");
            return sb.ToString();
        }
    }

    public class tocs : Rule
    {
        public override bool Apply(Context ctx, ref Node that)
        {
            NList list = that as NList;
            if (list != null)
            {
                NCode code = list.Head as NCode;
                if (code != null && list.Items.Count > 1 && code.Value.IndexOf("$") > -1)
                {
                    code.InsertChildren();
                    code.Remove();
                    Node.Replace(ref that, code);
                    return true;
                }
            }
            return false;
        }
    }

    public class parse : Rule
    {
        public parse()
        {
            level = 0;
        }
        public override bool Apply(Context ctx, ref Node that)
        {
            NList list = that as NList;
            if (list != null)
            {
                if (list.Head.Match("$"))
                {
                    Node.Replace(ref that, new NRef((list.Second as NString).Value));
                    return true;
                }
                else if (list.Head.Match(":"))
                {
                    NAttribute att = new NAttribute();
                    var it = list.Items.First.Next;
                    while (it != null)
                    {
                        att.Add(it.Value);
                        it = it.Next;
                    }
                    Node.Replace(ref that, att);
                    return true;
                }
            }
            return false;
        }
    }

    public class module : Rule
    {
        string moduleName;
        public override bool Apply(Context ctx, ref Node that)
        {
            if (Match(that))
            {
                if (that.FindAttribute("ignore_gen") == null
                    && File.Exists(ctx.PathCs(moduleName)))
                {
                    // load the cs file if it's existing and we don't want to ignore it.
                    ctx.LoadRulesFromCs(moduleName);

                    if (that.FindAttribute("rewrite_gen") == null)
                    {
                        // remove the rule if we don't want to rebuild it.
                        that.Remove();
                        that = null;
                        return true;
                    }
                }
            }
            return false;
        }

        public override bool ApplyAfter(Context ctx, ref Node that)
        {
            if (Match(that))
            {
                var code = GenerateCode(ctx, that);
                ctx.SaveCs(moduleName, code);
                ctx.LoadRulesFromCs(moduleName);
                that = null;
                return true;
            }
            return false;
        }

        bool Match(Node that)
        {
            NList list = that as NList;
            if (list != null)
            {
                var item = list.Items.First;
                if (!item.Value.Match("module")) return false;
                var str = item.Next.Value as NString;
                if (str == null) return false;
                moduleName = str.Value;
                return true;
            }
            return false;
        }

        protected string GenerateCode(Context ctx, Node node)
        {
            bool after = node.Match("after");
            CodeBuilder sb = new CodeBuilder();

            sb.AddLine("using System;");
            sb.AddLine("using akira;");
            sb.LineEnd();
            sb.AddLine("namespace akira." + moduleName);
            sb.BeginCurly();

            foreach (var item in node.Descendants)
            {
                NCode code = item as NCode;
                if (code != null)
                {
                    sb.AddLine(code.Value);
                }
            }

            sb.End();

            return sb.ToString();
        }
    }

    public class sample : Rule
    {
        public override bool Apply(Context ctx, ref Node node)
        {
            if (!node.MatchHead("sample")) return false;
            NList list = (NList)node;
            var code = _c("\n\r\n\r/* Sample input:\n" + node.ToString(1) + "\n\n\tOutput:\n");
            list.Items.AddAfter(list.Items.First, code);
            code.Parent = (NList)node;
            return false;
        }

        public override bool ApplyAfter(Context ctx, ref Node node)
        {
            if (!node.MatchHead("sample")) return false;
            var code = _c("*/");
            ((NList)node).Add(code);
            return false;
        }
    }
}