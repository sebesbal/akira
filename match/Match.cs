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
    public class Match : Rule
    {
        public override bool Apply(Context ctx, ref XElement node)
        {
            if (node.Name != "rule") return false;

            XAttribute a = node.Attribute("type");

            if (!(a == null || a.Value == "match")) return false;

            // Instance result = new Instance(node.Elements().ElementAt(0), node.Elements().ElementAt(1));
            Instance result = GenerateInstance(ctx, node);
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
            if (node.Name == "cond")
            {
                conds.Add(node.Value);
            }
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
            + "public class " + name + " : akira.Instance"
            + "{ "
            // + refDefs
            + "public " + name + "(){ " + condDefs + "}"
            + "}} ";

            return code;
        }

        //public class gen0 : akira.Match.Instance
        //{

        //}

        protected Instance GenerateInstance(Context ctx, XElement node)
        {
            string code = GenerateClass(ctx, node);

            // var a = Akira.AssemblyFromCode(code);

            Assembly assembly = Assembly.LoadFile(Directory.GetCurrentDirectory() + "/gen0.dll");

            Type t = assembly.GetTypes()[0];
            if (t.IsSubclassOf(typeof(Instance)))
            {
                var o = assembly.CreateInstance(t.FullName);
                Instance i = o as Instance;
                i.left = node.Elements().ElementAt(0);
                i.right = node.Elements().ElementAt(1);
                return i;
            }
            return null;
            
        }
    }

    [System.AttributeUsage(System.AttributeTargets.Field)]
    public class IsVar : System.Attribute
    {
        public IsVar() { }
    }

    public delegate bool Condition();

    public class Instance : Rule
    {
        public XElement left;
        public XElement right;

        //[IsVar()]
        //public XElement a;
        //[IsVar()]
        //public XElement b;

        protected List<Condition> conds = new List<Condition>();

        int condIndex = 0;

        //public Instance(XElement left, XElement right)
        //{
        //    this.left = left;
        //    this.right = right;

        //    //conds.Add(delegate()
        //    //{
        //    //    return a.Name == "num";
        //    //});
        //}
        public override bool Apply(Context ctx, ref XElement node)
        {
            clearVars();

            if (unify(left, node))
            {
                var result = construct(right);
                node.ReplaceWith(result);
                node = result;
                return true;
            }

            return false;
        }

        bool unify(XElement a, XElement b)
        {
            return false;

            if (a.Name == "any")
            {
                string av = a.Attribute("ref").Value;
                if (varDefined(av))
                {
                    if (!unify(getVar(av), b)) return false;
                }
                else
                {
                    setVar(av, b);
                }
            }

            if (a.Name == "any" || a.Name == b.Name) // && a.Nodes().Count() == b.Nodes().Count())
            {
                var bitem = b.Elements().GetEnumerator();
                bitem.MoveNext();
                foreach (var aitem in a.Elements())
                {
                    if (aitem.Name == "cond")
                    {
                        var cond = conds[condIndex++];
                        if (!cond())
                        {
                            return false;
                        }
                    }
                    else if (!unify(aitem, bitem.Current))
                    {
                        return false;
                    }
                    bitem.MoveNext();
                }
                return true;
            }

            return false;
        }

        void setVar(string name, XElement node)
        {
            GetType().GetField(name).SetValue(this, node);
        }

        XElement getVar(string name)
        {
            return (XElement)typeof(Instance).GetField(name).GetValue(this);
        }

        bool varDefined(string name)
        {
            return getVar(name) != null;
        }

        bool isVar(System.Reflection.FieldInfo field)
        {
            return field.GetCustomAttributes(typeof(IsVar), false).Count() > 0;
        }

        void clearVars()
        {
            condIndex = 0;
            foreach (var item in GetType().GetFields())
            {
                if (isVar(item))
                {
                    item.SetValue(this, null);
                }
            }
        }

        XElement construct(XElement template)
        {
            template = template.DeepCopy();
            ReplaceRefs(template);
            return template;
        }

        void ReplaceRefs(XElement elem)
        {
            if (elem.Name == "ref")
            {
                // elem.ReplaceWith(map[elem.Value].DeepCopy());
                elem.ReplaceWith(getVar(elem.Value).DeepCopy());
            }
            else
            {
                var v = elem.Elements().ToArray();
                foreach (var item in v)
                {
                    ReplaceRefs(item);
                }
            }
        }
    }
}
