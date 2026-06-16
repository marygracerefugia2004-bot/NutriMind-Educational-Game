using System;
using System.Reflection;
using NUnit.Framework;

namespace NutriMind.Tests.EditMode.App
{
    [TestFixture]
    public class AttemptCoordinatorTests
    {
        private const string AssemblyName = "NutriMind.Runtime.App";
        private static Type FindType(string fullTypeName) => Type.GetType(fullTypeName + ", " + AssemblyName);

        [Test]
        public void AttemptCoordinator_TypeExists()
        {
            Type t = FindType("NutriMind.Runtime.App.AttemptCoordinator");
            Assert.That(t, Is.Not.Null, "AttemptCoordinator class must exist");
            Assert.That(t.IsClass, Is.True);
        }

        [Test]
        public void AttemptCoordinator_HasSubmitMethod()
        {
            Type t = FindType("NutriMind.Runtime.App.AttemptCoordinator");
            Assert.That(t, Is.Not.Null);

            MethodInfo m = t.GetMethod("SubmitAttempt", BindingFlags.Public | BindingFlags.Instance)
                ?? t.GetMethod("TrySubmit", BindingFlags.Public | BindingFlags.Instance)
                ?? t.GetMethod("Submit", BindingFlags.Public | BindingFlags.Instance);

            Assert.That(m, Is.Not.Null, "AttemptCoordinator must have SubmitAttempt/TrySubmit/Submit method");
            Assert.That(m.ReturnType, Is.Not.EqualTo(typeof(void)));
        }

        [Test]
        public void AttemptCoordinator_EmbedsAttemptScope()
        {
            Type coordinator = FindType("NutriMind.Runtime.App.AttemptCoordinator");
            Type scopeType = FindType("NutriMind.Runtime.App.AttemptScope");
            if (scopeType == null || coordinator == null) return;

            PropertyInfo prop = coordinator.GetProperty("CurrentAttempt", BindingFlags.Public | BindingFlags.Instance)
                ?? coordinator.GetProperty("AttemptScope", BindingFlags.Public | BindingFlags.Instance)
                ?? coordinator.GetProperty("Scope", BindingFlags.Public | BindingFlags.Instance);

            if (prop != null)
                Assert.That(prop.PropertyType, Is.EqualTo(scopeType));
            else
            {
                PropertyInfo uuidProp = coordinator.GetProperty("ClientAttemptUuid", BindingFlags.Public | BindingFlags.Instance);
                if (uuidProp == null)
                    Assert.Ignore("Phase 03 AttemptCoordinator should expose AttemptScope or ClientAttemptUuid");
            }
        }

        [Test]
        public void AttemptCoordinator_HasRetryMethod()
        {
            Type t = FindType("NutriMind.Runtime.App.AttemptCoordinator");
            Assert.That(t, Is.Not.Null);

            MethodInfo m = t.GetMethod("RetryAttempt", BindingFlags.Public | BindingFlags.Instance)
                ?? t.GetMethod("Retry", BindingFlags.Public | BindingFlags.Instance);

            if (m == null)
                Assert.Ignore("Phase 03 AttemptCoordinator should have RetryAttempt or Retry method");
        }

        [Test]
        public void AttemptCoordinator_SubmitAttempt_RejectsSecondCall()
        {
            Type coordinator = FindType("NutriMind.Runtime.App.AttemptCoordinator");
            Assert.That(coordinator, Is.Not.Null);

            ConstructorInfo ctor = coordinator.GetConstructor(Type.EmptyTypes);
            if (ctor == null)
            {
                Type attemptScopeType = FindType("NutriMind.Runtime.App.AttemptScope");
                if (attemptScopeType != null)
                    ctor = coordinator.GetConstructor(new[] { attemptScopeType });
            }
            if (ctor == null) return;

            object instance = ctor.Invoke(new object[ctor.GetParameters().Length]);

            MethodInfo submitMethod = coordinator.GetMethod("SubmitAttempt", BindingFlags.Public | BindingFlags.Instance)
                ?? coordinator.GetMethod("TrySubmit", BindingFlags.Public | BindingFlags.Instance)
                ?? coordinator.GetMethod("Submit", BindingFlags.Public | BindingFlags.Instance);
            if (submitMethod == null) return;

            ParameterInfo[] submitParams = submitMethod.GetParameters();
            object[] defaultArgs = new object[submitParams.Length];
            for (int i = 0; i < submitParams.Length; i++)
                defaultArgs[i] = submitParams[i].ParameterType.IsValueType
                    ? Activator.CreateInstance(submitParams[i].ParameterType) : null;

            submitMethod.Invoke(instance, defaultArgs);
            object secondResult = submitMethod.Invoke(instance, defaultArgs);

            if (submitMethod.ReturnType == typeof(bool))
                Assert.That((bool)secondResult, Is.False, "Second SubmitAttempt must be rejected");
            else
            {
                PropertyInfo okProp = submitMethod.ReturnType.GetProperty("Success", BindingFlags.Public | BindingFlags.Instance)
                    ?? submitMethod.ReturnType.GetProperty("Accepted", BindingFlags.Public | BindingFlags.Instance);
                if (okProp?.PropertyType == typeof(bool))
                    Assert.That((bool)okProp.GetValue(secondResult), Is.False, "Second SubmitAttempt must not succeed");
            }
        }

        [Test]
        public void AttemptScope_ClientAttemptUuid_IsStableAcrossRetry()
        {
            Type t = FindType("NutriMind.Runtime.App.AttemptScope");
            Assert.That(t, Is.Not.Null);

            PropertyInfo uuidProp = t.GetProperty("ClientAttemptUuid", BindingFlags.Public | BindingFlags.Instance);
            Assert.That(uuidProp, Is.Not.Null);
            Assert.That(uuidProp.PropertyType, Is.EqualTo(typeof(string)));

            MethodInfo retryMethod = t.GetMethod("Retry", BindingFlags.Public | BindingFlags.Instance);
            Assert.That(retryMethod, Is.Not.Null);

            ConstructorInfo ctor = t.GetConstructor(Type.EmptyTypes);
            object scope = ctor.Invoke(null);

            string original = (string)uuidProp.GetValue(scope);
            Assert.That(original, Is.Not.Null.And.Not.Empty);

            retryMethod.Invoke(scope, null);
            Assert.That((string)uuidProp.GetValue(scope), Is.EqualTo(original),
                "Retry must reuse the same ClientAttemptUuid");
        }
    }
}
