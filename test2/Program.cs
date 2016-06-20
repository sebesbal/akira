using akira;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test2
{
    class Program
    {
        static public void test_0()
        {
            akira.akira a = new akira.akira();
            a.Run("test_0.slp");
            a.Save("result.xml");
        }

        static public void test_1()
        {
            akira.akira a = new akira.akira();
            a.Run("base.slp");
            a.Save("result.xml");
        }

        static public void test_3()
        {
            var n = Node.ParseFile("base.slp");
            n.SaveToXml("result.xml");
        }

        static public void test_4()
        {
            akira.akira a = new akira.akira();
            a.Run("base.slp");
            a.Save("result.xml");
        }

        static public void test_5()
        {
            var n = Node.ParseFile("../akira/tmp/basic.aki");
            n.Save("pretty.slp");
        }

        static public void test_6()
        {
            akira.akira a = new akira.akira();
            a.ctx.Import("write", 0);
            a = new akira.akira();
            a.ctx.Import("replace", 0);

            //a.CompileModule("write", 0);
            //a = new akira.akira();
            //a.CompileModule("replace", 0);

            // a.Save("result.slp");
        }

        static void Main(string[] args)
        {
            test_6();
        }
    }
}
