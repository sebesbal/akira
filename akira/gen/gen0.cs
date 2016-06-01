using System; using akira;
namespace akira {
public class gen0: Rule
{
public override bool Apply(Context ctx, ref Node that)
{
Node cur = that;
if ("alma" != cur.Name || 0 != cur.Children.Count) return false;
 Node.Replace(ref that, __("korte")); 
return true;
}
}}
