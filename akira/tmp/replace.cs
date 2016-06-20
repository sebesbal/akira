using System;
using akira;

namespace akira.replace
{
	public class gen0: Rule
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
}
