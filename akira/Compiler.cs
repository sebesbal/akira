using akira;
using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace akira
{
    // public class 

    public class CsCompiler: Compiler
    {
        //override protected Node Compile(string code, string path)
        //{
        //    var csc = new CSharpCodeProvider(new Dictionary<string, string>() { { "CompilerVersion", "v3.5" } });
        //    var parameters = new CompilerParameters(new[] { "mscorlib.dll", "System.Core.dll" }, path, true);
        //    parameters.GenerateExecutable = true;
        //    CompilerResults results = csc.CompileAssemblyFromSource(parameters, code);
        //    results.Errors.Cast<CompilerError>().ToList().ForEach(error => Console.WriteLine(error.ErrorText));
        //    return null;
        //}

        public CsCompiler()
        {
            Language = "cs";
        }

        override protected Exe Compile(string code, string path)
        {
            var csc = new CSharpCodeProvider(new Dictionary<string, string>() { { "CompilerVersion", "v3.5" } });
            var parameters = new CompilerParameters(new[] { "mscorlib.dll", "System.Core.dll", "akira.dll" }, path, true);
            parameters.GenerateExecutable = false;
            CompilerResults results = csc.CompileAssemblyFromSource(parameters, code);
            results.Errors.Cast<CompilerError>().ToList().ForEach(error => Console.WriteLine(error.ErrorText));
            var assembly = results.CompiledAssembly;
            return GetExeFromAssembly(assembly);
        }

        Exe GetExeFromAssembly(System.Reflection.Assembly assembly)
        {
            //Assembly assembly = Assembly.LoadFile(@"C:\dyn.dll");
            // don't know what or how to cast here
            // looking for a better way to do next 3 lines
            var types = assembly.GetTypes();
            var ruleClass = types[0];
            var rule = assembly.CreateInstance(ruleClass.Name);
            Exe exe = rule as Exe;
            if (exe == null) throw new Exception("broke");
            return exe;
        }
    }
}
