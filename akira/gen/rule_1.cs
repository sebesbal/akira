using System; using akira; using System.Xml.Linq;namespace akira {public class rule_1 : Rule{ public rule_1(){}public override bool Apply(Context ctx, ref Node that){if (that.Name == "num") {
that.Value = "3";
that = null;
return true;
}
else return false;
}}}