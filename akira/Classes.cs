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
    class Block
    {
        public Block()
        {
            for (int i = 0; i < 3; ++i)
            {
                ActiveRules[i] = new Stack<Rule>();
            }
        }
        // public Dictionary<string, Rule> Rules = new Dictionary<string, Rule>();
        public Stack<Rule> DefinedRules = new Stack<Rule>();
        public Stack<Rule>[] ActiveRules = new Stack<Rule>[3];
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

        protected List<string> searchPaths = new List<string>();

        public void AddSearchPath(string path)
        {
            path = Path.Combine(DirWorking, path);
            searchPaths.Remove(path);
            searchPaths.Insert(0, path);
        }

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
            currentBlock.ActiveRules[r.level].Push(r);
        }

        public FileInfo FindModule(string name, string folder = "")
        {
            if (folder == "")
            {
                foreach (var path in searchPaths)
                {
                    FileInfo result = FindModule(name, path);
                    if (result != null)
                    {
                        return result;
                    }
                }
                return null;
            }
            else
            {
                DateTime dt = DateTime.MinValue;
                FileInfo result = null;
                var dir = new DirectoryInfo(folder);
                foreach (var file in dir.GetFiles())
                {
                    if (Path.GetFileNameWithoutExtension(file.Name) == name
                        && file.LastWriteTime > dt)
                    {
                        result = file;
                        dt = file.LastWriteTime;
                    }
                }
                return result;
            }
        }

        public FileInfo FindFile(string fileName, string folder = "")
        {
            if (folder == "")
            {
                foreach (var path in searchPaths)
                {
                    FileInfo result = FindFile(fileName, path);
                    if (result != null)
                    {
                        return result;
                    }
                }
                return null;
            }
            else
            {
                DateTime dt = DateTime.MinValue;
                FileInfo result = null;
                var dir = new DirectoryInfo(folder);
                foreach (var file in dir.GetFiles())
                {
                    if (file.Name == fileName && file.LastWriteTime > dt)
                    {
                        result = file;
                        dt = file.LastWriteTime;
                    }
                }
                return result;
            }
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
        public System.Collections.Generic.IEnumerable<Rule> ActiveRules(int level)
        {
            foreach (var block in blocks)
            {
                foreach (var Rule in block.ActiveRules[level])
                {
                    yield return Rule;
                }
            }
        }

        public System.Collections.Generic.IEnumerable<Rule> ActiveRules()
        {
            for (int i = 0; i < 3; ++i)
            {
                foreach (var item in ActiveRules(i))
                {
                    yield return item;
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

        public void SaveCs(string filePath, string code)
        {
            //File.WriteAllText(PathCs(fileName), code);
            File.WriteAllText(filePath, code);
        }

        public string ModuleName;

        int submoduleCount = 0;
        public string GenSubmodule()
        {
            return ModuleName + "_" + ++submoduleCount;
        }

        public string DirWorking;

        public string DirGen { get { return "../akira/gen/"; } }

        public string DirTrash = "../akira/trash";

        public string LoadCs(string filePath)
        {
            return File.ReadAllText(filePath);
        }

        //public void Import(string name)
        //{
        //    var module = FindModule(name);
        //    if (module != null && module.Extension == ".dll")
        //    {
        //        var ass = Assembly.LoadFrom(module.FullName);
        //        LoadRulesFromAss(ass);
        //    }
        //}

        public void Import(string name)
        {
            var module = FindFile(name + ".dll");
            if (module != null)
            {
                var ass = Assembly.LoadFrom(module.FullName);
                LoadRulesFromAss(ass);
            }
        }

        /// <summary>
        /// Compiles Cs to Dll, and returns the Dll file's path
        /// </summary>
        public Assembly CompileCs(string csPath, out string dllPath)
        {
            var name = Path.GetFileNameWithoutExtension(csPath);
            dllPath = Path.Combine(Path.GetDirectoryName(csPath), name + ".dll");
            
            if (File.Exists(dllPath))
            {
                File.Delete(dllPath);
            }
            var csc = new CSharpCodeProvider(new Dictionary<string, string>() { { "CompilerVersion", "v4.0" } });
            var parameters = new CompilerParameters(new[] { "mscorlib.dll", "System.Core.dll", "System.dll", "System.Xml.dll", "System.Xml.Linq.dll", "akira.dll", "slp_ast.dll" }, dllPath, true);
            parameters.GenerateExecutable = false;

            CompilerResults results = csc.CompileAssemblyFromFile(parameters, csPath);

            if (results.Errors.Count > 0)
            {
                results.Errors.Cast<CompilerError>().ToList().ForEach(error => Console.WriteLine(error.ErrorText));
                return null;
            }
            else
            {
                var assembly = results.CompiledAssembly;
                return assembly;
            }
        }
        
        public void LoadRulesFromAss(Assembly ass)
        {
            foreach (var item in ass.GetTypes())
            {
                if (item.IsSubclassOf(typeof(Rule)))
                {
                    Rule r = (Rule)ass.CreateInstance(item.FullName);
                    ActivateRule(r);
                }
            }
        }

        public void LoadRulesFromDll(string fileName)
        {
            var ass = Assembly.Load(fileName);
            if (ass == null)
            {
                throw new Exception("Can't load dll: " + fileName);
            }
            else
            {
                LoadRulesFromAss(ass);
            }
        }
        
        public void LoadRulesFromCs(string fileName)
        {
            var ass = AssemblyFromCode(LoadCs(fileName));

            if (ass == null)
            {
                string name = Path.GetFileNameWithoutExtension(fileName);
                File.Move(fileName, Path.Combine(DirTrash, name + ".cs"));
            }
            else
            {
                LoadRulesFromAss(ass);
            }
        }

        public void GetType(string fileName, string code, ref Type type, ref System.Reflection.Assembly assembly)
        {
            SaveCs(fileName, code);
            assembly = AssemblyFromCode(code);
            type = assembly.GetTypes()[0];
        }

        static int dllCount = 0;

        public Assembly AssemblyFromCode(string code)
        {
            string dllName = "gen" + dllCount++ + ".dll";
            if (File.Exists(dllName))
            {
                File.Delete(dllName);
            }
            var csc = new CSharpCodeProvider(new Dictionary<string, string>() { { "CompilerVersion", "v4.0" } });
            var parameters = new CompilerParameters(new[] { "mscorlib.dll", "System.Core.dll", "System.dll", "System.Xml.dll", "System.Xml.Linq.dll", "akira.dll", "slp_ast.dll" }, dllName, false);
            parameters.GenerateExecutable = false;

            CompilerResults results = csc.CompileAssemblyFromSource(parameters, code);

            if (results.Errors.Count > 0)
            {
                Console.Write(code);

                results.Errors.Cast<CompilerError>().ToList().ForEach(error => Console.WriteLine(error.ErrorText));
                return null;
            }
            else
            {
                var assembly = results.CompiledAssembly;
                return assembly;
            }
        }

        public object InstanceFromCode(string code)
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
        public int level = 2;
        public string ID;
        public virtual bool Apply(Context ctx, ref Node node) { return false; }
        public virtual bool ApplyAfter(Context ctx, ref Node node) { return false; }
        
        public static Node __(Node node)
        {
            return (Node)node.Clone();
        }

        public static Node __(ICloneable data, params ICloneable[] items)
        {
            return new Node(data, items);
        }

        public static Node _c(string str) { return new Node(new NCode(str)); }
        public static NCode __c(string str) { return new NCode(str); }
    }
    
    public class NActiveNode: ICloneable
    {
        public int level = 2;
        public virtual bool Apply(Context ctx, ref Node node) { return false; }
        public virtual bool ApplyAfter(Context ctx, ref Node node) { return false; }
        public object Clone()
        {
            var result = new NActiveNode();
            result.level = 2;
            return result;
        }
    }

    public class akira : Rule
    {
        // List<Rule> Rules = new List<Rule>();
        public Context ctx = new Context();
        public XDocument doc { get; protected set; }
        public Node root;
        public akira()
        {
            ctx.ActivateRule(new sample());
            ctx.ActivateRule(new tocs());

            ctx.ActivateRule(new read());

            ctx.ActivateRule(new rule());
            ctx.ActivateRule(new parse());

            AutoLoadRules();

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

        void AutoLoadRules()
        {
            var ass = Assembly.GetCallingAssembly();
            var activeRules = ctx.ActiveRules().ToList();
            foreach (var type in ass.GetTypes())
            {
                if (type.IsSubclassOf(typeof(Rule)) 
                    && type.Name != "akira"
                    && type.Name != "Rule")
                {
                    foreach (var item in activeRules)
                    {
                        if (item.GetType() == type) goto next_type;
                    }

                    var rule = (Rule)ass.CreateInstance(type.FullName);
                    ctx.ActivateRule(rule);
                    activeRules.Add(rule);
                }
                next_type:;
            }
        }

        public void Run(string fileName)
        {
            Run(Node.ParseFile(fileName));
        }
        
        public static Node ParseModule(string file)
        {
            string name = Path.GetFileNameWithoutExtension(file);
            string code = File.ReadAllText(file);
            Node n = AkiraParser.Parse(code);
            n.AddFirst(__(":", __("id"), __(name)));
            n.Data = "module";
            return n;
        }

        public static akira Compile(string fileName)
        {
            //Console.WriteLine();
            //Console.WriteLine("Compile " + fileName);
            akira a = new akira();
            FileInfo info = new FileInfo(fileName);
            a.ctx.DirWorking = info.DirectoryName;
            a.ctx.ModuleName = Path.GetFileNameWithoutExtension(fileName);
            a.ctx.AddSearchPath("");
            var n = ParseModule(fileName);
            n.Save(Path.Combine(info.DirectoryName, Path.GetFileNameWithoutExtension(fileName) + "_parse.txt"));
            a.Run(n);
            return a;
        }
        
        public void Run(Node node)
        {
            root = node;
            Apply(ctx, ref root);
        }

        public void Save(string fileName)
        {
            if (root == null)
            {
                File.WriteAllText(fileName, "");
            }
            else
            {
                root.Save(fileName);
            }
        }

        public void SaveToXml(string fileName)
        {
            if (root == null)
            {
                File.WriteAllText(fileName, "");
            }
            else
            {
                root.SaveToXml(fileName);
            }
        }

        public override bool Apply(Context ctx, ref Node node)
        {
            for (int level = 0; level < 3; level++)
            {
                Apply(ctx, ref node, level);
            }
            return false;
        }

        public void Apply(Context ctx, ref Node node, int Level)
        {
            int level = Level;

            begin:
            while (level <= Level)
            // for (int level = 0; level < 3; ++level)
            {
                if (node == null) return;

                if (ApplyRules(ctx, ref node, level))
                {
                    level = 0;
                    goto begin;
                }
                
                var v = node.Items.ToArray();
                foreach (Node n in v)
                {
                    Node m = n;
                    Apply(ctx, ref m, level);
                }

                if (ApplyRulesAfter(ctx, ref node, level))
                {
                    level = 0;
                    goto begin;
                }
                ++level;
            }
        }

        void LogApply(object o)
        {
            Console.WriteLine("Apply rule:" + o.GetType());
        }

        public bool ApplyRules(Context ctx, ref Node node, int level)
        {
            var h = node.Data as NActiveNode;
            if (h != null && h.level == level)
            {
                if (h.Apply(ctx, ref node))
                {
                    LogApply(h);
                    return true;
                }
            }

            foreach (Rule r in ctx.ActiveRules(level).ToArray())
            {
                if (r.Apply(ctx, ref node))
                {
                    LogApply(r);
                    return true;
                }
            }
            return false;
        }

        public bool ApplyRulesAfter(Context ctx, ref Node node, int level)
        {
            var h = node.Data as NActiveNode;
            if (h != null && h.level == level)
            {
                if (h.ApplyAfter(ctx, ref node))
                {
                    LogApply(h);
                    return true;
                }
            }

            foreach (Rule r in ctx.ActiveRules(level).ToArray())
            {
                if (r.ApplyAfter(ctx, ref node))
                {
                    LogApply(r);
                    return true;
                }
            }
            return false;
        }
    }
}
