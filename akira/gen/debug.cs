using System; using akira;
namespace akira {
public class debug: Rule
{
public override bool Apply(Context ctx, ref Node that)
{
Node cur = that;
if (!Match(cur, "a", 2)) return false;
cur = cur.First;
Node b = cur;
cur = cur.First;
if (!Match(cur, "b", 0)) return false;
cur = cur.Parent;
cur = cur.Next;
if (!Match(cur, "c", 0)) return false;
cur = cur.Parent;
 Console.WriteLine(b); 
return false;
}
}}
