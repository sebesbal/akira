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
    public class Node
    {
        public NList Parent;
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

        virtual public Node Clone()
        {
            return null;
        }

        public void GetTree(List<Node> tree)
        {
            tree.Add(this);
            NList list = this as NList;
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
                NList list = this as NList;

                if (list != null)
                {
                    foreach (var item in list.Items)
                    {
                        yield return item;
                        foreach (var desc in item.Descendants)
                        {
                            yield return desc;
                        }
                    }
                }

                //foreach (var item in Items)
                //{
                //    yield return item;
                //    foreach (var desc in item.Descendants)
                //    {
                //        yield return desc;
                //    }
                //}
            }
        }

        virtual public bool Match(string s) { return false; }
        virtual public bool MatchHead(string s) { return false; }
        virtual public bool MatchPair(string key, ref string value) { return false; }
        virtual public bool Match(string key, string value) { return false; }
        virtual public bool Match(string name, int itemCount) { return false; }
        virtual public bool MatchItemCount(int itemCount) { return false; }

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

        public static void Replace(ref Node old, Node neu)
        {
            if (old.Parent != null)
            {
                old.ReplaceWith(neu);
            }
            old = neu;
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

        public virtual void Measure()
        {
        }

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
        
        public virtual void ToStringRec(CodeBuilder cb)
        {

        }

        public virtual bool Eq(Node obj)
        {
            return false;
        }

        virtual public void ToCsRec(StringBuilder sb)
        {
        }
        
        public string ToCs()
        {
            var sb = new StringBuilder();
            ToCsRec(sb);
            return sb.ToString();
        }
    }

    public class NList: Node
    {
        public LinkedList<Node> Items = new LinkedList<Node>();
        public Node Head { get { return Items.First.Value; } }
        public Node Second { get { return Items.First.Next.Value; } }
        
        public NList() { }
        public NList(params Node[] items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }

        public IEnumerable<Node> NonHeadItems
        {
            get
            {
                var n = Items.First;
                n = n.Next;
                while (n != null)
                {
                    yield return n.Value;
                    n = n.Next;
                }
            }
        }
        //override public IEnumerable<Node> Descendants
        //{
        //    get
        //    {
        //        foreach (var item in Items)
        //        {
        //            yield return item;
        //            foreach (var desc in item.Descendants)
        //            {
        //                yield return desc;
        //            }
        //        }
        //    }
        //}


        public void Add(Node n)
        {
            Items.AddLast(n);
            n.Parent = this;
        }

        override public Node Clone()
        {
            NList result = new NList();
            foreach (var item in Items)
            {
                result.Add(item.Clone());
            }
            return result;
        }

        override public bool Match(string name, int childCount)
        {
            return Head.Match(name) && Items.Count == childCount;
        }

        override public bool MatchItemCount(int itemCount)
        {
            return Items.Count == itemCount;
        }

        virtual public bool Eq(Node obj)
        {
            NList n = (NList)obj;
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

        override public bool Match(string key, string value)
        {
            return Head.Match(key) && Second.Match(value);

            //foreach (var item in Items)
            //{
            //    var p = item as NList;
            //    if (p != null
            //        && p.Head.Match(key)
            //        && p.Items.Count == 1
            //        && p.Second.Match(value))
            //    {
            //        return true;
            //    }
            //}
            //return false;
        }

        override public bool MatchHead(string s)
        {
            return Head.Match(s);
        }

        override public bool MatchPair(string key, ref string value)
        {
            var item = Items.First;
            if (!item.Value.Match(key)) return false;
            item = item.Next;
            var str = item.Value as NString;
            if (str == null) return false;
            value = str.Value;
            return true;
        }

        public bool HasString(string str)
        {
            foreach (var item in Items)
            {
                var s = item as NString;
                if (s != null && s.Value == str)
                {
                    return true;
                }
            }
            return false;
        }

        override public void Measure()
        {
            Size = 1;
            foreach (var item in Items)
            {
                item.Measure();
                Size += item.Size;
            }
        }

        override public void ToStringRec(CodeBuilder cb)
        {
            var item = Items.First;
            item.Value.ToStringRec(cb);
            item = item.Next;
            cb.PushInline(Size <= 2);
            cb.Begin();
            while (item != null)
            {
                item.Value.ToStringRec(cb);
                item = item.Next;
            }
            cb.End();
            cb.PopInline();
        }

        override public void ToCsRec(StringBuilder sb)
        {
            sb.Append("_l(");
            Head.ToCsRec(sb);
            foreach (var item in NonHeadItems)
            {
                sb.Append(", ");
                item.ToCsRec(sb);
            }
            sb.Append(")");
        }
    }

    public interface IString
    {
        string Value { get; set; }
    }

    public class NString : Node
    {
        public NString(string value) { Value = value; }
        virtual public string Value { get; set; }
        override public Node Clone() { return new NString(Value); }
        override public void ToStringRec(CodeBuilder cb) { cb.AddLine(Value); }
        override public void ToCsRec(StringBuilder cb) { cb.Append("_s(\"" + Value +"\")"); }
        override public bool Match(string s) { return Value == s; }
    }
    
    public class NOperator : NList
    {
        public NOperator(slp_parser.Operator op): base(new NString(op.Name)) { Operator = op; }
        public slp_parser.Operator Operator;
        // override public Node Clone() { return new NOperator(Operator); }
    }

    public class NCode : NString
    {
        public NCode(string value): base(value.Trim()) { }
        override public Node Clone() { return new NCode(Value); }
        override public void ToStringRec(CodeBuilder cb)
        {
            cb.PushInline(true);
            cb.BeginCurly();
            cb.AddLine(Value);
            cb.End();
            cb.PopInline();
        }
        override public void ToCsRec(StringBuilder cb) { cb.Append("_c(\"" + Value + "\")"); }
        public void InsertChildren()
        {
            int i = 0;
            foreach (var item in Parent.NonHeadItems)
            {
                Value = Regex.Replace(Value, "\\$" + ++i, item.ToCs());
            }
        }
    }

    public class NRef : NString
    {
        public NRef(string value): base(value) { }
        override public Node Clone() { return new NRef(Value); }
        override public void ToStringRec(CodeBuilder cb) { cb.AddLine("$" + Value); }
        override public void ToCsRec(StringBuilder cb) { cb.Append("__(" + Value + ")"); }
    }
}
