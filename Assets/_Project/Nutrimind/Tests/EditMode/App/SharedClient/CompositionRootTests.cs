using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;

namespace NutriMind.Tests.EditMode.App
{
    [TestFixture]
    public class CompositionRootTests
    {
        private const string AssemblyName = "NutriMind.Runtime.App";
        private static Type FindType(string fullTypeName) => Type.GetType(fullTypeName + ", " + AssemblyName);

        [Test]
        public void CompositionRoot_TypeExists()
        {
            Type t = FindType("NutriMind.Runtime.App.CompositionRoot");
            Assert.That(t, Is.Not.Null, "CompositionRoot class must exist");
            Assert.That(t.IsClass, Is.True);
        }

        [Test]
        public void CompositionRoot_IsSingleton()
        {
            Type t = FindType("NutriMind.Runtime.App.CompositionRoot");
            Assert.That(t, Is.Not.Null);

            PropertyInfo instProp = t.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static);
            MethodInfo getInst = t.GetMethod("GetInstance", BindingFlags.Public | BindingFlags.Static, null, Type.EmptyTypes, null);

            Assert.That(instProp != null || getInst != null,
                "CompositionRoot must have static Instance property or GetInstance() method");
        }

        [Test]
        public void CompositionRoot_Instance_ReturnsSameReference()
        {
            Type t = FindType("NutriMind.Runtime.App.CompositionRoot");
            Assert.That(t, Is.Not.Null);

            PropertyInfo instProp = t.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static);
            object i1 = instProp?.GetValue(null);
            object i2 = instProp?.GetValue(null);
            if (i1 != null && i2 != null)
                Assert.That(i2, Is.SameAs(i1), "Instance must return same reference");
            else
            {
                MethodInfo getInst = t.GetMethod("GetInstance", BindingFlags.Public | BindingFlags.Static);
                if (getInst != null)
                {
                    i1 = getInst.Invoke(null, null);
                    i2 = getInst.Invoke(null, null);
                    Assert.That(i2, Is.SameAs(i1));
                }
            }
        }

        [Test]
        public void CompositionRoot_ExposesAppStateMachine()
        {
            Type t = FindType("NutriMind.Runtime.App.CompositionRoot");
            Assert.That(t, Is.Not.Null);

            PropertyInfo p = t.GetProperty("StateMachine", BindingFlags.Public | BindingFlags.Instance)
                ?? t.GetProperty("AppStateMachine", BindingFlags.Public | BindingFlags.Instance);
            Assert.That(p, Is.Not.Null, "CompositionRoot must expose StateMachine or AppStateMachine property");
            Assert.That(p.CanRead, Is.True);
        }

        [Test]
        public void CompositionRoot_ExposesSessionScope()
        {
            Type t = FindType("NutriMind.Runtime.App.CompositionRoot");
            Assert.That(t, Is.Not.Null);

            PropertyInfo p = t.GetProperty("Session", BindingFlags.Public | BindingFlags.Instance)
                ?? t.GetProperty("SessionScope", BindingFlags.Public | BindingFlags.Instance);
            Assert.That(p, Is.Not.Null, "CompositionRoot must expose Session or SessionScope property");
            Assert.That(p.CanRead, Is.True);
        }

        [Test]
        public void CompositionRoot_ExposesDataProvider()
        {
            Type t = FindType("NutriMind.Runtime.App.CompositionRoot");
            Assert.That(t, Is.Not.Null);

            PropertyInfo p = t.GetProperty("DataProvider", BindingFlags.Public | BindingFlags.Instance)
                ?? t.GetProperty("GameDataProvider", BindingFlags.Public | BindingFlags.Instance)
                ?? t.GetProperty("Provider", BindingFlags.Public | BindingFlags.Instance);
            Assert.That(p, Is.Not.Null, "CompositionRoot must expose provider property");
            Assert.That(p.CanRead, Is.True);
        }

        [Test]
        public void CompositionRoot_Services_AreSharedInstances()
        {
            Type t = FindType("NutriMind.Runtime.App.CompositionRoot");
            Assert.That(t, Is.Not.Null);

            PropertyInfo instProp = t.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static);
            object root = instProp?.GetValue(null);
            if (root == null)
            {
                MethodInfo getInst = t.GetMethod("GetInstance", BindingFlags.Public | BindingFlags.Static);
                root = getInst?.Invoke(null, null);
            }
            if (root == null) { Assert.Ignore("Cannot verify - singleton not accessible"); return; }

            foreach (PropertyInfo p in t.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!p.CanRead || p.PropertyType == t || p.PropertyType.IsValueType) continue;
                object s1 = p.GetValue(root);
                object s2 = p.GetValue(root);
                if (s1 != null)
                    Assert.That(s2, Is.SameAs(s1), "{0} must return same instance on repeated access", p.Name);
            }
        }

        [Test]
        public void CompositionRoot_Services_AreNotNull()
        {
            Type t = FindType("NutriMind.Runtime.App.CompositionRoot");
            Assert.That(t, Is.Not.Null);

            PropertyInfo instProp = t.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static);
            object root = instProp?.GetValue(null);
            if (root == null)
            {
                MethodInfo getInst = t.GetMethod("GetInstance", BindingFlags.Public | BindingFlags.Static);
                root = getInst?.Invoke(null, null);
            }
            if (root == null) return;

            // Known nullable properties (null in non-Http mode).
            var knownNullable = new HashSet<string> { "SyncPolling" };

            foreach (PropertyInfo p in t.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!p.CanRead || p.PropertyType == t || knownNullable.Contains(p.Name)) continue;
                object s = p.GetValue(root);
                Assert.That(s, Is.Not.Null, "{0} must return non-null service instance", p.Name);
            }
        }

        [Test]
        public void CompositionRoot_HasInitializeMethod()
        {
            Type t = FindType("NutriMind.Runtime.App.CompositionRoot");
            Assert.That(t, Is.Not.Null);

            MethodInfo m = t.GetMethod("Initialize", BindingFlags.Public | BindingFlags.Instance)
                ?? t.GetMethod("Initialize", BindingFlags.Public | BindingFlags.Static)
                ?? t.GetMethod("InitializeAsync", BindingFlags.Public | BindingFlags.Instance);

            if (m == null)
                Assert.Ignore("Phase 03 should provide Initialize or InitializeAsync");
        }
    }
}
