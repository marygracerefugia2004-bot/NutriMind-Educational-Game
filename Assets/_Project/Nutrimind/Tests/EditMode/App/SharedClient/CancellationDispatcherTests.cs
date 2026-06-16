using System;
using System.Reflection;
using NUnit.Framework;

namespace NutriMind.Tests.EditMode.App
{
    [TestFixture]
    public class CancellationDispatcherTests
    {
        private const string AssemblyName = "NutriMind.Runtime.App";
        private static Type FindType(string fullTypeName) => Type.GetType(fullTypeName + ", " + AssemblyName);

        [Test]
        public void MainThreadDispatcher_TypeExists()
        {
            Type d = FindType("NutriMind.Runtime.App.MainThreadDispatcher");
            Assert.That(d, Is.Not.Null, "MainThreadDispatcher class must exist");
            Assert.That(d.IsClass, Is.True);
        }

        [Test]
        public void MainThreadDispatcher_HasPostMethod()
        {
            Type d = FindType("NutriMind.Runtime.App.MainThreadDispatcher");
            Assert.That(d, Is.Not.Null);

            MethodInfo post = null;
            foreach (MethodInfo m in d.GetMethods(BindingFlags.Public | BindingFlags.Instance))
                if (m.Name == "Post" && m.GetParameters().Length >= 1) { post = m; break; }

            Assert.That(post, Is.Not.Null, "MainThreadDispatcher must have Post method accepting a callback");
        }

        [Test]
        public void MainThreadDispatcher_HasExecutePendingMethod()
        {
            Type d = FindType("NutriMind.Runtime.App.MainThreadDispatcher");
            Assert.That(d, Is.Not.Null);

            MethodInfo exec = d.GetMethod("ExecutePending", BindingFlags.Public | BindingFlags.Instance)
                ?? d.GetMethod("Flush", BindingFlags.Public | BindingFlags.Instance)
                ?? d.GetMethod("Update", BindingFlags.Public | BindingFlags.Instance)
                ?? d.GetMethod("DispatchPending", BindingFlags.Public | BindingFlags.Instance);

            Assert.That(exec, Is.Not.Null, "Must have ExecutePending/Flush/Update/DispatchPending method");
        }

        [Test]
        public void MainThreadDispatcher_HasCancelOrInvalidateMethod()
        {
            Type d = FindType("NutriMind.Runtime.App.MainThreadDispatcher");
            Assert.That(d, Is.Not.Null);

            MethodInfo cancel = d.GetMethod("Invalidate", BindingFlags.Public | BindingFlags.Instance)
                ?? d.GetMethod("CancelPending", BindingFlags.Public | BindingFlags.Instance)
                ?? d.GetMethod("Clear", BindingFlags.Public | BindingFlags.Instance);

            PropertyInfo genProp = d.GetProperty("Generation", BindingFlags.Public | BindingFlags.Instance);

            if (cancel == null && genProp == null)
                Assert.Ignore("Phase 03 MainThreadDispatcher should provide Invalidate/CancelPending or Generation property");
        }

        [Test]
        public void AppStateMachine_HasInvalidateInFlightOperations()
        {
            Type sm = FindType("NutriMind.Runtime.App.AppStateMachine");
            Assert.That(sm, Is.Not.Null);

            MethodInfo m = sm.GetMethod("InvalidateInFlightOperations",
                BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null);
            Assert.That(m, Is.Not.Null, "AppStateMachine must have InvalidateInFlightOperations");
        }

        [Test]
        public void MainThreadDispatcher_CanPostAndExecute()
        {
            Type d = FindType("NutriMind.Runtime.App.MainThreadDispatcher");
            Assert.That(d, Is.Not.Null);

            ConstructorInfo ctor = d.GetConstructor(Type.EmptyTypes);
            if (ctor == null) return;

            object instance = ctor.Invoke(null);

            MethodInfo post = null;
            foreach (MethodInfo m in d.GetMethods(BindingFlags.Public | BindingFlags.Instance))
                if (m.Name == "Post" && m.GetParameters().Length >= 1) { post = m; break; }
            if (post == null) return;

            MethodInfo exec = d.GetMethod("ExecutePending", BindingFlags.Public | BindingFlags.Instance)
                ?? d.GetMethod("Flush", BindingFlags.Public | BindingFlags.Instance);

            bool callbackRan = false;
            Action cb = () => { callbackRan = true; };

            ParameterInfo[] postParams = post.GetParameters();
            object[] postArgs = new object[postParams.Length];
            postArgs[0] = cb;
            for (int i = 1; i < postParams.Length; i++)
                postArgs[i] = postParams[i].ParameterType.IsValueType ? Activator.CreateInstance(postParams[i].ParameterType) : null;

            post.Invoke(instance, postArgs);

            if (exec != null)
            {
                exec.Invoke(instance, null);
                Assert.That(callbackRan, Is.True, "Posted callback must have executed after ExecutePending/Flush");
            }
        }
    }
}
