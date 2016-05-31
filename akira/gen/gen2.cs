using System; using akira;
namespace akira {
public class gen2: Rule
{
public override bool Apply(Context ctx, ref Node that)
{
begin:
Node cur = that;
if ("alma" != cur.Name || 2 != cur.Children.Count) return false;
cur = cur.First;
Node a = cur;
cur = cur.Next;
Node b = cur;
cur = cur.Parent;
 that.Name = a.Name; 
return false;
}
}}
