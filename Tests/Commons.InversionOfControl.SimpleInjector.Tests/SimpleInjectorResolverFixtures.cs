﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BoC.InversionOfControl;
using BoC.InversionOfControl.SimpleInjector;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Commons.InverionOfControl.SimpleInjector.Tests
{
    [TestClass]
    public class SimpleInjectorResolverFixture
    {
        [TestMethod]
        public void ResolveAll_Should_Return_All_Registed_Types()
        {
            var resolver = new SimpleInjectorDependencyResolver();
            resolver.RegisterType<IFace1, Class1>();
            resolver.RegisterType<IFace1, Class2>();
            resolver.RegisterInstance<IFace1>(new Class3());

            var resolveAll = resolver.ResolveAll<IFace1>();
            Assert.AreEqual(3, resolveAll.Count());
            Assert.IsTrue(resolveAll.OfType<Class1>().Any());
            Assert.IsTrue(resolveAll.OfType<Class2>().Any());
            Assert.IsTrue(resolveAll.OfType<Class3>().Any());
        }

        [TestMethod]
        public void Array_Injector_Should_Inject_All_Registed_Types()
        {
            var resolver = new SimpleInjectorDependencyResolver();
            resolver.RegisterType<IFace1, Class1>();
            resolver.RegisterType<IFace1, Class2>();
            resolver.RegisterInstance<IFace1>(new Class3());

            var resolve = resolver.Resolve<Class4>();
            Assert.AreEqual(3, resolve.Ifaces.Count());
            Assert.IsTrue(resolve.Ifaces.OfType<Class1>().Any());
            Assert.IsTrue(resolve.Ifaces.OfType<Class2>().Any());
            Assert.IsTrue(resolve.Ifaces.OfType<Class3>().Any());
        }

        [TestMethod]
        public void Array_Injector_Should_Inject_All_Registed_Types_As_Enumerable()
        {
            var resolver = new SimpleInjectorDependencyResolver();
            resolver.RegisterType<IFace1, Class1>();
            resolver.RegisterType<IFace1, Class2>();
            resolver.RegisterInstance<IFace1>(new Class3());

            var resolve = resolver.Resolve<Class5>();
            Assert.AreEqual(3, resolve.Ifaces.Count());
            Assert.IsTrue(resolve.Ifaces.OfType<Class1>().Any());
            Assert.IsTrue(resolve.Ifaces.OfType<Class2>().Any());
            Assert.IsTrue(resolve.Ifaces.OfType<Class3>().Any());
        }

        [TestMethod]
        public void Array_Injector_Should_Inject_One_Registed_Type()
        {
            var resolver = new SimpleInjectorDependencyResolver();
            resolver.RegisterType<IFace1, Class1>();

            var resolve = resolver.Resolve<Class4>();
            Assert.AreEqual(1, resolve.Ifaces.Count());
            Assert.IsTrue(resolve.Ifaces.OfType<Class1>().Any());
            Assert.IsFalse(resolve.Ifaces.OfType<Class2>().Any());
            Assert.IsFalse(resolve.Ifaces.OfType<Class3>().Any());
        }

        [TestMethod]
        public void Array_Injector_Should_Inject_One_Registed_Type_As_Enumerable()
        {
            var resolver = new SimpleInjectorDependencyResolver();
            resolver.RegisterType<IFace1, Class1>();

            var resolve = resolver.Resolve<Class5>();
            Assert.AreEqual(1, resolve.Ifaces.Count());
            Assert.IsTrue(resolve.Ifaces.OfType<Class1>().Any());
            Assert.IsFalse(resolve.Ifaces.OfType<Class2>().Any());
            Assert.IsFalse(resolve.Ifaces.OfType<Class3>().Any());
        }

        [TestMethod]
        public void IsRegistered_Should_Return_True()
        {
            var resolver = new SimpleInjectorDependencyResolver();
            resolver.RegisterType<IFace1, Class1>();
            resolver.RegisterType<IFace1, Class2>();
            resolver.RegisterInstance<IFace1>(new Class3());

            var result = resolver.IsRegistered<IFace1>();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsRegistered_Should_Return_False()
        {
            var resolver = new SimpleInjectorDependencyResolver();
            resolver.RegisterType<IFace1, Class1>();
            resolver.RegisterType<IFace1, Class2>();
            resolver.RegisterInstance<IFace1>(new Class3());

            var result = resolver.IsRegistered<Class3>();
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Unresolveable_Should_Return_Null()
        {
            var resolver = new SimpleInjectorDependencyResolver();

            var result = resolver.Resolve<IFace1>();
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Unresolveable_Should_Return_Default()
        {
            var resolver = new SimpleInjectorDependencyResolver();

            var result = resolver.Resolve<Class3>();
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void RegisterFactory_WithSingleton_Should_Return_OneInstance()
        {
            var resolver = new SimpleInjectorDependencyResolver();
            resolver.RegisterFactory(typeof(Class6), () => new Class6(), LifetimeScope.Singleton);

            var result = resolver.Resolve<Class6>();
            var anotherResult = resolver.Resolve<Class6>();

            Assert.IsNotNull(result);
            Assert.AreEqual(result, anotherResult);
            Assert.AreEqual(1, Class6.Instances);
        }
    }

    public interface IFace1 {}
    public class Class1: IFace1 { }
    public class Class2 : IFace1 { }
    public class Class3 : IFace1 { }

    public class Class4
    {
        public IFace1[] Ifaces { get; set; }

        public Class4(IFace1[] ifaces)
        {
            Ifaces = ifaces;
        }
    }

    public class Class5
    {
        public IEnumerable<IFace1> Ifaces { get; set; }

        public Class5(IEnumerable<IFace1> ifaces)
        {
            Ifaces = ifaces;
        }
    }
    public class Class6
    {
        public static int Instances;
        public Class6()
        {
            Instances++;

        }
    }
}
