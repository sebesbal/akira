using System; using akira; using System.Xml.Linq;
namespace akira {
public class lofusz : match
{ 
[IsVar()] public XElement a; 
[IsVar()] public XElement b; 

public lofusz(){ 
conds.Add(delegate(){
return a.Name == "num";
}); 

}
}}
 