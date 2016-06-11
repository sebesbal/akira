using System;
using akira;

namespace akira.tmp
{
	/* Sample input:
	sample
		-->
			alma
			{ korte } 


	Output:
	public class gen0: Rule
	{
		public override bool Apply(Context ctx, ref Node that)
		{
			Node cur = that;
			if (!cur.Match("alma")) return false;
			Node.Replace(ref that, _c("korte")); return true;
			return false;
		}
	}
	*/
	/* Sample input:
	sample
		alma


	Output:
	korte
	*/
}
