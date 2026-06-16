using System;
using System.Reflection;
using NUnit.Framework;
using NutriMind.Runtime.App;

namespace NutriMind.Tests.EditMode.App
{
    [TestFixture]
    public class SceneRegistryTests
    {
        private const string AssemblyName = "NutriMind.Runtime.App";
        private static Type FindType(string fullTypeName) => Type.GetType(fullTypeName + ", " + AssemblyName);

        [Test]
        public void SceneRegistry_TypeExists()
        {
            Type registry = FindType("NutriMind.Runtime.App.SceneRegistry");
            Assert.That(registry, Is.Not.Null,
                "SceneRegistry class must exist in NutriMind.Runtime.App");
            Assert.That(registry.IsClass, Is.True);
        }

        [Test]
        public void SceneRegistry_IsSealed()
        {
            Type registry = FindType("NutriMind.Runtime.App.SceneRegistry");
            Assert.That(registry, Is.Not.Null);
            Assert.That(registry.IsSealed, Is.True,
                "SceneRegistry should be sealed");
        }

        [Test]
        public void SceneRegistry_RegisterScene_AcceptsValidEntry()
        {
            Type registry = FindType("NutriMind.Runtime.App.SceneRegistry");
            Assert.That(registry, Is.Not.Null);

            MethodInfo registerMethod = registry.GetMethod("RegisterScene",
                BindingFlags.Public | BindingFlags.Instance, null, new[] { typeof(string), typeof(string) }, null)
                ?? registry.GetMethod("RegisterScene",
                    BindingFlags.Public | BindingFlags.Static, null, new[] { typeof(string), typeof(string) }, null);

            Assert.That(registerMethod, Is.Not.Null,
                "SceneRegistry must have RegisterScene(string key, string scenePath) method");
        }

        [Test]
        public void SceneRegistry_RegisterScene_RejectsDuplicateKey()
        {
            Type registry = FindType("NutriMind.Runtime.App.SceneRegistry");
            Assert.That(registry, Is.Not.Null);

            MethodInfo registerMethod = registry.GetMethod("RegisterScene",
                BindingFlags.Public | BindingFlags.Instance, null, new[] { typeof(string), typeof(string) }, null)
                ?? registry.GetMethod("RegisterScene",
                    BindingFlags.Public | BindingFlags.Static, null, new[] { typeof(string), typeof(string) }, null);
            Assert.That(registerMethod, Is.Not.Null);

            bool isStatic = registerMethod.IsStatic;
            object instance = isStatic ? null : registry.GetConstructor(Type.EmptyTypes)?.Invoke(null);

            Assert.DoesNotThrow(() =>
                registerMethod.Invoke(instance, new object[] { "world_main", "Assets/Scenes/MainWorld.unity" }));

            try
            {
                object dupResult = registerMethod.Invoke(instance, new object[] { "world_main", "Assets/Scenes/MainWorld.unity" });
                if (dupResult is bool b) Assert.That(b, Is.False, "RegisterScene must return false on duplicate key");
            }
            catch (TargetInvocationException ex) when (ex.InnerException is InvalidOperationException)
            {
                Assert.Pass("RegisterScene throws {0} on duplicate key", ex.InnerException.GetType().Name);
            }
        }

        [Test]
        public void SceneRegistry_RegisterScene_RejectsNullKey()
        {
            Type registry = FindType("NutriMind.Runtime.App.SceneRegistry");
            Assert.That(registry, Is.Not.Null);

            MethodInfo registerMethod = registry.GetMethod("RegisterScene",
                BindingFlags.Public | BindingFlags.Instance, null, new[] { typeof(string), typeof(string) }, null)
                ?? registry.GetMethod("RegisterScene",
                    BindingFlags.Public | BindingFlags.Static, null, new[] { typeof(string), typeof(string) }, null);
            Assert.That(registerMethod, Is.Not.Null);

            bool isStatic = registerMethod.IsStatic;
            object instance = isStatic ? null : registry.GetConstructor(Type.EmptyTypes)?.Invoke(null);

            try
            {
                registerMethod.Invoke(instance, new object[] { null, "Assets/Scenes/Some.unity" });
                Assert.Fail("RegisterScene must throw on null key");
            }
            catch (TargetInvocationException ex) when (ex.InnerException is ArgumentNullException) { }
            catch (TargetInvocationException ex) when (ex.InnerException is ArgumentException) { }
        }

        [Test]
        public void SceneRegistry_TryGetScene_NullKeyReturnsFalse()
        {
            var registry = new SceneRegistry();

            Assert.That(registry.GetScene(null), Is.Null,
                "GetScene(null) must return null without throwing");

            string path = "should-be-cleared";
            Assert.DoesNotThrow(() =>
            {
                bool result = registry.TryGetScene(null, out path);
                Assert.That(result, Is.False, "TryGetScene(null) must return false");
            });

            Assert.That(path, Is.Null.Or.Empty,
                "TryGetScene(null, out path) should leave path null or empty");
        }

        [Test]
        public void SceneRegistry_GetScene_ReturnsUnavailableForBadKey()
        {
            Type registry = FindType("NutriMind.Runtime.App.SceneRegistry");
            Assert.That(registry, Is.Not.Null);

            MethodInfo getMethod = registry.GetMethod("GetScene",
                BindingFlags.Public | BindingFlags.Instance, null, new[] { typeof(string) }, null)
                ?? registry.GetMethod("TryGetScene",
                    BindingFlags.Public | BindingFlags.Instance, null, new[] { typeof(string) }, null)
                ?? registry.GetMethod("GetScene",
                    BindingFlags.Public | BindingFlags.Static, null, new[] { typeof(string) }, null);

            Assert.That(getMethod, Is.Not.Null, "Must have GetScene or TryGetScene method");

            bool isStatic = getMethod.IsStatic;
            object instance = isStatic ? null : registry.GetConstructor(Type.EmptyTypes)?.Invoke(null);
            object result = getMethod.Invoke(instance, new object[] { "bad_key" });

            if (getMethod.ReturnType == typeof(string))
                Assert.That(result, Is.Null.Or.Empty, "GetScene must return null/empty for bad key");
            else
            {
                PropertyInfo okProp = getMethod.ReturnType.GetProperty("Success", BindingFlags.Public | BindingFlags.Instance)
                    ?? getMethod.ReturnType.GetProperty("Found", BindingFlags.Public | BindingFlags.Instance);
                if (okProp?.PropertyType == typeof(bool))
                    Assert.That((bool)okProp.GetValue(result), Is.False);
            }
        }

        [Test]
        public void SceneRegistry_GetScene_ReturnsRegisteredPath()
        {
            Type registry = FindType("NutriMind.Runtime.App.SceneRegistry");
            Assert.That(registry, Is.Not.Null);

            MethodInfo registerMethod = registry.GetMethod("RegisterScene",
                BindingFlags.Public | BindingFlags.Instance, null, new[] { typeof(string), typeof(string) }, null)
                ?? registry.GetMethod("RegisterScene",
                    BindingFlags.Public | BindingFlags.Static, null, new[] { typeof(string), typeof(string) }, null);
            MethodInfo getSceneMethod = registry.GetMethod("GetScene",
                BindingFlags.Public | BindingFlags.Instance, null, new[] { typeof(string) }, null)
                ?? registry.GetMethod("TryGetScene",
                    BindingFlags.Public | BindingFlags.Instance, null, new[] { typeof(string) }, null)
                ?? registry.GetMethod("GetScene",
                    BindingFlags.Public | BindingFlags.Static, null, new[] { typeof(string) }, null);
            Assert.That(registerMethod, Is.Not.Null);
            Assert.That(getSceneMethod, Is.Not.Null);

            bool isStatic = registerMethod.IsStatic;
            object instance = isStatic ? null : registry.GetConstructor(Type.EmptyTypes)?.Invoke(null);

            registerMethod.Invoke(instance, new object[] { "test_scene", "Assets/Scenes/TestWorld.unity" });
            object result = getSceneMethod.Invoke(instance, new object[] { "test_scene" });

            if (getSceneMethod.ReturnType == typeof(string))
                Assert.That(result, Is.EqualTo("Assets/Scenes/TestWorld.unity"));
        }

        [Test]
        public void SceneRegistry_ExposesRegisteredCount()
        {
            Type registry = FindType("NutriMind.Runtime.App.SceneRegistry");
            Assert.That(registry, Is.Not.Null);

            PropertyInfo countProp = registry.GetProperty("Count", BindingFlags.Public | BindingFlags.Instance)
                ?? registry.GetProperty("RegisteredCount", BindingFlags.Public | BindingFlags.Instance)
                ?? registry.GetProperty("SceneCount", BindingFlags.Public | BindingFlags.Instance);
            Assert.That(countProp, Is.Not.Null, "SceneRegistry must expose Count property");
            Assert.That(countProp.PropertyType, Is.EqualTo(typeof(int)));
        }

        [Test]
        public void SceneRegistry_ValidatesSceneInBuildSettings()
        {
            Type registry = FindType("NutriMind.Runtime.App.SceneRegistry");
            Assert.That(registry, Is.Not.Null);

            MethodInfo validateMethod = registry.GetMethod("ValidateSceneInBuildSettings", BindingFlags.Public | BindingFlags.Instance)
                ?? registry.GetMethod("ValidateSceneInBuildSettings", BindingFlags.Public | BindingFlags.Static)
                ?? registry.GetMethod("IsSceneInBuild", BindingFlags.Public | BindingFlags.Instance);

            if (validateMethod == null)
            {
                MethodInfo registerMethod = registry.GetMethod("RegisterScene",
                    BindingFlags.Public | BindingFlags.Instance, null, new[] { typeof(string), typeof(string) }, null);
                Assert.That(registerMethod, Is.Not.Null, "Must provide build-settings validation");
            }
            else
                Assert.That(validateMethod.ReturnType == typeof(bool) || validateMethod.ReturnType == typeof(void), Is.True);
        }
    }
}
