using System;
using akira;

namespace akira.gen0
{
	public class gen1: Rule
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
}
