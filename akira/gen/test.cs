using System; using akira;
namespace akira {
public class test: Rule
{
public override bool ApplyAfter(Context ctx, ref Node that)
{
Node cur = that;
if ("test" != cur.Name || 2 != cur.Children.Count) return false;
cur = cur.First;
Node a = cur;
cur = cur.Next;
Node b = cur;
cur = cur.Parent;
 Console.WriteLine("a:" + a.ToString() + "b:" + b.ToString()); 
 if (! a.Eq(b)) Console.WriteLine("Test failed!"); 
 return false; 
return true;
}
}}
