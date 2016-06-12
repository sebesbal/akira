using System;
using akira;

namespace akira.basic
{
	public class gen0: Rule
	{
		public override bool Apply(Context ctx, ref Node that)
		{
			Node cur = that;
			if (!cur.MatchItemCount(2)) return false;
			cur = ((NList)cur).Head;
			if (!cur.Match("write")) return false;
			cur = cur.Next;
			Node a = cur;
			cur = cur.Parent;
			Node.Replace(ref that, _l(_c("Node.Replace(ref that, $1); return true;"), __(a))); return true;
			return false;
		}
	}
	public class gen1: Rule
	{
		public override bool Apply(Context ctx, ref Node that)
		{
			Node cur = that;
			if (!cur.MatchItemCount(3)) return false;
			cur = ((NList)cur).Head;
			if (!cur.Match("-->")) return false;
			cur = cur.Next;
			Node a = cur;
			cur = cur.Next;
			Node b = cur;
			cur = cur.Parent;
			Node.Replace(ref that, _l(_s("rule"), _l(_s("read"), __(a)), _l(_s("write"), __(b)))); return true;
			return false;
		}
	}
	public class gen2: Rule
	{
		public override bool Apply(Context ctx, ref Node that)
		{
			Node cur = that;
			if (!cur.Match("alma")) return false;
			Node.Replace(ref that, _c("korte")); return true;
			return false;
		}
	}
    public class replace_that : Rule
    {
        public override bool Apply(Context ctx, ref Node that)
        {
            Node cur = that;
            if (!cur.MatchItemCount(2)) return false;
            cur = ((NList)cur).Head;
            if (!cur.Match("replace_that")) return false;
            cur = cur.Next;
            Node a = cur;
            cur = cur.Parent;
            Node.Replace(ref that, _l(_c("Node.Replace(ref that, $1); return true;"), __(a))); return true;
            return false;
        }
    }
}
