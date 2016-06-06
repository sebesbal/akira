using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace akira
{
    public class rule : Rule
    {
        public override bool ApplyAfter(Context ctx, ref Node that)
        {
            if (!that.MatchHead("rule")) return false;
            var code = GenerateCode(ctx, that, ctx.GenName());
            NCode n = new NCode(code);
            Node.Replace(ref that, n);
            return true;
        }
        
        protected string GenerateCode(Context ctx, Node node, string className)
        {
            bool after = node.Match("after");
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
}