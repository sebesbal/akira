using System; using akira;
namespace akira {
public class write: Rule
{
public override bool Apply(Context ctx, ref Node that)
{
Node cur = that;
if ("write" != cur.Name || 1 != cur.Children.Count) return false;
cur = cur.First;
Node a = cur;
cur = cur.Parent;
 that.ReplaceWith(_c(" that.ReplaceWith(" + a.ToCs() + "); ")); 
return false;
}
}}
