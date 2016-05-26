using System;
using System.Reflection;
using System.Text;

namespace akira
{
    public class replace : Rule
    {
        bool after = false;
        string className;

        public override bool Apply(Context ctx, ref Node node)
        {
            if (!(node.Name == "rule")) return false;
            Rule result = GenerateInstance(ctx, node);
            ctx.ActivateRule(result);

            node.Remove();
            node = null;
            return true;
        }

        bool IsReference(Node n)
        {
            return n.Name == "a"
                || n.Name == "b"
                || n.Name == "c"
                || n.Name == "d";
        }

        //void FindRefs(Node node, List<string> refs, List<string> conds)
        //{
        //    if (IsReference(node))
        //    {
        //        refs.Add(node.Name);
        //    }

        //    foreach (var item in node.Elements())
        //    {
        //        FindRefs(item, refs, conds);
        //    }
        //}

        protected void GenCodeRec(StringBuilder sb, Context ctx, Node node)
        {
            if (node.Name == "code")
            {
                sb.AppendLine(node.Value);
            }
            else if (IsReference(node))
            {
                sb.AppendLine("Node " + node.Name + " = cur;");
            }
            else
            {
                string name = node.Name;
                sb.AppendLine("if (\"" + node.Name + "\" != cur.Name" + ") return false;");

                if (node.Children.Count > 0)
                {
                    sb.AppendLine("cur = cur.First;");

                    foreach (var item in node.Children)
                    {
                        GenCodeRec(sb, ctx, item);
                    }

                    sb.AppendLine("cur = cur.Parent;");
                }
            }

            
        }

        protected string GenerateCode(Context ctx, Node node)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("using System; using akira;");
            sb.AppendLine("namespace akira {");
            sb.AppendLine("public class " + className + ": Rule");
            sb.AppendLine("{");
            sb.AppendLine(after ? "public override bool ApplyAfter(Context ctx, ref Node node)"
                                : "public override bool Apply(Context ctx, ref Node node)");

            sb.AppendLine("{");
            sb.AppendLine("begin:");
            sb.AppendLine("Node cur = node;");
            foreach (var item in node.Children)
            {
                GenCodeRec(sb, ctx, item);
            }
            sb.AppendLine("return false;");
            sb.AppendLine("}");
            sb.AppendLine("}}");
            return sb.ToString();
        }

        protected Rule GenerateInstance(Context ctx, Node node)
        {
            className = ctx.GenName();
            Assembly a = null;
            Type t = null;
            if (!ctx.GetType(className, ref t, ref a))
            {
                ctx.GetType(className, GenerateCode(ctx, node), ref t, ref a);
            }
            var o = a.CreateInstance(t.FullName);
            return (Rule)o;
        }
    }
}