using System; using akira;
namespace akira {
public class gen1: Rule
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
 Node.Replace(ref that, __("rule", __(a), __("write", __(b)))); 
return true;
}
}}
