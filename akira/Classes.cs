﻿using Microsoft.CSharp;
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

        public static bool MatchAttribute(this XElement node, string name, string value)
        {
            var attribute = node.Attribute(name);
            return attribute != null && attribute.Value == value;
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
        public string GenName()
        {
            return "gen" + nameCount++;
        }
        private int nameCount = 0;
        public string dummy;
    }

    public class exe
    {
        public static exe Load(string fileName)
        {
            Assembly assembly = Assembly.LoadFile(Directory.GetCurrentDirectory() + "/" + fileName);
            return Load(assembly);
        }
        public static exe Load(Assembly assembly)
        {
            var types = assembly.GetTypes();
            foreach (var ruleClass in types)
            {
                if (ruleClass.IsSubclassOf(typeof(exe)))
                {
                    return (exe)assembly.CreateInstance(ruleClass.FullName);
                }
            }
            throw new Exception("exe descendentant not found");
        }
    }

    public class Program : exe
    {
        public virtual void Run(Context ctx) { }
    }

    public class Rule : exe
    {
        public string ID;
        public virtual bool Apply(Context ctx, ref XElement node) { return false; }
    }

    public class akira : Rule
    {
        List<Rule> Rules = new List<Rule>();
        public XDocument doc { get; protected set; }
        public akira()
        {
            Rules.Add(new cs_exe());
            Rules.Add(new match_exe());
            Rules.Add(new cs_cs());
            Rules.Add(new cs_run());
        }
        public void Run(string fileName)
        {
            doc = new XDocument();
            doc.Add(new XElement("akira"));
            XElement e = doc.Root;
            XElement apply = new XElement("apply");
            apply.SetAttributeValue("src", "slp_ast");
            e.Add(apply);
            XElement slp = new XElement("slp");
            slp.Value = File.ReadAllText(fileName);
            e.Add(slp);
            Context ctx = new Context();
            Apply(ctx, ref e);
        }
        public void RunXml(string fileName)
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

                if (m == null)
                {
                    // Skip
                }
                else if (m.Name == "apply")
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
                        rule = Load(ruleID + ".dll") as Rule;
                    }
                    Rules.Insert(0, rule);
                    m.Remove();
                }
                //else if (m.Name == "run")
                //{
                //    XAttribute a = m.Attribute("src");
                //    string ruleID = a.Value;
                //    Rule rule;
                //    if (ctx.Rules.ContainsKey(ruleID))
                //    {
                //        rule = ctx.Rules[ruleID];
                //    }
                //    else
                //    {
                //        rule = Load(ruleID + ".dll");
                //    }
                //    Rules.Insert(0, rule);
                //    m.Remove();
                //}
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

        public static Assembly AssemblyFromCode(string code)
        {
            var csc = new CSharpCodeProvider(new Dictionary<string, string>() { { "CompilerVersion", "v4.0" } });
            var parameters = new CompilerParameters(new[] { "mscorlib.dll", "System.Core.dll", "System.Xml.dll", "System.Xml.Linq.dll", "akira.dll"}, "gen0.dll", true);
            parameters.GenerateExecutable = false;
            
            CompilerResults results = csc.CompileAssemblyFromSource(parameters, code);
            results.Errors.Cast<CompilerError>().ToList().ForEach(error => Console.WriteLine(error.ErrorText));
            var assembly = results.CompiledAssembly;
            // results.
            return assembly;
        }

        public static object InstanceFromCode(string code)
        {
            var csc = new CSharpCodeProvider(new Dictionary<string, string>() { { "CompilerVersion", "v3.5" } });
            var parameters = new CompilerParameters(new[] { "mscorlib.dll", "System.Core.dll", "System.Xml.dll", "System.Xml.Linq.dll", "akira.dll" });
            parameters.GenerateExecutable = false;
            CompilerResults results = csc.CompileAssemblyFromSource(parameters, code);
            results.Errors.Cast<CompilerError>().ToList().ForEach(error => Console.WriteLine(error.ErrorText));
            var assembly = results.CompiledAssembly;

            var t = assembly.GetTypes()[0];
            return assembly.CreateInstance(t.FullName);
        }
    }
}
