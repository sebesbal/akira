using System; using akira;
namespace akira {
public class gen0: Rule
{
public override bool Apply(Context ctx, ref Node that)
{
Node cur = that;
if ("?" != cur.Name || 1 != cur.Children.Count) return false;
cur = cur.First;
Node a = cur;
cur = cur.Parent;
 Node.Replace(ref that, __("citrom")); 
return true;
}
}}
