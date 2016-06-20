using System;
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
                else if (list.Head.Match("module"))
                {
                    NModule module = new NModule();
                    Node n = list.Head;
                    Node.Replace(ref n, module);
                    return true;
                }
            }
            return false;
        }
    }

    public class import : Rule
    {
        public override bool Apply(Context ctx, ref Node that)
        {
            if (that.MatchHead("import"))
            {
                ctx.Import(((NString)((NList)that).Second).Value);
                that.Remove();
                that = null;
                return true;
            }
            return false;
        }
    }

    public class NModule: NActiveNode
    {
        string moduleName, path, dir;

        public override bool Apply(Context ctx, ref Node node)
        {
            if (State >= NActiveNodeState.Applied) return false;
            State = NActiveNodeState.Applied;

            var id = node.FindAttribute("id");
            if (id == null)
            {
                moduleName = ctx.GenName();
                dir = ctx.DirGen;
            }
            else
            {
                moduleName = (id.Second as NString).Value;
                dir = ctx.DirWorking;
                ctx.ImportBase(moduleName);
            }
            path = Path.Combine(dir, moduleName + ".cs");
            return true;
        }

        public override bool ApplyAfter(Context ctx, ref Node that)
        {
            if (State == NActiveNodeState.AppliedAfter) return false;
            State = NActiveNodeState.AppliedAfter;

            var code = GenerateCode(ctx, that);
            ctx.SaveCs(path, code);
            ctx.LoadRulesFromCs(path);
            return true;
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

    public class include: Rule
    {
        public override bool Apply(Context ctx, ref Node node)
        {
            if (!node.MatchHead("include")) return false;
            NList list = (NList)node;
            NList newList = new NList();
            foreach (var item in list.NonHeadItems)
            {
                var module = item as NString;
                var file = ctx.FindFile(module.Value + ".aki");
                if (file != null)
                {
                    var n = Node.ParseFile(file.FullName);
                    newList.Add(n);
                }
            }
            Node.ReplaceList(ref node, newList);
            return true;
        }
    }

    public class compile : Rule
    {
        public override bool Apply(Context ctx, ref Node node)
        {
            if (!node.MatchHead("compile")) return false;
            NList list = (NList)node;
            foreach (var item in list.NonHeadItems)
            {
                var module = item as NString;
                var file = ctx.FindModule(module.Value);
                if (file != null && file.Extension == ".aki")
                {
                    akira.Compile(file.FullName);
                }
            }
            node.Remove();
            node = null;
            return true;
        }
    }

    public class search : Rule
    {
        public override bool Apply(Context ctx, ref Node node)
        {
            if (!node.MatchHead("search")) return false;
            NList list = (NList)node;
            foreach (var item in list.NonHeadItems)
            {
                var path = item as NString;
                ctx.AddSearchPath(path.Value);
            }
            return false;
        }
    }
}