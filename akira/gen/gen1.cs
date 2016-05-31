using System; using akira;
namespace akira {
public class gen1: Rule
{
public override bool Apply(Context ctx, ref Node that)
{
begin:
Node cur = that;
if ("-->" != cur.Name || 2 != cur.Children.Count) return false;
cur = cur.First;
Node a = cur;
cur = cur.Next;
Node b = cur;
cur = cur.Parent;
return false;
}
}}
