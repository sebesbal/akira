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
    public class Rule
    {
        public string ID;
        public virtual bool Apply(XElement node) { return false; }
        public static Rule Load(string fileName)
        {
            Assembly assembly = Assembly.LoadFile(Directory.GetCurrentDirectory() + "/" + fileName);
            var types = assembly.GetTypes();
            var ruleClass = types[0];
            var rule = assembly.CreateInstance(ruleClass.FullName);
            Rule result = rule as Rule;
            if (result == null) throw new Exception("broke");
            return result;
        }
    }

    public class Akira : Rule
    {
        List<Rule> Rules = new List<Rule>();
        public void Run(string fileName)
        {
            XDocument doc = XDocument.Load(fileName);
            XElement e = doc.Root;
            Apply(e);
        }
        public override bool Apply(XElement node)
        {
            if (node.Name != "akira") return false;

            var v = node.Elements();
            foreach (XElement n in v)
            {
                if (n.Name == "apply")
                {
                    XAttribute a = n.Attribute("src");
                    Rule rule = Load(a.Value + ".dll");
                    Rules.Insert(0, rule);
                }
                else
                {
                    while (ApplyRules(n)) ;
                }
            }

            return true;
        }
        public bool ApplyRules(XElement node)
        {
            foreach (Rule r in Rules)
            {
                if (r.Apply(node))
                {
                    return true;
                }
            }
            return false;
        }
    }

}
