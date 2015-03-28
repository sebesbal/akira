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
    class cs_exe: Rule
    {
        public override bool Apply(Context ctx, ref XElement node)
        {
            //if (node.Name != "cs" || (node.Attribute("loaded") != null && node.Attribute("loaded").Value == "true")) return false;
            if (!(node.Name == "rule" && node.Attribute("type") != null && node.Attribute("type").Value == "cs")) return false;
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
}
