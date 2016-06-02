using System; using akira;
namespace akira {
public class test: Rule
{
public override bool ApplyAfter(Context ctx, ref Node that)
{
Node cur = that;
 Console.WriteLine("a:" + a.ToString() + "b:" + b.ToString()); 
 if (! a.Eq(b)) Console.WriteLine("Test failed!"); 
 return false; 
return false;
}
}}
