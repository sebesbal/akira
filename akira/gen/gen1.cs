using System; using akira;
namespace akira {
public class gen1: Rule
{
public override bool Apply(Context ctx, ref Node that)
{
Node cur = that;
if ("korte" != cur.Name || 0 != cur.Children.Count) return false;
 Node.Replace(ref that, __("citrom")); 
return false;
}
}}
