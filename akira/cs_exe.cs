using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace akira
{
    /// <summary>
    /// <rule type="cs"> --> <rule type="exe"/>
    /// type "cs" means the content is a full .cs code
    /// Interprets full cs code (with usings). See test_cs_rule.aki
    /// </summary>
    class cs_exe : Rule
    {
        public override bool Apply(Context ctx, ref XElement node)
        {
            if (!(node.Name == "rule" && node.MatchAttribute("type", "cs"))) return false;
            // string code = node.Value;
            string code = node.Attribute("code").Value;
            string path = node.Attribute("src").Value;
            var csc = new CSharpCodeProvider(new Dictionary<string, string>() { { "CompilerVersion", "v4.0" } });
            var parameters = new CompilerParameters(new[] { "mscorlib.dll", "System.Core.dll", "System.Xml.dll", "System.Xml.Linq.dll", "akira.dll" }, path + ".dll", true);
            parameters.GenerateExecutable = false;
            CompilerResults results = csc.CompileAssemblyFromSource(parameters, code);
            results.Errors.Cast<CompilerError>().ToList().ForEach(error => Console.WriteLine(error.ErrorText));
            var assembly = results.CompiledAssembly;
            //node.SetAttributeValue("loaded", "true");
            node = null;
            return true;
        }
    }

    /// <summary>
    /// The head of a rule
    /// name="rule" + type="" means this is the root of a cs rule --> template for a Rule class. The children will be the content of the rule
    /// </summary>
    class cs_rule : Rule
    {
        public override bool Apply(Context ctx, ref XElement node)
        {
            if (!(node.Name == "rule" && node.Attribute("type") == null)) return false;
            string code = node.Attribute("code").Value;
            // string path = node.Attribute("src").Value;
            string path = ctx.GenName();
            var csc = new CSharpCodeProvider(new Dictionary<string, string>() { { "CompilerVersion", "v4.0" } });
            var parameters = new CompilerParameters(new[] { "mscorlib.dll", "System.Core.dll", "System.Xml.dll", "System.Xml.Linq.dll", "akira.dll" }, path + ".dll", true);
            parameters.GenerateExecutable = false;
            CompilerResults results = csc.CompileAssemblyFromSource(parameters, code);
            results.Errors.Cast<CompilerError>().ToList().ForEach(error => Console.WriteLine(error.ErrorText));
            var assembly = results.CompiledAssembly;
            node = null;
            return true;
        }


        protected string GenerateClass(Context ctx, XElement node)
        {
            string name = ctx.GenName();
            string code = "using System; using akira; using System.Xml.Linq;"
            + "namespace akira {"
            + "public class " + name + " : match"
            + "{ "
            + "public " + name + "(){" + "" + "}"
            + "public override bool Apply(Context ctx, ref XElement node)"
            + "{"
            + "}"
            + "}}";
            return code;
        }

        protected string GetRuleBody(Context ctx, XElement node)
        {
            string s = "";



            return s;
        }
        
        protected string Content()
        {
            string cs = "";
            return cs;
        }

        protected Rule GenerateInstance(Context ctx, XElement node)
        {
            string code = GenerateClass(ctx, node);
            var a = akira.AssemblyFromCode(code);
            Type t = a.GetTypes()[0];
            var o = a.CreateInstance(t.FullName);
            t.GetField("left").SetValue(o, node.Elements().ElementAt(0));
            t.GetField("right").SetValue(o, node.Elements().ElementAt(1));
            return (Rule)o;
        }
    }
}
