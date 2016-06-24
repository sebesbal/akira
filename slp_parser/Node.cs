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
        public string SData { get { return escape(Data.ToString()); } }

        public LinkedList<Node> Items = new LinkedList<Node>();
        public Node() { }
        //public Node(ICloneable data, params Node[] items)
        //{
        //    Data = data;
        //    foreach (var item in items)
        //    {
        //        Add(item);
        //    }
        //}

        public Node(ICloneable data, params ICloneable[] items)
        {
            Data = data;
            foreach (var item in items)
            {
                if (item is Node)
                {
                    Add((Node)item);
                }
                else
                {
                    Add(new Node(item));
                }
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

        public string Deref
        {
            get
            {
                if (Match("$") && First.Data is string)
                {
                    return (string)First.Data;
                }
                else
                {
                    return "?";
                }
            }
        }

        //public Node FindAttribute(string name)
        //{
        //    foreach (var item in Items)
        //    {
        //        if (item.Data is NAttribute && item.Items.First.Value.Match(name))
        //        {
        //            return item;
        //        }
        //    }
        //    return null;
        //}

        public Node FindAttribute(string name)
        {
            foreach (var item in Items)
            {
                if (item.Match(":") && item.First.Match(name))
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
            result.Data = Data == null ? null : (ICloneable)Data.Clone();
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
            //return Data != null && Data.GetType().IsSubclassOf(t);
            return Data != null && Data.GetType().Equals(t);
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

        public static string escape(string s)
        {
            return s.Replace("\"", "\\\"");
        }

        virtual public string DataToString()
        {
            return Data == null ? "null" : SData;
        }

        virtual public void ToStringRec(CodeBuilder cb)
        {
            if (Data is NodeData)
            {
                ((NodeData)Data).ToStringRec(cb);
            }
            else
            {
                cb.AddLine(DataToString());
            }
            if (Items.Count > 0)
            {
                cb.PushInline(Size <= 2);
                cb.Begin();
                foreach (var item in Items)
                {
                    item.ToStringRec(cb);
                }
                cb.End();
                cb.PopInline();
            }
        }
        
        virtual public void ToCsRec(StringBuilder sb)
        {
            if ("$".Equals(Data))
            {
                sb.Append("__(" + First.SData + ")");
                return;
            }
            //else if (Data is NCode)
            //{
            //    var code = (NCode)Data;
            //    code.InsertChildren(this);
            //}

            sb.Append("__(");

            if (Data is NodeData)
            {
                ((NodeData)Data).ToCsRec(sb);
            }
            else
            {
                sb.Append("\"" + DataToString() + "\"");
            }
            
            foreach (var item in Items)
            {
                sb.Append(", ");
                item.ToCsRec(sb);
            }
            sb.Append(")");
        }
    }

    //public class NAttribute : ICloneable
    //{
    //    public object Clone() { return new NAttribute(); }
    //}

    public abstract class NodeData: ICloneable
    {
        public abstract object Clone();
        public abstract void ToStringRec(CodeBuilder cb);
        public abstract void ToCsRec(StringBuilder cb);
    }

    public class NCode : NodeData
    {
        public NCode(string value) { Value = value.Trim(); }
        override public object Clone() { return new NCode(Value); }
        override public void ToStringRec(CodeBuilder cb)
        {
            cb.PushInline(true);
            cb.BeginCurly();
            cb.AddLine(Value);
            cb.End();
            cb.PopInline();
        }
        //override public void ToCsRec(StringBuilder cb) { cb.Append("__c(\"" + Node.escape(Value) + "\")"); }
        override public void ToCsRec(StringBuilder cb) { cb.Append("__c(\"" + Value + "\")"); }
        public void InsertChildren(Node node)
        {
            Value = Regex.Replace(Value, "\\$(\\d+)", "___$$1");
            int i = 1;
            foreach (var item in node.Items)
            {
                Value = Regex.Replace(Value, "___\\$" + i, item.ToCs());
                ++i;
            }
        }

        public void InsertChildren2(Node node)
        {
            if (node.Items.Count > 0 && Value.IndexOf("#") > -1)
            {
                Value = Regex.Replace(Value, "\\#(\\d+)", "___#$1");
                int i = 1;
                var v = node.Items.ToArray();
                foreach (var item in v)
                {
                    // Value = Regex.Replace(Value, "___#" + i, item.SData);
                    var newValue = Regex.Replace(Value, "___#" + i, @""" + " +  item.Deref + @".SData + """);
                    if (newValue != Value)
                    {
                        item.Remove();
                        Value = newValue;
                    }
                    ++i;
                }
            }
        }

        public bool CreateChildren(Node node)
        {
            bool changed = false;
            while (true)
            {
                var m = Regex.Match(Value, "(\\$|#)([a-zA-Z][a-zA-Z0-9]*)");
                if (m.Success)
                {
                    var sign = m.Groups[1].Value;
                    var name = m.Groups[2].Value;
                    int count = node.Items.Count + 1;
                    Value = Regex.Replace(Value, "\\" + sign + name, sign + count);
                    node.Add(new Node("$", name));
                    changed = true;
                }
                else
                {
                    break;
                }
            }
            return changed;
        }

        public string Value;
    }
}
