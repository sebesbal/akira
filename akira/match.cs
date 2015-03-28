using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace akira
{
    [System.AttributeUsage(System.AttributeTargets.Field)]
    public class IsVar : System.Attribute
    {
        public IsVar() { }
    }

    public delegate bool Condition();

    public class match : Rule
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
                    XAttribute att;
                    if (aitem.GetAttribute("pre", out att))
                    {
                        var cond = conds[condIndex++];
                        if (!cond())
                        {
                            return false;
                        }
                    }

                    if (!unify(aitem, bitem.Current))
                    {
                        return false;
                    }

                    if (aitem.GetAttribute("post", out att))
                    {
                        var cond = conds[condIndex++];
                        if (!cond())
                        {
                            return false;
                        }
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
            return (XElement)GetType().GetField(name).GetValue(this);
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
            if (elem.Name == "any")
            {
                // elem.ReplaceWith(map[elem.Value].DeepCopy());
                elem.ReplaceWith(getVar(elem.Attribute("ref").Value).DeepCopy());
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
