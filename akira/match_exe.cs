using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace akira
{
    /// <summary>
    /// Compiles match rules
    /// </summary>
    public class match_exe : Rule
    {
        public override bool Apply(Context ctx, ref XElement node)
        {
            if (!(node.Name == "rule" && node.MatchAttribute("type", "match"))) return false;
            // Instance result = new Instance(node.Elements().ElementAt(0), node.Elements().ElementAt(1));
            Rule result = GenerateInstance(ctx, node);
            result.ID = node.Attribute("src").Value;
            // ctx.Rules.Add(node.Attribute("src").Value, result);
            ctx.DefineRule(result);


            node.Remove();
            node = null;

            //XElement m = new XElement("txt", GenerateClass(ctx, node));
            //node.ReplaceWith(m);
            //node = m;

            return true;
        }

        void FindRefs(XElement node, List<string> refs, List<string> conds)
        {
            XAttribute a;
            if (node.GetAttribute("ref", out a))
            {
                refs.Add(a.Value);
            }
            if (node.GetAttribute("pre", out a))
            {
                conds.Add(a.Value);
            }
            if (node.GetAttribute("post", out a))
            {
                conds.Add(a.Value);
            }

            //if (node.Name == "cond")
            //{
            //    conds.Add(node.Value);
            //}
            foreach (var item in node.Elements())
            {
                FindRefs(item, refs, conds);
            }
        }

        delegate bool check();

        protected string GenerateCode(Context ctx, XElement node)
        {
            List<string> refs = new List<string>();
            List<string> conds = new List<string>();
            FindRefs(node.Elements().ElementAt(0), refs, conds);
            string refDefs = "";
            foreach (var item in refs)
            {
                refDefs += "[IsVar()] public XElement " + item + "; \n";
            }

            string condDefs = "";
            foreach (var item in conds)
            {
                condDefs += "conds.Add(delegate(){\n" + item + "\n}); \n";
            }

            string name = node.Attribute("src").Value; // ctx.GenName();
            string code = "using System; using akira; using System.Xml.Linq;\n"
            + "namespace akira {\n"
            + "public class " + name + " : match\n"
            + "{ \n"
            + refDefs + "\n"
            + "public " + name + "(){ \n" + condDefs + "\n}\n"
            + "}}\n ";

            return code;
        }

        protected Rule GenerateInstance(Context ctx, XElement node)
        {
            string name = node.Attribute("src").Value;
            Assembly a = null;
            Type t = null;
            if (! ctx.GetType(name, ref t, ref a))
            {
                ctx.GetType(name, GenerateCode(ctx, node), ref t, ref a);
            }
            var o = a.CreateInstance(t.FullName);
            t.GetField("left").SetValue(o, node.Elements().ElementAt(0));
            t.GetField("right").SetValue(o, node.Elements().ElementAt(1));
            return (Rule)o;
        }
    }

}
