﻿using System;
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

        private static Type CreateType(AppDomain currentDomain)
        {
            // Create an assembly.
            AssemblyName myAssemblyName = new AssemblyName();
            myAssemblyName.Name = "DynamicAssembly";
            AssemblyBuilder myAssembly =
                           currentDomain.DefineDynamicAssembly(myAssemblyName, AssemblyBuilderAccess.Run);
            // Create a dynamic module in Dynamic Assembly.
            ModuleBuilder myModuleBuilder = myAssembly.DefineDynamicModule("MyModule");
            // Define a public class named "MyClass" in the assembly.
            TypeBuilder myTypeBuilder = myModuleBuilder.DefineType("MyClass", TypeAttributes.Public);

            // Define a private String field named "MyField" in the type.
            FieldBuilder myFieldBuilder = myTypeBuilder.DefineField("MyField",
                typeof(string), FieldAttributes.Private | FieldAttributes.Static);
            // Create the constructor.
            Type[] constructorArgs = { typeof(String) };
            ConstructorBuilder constructor = myTypeBuilder.DefineConstructor(
               MethodAttributes.Public, CallingConventions.Standard, constructorArgs);
            ILGenerator constructorIL = constructor.GetILGenerator();
            constructorIL.Emit(OpCodes.Ldarg_0);
            ConstructorInfo superConstructor = typeof(Object).GetConstructor(new Type[0]);
            constructorIL.Emit(OpCodes.Call, superConstructor);
            constructorIL.Emit(OpCodes.Ldarg_0);
            constructorIL.Emit(OpCodes.Ldarg_1);
            constructorIL.Emit(OpCodes.Stfld, myFieldBuilder);
            constructorIL.Emit(OpCodes.Ret);

            // Create the MyMethod method.
            MethodBuilder myMethodBuilder = myTypeBuilder.DefineMethod("MyMethod",
                                 MethodAttributes.Public, typeof(String), null);
            ILGenerator methodIL = myMethodBuilder.GetILGenerator();
            methodIL.Emit(OpCodes.Ldarg_0);
            methodIL.Emit(OpCodes.Ldfld, myFieldBuilder);
            methodIL.Emit(OpCodes.Ret);
            Console.WriteLine("Name               :" + myFieldBuilder.Name);
            Console.WriteLine("DeclaringType      :" + myFieldBuilder.DeclaringType);
            Console.WriteLine("Type               :" + myFieldBuilder.FieldType);
            Console.WriteLine("Token              :" + myFieldBuilder.GetToken().Token);
            return myTypeBuilder.CreateType();
        }

        [TestMethod]
        public void TestEmit()
        {
            try
            {
                Type myType = CreateType(Thread.GetDomain());
                // Create an instance of the "HelloWorld" class.
                Object helloWorld = Activator.CreateInstance(myType, new Object[] { "HelloWorld" });
                // Invoke the "MyMethod" method of the "MyClass" class.
                Object myObject = myType.InvokeMember("MyMethod",
                               BindingFlags.InvokeMethod, null, helloWorld, null);
                Console.WriteLine("MyClass.MyMethod returned: \"" + myObject + "\"");
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception Caught " + e.Message);
            }
        }
    }
}
