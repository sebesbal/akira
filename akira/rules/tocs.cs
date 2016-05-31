using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace akira.rules
{
    class tocs: Rule
    {
        public override bool Apply(Context ctx, ref Node node)
        {
            //if (node.Name != "code") return false;

            //string s = node.Value;

            //var v = node.Elements().ToArray();
            //string s = node.Attribute("code").Value;

            //for (int i = 0; i < v.Count(); ++i)
            //{
            //    var c = v[i];
            //    s = Regex.Replace(s, "\\$" + (i + 1).ToString(), c.Attribute("code").Value);
            //}
            
            return false;
        }

    }
}
