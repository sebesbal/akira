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
            assembly = AssemblyFromCode(code);
            type = assembly.GetTypes()[0];
            File.WriteAllText("../akira/gen/" + name + ".cs", code);
        }

        static int dllCount = 0;

        public static Assembly AssemblyFromCode(string code)
        {
            var csc = new CSharpCodeProvider(new Dictionary<string, string>() { { "CompilerVersion", "v4.0" } });
            var parameters = new CompilerParameters(new[] { "mscorlib.dll", "System.Core.dll", "System.dll", "System.Xml.dll", "System.Xml.Linq.dll", "akira.dll", "slp_ast.dll" }, "gen" + dllCount++ + ".dll", true);
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
            var parameters = new CompilerParameters(new[] { "mscorlib.dll", "System.Core.dll", "System.dll", "System.Xml.dll", "System.Xml.Linq.dll", "akira.dll", "slp_ast.dll" });
            parameters.GenerateExecutable = false;
            CompilerResults results = csc.CompileAssemblyFromSource(parameters, code);
            results.Errors.Cast<CompilerError>().ToList().ForEach(error => Console.WriteLine(error.ErrorText));
            var assembly = results.CompiledAssembly;

            var t = assembly.GetTypes()[0];
            return assembly.CreateInstance(t.FullName);
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
        public virtual bool Apply(Context ctx, ref Node node) { return false; }
        public virtual bool ApplyAfter(Context ctx, ref Node node) { return false; }
    }

    public class akira : Rule
    {
        // List<Rule> Rules = new List<Rule>();
        Context ctx = new Context();
        public XDocument doc { get; protected set; }
        Node root;
        public akira()
        {
            ctx.ActivateRule(new rule());
            //ctx.ActivateRule(new misc_atttribute());
            //ctx.ActivateRule(new cs_rule());
            //ctx.ActivateRule(new cs_exe());
            //ctx.ActivateRule(new match_exe());
            //ctx.ActivateRule(new cs_cs());
            //ctx.ActivateRule(new if_cs());
            //ctx.ActivateRule(new cs_run());
            //ctx.ActivateRule(new apply());
            //ctx.ActivateRule(new misc_variables());
        }

        public void Run(string fileName)
        {
            root = Node.ParseFile(fileName);
            Apply(ctx, ref root);
        }

        public void Save(string fileName)
        {
            // doc.Save(fileName);
            root.SaveToXml(fileName);
        }

        public override bool Apply(Context ctx, ref Node node)
        {
            begin:
            if (node == null) return false;

            if (ApplyRules(ctx, ref node))
            {
                goto begin;
            }

            // while (node != null && ApplyRules(ctx, ref node)) ;
            //if (node == null) return false;

            var v = node.Elements().ToArray();
            foreach (Node n in v)
            {
                Node m = n;
                Apply(ctx, ref m);
            }

            if (ApplyRulesAfter(ctx, ref node))
            {
                goto begin;
            }
            
            return false;
        }

        public bool ApplyRules(Context ctx, ref Node node)
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

        public bool ApplyRulesAfter(Context ctx, ref Node node)
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
    }
}
