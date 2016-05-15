using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using akira;
using System.Xml.Linq;
using Antlr4.Runtime.Tree;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;

namespace slp_parser
{
    public enum Associativity
    {
        f, fx, fy, xf, yf, xfx, xfy, yfx, yfy
    }

    public enum AssociativitySide
    {
        n, x, y
    }

    public class Operator
    {
        static Operator()
        {
            nullop.Name = "nullop";
            nullop.Precedence = 1100;
            nullop.Associativity = Associativity.f;
            
            listop.Name = "list";
            listop.Precedence = 10000;
            listop.Associativity = Associativity.yfy;
        }

        public string Name;
        public int Precedence;
        public Associativity _Associativity;
        public Associativity Associativity
        {
            get { return Associativity; }
            set
            {
                _Associativity = value;
                switch (value)
                {
                    case Associativity.f:
                        left = right = AssociativitySide.n;
                        Count = 0;
                        break;
                    case Associativity.fx:
                        left = AssociativitySide.n;
                        right = AssociativitySide.x;
                        Count = 1;
                        break;
                    case Associativity.fy:
                        left = AssociativitySide.n;
                        right = AssociativitySide.y;
                        Count = 1;
                        break;
                    case Associativity.xf:
                        left = AssociativitySide.x;
                        right = AssociativitySide.n;
                        Count = 1;
                        break;
                    case Associativity.yf:
                        left = AssociativitySide.y;
                        right = AssociativitySide.n;
                        Count = 1;
                        break;
                    case Associativity.xfx:
                        left = AssociativitySide.x;
                        right = AssociativitySide.x;
                        Count = 2;
                        break;
                    case Associativity.xfy:
                        left = AssociativitySide.x;
                        right = AssociativitySide.y;
                        Count = 2;
                        break;
                    case Associativity.yfx:
                        left = AssociativitySide.y;
                        right = AssociativitySide.x;
                        Count = 2;
                        break;
                    case Associativity.yfy:
                        left = AssociativitySide.y;
                        right = AssociativitySide.y;
                        Count = 2;
                        break;
                    default:
                        break;
                }
            }
        }
        public static Operator nullop = new Operator();
        public static Operator listop = new Operator();
        public AssociativitySide left { get; protected set; }
        public AssociativitySide right { get; protected set; }
        public int Count { get; protected set; }

        /// <summary>
        /// a alá berakható b
        /// </summary>
        public static bool operator <(Operator a, Operator b)
        {
            if (a.Precedence > b.Precedence)
            {
                if (a.right != AssociativitySide.n)
                {
                    return true;
                }
            }
            else if (a.Precedence == b.Precedence)
            {
                if (a.right == AssociativitySide.x)
                {
                    return true;
                }
            }
            else // (a.Precedence < b.Precedence)
            {
                if (a.right != AssociativitySide.n && b.left == AssociativitySide.n)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// b alá berakható a
        /// </summary>
        public static bool operator >(Operator a, Operator b)
        {
            if (a.Precedence < b.Precedence)
            {
                if (b.left != AssociativitySide.n)
                {
                    return true;
                }
            }
            else if (a.Precedence == b.Precedence)
            {
                if (b.left == AssociativitySide.x)
                {
                    return true;
                }
            }
            else // (a.Precedence > b.Precedence)
            {
                if (a.right == AssociativitySide.n && b.left != AssociativitySide.n)
                {
                    return true;
                }
            }
            return false;
        }
    }

    class Listener: slpBaseListener
    {
        public Dictionary<string, Operator> operators = new Dictionary<string, Operator>();
        Antlr4.Runtime.Tree.ParseTreeProperty<XElement> m = new Antlr4.Runtime.Tree.ParseTreeProperty<XElement>();
        public XElement program;

        public Listener()
        {
            Operator op = Operator.listop;
            operators.Add(op.Name, op);
        }

        //public void EnterEveryRule(Antlr4.Runtime.ParserRuleContext ctx)
        //{
        //    throw new NotImplementedException();
        //}

        //public void ExitEveryRule(Antlr4.Runtime.ParserRuleContext ctx)
        //{
        //    throw new NotImplementedException();
        //}

        //public void VisitErrorNode(Antlr4.Runtime.Tree.IErrorNode node)
        //{
        //    throw new NotImplementedException();
        //}

        //public void VisitTerminal(Antlr4.Runtime.Tree.ITerminalNode node)
        //{
        //    throw new NotImplementedException();
        //}


        protected void add(ParserRuleContext context, XElement node)
        {
            m.Put(context, node);
            addChildrens(context, node);
        }

        protected void addChildrens(ParserRuleContext context, XElement node)
        {
            foreach (var item in context.children)
            {
                var n = m.Get(item);
                if (n != null)
                {
                    node.Add(n);
                }
            }
        }

        public override void EnterOpdef(slpParser.OpdefContext context)
        {
            Operator op = new Operator();
            op.Name = context.OP().GetText();
            op.Precedence = int.Parse(context.INT().GetText());
            op.Associativity = (Associativity)Enum.Parse(typeof(Associativity), context.assoc().GetText());
            operators.Add(op.Name, op);
            base.EnterOpdef(context);
        }
        
        void traverse0(XElement e)
        {
            if (e.Name == "id")
            {
                e.Name = e.Value;
                e.RemoveNodes();
            }
            else if (e.Name == "op" && e.MatchAttribute("id", "list"))
            {
                e.Name = "list";
                e.Attribute("id").Remove();
            }

            foreach (var f in e.Elements())
            {
                traverse0(f);
            }
        }

        void traverse1(XElement e)
        {
            if (e.Name == "op" && e.MatchAttribute("id", ":"))
            {
                var f = e.Elements().ElementAt(1);
                
                e.Attribute("id").Remove();
                e.Name = "list";

                if (f.Name == "list")
                {
                    f.Remove();
                    foreach (var h in f.Elements())
                    {
                        e.Add(h);
                    }
                }
            }
            
            foreach (var f in e.Elements())
            {
                traverse1(f);
            }
        }

        void traverse2(ref XElement e)
        {
            // remove lists with one element
            if (e.Name == "list" && e.Elements().Count() == 1)
            {
                var f = e.Elements().ElementAt(0);
                f.Remove();
                if (e.Parent != null)
                {
                    e.ReplaceWith(f);
                }
                e = f;
                return;
            }

            var h = e.FirstNode;
            while (h != null)
            {
                if (h is XElement)
                {
                    XElement i = (XElement)h;
                    traverse2(ref i);
                    h = i;
                }
                h = h.NextNode;
            }
            
            if (e.Name == "list")
            {
                var f = e.Elements().ElementAt(0);
                if (f.Elements().Count() == 0)
                {
                    f.Remove();
                    if (e.Parent != null)
                    {
                        e.ReplaceWith(f);
                    }
                    foreach (var g in e.Elements())
                    {
                        f.Add(g);
                    }
                    e = f;
                }
            }
        }

        protected XElement ParseOperators(List<Tuple<Operator, XElement>> list)
        {
            var open = new Stack<Tuple<Operator, XElement>>();
            var root = new Tuple<Operator, XElement>(Operator.listop, new XElement("list"));
            open.Push(root);

            foreach (var b in list)
            {
                Operator opb = b.Item1;
                Tuple<Operator, XElement> c = null;

                while (open.Count > 0)
                {
                    var a = open.Peek();
                    Operator opa = a.Item1;

                    if (opa < opb) // can push b under a
                    {
                        if (c != null)
                        {
                            Operator opc = c.Item1;
                            if (opc > opb) // can pus c under b
                            {
                                // check a's size
                                if (!(opa == Operator.listop || opa.Count >= a.Item2.Elements().Count())) goto move_up;

                                // remove c from a
                                c.Item2.Remove();

                                // add c to b
                                b.Item2.Add(c.Item2);
                            }
                            else
                            {
                                // Skip c
                                // check a's size
                                if (!(opa == Operator.listop || opa.Count > a.Item2.Elements().Count())) goto move_up;
                            }
                        }

                        a.Item2.Add(b.Item2); // push b under a
                        open.Push(b);
                        break; // take the next b
                    }

                    move_up:
                    c = a;
                    open.Pop();
                }
            }

            //XElement e = root.Item2;
            //if (e.Elements().Count() == 1)
            //{
            //    e = e.Elements().ElementAt(0);
            //    e.Remove();
            //}

            return root.Item2;
        }

        public override void ExitExp([NotNull] slpParser.ExpContext context)
        {
            var list = new List<Tuple<Operator, XElement>>();
            foreach (var item in context.children)
            {
                var n = m.Get(item);
                if (n == null) continue;

                if (n.Name == "op")
                {
                    var txt = n.Attribute("id").Value;
                    if (operators.ContainsKey(txt))
                    {
                        var op = operators[txt];
                        //if (op == Operator.listop)
                        //{
                        //    op = Operator.nullop;
                        //}
                        list.Add(new Tuple<Operator, XElement>(op, n));
                    }
                    else
                    {
                        throw new Exception("Unknown operator: " + txt);
                    }
                }
                else
                {
                    list.Add(new Tuple<Operator, XElement>(Operator.nullop, n));
                }
            }

            if (list.Count == 1)
            {
                m.Put(context, list.First().Item2);
            }
            else if (list.Count > 0)
            {
                m.Put(context, ParseOperators(list));
            }
        }

        public override void ExitToken(slpParser.TokenContext context)
        {
            string name = "token";
            string value = context.GetText();
            if (context.FLOAT() != null)
            {
                name = "flo";
            }
            else if (context.INT() != null)
            {
                name = "int";
            } 
            else if (context.OP() != null)
            {
                name = "op";
            }
            else if (context.STRING() != null)
            {
                name = "str";
            }
            else if (context.ID() != null)
            {
                name = "id";
            }
            else if (context.NEWLINE() != null)
            {
                //XElement n = new XElement("op");
                //n.SetAttributeValue("id", "list");
                //m.Put(context, n);
                return;
            }
            else if (context.NATIVE() != null)
            {
                XElement n = new XElement("run");
                n.SetAttributeValue("type", "cs");
                string code = context.NATIVE().GetText();
                n.SetAttributeValue("code", code.Substring(1, code.Length - 2));
                m.Put(context, n);
                return;
            }

            XElement node = new XElement(name);
            
            if (context.OP() == null)
            {
                node.Value = context.GetText();
            }
            else
            {
                node.SetAttributeValue("id", context.GetText());
            }
            m.Put(context, node);
        }

        public override void ExitBlock(slpParser.BlockContext context)
        {
            if (context.ChildCount == 3)
            {
                m.Put(context, m.Get(context.GetChild(1)));
            }
            else
            {
                XElement node = new XElement("block");
                add(context, node);
            }
        }

        public override void ExitProgram(slpParser.ProgramContext context)
        {
            program = m.Get(context.exp());
            traverse0(program);
            traverse1(program);
            traverse2(ref program);
            m.Put(context, program);
        }

        //public override void ExitLList([NotNull] slpParser.LListContext context)
        //{
        //    XElement n = m.Get(context.GetChild(0));
        //    XAttribute id;
        //    if (n.GetAttribute("id", out id) && id.Value == "list")
        //    {
        //        n.Add(m.Get(context.GetChild(1)));
        //        m.Put(context, n);
        //    }
        //    else
        //    {
        //        XElement list = new XElement("op");
        //        list.SetAttributeValue("id", "list");
        //        add(context, list);
        //    }
        //}
    }
}

