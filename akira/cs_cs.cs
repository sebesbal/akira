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
        public override bool Apply(Context ctx, ref XElement node)
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
}
