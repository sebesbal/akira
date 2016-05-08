using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace akira
{
    class apply : Rule
    {
        public override bool Apply(Context ctx, ref XElement node)
        {
            if (node.Name != "apply") return false;
            XAttribute a = node.Attribute("src");
            string ruleID = a.Value;
            Rule rule = ctx.DefinedRule(ruleID);
            if (rule == null)
            {
                rule = Load(ruleID + ".dll") as Rule;
            }
            ctx.ActivateRule(rule);
            node.Remove();
            node = null;
            return true;
        }
    }
}
