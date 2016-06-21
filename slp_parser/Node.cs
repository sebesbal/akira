using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace akira
{
    public class Node : ICloneable
    {
        public Node Parent;
        public ICloneable Data;
        public string SData { get { return Data as string; } }

        public LinkedList<Node> Items = new LinkedList<Node>();
        public Node() { }
        public Node(ICloneable data, params Node[] items)
        {
            Data = data;
            foreach (var item in items)
            {
                Add(item);
            }
        }

        public Node Next
        {
            get
            {
                if (Parent == null) return null;
                var v = Parent.Items.Find(this);
                v = v.Next;
                if (v == null) return null;
                return v.Value;
            }
        }

        public Node First { get { return Items.First(); } }
        public Node Second { get { return Items.First.Next.Value; } }

        public void GetTree(List<Node> tree)
        {
            tree.Add(this);
            Node list = this as Node;
            if (list != null)
            {
                foreach (var item in list.Items)
                {
                    item.GetTree(tree);
                }
            }
        }

        public IEnumerable<Node> Descendants
        {
            get
            {
                foreach (var item in Items)
                {
                    yield return item;
                    foreach (var desc in item.Descendants)
                    {
                        yield return desc;
                    }
                }
            }
        }

        virtual public bool Match(string s) { return s.Equals(Data); }
        //virtual public bool MatchHead(string s) { return false; }
        //virtual public bool MatchHead(Type t) { return false; }
        //virtual public bool MatchPair(string key, ref string value) { return false; }
        //virtual public bool Match(string key, string value) { return false; }
        //virtual public bool Match(string name, int itemCount) { return false; }
        //virtual public bool MatchItemCount(int itemCount) { return false; }
        //virtual public NAttribute FindAttribute(string name) { return null; }
        //virtual public NString FindString(string name) { return null; }

        public void Remove()
        {
            if (Parent == null)
            {
                // throw new Exception("Parent == null!");
            }
            else
            {
                Parent.Items.Remove(this);
                Parent = null;
            }
        }

        public void ReplaceWith(Node n)
        {
            if (Parent == null)
            {
                throw new Exception("Parent == null!");
            }
            else if (n.Parent != null)
            {
                throw new Exception("n.Parent must be null!");
            }
            else
            {
                var m = Parent.Items.Find(this);
                m.Value = n;
                n.Parent = Parent;
                Parent = null;
            }
        }

        public void ReplaceWithList(Node n)
        {
            if (Parent == null)
            {
                throw new Exception("Parent == null!");
            }
            else if (n.Parent != null)
            {
                throw new Exception("n.Parent must be null!");
            }
            else
            {
                var m = Parent.Items.Find(this);
                var o = m;
                foreach (var item in n.Items)
                {
                    Parent.Items.AddAfter(m, item);
                    item.Parent = Parent;
                    m = m.Next;
                }
                Parent.Items.Remove(o);
                Parent = null;
            }
        }

        public static void Replace(ref Node old, Node neu)
        {
            if (old.Parent != null)
            {
                old.ReplaceWith(neu);
            }
            old = neu;
        }

        public static void ReplaceList(ref Node old, Node list)
        {
            if (old.Parent != null)
            {
                old.ReplaceWithList(list);
            }
            old = list.First;
        }

        public void Save(string file)
        {
            File.WriteAllText(file, ToString());
        }

        public void SaveToXml(string file)
        {
            // Skip
        }

        public static Node ParseFile(string file)
        {
            string code = File.ReadAllText(file);
            Node n = AkiraParser.Parse(code);
            return n;
        }

        public int Size = 1;
        
        public string ToString(int indent = 0)
        {
            Measure();
            var cb = new CodeBuilder();
            cb.Indent = indent;
            ToStringRec(cb);
            return cb.ToString();
        }

        public override string ToString()
        {
            Measure();
            var cb = new CodeBuilder();
            ToStringRec(cb);
            return cb.ToString();
        }
        
        public string ToCs()
        {
            var sb = new StringBuilder();
            ToCsRec(sb);
            return sb.ToString();
        }

        public Node FindAttribute(string name)
        {
            foreach (var item in Items)
            {
                if (item.Data is NAttribute && item.Items.First.Value.Match(name))
                {
                    return item;
                }
            }
            return null;
        }

        public void Add(Node n)
        {
            Items.AddLast(n);
            n.Parent = this;
        }

        public void AddFirst(Node n)
        {
            Items.AddFirst(n);
            n.Parent = this;
        }

        virtual public object Clone()
        {
            Node result = new Node();
            result.Data = (ICloneable)Data.Clone();
            foreach (var item in Items)
            {
                result.Add((Node)item.Clone());
            }
            return result;
        }

        public bool Match(string name, int childCount)
        {
            return Match(name) && Items.Count == childCount;
        }

        public bool MatchItemCount(int itemCount)
        {
            return Items.Count == itemCount;
        }

        public bool Eq(Node obj)
        {
            Node n = (Node)obj;
            if (Items.Count != n.Items.Count)
            {
                return false;
            }

            var c = Items.First;
            var nc = n.Items.First;
            while (c != null)
            {
                if (!c.Value.Eq(nc.Value)) return false;
                c = c.Next;
                nc = nc.Next;
            }
            return true;
        }
        
        public void Insert(ref LinkedListNode<Node> after, Node n)
        {
            after = Items.AddAfter(after, n);
            n.Parent = this;
        }
     
        public bool Match(Type t)
        {
            return Data.GetType().IsSubclassOf(t);
        }

        public void Measure()
        {
            Size = 1;
            foreach (var item in Items)
            {
                item.Measure();
                Size += item.Size;
            }
        }

        virtual public string DataToString()
        {
            return Data == null ? "null" : Data.ToString();
        }

        virtual public void ToStringRec(CodeBuilder cb)
        {
            cb.AddLine(DataToString());
            cb.PushInline(Size <= 2);
            cb.Begin();
            foreach (var item in Items)
            {
                item.ToStringRec(cb);
            }
            cb.End();
            cb.PopInline();
        }
        
        virtual public void ToCsRec(StringBuilder sb)
        {
            sb.Append("_l(");
            sb.Append(DataToString());
            foreach (var item in Items)
            {
                sb.Append(", ");
                item.ToCsRec(sb);
            }
            sb.Append(")");
        }
    }

    public class NAttribute : ICloneable
    {
        public object Clone() { return new NAttribute(); }
    }

    public class NCode : Node
    {
        public NCode(string value): base(value.Trim()) { }
        override public object Clone() { return new NCode(Data as string); }
        public override string DataToString()
        {
            return "{ " + Data + " }";
        }
        //override public void ToStringRec(CodeBuilder cb)
        //{
        //    cb.PushInline(true);
        //    cb.BeginCurly();
        //    cb.AddLine(Data as string);
        //    cb.End();
        //    cb.PopInline();
        //}
        override public void ToCsRec(StringBuilder cb) { cb.Append("_c(\"" + Data + "\")"); }
        public void InsertChildren()
        {
            int i = 0;
            foreach (var item in Items)
            {
                Data = Regex.Replace(Data as string, "\\$" + ++i, item.ToCs());
            }
        }
    }

    public class NRef : Node
    {
        public NRef(string value): base(value) { }
        override public object Clone() { return new NRef(Data as string); }
        override public void ToStringRec(CodeBuilder cb) { cb.AddLine("$" + Data); }
        override public void ToCsRec(StringBuilder cb) { cb.Append("__(" + Data + ")"); }
    }
}
