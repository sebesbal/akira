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
    public class Match : Rule
    {
        public override bool Apply(Context ctx, ref XElement node)
        {
            if (node.Name != "rule") return false;

            XAttribute a = node.Attribute("type");

            if (!(a == null || a.Value == "match")) return false;

            Instance result = new Instance(node.Elements().ElementAt(0), node.Elements().ElementAt(1));
            ctx.Rules.Add(node.Attribute("src").Value, result);

            node.Remove();
            node = null;

            return true;
        }

        void FindRefs(XElement node, List<string> refs)
        {
            XAttribute a;
            if (node.GetAttribute("ref", out a))
            {
                refs.Add(a.Value);
            }
            foreach (var item in node.Elements())
            {
                FindRefs(item, refs);
            }
        }

        delegate bool check();

        protected string GenerateClass(Context ctx, XElement node)
        {
            
            List<string> refs = new List<string>();
            FindRefs(node, refs);
            string refDefs = "";
            foreach (var item in refs)
            {
                refDefs += "public XElement " + item + ";\n";
            }

            string code = "public class " + ctx.GenName() + " : akira.Match.Instance"
                + "{\n"
                + refDefs
                + "}\n";

            return code;
        }

        [System.AttributeUsage(System.AttributeTargets.Field)]
        public class IsVar : System.Attribute
        {
            public IsVar() { }
        }

        public class Instance: Rule
        {
            XElement left;
            XElement right;

            [IsVar()]
            public XElement a;
            [IsVar()]
            public XElement b;

            public Instance(XElement left, XElement right)
            {
                this.left = left;
                this.right = right;
            }
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
                if (a.Name == "any")
                {
                    string av = a.Attribute("ref").Value;
                    if (varDefined(av))
                    {
                        return unify(getVar(av), b);
                    }
                    else
                    {
                        setVar(av, b);
                        return true;
                    }
                }
                else if (a.Name == b.Name && a.Nodes().Count() == b.Nodes().Count())
                {
                    var bitem = b.Elements().GetEnumerator();
                    bitem.MoveNext();
                    foreach (var aitem in a.Elements())
                    {
                        if (!unify(aitem, bitem.Current))
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
}
