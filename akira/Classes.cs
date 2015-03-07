using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace akira
{
    public static class Extensions
    {
        public static bool GetAttribute(this XElement node, string name, out XAttribute attribute)
        {
            attribute = node.Attribute(name);
            return attribute != null;
        }

        public static XElement DeepCopy(this XElement node)
        {
            return new XElement(node);
        }

        public static XElement ShallowCopy(this XElement node)
        {
            XElement result = new XElement(node.Name);
            foreach (var item in node.Attributes())
            {
                result.SetAttributeValue(item.Name, item.Value);
            }
            return result;
        }
    }

    public class Context
    {
        public Dictionary<string, Rule> Rules = new Dictionary<string, Rule>();
    }

    public class Rule
    {
        public string ID;
        public virtual bool Apply(Context ctx, ref XElement node) { return false; }
        public static Rule Load(string fileName)
        {
            Assembly assembly = Assembly.LoadFile(Directory.GetCurrentDirectory() + "/" + fileName);
            var types = assembly.GetTypes();

            foreach (var ruleClass in types)
            {
                if (ruleClass.IsSubclassOf(typeof(Rule)))
                {
                    return (Rule)assembly.CreateInstance(ruleClass.FullName);
                }
            }
            throw new Exception("Rule descendentant not found");
        }
    }

    public class Akira : Rule
    {
        List<Rule> Rules = new List<Rule>();
        XDocument doc;
        public void Run(string fileName)
        {
            doc = XDocument.Load(fileName);
            XElement e = doc.Root;
            Context ctx = new Context();
            Apply(ctx, ref e);
        }
        public void Save(string fileName)
        {
            doc.Save(fileName);
        }
        public override bool Apply(Context ctx, ref XElement node)
        {
            if (node.Name != "akira") return false;

            var v = node.Elements().ToArray();
            foreach (XElement n in v)
            {
                XElement m = n;
                while (m != null && ApplyRules(ctx, ref m)) ;

                if (m != null && m.Name == "apply")
                {
                    XAttribute a = m.Attribute("src");
                    string ruleID = a.Value;
                    Rule rule;
                    if (ctx.Rules.ContainsKey(ruleID))
                    {
                        rule = ctx.Rules[ruleID];
                    }
                    else
                    {
                        rule = Load(ruleID + ".dll");
                    }
                    Rules.Insert(0, rule);
                    m.Remove();
                }
            }

            return true;
        }
        public bool ApplyRules(Context ctx, ref XElement node)
        {
            foreach (Rule r in Rules)
            {
                if (r.Apply(ctx, ref node))
                {
                    return true;
                }
            }
            return false;
        }
    }

}
