using System; using akira;
namespace akira {
public class debug: Rule
{
public override bool Apply(Context ctx, ref Node that)
{
begin:
Node cur = that;
if ("print" != cur.Name || 1 != cur.Children.Count) return false;
cur = cur.First;
Node a = cur;
cur = cur.Parent;

			Node b = a.Clone();
			that.ReplaceWith(b);
		
return false;
}
}}
