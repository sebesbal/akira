using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace akira
{
    /// <summary>
    /// <cs code="..$1.."><cs code="alma"/></cs> 
	///	    --> <cs code="..alma.."/>
    /// </summary>
    class cs_cs : Rule
    {
        public override bool Apply(Context ctx, ref Node node)
        {
            var p = node.Parent;
            if (!(node.Name == "cs" && node.Elements().Count() > 0)) return false;

            foreach (var c in node.Elements())
            {
                if (c.Name != "cs") return false;
            }

            var v = node.Elements().ToArray();
            string s = node.Attribute("code").Value;

            for (int i = 0; i < v.Count(); ++i)
            {
                var c = v[i];
                s = Regex.Replace(s, "\\$" + (i + 1).ToString(), c.Attribute("code").Value);
            }

            node.RemoveNodes();
            node.SetAttributeValue("code", s);
            return true;
        }
    }

    class if_cs : Rule
    {
        public override bool ApplyAfter(Context ctx, ref Node node)
        {
            if (!(node.Name == "if")) return false;
            node.Name = "cs";
            node.RemoveNodes();
            string code = GetBody(ctx, node);
            if (code == "") return false;
            node.SetAttributeValue("code", code);
            return true;
        }

        public static string GetBody(Context ctx, Node node)
        {
            string code = "";

            code = "if (" + node.Attribute("code").Value + ") {\n";
            foreach (var item in node.Elements())
            {
                if (item.Name != "cs") return "";
                code += item.Attribute("code") + "\n";
            }
            code += "}\n";

            return code;
        }
    }
}
