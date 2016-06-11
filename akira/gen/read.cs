using System;
using akira;
namespace akira
{
    public class read : Rule
    {
        public read() { level = 1; }
        public override bool ApplyAfter(Context ctx, ref Node that)
        {
            if (!that.Match("read", 2)) return false;
            Node a = ((NList)that).Second;
            var list = new NList();
            var p = that.Parent;
            var n = p.Items.Find(that);

            if (a is NRef)
            {
                //list.Add(_c("Node " + ((NRef)a).Value + " = cur;"));
                Node.Replace(ref that, _c("Node " + ((NRef)a).Value + " = cur;"));
                return true;
            }
            else if (a is NString)
            {
                list.Add(_c("if (!cur.Match(\"" + ((NString)a).Value + "\")) return false;"));
            }
            else if (a is NList)
            {
                var l = (NList)a;
                list.Add(_c("if (!cur.MatchItemCount(" + l.Items.Count + ")) return false;"));
                list.Add(_c("cur = ((NList)cur).Head;"));
                var item = l.Items.First;
                while (item != null)
                {
                    list.Add(_l(_s("read"), __(item.Value)));
                    if (item != l.Items.Last)
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
