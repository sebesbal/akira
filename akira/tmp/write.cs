using System;
using akira;

namespace akira.write
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
			throw new Exception("lofusz!");
			Node.Replace(ref that, _l(_c("Node.Replace(ref that, $1); return true;"), __(a))); return true;
			return false;
		}
	}
}
