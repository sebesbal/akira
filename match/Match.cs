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

        protected class Instance: Rule
        {
            XElement left;
            XElement right;
            Dictionary<string, XElement> map = new Dictionary<string, XElement>();
            public Instance(XElement left, XElement right)
            {
                this.left = left;
                this.right = right;
            }
            public override bool Apply(Context ctx, ref XElement node)
            {
                map.Clear();

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
                if (a.Name == "ref")
                {
                    string av = a.Value;
                    if (map.ContainsKey(av))
                    {
                        return unify(map[av], b);
                    }
                    else
                    {
                        map[av] = b;
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
                    elem.ReplaceWith(map[elem.Value].DeepCopy());
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
