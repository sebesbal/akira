using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace akira
{
    public class CodeBuilder
    {
        public CodeBuilder()
        {

        }

        public CodeBuilder(string begin, string end)
        {
            CharBegin = begin;
            CharEnd = end;
        }

        protected StringBuilder sb = new StringBuilder();

        public int Indent;

        public bool Inline;

        protected string CharBegin = "";

        protected string CharEnd = "";

        protected bool isNewLine = true;

        Stack<string> blocks = new Stack<string>();

        Stack<bool> inlines = new Stack<bool>();

        public void PushInline(bool inline)
        {
            Inline |= inline;
            inlines.Push(Inline);
        }

        public bool PopInline()
        {
            bool newInline = inlines.Pop();
            if (!newInline && Inline)
            {
                LineEnd();
            }
            Inline = newInline;
            return Inline;
        }

        public void Begin()
        {
            Begin(CharBegin, CharEnd);
        }

        public void BeginCurly()
        {
            Begin("{", "}");
        }

        public void Begin(string begin, string end)
        {
            if (begin != "")
            {
                AddLine(begin);
            }
            blocks.Push(end);
            ++Indent;
        }
        
        public void End()
        {
            --Indent;
            string end = blocks.Pop();
            if (end != "")
            {
                AddLine(end);
            }
        }

        public void AddTabs()
        {
            if (isNewLine)
            {
                for (int i = 0; i < Indent; ++i)
                {
                    sb.Append('\t');
                }
                isNewLine = false;
            }
        }

        public void Add(char c)
        {
            AddTabs();
            sb.Append(c);
        }

        public void Add(string s)
        {
            AddTabs();
            sb.Append(s);
        }

        public void Space()
        {
            Add(' ');
        }

        public void White()
        {
            if (Inline)
            {
                Space();
            }
            else
            {
                LineEnd();
            }
        }

        public void Tab()
        {
            Add('\t');
        }

        public void LineEnd()
        {
            AddTabs();
            sb.AppendLine();
            isNewLine = true;
        }

        public void AddLine(string s = "")
        {
            if (Inline)
            {
                Add(s);
                White();
            }
            else
            {
                AddTabs();
                sb.AppendLine(s);
                isNewLine = true;
            }
        }

        public override string ToString()
        {
            return sb.ToString();
        }
    }
}
