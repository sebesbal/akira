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
            Node.Replace(ref that, new Node(n));
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
                NCode code = item.Data as NCode;
                if (code == null)
                {
                    // sb.AddLine("//" + item.Data);
                }
                else
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
            NCode code = that.Data as NCode;
            if (code != null && that.Items.Count > 0 
                && (code.Value.IndexOf("$") > -1 || code.Value.IndexOf("#") > -1))
            {
                code.InsertChildren(that);
                that.Items.Clear();
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
            //if (that.Match("$"))
            //{
            //    Node.Replace(ref that, new NRef(that.First.SData));
            //    return true;
            //}
            //else 
            //if (that.Match(":"))
            //{
            //    NAttribute att = new NAttribute();
            //    that.Data = att;
            //    return true;
            //}
            //else 
            if (that.Match("module"))
            {
                that.Data = new NModule();
                return true;
            }
            else if (that.Match(typeof(NCode)))
            {
                var code = ((NCode)that.Data);
                bool b = code.CreateChildren(that);
                code.InsertChildren2(that);
                return b;
            }
            return false;
        }
    }

    public class NModule: NActiveNode
    {
        string moduleName;
        public override bool Apply(Context ctx, ref Node that)
        {
            var id = that.FindAttribute("id");
            if (id == null)
            {
                moduleName = ctx.GenSubmodule();
                // throw new Exception("Module must have id!");
            }
            else
            {
                moduleName = id.Second.SData;
            }

            Console.WriteLine();
            Console.WriteLine("Compile " + moduleName);

            return false;
        }
        public override bool ApplyAfter(Context ctx, ref Node that)
        {
            var code = GenerateCode(ctx, moduleName, that);
            string pathCs = Path.Combine(ctx.DirWorking, moduleName + ".cs");
            File.WriteAllText(pathCs, code);
            string pathDll;
            var ass = ctx.CompileCs(pathCs, out pathDll);
            ctx.LoadRulesFromAss(ass);
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
                NCode code = item.Data as NCode;
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
            if (!node.Match("sample")) return false;
            var code = _c("\n/*\n\tSample input:\n" + node.First.ToString(1) + "\n\tOutput:\n");
            node.AddFirst(code);
            return false;
        }

        public override bool ApplyAfter(Context ctx, ref Node node)
        {
            if (!node.Match("sample")) return false;
            var output = new Node();
            foreach (var item in node.Items)
            {
                if (item == node.First)
                {
                    output.Add(item);
                }
                else
                {
                    output.Add(_c(item.ToString(1)));
                }
            }
            output.Add(_c("*/"));
            Node.Replace(ref node, output);
            return true;
        }
    }

    public class print : Rule
    {
        public override bool Apply(Context ctx, ref Node node)
        {
            if (!node.Match("print")) return false;
            Console.WriteLine("before: \n" + node.First.ToString(1));
            return false;
        }

        public override bool ApplyAfter(Context ctx, ref Node node)
        {
            if (!node.Match("print")) return false;
            Console.WriteLine("after: \n" + node.First.ToString(1));
            node.Remove();
            node = null;
            return true;
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
            if (!that.Match("import")) return false;
            foreach (var item in that.Items)
            {
                ctx.Import(item.SData);
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
            bool recompile = node.Match("recompile");
            if (!recompile && !node.Match("compile")) return false;
            foreach (var item in node.Items)
            {
                var file = recompile ? ctx.FindFile(item.SData + ".aki") : ctx.FindModule(item.SData);
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