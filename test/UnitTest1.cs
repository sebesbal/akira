using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using akira;

namespace test
{
    [TestClass]
    public class UnitTest1
    {
//        [TestMethod]
//        public void TestMethod1()
//        {
//            string code = @"using System.Linq; using System;
//            class Program {
//              public static void Main(string[] args) {
//                var q = from i in Enumerable.Range(1,100)
//                          where i % 2 == 0
//                          select i;
//                Console.ReadKey();
//              }
//            }";
//            Compiler comp = new CsCompiler();
//            comp.Compile(code, "alma.exe");
//            //Console.ReadKey();
//        }

        [TestMethod]
        public void TestMethod1()
        {
            string code = @"using System; using akira;
                public class TestRule : Exe
                {
                    public override Node Apply(Node node)
                    {   
                        Text t = new Text();
                        t.Content = ""fasza!"";
                        return t;
                    }
                }";

            Text text = new Text();
            text.Language = "cs";
            text.Content = code;

            Compiler comp = new CsCompiler();
            Node r = comp.Apply(text);
            Assert.IsTrue(r is Exe);
            Exe e = r as Exe;
            Node t = e.Apply(null);
            Assert.IsInstanceOfType(t, typeof(Text));
            Assert.AreEqual((t as Text).Content, "fasza!");
        }
    }
}
