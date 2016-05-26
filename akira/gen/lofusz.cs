using System; using akira; using System.Xml.Linq;
namespace akira {
public class lofusz : match
{ 
[IsVar()] public Node a; 
[IsVar()] public Node b; 

public lofusz(){ 
conds.Add(delegate(){
return a.Name == "num";
}); 

}
}}
 