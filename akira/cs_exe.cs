using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        public override bool Apply(Context ctx, ref Node node)
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
        public override bool ApplyAfter(Context ctx, ref Node node)
        {
            if (!(node.Name == "rule" && node.Attribute("type") == null)) return false;
            ruleName = node.Attribute("src").Value;
            Rule rule = GenerateInstance(ctx, node);
            ctx.ActivateRule(rule);
            node.Remove();
            node = null;
            return true;
        }

        bool after = false;
        string ruleName;

        protected string GenerateCode(Context ctx, Node node)
        {
            string code = "using System; using akira; using System.Xml.Linq;"
            + "namespace akira {"
            + "public class " + ruleName + " : Rule"
            + "{ "
            + "public " + ruleName + "(){" + "" + "}"
            + (after ? "public override bool ApplyAfter(Context ctx, ref Node that)"
                     : "public override bool Apply(Context ctx, ref Node that)")
            + "{"
            + GetRuleBody(ctx, node)
            + "}"
            + "}}";
            return code;
        }

        protected string GetRuleBody(Context ctx, Node node)
        {
            string code = "";

            code = "if (" + node.Attribute("code").Value + ") {\n";
            foreach (var item in node.Elements())
            {
                if (item.Name != "cs") return "";
                code += item.Attribute("code").Value + "\n";
            }
            code += "that = null;\n";
            code += "return true;\n";
            code += "}\n";
            code += "else return false;\n";

            return code;
        }

        protected Rule GenerateInstance(Context ctx, Node node)
        {
            Assembly a = null;
            Type t = null;
            if (!ctx.GetType(ruleName, ref t, ref a))
            {
                ctx.GetType(ruleName, GenerateCode(ctx, node), ref t, ref a);
            }
            var o = a.CreateInstance(t.FullName);
            return (Rule)o;
        }
    }
}
