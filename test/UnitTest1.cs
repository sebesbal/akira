using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using akira;
using System.Xml.Linq;
using System.Linq;

namespace test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestCsCompiler()
        {
            Akira a = new Akira();
            a.Run("test.aki");
        }

        [TestMethod]
        public void TestSlpParser()
        {
            Akira a = new Akira();
            a.Run("test_parser.aki");
            a.Save("result.xml");
        }

        [TestMethod]
        public void TestMatch()
        {
            Akira a = new Akira();
            a.Run("test_match.aki");
            a.Save("result.xml");
        }

        [TestMethod]
        public void ShouldGetXElementDeepCopyUsingConstructorArgument()
        {
            XElement original = new XElement("original");
            XElement orichild = new XElement("orichild");
            original.Add(orichild);

            XElement deepCopy = new XElement(original);
            // XElement newchild = new XElement("orichild");
            //deepCopy.Add(newchild);

            var newchild = deepCopy.Elements().First();
            newchild.Name = "lofusz";
                //. = "copy";
            // Assert.AreEqual("original", original.Name);
            Assert.AreEqual("orichild", orichild.Name);
        }
    }
}
