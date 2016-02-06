using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using akira;
using System.Xml.Linq;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void test_cs_rule()
        {
            akira.akira a = new akira.akira();
            a.RunXml("test_cs_rule.aki");
            a.Save("result.xml");
            var lof = a.doc.Descendants("lofusz");
            Assert.IsNotNull(lof);
        }

        [TestMethod]
        public void test_parser()
        {
            akira.akira a = new akira.akira();
            a.RunXml("test_parser.aki");
            a.Save("result.xml");
        }

        [TestMethod]
        public void test_parser2()
        {
            akira.akira a = new akira.akira();
            a.RunXml("test_parser.aki");
            a.Save("result.xml");
        }

        [TestMethod]
        public void test_0()
        {
            akira.akira a = new akira.akira();
            a.Run("test_0.aki");
            a.Save("result.xml");
        }

        [TestMethod]
        public void test_match()
        {
            akira.akira a = new akira.akira();
            a.RunXml("test_match.aki");
            a.Save("result.xml");

            var mul = a.doc.Descendants("mul").First();
            Assert.IsNotNull(mul);
            Assert.AreEqual(mul.Elements().ElementAt(0).Value, "2");
            Assert.AreEqual(mul.Elements().ElementAt(1).Value, "7");
            Assert.AreEqual(mul.Elements().ElementAt(2).Value, "8");
        }

        [TestMethod]
        public void test_cs1()
        {
            akira.akira a = new akira.akira();
            a.RunXml("test_cs1.aki");
            a.Save("result.xml");
            var node = a.doc.Descendants("cs").First();
            Assert.AreEqual(node.Attribute("code").Value, "a 1 b 2 c 1");
        }

        [TestMethod]
        public void test_xml()
        {
            var doc = XDocument.Load("test_xml.xml");
            XElement e = doc.Root;
            foreach (var item in e.Elements())
            {
                Console.WriteLine(item.ToString());
            }
        }
    }
}
