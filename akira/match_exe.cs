using System;
using System.Collections.Generic;
using System.Linq;
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
            ctx.Rules.Add(node.Attribute("src").Value, result);

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

        protected string GenerateClass(Context ctx, XElement node)
        {
            List<string> refs = new List<string>();
            List<string> conds = new List<string>();
            FindRefs(node.Elements().ElementAt(0), refs, conds);
            string refDefs = "";
            foreach (var item in refs)
            {
                refDefs += "[IsVar()] public XElement " + item + "; ";
            }

            string condDefs = "";
            foreach (var item in conds)
            {
                condDefs += "conds.Add(delegate(){ " + item + "}); ";
            }

            string name = ctx.GenName();
            string code = "using System; using akira; using System.Xml.Linq;"
            + "namespace akira {"
            + "public class " + name + " : match"
            + "{ "
            + refDefs
            + "public " + name + "(){ " + condDefs + "}"
            + "}} ";

            return code;
        }

        //public class gen0 : akira.Match.Instance
        //{

        //}

        //protected Instance GenerateInstance(Context ctx, XElement node)
        //{
        //    string code = GenerateClass(ctx, node);

        //    // var a = Akira.AssemblyFromCode(code);

        //    Assembly assembly = Assembly.LoadFile(Directory.GetCurrentDirectory() + "/gen0.dll");

        //    Type t = assembly.GetTypes()[0];
        //    if (t.IsSubclassOf(typeof(Instance)))
        //    {
        //        var o = assembly.CreateInstance(t.FullName);
        //        Instance i = o as Instance;
        //        i.left = node.Elements().ElementAt(0);
        //        i.right = node.Elements().ElementAt(1);
        //        return i;
        //    }
        //    return null;

        //}

        protected Rule GenerateInstance(Context ctx, XElement node)
        {
            string code = GenerateClass(ctx, node);

            var a = akira.AssemblyFromCode(code);

            // Assembly assembly = Assembly.LoadFile(Directory.GetCurrentDirectory() + "/gen0.dll");

            Type t = a.GetTypes()[0];
            var o = a.CreateInstance(t.FullName);

            t.GetField("left").SetValue(o, node.Elements().ElementAt(0));
            t.GetField("right").SetValue(o, node.Elements().ElementAt(1));

            return (Rule)o;
        }
    }

}
