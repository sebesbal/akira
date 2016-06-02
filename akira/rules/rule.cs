using System;
using System.Reflection;
using System.Text;

namespace akira
{
    public class rule : Rule
    {
        public override bool Apply(Context ctx, ref Node node)
        {
            if (!(node.Name == "rule")) return false;
            
            Rule result = GenerateInstanceFromCS(ctx, node);
            if (result == null)
            {
                ctx.PushBlock();
                return false;
            }
            else
            {
                ctx.ActivateRule(result);
                node.Remove();
                node = null;
                return true;
            }
        }

        public override bool ApplyAfter(Context ctx, ref Node node)
        {
            if (!(node.Name == "rule")) return false;
            Rule result = GenerateInstanceFromCode(ctx, node);
            ctx.PopBlock();
            ctx.ActivateRule(result);

            node.Remove();
            node = null;
            return true;
        }

        //bool IsReference(Node n)
        //{
        //    return n.Name == "$";
        //    //return n.Name == "a"
        //    //    || n.Name == "b"
        //    //    || n.Name == "c"
        //    //    || n.Name == "d";
        //}

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
            if (node.IsCode)
            {
                sb.AppendLine(node.Name);
            }
            else if (node.IsRef)
            {
                sb.AppendLine("Node " + node.Value + " = cur;");
            }
            else
            {
                int count = 0;
                foreach (var item in node.Children)
                {
                    if (!item.IsCode)
                    {
                        ++count;
                    }
                }

                string name = node.Name;
                sb.AppendLine("if (\"" + node.Name + "\" != cur.Name || "
                    + count + " != cur.Children.Count) return false;");

                if (node.Children.Count > 0)
                {
                    sb.AppendLine("cur = cur.First;");

                    foreach (var item in node.Children)
                    {
                        GenCodeRec(sb, ctx, item);

                        if (item != node.Children.Last.Value)
                        {
                            sb.AppendLine("cur = cur.Next;");
                        }
                    }

                    sb.AppendLine("cur = cur.Parent;");
                }
            }
        }

        protected string GenerateCode(Context ctx, Node node, string className)
        {
            bool after = node.Match("after");
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("using System; using akira;");
            sb.AppendLine("namespace akira {");
            sb.AppendLine("public class " + className + ": Rule");
            sb.AppendLine("{");
            sb.AppendLine(after ? "public override bool ApplyAfter(Context ctx, ref Node that)"
                                : "public override bool Apply(Context ctx, ref Node that)");

            sb.AppendLine("{");
            //sb.AppendLine("begin:");
            sb.AppendLine("Node cur = that;");
            foreach (var item in node.Children)
            {
                GenCodeRec(sb, ctx, item);
            }
            sb.AppendLine("return true;");
            sb.AppendLine("}");
            sb.AppendLine("}}");
            return sb.ToString();
        }

        //protected Rule GenerateInstance(Context ctx, Node node)
        //{
        //    Node src = node["src"];
        //    if (src == null)
        //    {
        //        className = ctx.GenName();
        //    }
        //    else
        //    {
        //        className = src.Name;
        //    }

        //    Assembly a = null;
        //    Type t = null;
        //    if (!ctx.GetType(className, ref t, ref a))
        //    {
        //        ctx.GetType(className, GenerateCode(ctx, node), ref t, ref a);
        //    }
        //    var o = a.CreateInstance(t.FullName);
        //    return (Rule)o;
        //}

        protected Rule GenerateInstanceFromCode(Context ctx, Node node)
        {
            string className;
            Node src = node["src"];
            if (src == null)
            {
                className = ctx.GenName();
            }
            else
            {
                className = src.Name;
            }

            Assembly a = null;
            Type t = null;
            ctx.GetType(className, GenerateCode(ctx, node, className), ref t, ref a);
            var o = a.CreateInstance(t.FullName);
            return (Rule)o;
        }

        protected Rule GenerateInstanceFromCS(Context ctx, Node node)
        {
            string className;
            Node src = node["src"];
            if (src == null)
            {
                return null;
            }
            else
            {
                className = src.Name;
            }

            Assembly a = null;
            Type t = null;
            if (ctx.GetType(className, ref t, ref a))
            {
                var o = a.CreateInstance(t.FullName);
                return (Rule)o;
            }
            else
            {
                return null;
            }
        }
    }
}