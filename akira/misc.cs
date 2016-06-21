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
            if (!that.Match("rule")) return false;
            
            var id = that.FindAttribute("id");
            string className = id == null ? ctx.GenName() : id.Second.Data as string;
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
                    sb.AddLine(code.SData);
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
            NCode code = that as NCode;
            if (code != null && that.Items.Count > 1 && code.SData.IndexOf("$") > -1)
            {
                code.InsertChildren();
                //code.Remove();
                //Node.Replace(ref that, code);
                return true;
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
            if (that.Match("$"))
            {
                Node.Replace(ref that, new NRef(that.First.SData));
                return true;
            }
            else if (that.Match(":"))
            {
                NAttribute att = new NAttribute();
                that.Data = att;
                return true;
            }
            else if (that.Match("module"))
            {
                that.Data = new NModule();
                return true;
            }
            return false;
        }
    }

    public class NModule: NActiveNode
    {
        public override bool ApplyAfter(Context ctx, ref Node that)
        {
            string moduleName;
            var id = that.FindAttribute("id");
            if (id == null)
            {
                throw new Exception("Module must have id!");
            }
            else
            {
                moduleName = id.First.SData;
            }

            var code = GenerateCode(ctx, moduleName, that);
            string pathCs = Path.Combine(ctx.DirWorking, moduleName + ".cs");
            File.WriteAllText(pathCs, code);
            string pathDll;
            ctx.CompileCs(pathCs, out pathDll);

            that.Remove();
            that = null;

            return true;
        }
        
        protected string GenerateCode(Context ctx, string moduleName, Node node)
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
                    sb.AddLine(code.SData);
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
            if (!node.Match("sample")) return false;
            var code = _c("\n\r\n\r/* Sample input:\n" + node.ToString(1) + "\n\n\tOutput:\n");
            node.AddFirst(code);
            return false;
        }

        public override bool ApplyAfter(Context ctx, ref Node node)
        {
            if (!node.Match("sample")) return false;
            var code = _c("*/");
            node.Add(code);
            return false;
        }
    }

    public class include: Rule
    {
        public override bool Apply(Context ctx, ref Node node)
        {
            if (!node.Match("include")) return false;
            Node newList = new Node();
            foreach (var item in node.Items)
            {
                var module = item.SData;
                var file = ctx.FindFile(module + ".aki");
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

    public class import : Rule
    {
        public override bool Apply(Context ctx, ref Node that)
        {
            if (that.Match("import"))
            {
                foreach (var item in that.Items)
                {
                    ctx.Import(item.SData);
                }
            }
            that.Remove();
            that = null;
            return true;
        }
    }

    public class compile : Rule
    {
        public override bool Apply(Context ctx, ref Node node)
        {
            if (!node.Match("compile")) return false;
            foreach (var item in node.Items)
            {
                var file = ctx.FindModule(item.SData);
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
            if (!node.Match("search")) return false;
            foreach (var item in node.Items)
            {
                ctx.AddSearchPath(item.SData);
            }
            return false;
        }
    }
}