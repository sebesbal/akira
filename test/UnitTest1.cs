using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using akira;

namespace test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestCsCompiler2()
        {
            Akira a = new Akira();
            a.Run("test.aki");
        }
    }
}
