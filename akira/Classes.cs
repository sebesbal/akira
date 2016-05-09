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

    class Block
    {
        // public Dictionary<string, Rule> Rules = new Dictionary<string, Rule>();
        public Stack<Rule> DefinedRules = new Stack<Rule>();
        public Stack<Rule> ActiveRules = new Stack<Rule>();
    }

    public class Context
    {
        //public Dictionary<string, Rule> Rules = new Dictionary<string, Rule>();
        public Context()
        {
            PushBlock();
        }
        private Stack<Block> blocks = new Stack<Block>();
        private Block currentBlock;
        public string GenName()
        {
            return "gen" + nameCount++;
        }
        private int nameCount = 0;
        public string dummy;
        public void PushBlock()
        {
            currentBlock = new Block();
            blocks.Push(currentBlock);
        }
        public void PopBlock()
        {
            blocks.Pop();
            currentBlock = blocks.Peek();
        }
        public void DefineRule(Rule r)
        {
            currentBlock.DefinedRules.Push(r);
        }
        public void ActivateRule(Rule r)
        {
            currentBlock.ActiveRules.Push(r);
        }
        public System.Collections.Generic.IEnumerable<Rule> DefinedRules()
        {
            foreach (var block in blocks)
            {
                foreach (var Rule in block.DefinedRules)
                {
                    yield return Rule;
                }
            }
        }
        public System.Collections.Generic.IEnumerable<Rule> ActiveRules()
        {
            foreach (var block in blocks)
            {
                foreach (var Rule in block.ActiveRules)
                {
                    yield return Rule;
                }
            }
        }
        public Rule DefinedRule(string id)
        {
            foreach (var rule in DefinedRules())
            {
                if (rule.ID == id) return rule;
            }
            return null;
        }
        public bool GetType(string name, ref Type type, ref System.Reflection.Assembly assembly)
        {
            assembly = System.Reflection.Assembly.GetCallingAssembly();
            type = assembly.GetType("akira." + name);
            return type != null;
        }

        public void GetType(string name, string code, ref Type type, ref System.Reflection.Assembly assembly)
        {
            assembly = akira.AssemblyFromCode(code);
            type = assembly.GetTypes()[0];
            File.WriteAllText("../akira/gen/" + name + ".cs", code);
        }
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
        public virtual bool ApplyAfter(Context ctx, ref XElement node) { return false; }
    }

    public class akira : Rule
    {
        // List<Rule> Rules = new List<Rule>();
        Context ctx = new Context();
        public XDocument doc { get; protected set; }
        public akira()
        {
            ctx.ActivateRule(new cs_rule());
            ctx.ActivateRule(new cs_exe());
            ctx.ActivateRule(new match_exe());
            ctx.ActivateRule(new cs_cs());
            ctx.ActivateRule(new if_cs());
            ctx.ActivateRule(new cs_run());
            ctx.ActivateRule(new apply());
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
            // Context ctx = new Context();
            Apply(ctx, ref e);
        }
        public void RunXml(string fileName)
        {
            doc = XDocument.Load(fileName);
            XElement e = doc.Root;
            // Context ctx = new Context();
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

                if (m != null)
                {
                    foreach (XElement u in m.Elements())
                    {
                        XElement u1 = u;
                        Apply(ctx, ref u1);
                    }

                    while (m != null && ApplyRulesAfter(ctx, ref m)) ;
                }
            }
            return true;
        }

        public bool ApplyRules(Context ctx, ref XElement node)
        {
            foreach (Rule r in ctx.ActiveRules().ToArray())
            {
                if (r.Apply(ctx, ref node))
                {
                    return true;
                }
            }
            return false;
        }

        public bool ApplyRulesAfter(Context ctx, ref XElement node)
        {
            foreach (Rule r in ctx.ActiveRules().ToArray())
            {
                if (r.ApplyAfter(ctx, ref node))
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
