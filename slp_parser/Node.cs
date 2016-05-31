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
        public string Name;
        public Node Parent;
        public bool IsAttribute;
        public LinkedList<Node> Attributes = new LinkedList<Node>();
        public LinkedList<Node> Children = new LinkedList<Node>();

        public LinkedList<Node> Elements() { return Children; }

        public Node()
        {
        }

        public IEnumerable<Node> Items
        {
            get
            {
                foreach (var item in Attributes)
                {
                    yield return item;
                }
                foreach (var item in Children)
                {
                    yield return item;
                }
            }
        }

        public Node(string name, params Node[] children)
        {
            Name = name;
            foreach (var item in children)
            {
                if (item.IsAttribute)
                {
                    Attributes.AddLast(item);
                    item.Parent = this;
                }
                else
                {
                    Add(item);
                }
            }
        }

        public string Value
        {
            get { return First.Name; }
        }

        public Node First
        {
            get { return Children.First.Value; }
        }

        public Node Next
        {
            get
            {
                var n = Parent.Children.Find(this).Next;
                if (n == null)
                {
                    return null;
                }
                else
                {
                    return n.Value;
                }
            }
        }

        public Node this[string name]
        {
            get
            {
                var n = FindAtttribute(name);
                if (n == null)
                {
                    return null;
                }
                else
                {
                    return n.Value.Children.First.Value;
                }
            }
            set
            {
                SetAttribute(name, value);
            }
        }
        
        protected LinkedListNode<Node> FindAtttribute(string name)
        {
            var n = Attributes.First;
            while (n != null)
            {
                if (n.Value.Name == name)
                {
                    return n;
                }
                n = n.Next;
            }
            return null;
        }

        public void Add(Node n)
        {
            Children.AddLast(n);
            n.Parent = this;
        }

        public Node Add(string name)
        {
            Node n = new Node(name);
            Add(n);
            return n;
        }

        protected Node SetAttribute(string key, Node value)
        {
            Node n = new Node(key);
            n.IsAttribute = true;
            n.Add(value);
            n.Parent = this;
            var m = FindAtttribute(key);
            if (m == null)
            {
                Attributes.AddLast(n);
            }
            else
            {
                m.Value = n;
            }
            return n;
        }
        
        public Node Clone()
        {
            var node = CloneRec();
            node.Parent = null;
            return node;
        }

        Node CloneRec()
        {
            Node result = new Node(Name);
            result.IsAttribute = IsAttribute;
            foreach (var item in Children)
            {
                result.Add(item.CloneRec());
            }
            return result;
        }

        public void Clear()
        {
            foreach (var item in Children)
            {
                item.Parent = null;
            }
            Children.Clear();
        }

        public void Remove(string key)
        {
            var n = FindAtttribute(key);
            if (n != null)
            {
                Attributes.Remove(n.Value);
            }
        }

        public void Remove()
        {
            if (Parent == null)
            {
                throw new Exception("Parent == null!");
            }
            else
            {
                Parent.Children.Remove(this);
                Parent = null;
            }
        }

        public void InheritAttributesFrom(Node n)
        {
            foreach (var a in n.Attributes)
            {
                this[a.Name] = a.First;
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
                var m = Parent.Children.Find(this);
                m.Value = n;
                n.Parent = Parent;
                Parent = null;
                n.InheritAttributesFrom(this);
            }
        }

        public bool Match(string key, string value)
        {
            var n = this[key];
            if (n == null)
            {
                return false;
            }
            else
            {
                return n.Name == value;
            }
        }

        public bool IsCode
        {
            get { return Match("type", "code"); }
        }

        public bool IsRef
        {
            get { return Name == "$"; }
        }

        public bool IsLeaf()
        {
            return Children.Count == 0;
        }

        public virtual XElement ToXml()
        {
            XElement n;
            try
            {
                n = new XElement(Name);
            }
            catch (Exception e)
            {
                n = new XElement("node");
                n.SetAttributeValue("name", Name);
            }

            foreach (var item in Attributes)
            {
                if (item.First.IsLeaf())
                {
                    n.SetAttributeValue(item.Name, item.First.Name);
                }
                else
                {
                    var m = item.ToXml();
                    m.SetAttributeValue("att", "true");
                    n.Add(m);
                }
            }

            foreach (var item in Children)
            {
                n.Add(item.ToXml());
            }

            return n;
        }

        public void SaveToXml(string file)
        {
            XDocument doc = new XDocument();
            doc.Add(this.ToXml());
            doc.Save(file);
        }

        public void Save(string file)
        {
            File.WriteAllText(file, ToString());
            // File.WriteAllText(file, ToCs());
        }

        public static Node ParseFile(string file)
        {
            string code = File.ReadAllText(file);
            Node n = AkiraParser.Parse(code);
            return n;
        }

        protected int Size = 1;

        void Measure()
        {
            Size = 1;
            foreach (var item in Attributes)
            {
                item.Measure();
                Size += item.Size;
            }
            foreach (var item in Children)
            {
                item.Measure();
                Size += item.Size;
            }
        }

        public override string ToString()
        {
            Measure();
            var cb = new CodeBuilder();
            ToStringRec(cb, false, false);
            return cb.ToString();
        }
        
        virtual public void ToStringRec(CodeBuilder cb, bool inline, bool isAttribute)
        {
            if (IsAttribute && Name == "type")
            {
                return;
            }

            if (IsRef)
            {
                cb.AddLine("$" + Value);
                return;
            }

            bool code = IsCode;
            cb.PushInline(Size <= 2);

            if (code)
            {
                cb.PushInline(true);
                cb.BeginCurly();
                cb.AddLine(Name);
                cb.End();
                cb.PopInline();
            }
            else
            {
                if (IsAttribute)
                {
                    cb.AddLine(Name + ":");
                }
                else
                {
                    cb.AddLine(Name);
                }
            }

            cb.Begin();

            foreach (var item in Attributes)
            {
                item.ToStringRec(cb, inline, true);
            }
            
            foreach (var item in Children)
            {
                item.ToStringRec(cb, inline, false);
            }

            cb.End();

        end:
            cb.PopInline();
        }

        public void InsertChildren()
        {
            int i = 0;
            foreach (var item in Children)
            {
                Name = Regex.Replace(Name, "\\$" + ++i, item.ToCs());
            }
            Clear();
        }

        void ToCsRec(StringBuilder sb)
        {
            if (IsAttribute)
            {
                sb.Append("_a(");
            }
            else if (IsCode)
            {
                sb.Append("_c(");
            }
            else if (IsRef)
            {
                //sb.Append("__(" + Value + ")");
                sb.Append("\" + " + Value + ".ToCs() + \"");
                return;
            }
            else
            {
                sb.Append("__(");
            }
            
            sb.Append("\"" + Name + "\"");
            foreach (var item in Items)
            {
                if (item.IsAttribute && item.Name == "type" && item.Value == "code") continue;
                sb.Append(", ");
                item.ToCsRec(sb);
            }
            sb.Append(")");
        }

        public string ToCs()
        {
            var sb = new StringBuilder();
            ToCsRec(sb);
            return sb.ToString();
        }
    }
}
