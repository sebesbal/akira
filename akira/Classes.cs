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
        public virtual bool Apply(ref XElement node) { return false; }
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
            Apply(ref e);
        }
        public void Save(string fileName)
        {
            doc.Save(fileName);
        }
        public override bool Apply(ref XElement node)
        {
            if (node.Name != "akira") return false;

            var v = node.Elements().ToArray();
            foreach (XElement n in v)
            {
                XElement m = n;
                while (ApplyRules(ref m)) ;

                if (m != null && m.Name == "apply")
                {
                    XAttribute a = m.Attribute("src");
                    Rule rule = Load(a.Value + ".dll");
                    Rules.Insert(0, rule);
                    m.Remove();
                }
            }

            return true;
        }
        public bool ApplyRules(ref XElement node)
        {
            foreach (Rule r in Rules)
            {
                if (r.Apply(ref node))
                {
                    return true;
                }
            }
            return false;
        }
    }

}
