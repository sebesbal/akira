using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                if (code != null)
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



}
