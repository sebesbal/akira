using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using akira;
using System.Xml.Linq;
using Antlr4.Runtime.Tree;
using Antlr4.Runtime;

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
            nullop.Precedence = 0;
            nullop.Associativity = Associativity.f;
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
        public AssociativitySide left { get; protected set; }
        public AssociativitySide right { get; protected set; }
        public int Count { get; protected set; }
    }

    class Listener: slpBaseListener
    {
        public Dictionary<string, Operator> operators = new Dictionary<string, Operator>();
        Antlr4.Runtime.Tree.ParseTreeProperty<XElement> m = new Antlr4.Runtime.Tree.ParseTreeProperty<XElement>();
        public XElement program;

        public Listener()
        {
            Operator op = new Operator();
            op.Name = "list";
            op.Precedence = 1000;
            op.Associativity = Associativity.yfy;
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

        protected XElement ParseOperators(List<Tuple<Operator, XElement>> list)
        {
            Stack<Tuple<Operator, XElement>> open = new Stack<Tuple<Operator, XElement>>();
            Stack<XElement> closed = new Stack<XElement>();

            foreach (var x in list)
            {
                Operator op = x.Item1;
                if (op == Operator.nullop || x.Item2 != null && x.Item2.Nodes().Count() > 0)
                {
                    closed.Push(x.Item2);
                }
                else
                {
                    while (true)
                    {
                        if (open.Count == 0) break;
                        var top = open.Peek();
                        var o = top.Item1;
                        if (op != null && (o.Precedence > op.Precedence
                            || o.Precedence == op.Precedence && o.right == AssociativitySide.y))
                        {
                            break;
                        }
                        else
                        {
                            open.Pop();
                            var l = new List<XElement>();
                            for (int i = 0; i < o.Count; ++i)
                            {
                                l.Add(closed.Pop());
                            }
                            l.Reverse();
                            foreach (var item in l)
                            {
                                top.Item2.Add(item);
                            }
                            closed.Push(top.Item2);
                        }
                    }

                    open.Push(x);
                }
            }

            if (closed.Count == 1)
            {
                return closed.Peek();
            }
            else
            {
                var l = new XElement("op");
                l.SetAttributeValue("id", "list");
                foreach (var x in closed)
                {
                    l.Add(x);
                }
                return l;
                // throw new Exception("Closed.Count > 0: " + closed.Count);
            }
        }

        public override void ExitList(slpParser.ListContext context)
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
            if (list.Count > 0)
            {
                list.Add(new Tuple<Operator, XElement>(null, null));
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
                //name = "list";
                // return;
                XElement n = new XElement("op");
                n.SetAttributeValue("id", "list");
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
            XElement node = new XElement("program");
            add(context, node);
            program = node;
        }
    }
}
