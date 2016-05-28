﻿using akira;
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
            var n = Node.ParseFile("base.slp");
            n.SaveTo("result.xml");
        }

        static void Main(string[] args)
        {
            test_5();
        }
    }
}
