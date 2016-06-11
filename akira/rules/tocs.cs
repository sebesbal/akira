using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace akira
{
    public class tocs: Rule
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
            }
            return false;
        }
    }

    public class module : Rule
    {
        string moduleName;
        public override bool Apply(Context ctx, ref Node that)
        {
            //if (Match(that) && File.Exists(ctx.PathCs(moduleName)))
            //{
            //    ctx.LoadRulesFromCs(moduleName);
            //    that.Remove();
            //    that = null;
            //    return true;
            //}
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

    public class sample: Rule
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
