using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using akira;

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
    }
}
