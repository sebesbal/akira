using System; using akira;
namespace akira {
public class gen2: Rule
{
public override bool Apply(Context ctx, ref Node that)
{
Node cur = that;
if ("-->" != cur.Name || 2 != cur.Children.Count) return false;
cur = cur.First;
Node a = cur;
cur = cur.Next;
Node b = cur;
cur = cur.Parent;
if ("replace" != cur.Name || 0 != cur.Children.Count) return false;
if ("write" != cur.Name || 0 != cur.Children.Count) return false;
return true;
}
}}
