using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace akira
{
    public class tocs: Rule
    {
        public override bool ApplyAfter(Context ctx, ref Node node)
        {
            if (node.Match("type", "code") && node.Children.Count > 0)
            {
                node.InsertChildren();
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
