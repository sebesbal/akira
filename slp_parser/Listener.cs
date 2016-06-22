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
        f, fx, fy, xf, yf, xfx, xfy, yfx, yfy, yfxx, fxx
    }

    // x: less or equal precedence
    // y: less precedence
    public enum AssociativitySide
    {
        n, x, y
    }

    public class Operator: ICloneable
    {
        static Operator()
        {
            nullop.Name = "nullop";
            nullop.Precedence = 1000;
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
            get { return _Associativity; }
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
                    case Associativity.yfxx:
                        left = AssociativitySide.y;
                        right = AssociativitySide.x;
                        Count = int.MaxValue;
                        break;
                    case Associativity.fxx:
                        left = AssociativitySide.n;
                        right = AssociativitySide.x;
                        Count = int.MaxValue;
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

        public object Clone()
        {
            var result = new Operator();
            result.Associativity = Associativity;
            result.Name = Name;
            result.Precedence = Precedence;
            return result;
        }
    }

    class Listener: slpBaseListener
    {
        public Dictionary<string, Operator> operators = new Dictionary<string, Operator>();
        Antlr4.Runtime.Tree.ParseTreeProperty<Node> m = new Antlr4.Runtime.Tree.ParseTreeProperty<Node>();
        public Node program;

        public Listener()
        {
            Operator op = Operator.listop;
            operators.Add(op.Name, op);
            
            Action<string, int, Associativity> f = (string s, int precedence, Associativity ass) =>
            {
                Operator o = new Operator();
                o.Name = s;
                o.Associativity = ass;
                o.Precedence = precedence;
                operators.Add(s, o);
            };
            f("-->", 11000, Associativity.yfxx);
            f(":", 200, Associativity.yf);
            f("$", 100, Associativity.fx);
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
        
        public override void EnterOpdef(slpParser.OpdefContext context)
        {
            Operator op = new Operator();
            op.Name = context.OP().GetText();
            op.Precedence = int.Parse(context.INT().GetText());
            op.Associativity = (Associativity)Enum.Parse(typeof(Associativity), context.assoc().GetText());
            operators.Add(op.Name, op);
            base.EnterOpdef(context);
        }
        
        //void traverse2(ref Node e)
        //{
        //    // remove lists with zero element
        //    if (e.Name == "list" && e.Children.Count() == 0)
        //    {
        //        if (e.Parent != null)
        //        {
        //            e.Parent.InheritAttributesFrom(e);
        //        }
        //        e.Remove();
        //        return;
        //    }

        //    // remove lists with one element
        //    if (e.Name == "list" && e.Children.Count() == 1)
        //    {
        //        var f = e.Elements().ElementAt(0);
        //        f.Remove();
        //        Node.Replace(ref e, f);
        //        traverse2(ref e);
        //        return;
        //    }
            
        //    var v = e.Items.ToArray();
        //    foreach (var item in v)
        //    {
        //        Node n = item;
        //        traverse2(ref n);
        //    }

        //    // if the first item is a leaf, replace the list with it.
        //    if (e.Name == "list")
        //    {
        //        var f = e.Elements().ElementAt(0);
        //        if (f.Elements().Count() == 0)
        //        {
        //            f.Remove();
        //            foreach (var g in e.Elements())
        //            {
        //                f.Add(g);
        //            }
        //            Node.Replace(ref e, f);
        //            traverse2(ref e);
        //        }
        //    }

        //    if (e.Name == "op")
        //    {
        //        e.Name = e["__id"].Name;
        //        e.Remove("__id");
        //    }
        //}

        //void traverse3(Node e)
        //{
        //    var v = e.Items.ToArray();
        //    foreach (var item in v)
        //    {
        //        traverse3(item);
        //    }

        //    if (e.IsAttribute && e.Parent.Children.Find(e) != null)
        //    {
        //        Node p = e.Parent;
        //        e.Remove();
        //        p.AddAttribute(e);
        //    }
        //}

        protected Tuple<Operator, Node> ParseOperators2(List<Tuple<Operator, Node>> list)
        {
            var a = new List<Tuple<Operator, Node>>();
            var b = new List<Tuple<Operator, Node>>();
            bool bb = false;

            foreach (var item in list)
            {
                if (!bb && item.Item1.Precedence > Operator.listop.Precedence)
                {
                    if (a.Count > 1)
                    {
                        var n = ParseOperators2(a);
                        a.Clear();
                        a.Add(n);
                    }
                    a.Add(item);
                    bb = true;
                }
                else if (bb)
                {
                    b.Add(item);
                }
                else
                {
                    a.Add(item);
                }
                //bb |= item.Item1.Precedence > Operator.listop.Precedence;
            }

            if (b.Count > 0)
            {
                var n = ParseOperators2(b);
                a.Add(n);
            }
            return ParseOperators(a);
        }

        protected Tuple<Operator, Node> ParseOperators(List<Tuple<Operator, Node>> list)
        {
            var open = new Stack<Tuple<Operator, Node>>();
            var root = new Tuple<Operator, Node>(Operator.listop, new Node());
            open.Push(root);

            //      a
            //     / \
            //    *   b
            //   /   /
            //  *   c
            foreach (var b in list)
            {
                Operator opb = b.Item1;
                Tuple<Operator, Node> c = null;

                while (open.Count > 0)
                {
                    var a = open.Peek();
                    Node alist = a.Item2;
                    
                    Operator opa = a.Item1;

                    if (a == root || opa < opb) // can push b under a
                    //if (opa < opb) // can push b under a
                    {
                        if (c != null)
                        {
                            Operator opc = c.Item1;
                            if (opc > opb) // can push c under b
                            {
                                // check a's size
                                if (!(opa == Operator.listop || opa.Count >= alist.Items.Count())) goto move_up;

                                // remove c from a
                                c.Item2.Remove();

                                // add c to b
                                b.Item2.Add(c.Item2);
                            }
                            else
                            {
                                // Skip c
                                // check a's size
                                if (!(opa == Operator.listop || opa.Count > alist.Items.Count())) goto move_up;
                            }
                        }

                        alist.Add(b.Item2); // push b under a
                        open.Push(b);
                        break; // take the next b
                    }

                    move_up:
                    c = a;
                    open.Pop();
                }
            }
            
            return root;
        }

        public override void ExitExp([NotNull] slpParser.ExpContext context)
        {
            var list = new List<Tuple<Operator, Node>>();
            foreach (var item in context.children)
            {
                var n = m.Get(item);
                if (n == null) continue;

                if (n.Data is Operator)
                {
                     list.Add(new Tuple<Operator, Node>((Operator)n.Data, n));
                }
                else
                {
                    list.Add(new Tuple<Operator, Node>(Operator.nullop, n));
                }
            }

            if (list.Count == 1)
            {
                m.Put(context, list.First().Item2);
            }
            else if (list.Count > 0)
            {
                m.Put(context, ParseOperators2(list).Item2);
            }
        }

        public override void ExitToken(slpParser.TokenContext context)
        {
            string txt = context.GetText();
            string value = context.GetText();
            Node n = null;
            if (context.FLOAT() != null)
            {
                n = new Node(txt);
            }
            else if (context.INT() != null)
            {
                n = new Node(txt);
            }
            else if (context.STRING() != null)
            {
                txt = txt.Substring(1, txt.Length - 2); // remove surrounding " "
                n = new Node(txt);
            }
            else if (context.ID() != null)
            {
                n = new Node(txt);
            }
            else if (context.NEWLINE() != null)
            {
                return;
            }
            else if (context.OP() != null)
            {
                if (operators.ContainsKey(txt))
                {
                    Operator op = operators[txt];
                    n = new Node(op);
                }
                else
                {
                    throw new Exception("Unknown operator: " + txt);
                }
            }
            else if (context.NATIVE() != null)
            {
                if (txt.Length >= 2)
                {
                    txt = txt.Substring(1, txt.Length - 2); // remove surrounding { }
                }
                n = new Node(new NCode(txt));
            }

            m.Put(context, n);
        }

        public override void ExitBlock(slpParser.BlockContext context)
        {
            Node n = m.Get(context.GetChild(1));
            if (n == null)
            {
                m.Put(context, n);
            }
            else
            {
                var op = new Node(Operator.nullop);
                op.Add(n);
                m.Put(context, op);
            }

            //m.Put(context, m.Get(context.GetChild(1)));
        }

        public override void ExitProgram(slpParser.ProgramContext context)
        {
            program = m.Get(context.exp());
            if (program.Data == null)
            {
                program.Data = "program";
            }
            else
            {
                program = new Node("program", program);
            }

            traverse0(program);
            traverse1(program);
            //traverse1(program);
            //// traverse0(program);
            //traverse2(ref program);
            //traverse3(program); 
            m.Put(context, program);
        }

        void traverse0(Node n)
        {
            foreach (var item in n.Items)
            {
                traverse0(item);
            }

            if (n.Items.Count == 1 && (n.Data == null || n.Data == Operator.nullop))
            {
                var h = n.First;
                h.Remove();
                n.ReplaceWith(h);
            }
        }

        void traverse1(Node n)
        {
            foreach (var item in n.Items)
            {
                traverse1(item);
            }

            if (n.Data == null && n.Items.Count > 0 && n.First.Items.Count == 0)
            {
                var h = n.First;
                h.Remove();
                foreach (var item in n.Items)
                {
                    h.Add(item);
                }
                n.ReplaceWith(h);
                //n.Data = n.First.Data;
                //n.First.Remove();
            }

            if (n.Data is Operator)
            {
                n.Data = (n.Data as Operator).Name;
            }
        }

            //public override void ExitLList([NotNull] slpParser.LListContext context)
            //{
            //    Node n = m.Get(context.GetChild(0));
            //    XAttribute id;
            //    if (n.GetAttribute("id", out id) && id.Value == "list")
            //    {
            //        n.Add(m.Get(context.GetChild(1)));
            //        m.Put(context, n);
            //    }
            //    else
            //    {
            //        Node list = new Node("op");
            //        list.SetAttributeValue("id", "list");
            //        add(context, list);
            //    }
            //}
        }
}

