using System;
using akira;
namespace akira
{
    public class read : Rule
    {
        //public read() { level = 2; }
        public override bool Apply(Context ctx, ref Node that)
        {
            if (!that.Match("read", 1)) return false;
            Node a = that.First;
            if (a.Match("$") && a.First.Data is string && a.First.Items.Count == 0)
            {
                Node.Replace(ref that, _c("Node " + a.First.SData + " = cur;"));
                return true;
            }
            else
            {
                var list = new Node();
                //list.Add(_c("if (!cur.MatchItemCount(" + a.Items.Count + ")) return false;"));
                //list.Add(_c("if (!cur.Match(\"" + a.Data + "\" ," + a.Items.Count + ")) return false;"));
                list.Add(_c("if (!cur.Match(\"" + a.Data + "\")) return false;"));
                if (a.Items.Count > 0)
                {
                    list.Add(_c("cur = cur.First;"));
                    var item = a.Items.First;
                    while (item != null)
                    {
                        list.Add(__("read", item.Value));
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
}
