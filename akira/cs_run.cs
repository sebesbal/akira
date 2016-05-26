using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace akira
{
    class cs_run : Rule
    {
        public override bool Apply(Context ctx, ref Node node)
        {
            if (!(node.Name == "run" && node.MatchAttribute("type", "cs"))) return false;
            string code = node.Attribute("code").Value;
            //string path = node.Attribute("src").Value;
            var csc = new CSharpCodeProvider(new Dictionary<string, string>() { { "CompilerVersion", "v4.0" } });
            // var parameters = new CompilerParameters(new[] { "mscorlib.dll", "System.Core.dll", "System.Xml.dll", "System.Xml.Linq.dll", "akira.dll" }, path + ".dll", true);
            var parameters = new CompilerParameters(new[] { "mscorlib.dll", "System.dll", "System.Core.dll", "System.Xml.dll", "System.Xml.Linq.dll", "akira.dll" });
            parameters.GenerateExecutable = false;
            CompilerResults results = csc.CompileAssemblyFromSource(parameters, code);
            results.Errors.Cast<CompilerError>().ToList().ForEach(error => Trace.WriteLine(error.ErrorText));
            var assembly = results.CompiledAssembly;
            var prog = Load(assembly) as Program;
            prog.Run(ctx);
            node = null;
            return true;
        }
    }
}
