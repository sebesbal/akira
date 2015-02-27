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
    public class Rule2
    {
        public string ID;
        public virtual bool Apply(XElement node) { return false; }
        public static Rule2 Load(string fileName)
        {
            if (fileName == "cs")
            {
                return new Cs();
            }

            Assembly assembly = Assembly.LoadFile(Directory.GetCurrentDirectory() + "/" + fileName);
            var types = assembly.GetTypes();
            var ruleClass = types[0];
            var rule = assembly.CreateInstance(ruleClass.Name);
            Rule2 result = rule as Rule2;
            if (result == null) throw new Exception("broke");
            return result;
        }
    }

    public class Akira : Rule2
    {
        List<Rule2> Rules = new List<Rule2>();
        public void Run(string fileName)
        {
            XDocument doc = XDocument.Load(fileName);
            XElement e = doc.Root;
            Apply(e);
        }
        public override bool Apply(XElement node)
        {
            if (node.Name != "akira") return false;

            //return base.Apply(ref node);
            XElement t = new XElement("lofusz");
            t.Value = "fasza!";


            var v = node.Elements();
            foreach (XElement n in v)
            {
                if (n.Name == "apply")
                {
                    XAttribute a = n.Attribute("src");
                    Rule2 rule = Load(a.Value);
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
            foreach (Rule2 r in Rules)
            {
                if (r.Apply(node))
                {
                    return true;
                }
            }
            return false;
        }
    }

    public class Cs : Rule2
    {
        public override bool Apply(XElement node)
        {
            if (node.Name != "cs" || (node.Attribute("loaded") != null && node.Attribute("loaded").Value == "true")) return false;
            string code = node.Value;
            string path = node.Attribute("src").Value;


            var csc = new CSharpCodeProvider(new Dictionary<string, string>() { { "CompilerVersion", "v3.5" } });
            var parameters = new CompilerParameters(new[] { "mscorlib.dll", "System.Core.dll", "System.Xml.dll", "System.Xml.Linq.dll", "akira.dll" }, path, true);
            //parameters.ReferencedAssemblies.Add(typeof(System.Xml.Linq.dll).Assembly.Location);
            parameters.GenerateExecutable = false;
            CompilerResults results = csc.CompileAssemblyFromSource(parameters, code);
            results.Errors.Cast<CompilerError>().ToList().ForEach(error => Console.WriteLine(error.ErrorText));
            var assembly = results.CompiledAssembly;
            // return GetExeFromAssembly(assembly);
            node.SetAttributeValue("loaded", "true");
            return true;
        }
    }

    public class Node
    {
        public List<Node> Children = new List<Node>();
    }

    public class Text : Node
    {
        public string Language;
        public string Content;
    }

    public class Rule : Node
    {
        public string ID;
        public virtual Node Apply(Node node) { return null; }
    }

    /// <summary>
    /// Executable code (Assembly)
    /// </summary>
    public class Exe : Rule
    {

    }

    public class TestRule : Exe
    {
        public override Node Apply(Node node) { return null; }
    }

    /// <summary>
    /// Creates Exe from string
    /// </summary>
    public class Compiler : Exe
    {
        public string Language;
        override public Node Apply(Node node)
        {
            if (node is Text)
            {
                Text text = (Text)node;
                if (text.Language == Language)
                {
                    return Compile(text.Content, ID);
                }
            }
            return null;
        }

        protected virtual Exe Compile(string code, string path) { return null; }
    }

    /// <summary>
    /// Creates Node from string
    /// </summary>
    public class Parser : Exe
    {

    }

      
    /*
    public class SourceCode : Node
    {
        public string Code;
    }

    public class NativeNode: Node
    {
        public string Code;
    }

    public interface ICompiler
    {
        void Compile(string code, string path);
    }

    public interface IRule
    {
        Node Left { get; set; }
        Node Right { get; set; }
    }

    public interface IParser
    {
        Node parse(string src);
    }

    public class Context
    {
        public IParser Parser { get; set; }
        public ICompiler Compiler { get; set; }
        public List<IRule> Rules { get; set; }
    }
    */
}
