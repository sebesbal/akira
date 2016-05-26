using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace akira
{
    class misc_atttribute : Rule
    {
        public override bool Apply(Context ctx, ref Node node)
        {
            if (!(node.Name == "op" && node.MatchAttribute("id", "=") && node.Nodes().Count() == 2)) return false;

            var p = node.Parent;
            if (p == null) return false;

            var key = node.Elements().ElementAt(0).Name.ToString();
            var value = node.Nodes().ElementAt(1);
            string val = value is Node ? ((Node)value).Name.ToString() : value.ToString();

            p.SetAttributeValue(key, val);

            node.Remove();
            node = null;

            return true;
        }
    }

    class misc_variables : Rule
    {
        public override bool Apply(Context ctx, ref Node node)
        {
            if (node.Name.ToString().Length != 1) return false;

            var e = new Node("any");
            e.SetAttributeValue("ref", node.Name);
            node.ReplaceWith(e);
            node = e;

            return true;
        }
    }

    class misc_rule : Rule
    {
        public override bool ApplyAfter(Context ctx, ref Node node)
        {
            if (node.Name != "rule") return false;
            ruleName = node.Attribute("src").Value;
            Rule rule = GenerateInstance(ctx, node);
            ctx.ActivateRule(rule);
            node.Remove();
            node = null;
            return true;
        }

        bool after = false;
        string ruleName;

        protected string GenerateCode(Context ctx, Node node)
        {
            string code = "using System; using akira; using System.Xml.Linq;"
            + "namespace akira {"
            + "public class " + ruleName + " : Rule"
            + "{ "
            + "public " + ruleName + "(){" + "" + "}"
            + (after ? "public override bool ApplyAfter(Context ctx, ref Node that)"
                     : "public override bool Apply(Context ctx, ref Node that)")
            + "{"
            + GetRuleBody(ctx, node)
            + "}"
            + "}}";
            return code;
        }

        protected string GetRuleBody(Context ctx, Node node)
        {
            string code = "";

            code = "if (" + node.Attribute("code").Value + ") {\n";
            foreach (var item in node.Elements())
            {
                if (item.Name != "cs") return "";
                code += item.Attribute("code").Value + "\n";
            }
            code += "that = null;\n";
            code += "return true;\n";
            code += "}\n";
            code += "else return false;\n";

            return code;
        }

        protected Rule GenerateInstance(Context ctx, Node node)
        {
            Assembly a = null;
            Type t = null;
            if (!ctx.GetType(ruleName, ref t, ref a))
            {
                ctx.GetType(ruleName, GenerateCode(ctx, node), ref t, ref a);
            }
            var o = a.CreateInstance(t.FullName);
            return (Rule)o;
        }
    }

    //class misc_op_replace : Rule
    //{
    //    public override bool Apply(Context ctx, ref Node node)
    //    {
    //        if (!(node.Name == "op" && node.MatchAttribute("id", "-->")) return false;

    //        var e = new Node("any");
    //        e.SetAttributeValue("ref", node.Name);
    //        node.ReplaceWith(e);
    //        node = e;

    //        return true;
    //    }
    //}

    //class misc_bool : Rule
    //{
    //    public override bool Apply(Context ctx, ref Node node)
    //    {
    //        if (!(node.Name == "bool")) return false;

    //        return true;
    //    }
    //}

    ///// <summary>
    ///// The head of a rule
    ///// name="rule" + type="" means this is the root of a cs rule --> template for a Rule class. The children will be the content of the rule
    ///// </summary>
    //class misc_rule : Rule
    //{
    //    public override bool ApplyAfter(Context ctx, ref Node node)
    //    {
    //        if (node.Name != "rule") return false;
    //        ruleName = node.Attribute("src").Value;
    //        Rule rule = GenerateInstance(ctx, node);
    //        ctx.ActivateRule(rule);
    //        node.Remove();
    //        node = null;
    //        return true;
    //    }

    //    bool after = false;
    //    string ruleName;

    //    protected string GenerateCode(Context ctx, Node node)
    //    {
    //        string code = "using System; using akira; using System.Xml.Linq;"
    //        + "namespace akira {"
    //        + "public class " + ruleName + " : Rule"
    //        + "{ "
    //        + "public " + ruleName + "(){" + "" + "}"
    //        + (after ? "public override bool ApplyAfter(Context ctx, ref Node that)"
    //                 : "public override bool Apply(Context ctx, ref Node that)")
    //        + "{"
    //        + GetRuleBody(ctx, node)
    //        + "}"
    //        + "}}";
    //        return code;
    //    }

    //    protected string GetRuleBody(Context ctx, Node node)
    //    {
    //        string code = "";

    //        code = "if (" + node.Attribute("code").Value + ") {\n";
    //        foreach (var item in node.Elements())
    //        {
    //            if (item.Name != "cs") return "";
    //            code += item.Attribute("code").Value + "\n";
    //        }
    //        code += "that = null;\n";
    //        code += "return true;\n";
    //        code += "}\n";
    //        code += "else return false;\n";

    //        return code;
    //    }

    //    protected Rule GenerateInstance(Context ctx, Node node)
    //    {
    //        Assembly a = null;
    //        Type t = null;
    //        if (!ctx.GetType(ruleName, ref t, ref a))
    //        {
    //            ctx.GetType(ruleName, GenerateCode(ctx, node), ref t, ref a);
    //        }
    //        var o = a.CreateInstance(t.FullName);
    //        return (Rule)o;
    //    }
    //}
}
