using System;
using akira;
namespace akira
{
    public class read : Rule
    {
        //public read() { level = 2; }
        public override bool Apply(Context ctx, ref Node that)
        {
            if (!that.Match("read", 2)) return false;
            Node a = that.First;
            var list = new Node();
            var p = that.Parent;
            var n = p.Items.Find(that);

            if (a is NRef)
            {
                //list.Add(_c("Node " + ((NRef)a).Value + " = cur;"));
                Node.Replace(ref that, _c("Node " + a.Data + " = cur;"));
                return true;
            }
            //else if (a is NString)
            //{
            //    list.Add(_c("if (!cur.Match(\"" + ((NString)a).Value + "\")) return false;"));
            //}
            else
            {
                //list.Add(_c("if (!cur.MatchItemCount(" + a.Items.Count + ")) return false;"));
                list.Add(_c("if (!cur.Match(" + a.Data + "," + a.Items.Count + ")) return false;"));
                list.Add(_c("cur = cur.First;"));
                var item = a.Items.First;
                while (item != null)
                {
                    list.Add(__(__("read"), __(item.Value)));
                    if (item != a.Items.Last)
                    {
                        list.Add(_c("cur = cur.Next;"));
                    }
                    item = item.Next;
                }
                list.Add(_c("cur = cur.Parent;"));
            }
            Node.Replace(ref that, list);
            return true;
        }
    }
}