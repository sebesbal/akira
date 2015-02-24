using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace akira
{
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
