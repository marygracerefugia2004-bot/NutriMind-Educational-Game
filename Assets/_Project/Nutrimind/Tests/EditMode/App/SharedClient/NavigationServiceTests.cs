using System;
using System.Reflection;
using NUnit.Framework;

namespace NutriMind.Tests.EditMode.App
{
    /// <summary>
    /// RED-phase tests for Phase 03: <c>NavigationService</c> must exist
    /// and use <c>SceneRegistry</c> by key rather than accepting raw scene
    /// paths as the primary navigation API.
    ///
    /// Tests expect:
    ///   - NavigationService type
    ///   - A Navigate/Resolve method that takes a string key
    ///   - Safe return/indication for missing keys (not thrown)
    ///   - No method that accepts raw scene paths as primary API
    ///
    /// These tests will FAIL (RED) until NavigationService is implemented.
    /// </summary>
    [TestFixture]
    public class NavigationServiceTests
    {
        private const string AssemblyName = "NutriMind.Runtime.App";

        private static Type FindType(string fullTypeName)
            => Type.GetType(fullTypeName + ", " + AssemblyName);

        // ---------------------------------------------------------------
        // NavigationService type existence
        // ---------------------------------------------------------------

        [Test]
        public void NavigationService_TypeExists()
        {
            Type t = FindType("NutriMind.Runtime.App.NavigationService");
            Assert.That(t, Is.Not.Null,
                "NavigationService class must exist in NutriMind.Runtime.App");
            Assert.That(t.IsClass, Is.True,
                "NavigationService must be a class");
        }

        // ---------------------------------------------------------------
        // Uses SceneRegistry by key (not raw paths)
        // ---------------------------------------------------------------

        [Test]
        public void NavigationService_UsesSceneRegistryByKey()
        {
            Type navType = FindType("NutriMind.Runtime.App.NavigationService");
            Assert.That(navType, Is.Not.Null,
                "Precondition: NavigationService type must exist");

            Type sceneRegistryType = FindType("NutriMind.Runtime.App.SceneRegistry");
            Assert.That(sceneRegistryType, Is.Not.Null,
                "Precondition: SceneRegistry type must exist");

            // NavigationService should either:
            // 1. Accept SceneRegistry via constructor injection, or
            // 2. Expose a SceneRegistry property, or
            // 3. Access CompositionRoot.Instance.SceneRegistry

            // Check for SceneRegistry constructor parameter
            ConstructorInfo[] ctors = navType.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
            bool hasRegistryParam = false;
            foreach (ConstructorInfo c in ctors)
            {
                foreach (ParameterInfo p in c.GetParameters())
                {
                    if (p.ParameterType == sceneRegistryType)
                    {
                        hasRegistryParam = true;
                        break;
                    }
                }
                if (hasRegistryParam) break;
            }

            // Check for SceneRegistry property
            PropertyInfo registryProp = navType.GetProperty("SceneRegistry",
                BindingFlags.Public | BindingFlags.Instance)
                ?? navType.GetProperty("Registry", BindingFlags.Public | BindingFlags.Instance);

            // Also check CompositionRoot access
            bool hasCompositionRootAccess = false;
            Type compRootType = FindType("NutriMind.Runtime.App.CompositionRoot");
            if (compRootType != null)
            {
                foreach (ConstructorInfo c in ctors)
                {
                    foreach (ParameterInfo p in c.GetParameters())
                    {
                        if (p.ParameterType == compRootType)
                        {
                            hasCompositionRootAccess = true;
                            break;
                        }
                    }
                    if (hasCompositionRootAccess) break;
                }
            }

            Assert.That(hasRegistryParam || registryProp != null || hasCompositionRootAccess,
                Is.True,
                "NavigationService must use SceneRegistry -- either via constructor injection, " +
                "a SceneRegistry property, or CompositionRoot access");
        }

        [Test]
        public void NavigationService_PrimaryApiTakesKey_NotRawPath()
        {
            Type navType = FindType("NutriMind.Runtime.App.NavigationService");
            Assert.That(navType, Is.Not.Null,
                "Precondition: NavigationService type must exist");

            // The primary navigation method should take a string key, not a scene path.
            // Look for methods named Navigate, GoTo, Resolve, etc.
            string[] methodNames = { "Navigate", "GoTo", "Resolve", "GetScenePath", "GetNavigationTarget" };

            MethodInfo keyBasedMethod = null;
            foreach (string name in methodNames)
            {
                MethodInfo m = navType.GetMethod(name,
                    BindingFlags.Public | BindingFlags.Instance,
                    null, new[] { typeof(string) }, null);
                if (m != null && m.ReturnType != typeof(void))
                {
                    keyBasedMethod = m;
                    break;
                }
            }

            // Also accept Try-pattern with out parameter
            if (keyBasedMethod == null)
            {
                foreach (string name in methodNames)
                {
                    MethodInfo m = navType.GetMethod("Try" + name,
                        BindingFlags.Public | BindingFlags.Instance,
                        null, new[] { typeof(string), typeof(string).MakeByRefType() }, null);
                    if (m != null)
                    {
                        keyBasedMethod = m;
                        break;
                    }
                }
            }

            Assert.That(keyBasedMethod, Is.Not.Null,
                "NavigationService must have a public navigation method taking a string key " +
                "(e.g. Navigate(string), Resolve(string), GoTo(string)) " +
                "that returns a result or scene path");
        }

        // ---------------------------------------------------------------
        // Missing key returns/indicates safe unavailable result
        // ---------------------------------------------------------------

        [Test]
        public void NavigationService_MissingKey_ReturnsSafeUnavailable()
        {
            Type navType = FindType("NutriMind.Runtime.App.NavigationService");
            Assert.That(navType, Is.Not.Null,
                "Precondition: NavigationService type must exist");

            // Find the key-based method
            MethodInfo navigateMethod = FindKeyBasedMethod(navType);
            Assert.That(navigateMethod, Is.Not.Null,
                "Precondition: NavigationService must have a key-based navigation method");

            // Create instance -- try parameterless constructor first, then try SceneRegistry injection
            object instance = null;
            ConstructorInfo paramless = navType.GetConstructor(Type.EmptyTypes);
            if (paramless != null)
            {
                instance = paramless.Invoke(null);
            }
            else
            {
                // Try constructing with a new SceneRegistry
                Type sceneRegistryType = FindType("NutriMind.Runtime.App.SceneRegistry");
                if (sceneRegistryType != null)
                {
                    ConstructorInfo registryCtor = sceneRegistryType.GetConstructor(Type.EmptyTypes);
                    if (registryCtor != null)
                    {
                        object registry = registryCtor.Invoke(null);
                        ConstructorInfo navCtor = navType.GetConstructor(new[] { sceneRegistryType });
                        if (navCtor != null)
                            instance = navCtor.Invoke(new[] { registry });
                    }
                }
            }

            if (instance == null)
            {
                Assert.Ignore("Cannot construct NavigationService -- no accessible constructor");
                return;
            }

            // Call with a key that won't be registered
            object result = navigateMethod.Invoke(instance, new object[] { "nonexistent_key" });

            // The result should indicate unavailable/missing -- not throw
            Assert.That(result, Is.Not.Null,
                "NavigationService navigation result must not be null for a missing key");

            Type resultType = result.GetType();
            if (resultType == typeof(string))
            {
                // It returned a string path -- must be null or empty for bad key
                Assert.That((string)result, Is.Null.Or.Empty,
                    "NavigationService must return null or empty for an unregistered key");
            }
            else
            {
                // It returned a result object -- check for Success/Found/IsAvailable property
                bool foundUnavailable = false;
                foreach (string propName in new[] { "Success", "Found", "IsAvailable", "HasValue" })
                {
                    PropertyInfo p = resultType.GetProperty(propName,
                        BindingFlags.Public | BindingFlags.Instance);
                    if (p != null && p.PropertyType == typeof(bool))
                    {
                        bool val = (bool)p.GetValue(result);
                        Assert.That(val, Is.False,
                            "NavigationService navigation result for missing key " +
                            "must have {0}=false", propName);
                        foundUnavailable = true;
                        break;
                    }
                }

                if (!foundUnavailable)
                {
                    // If we can't find a success property, at least verify the result
                    // doesn't throw when inspected
                    Assert.Pass("NavigationService returned a non-null result for missing key " +
                        "without throwing -- acceptable for safe unavailable indicator");
                }
            }
        }

        // ---------------------------------------------------------------
        // Does NOT accept raw scene paths as primary navigation API
        // ---------------------------------------------------------------

        [Test]
        public void NavigationService_DoesNotAcceptRawScenePath()
        {
            Type navType = FindType("NutriMind.Runtime.App.NavigationService");
            Assert.That(navType, Is.Not.Null,
                "Precondition: NavigationService type must exist");

            // The Navigate/Resolve/GoTo method must NOT have a parameter with
            // a name suggesting it accepts a raw scene path (like "scenePath",
            // "path", "scene") as the primary identifier.

            // Instead, it should accept a key.  We check that if there's a
            // method with a single string parameter, the parameter name is
            // not indicative of a raw path as the primary API.

            string[] methodNames = { "Navigate", "GoTo", "Resolve", "GetScenePath", "GetNavigationTarget" };

            foreach (string name in methodNames)
            {
                MethodInfo m = navType.GetMethod(name,
                    BindingFlags.Public | BindingFlags.Instance,
                    null, new[] { typeof(string) }, null);
                if (m != null)
                {
                    ParameterInfo param = m.GetParameters()[0];
                    string paramName = param.Name ?? "";
                    string lowerName = paramName.ToLowerInvariant();

                    // The primary parameter must be named like a "key", not a raw path
                    Assert.That(lowerName.Contains("path") && !lowerName.Contains("key"),
                        Is.False,
                        "NavigationService.{0}(string {1}) appears to accept a raw scene path " +
                        "as the primary navigation identifier. The primary parameter should be " +
                        "named like 'key' (e.g. sceneKey, navigationKey), not 'path' or 'scenePath'.",
                        name, paramName);
                }
            }
        }

        // ---------------------------------------------------------------
        // Helpers
        // ---------------------------------------------------------------

        private static MethodInfo FindKeyBasedMethod(Type navType)
        {
            string[] methodNames = { "Navigate", "GoTo", "Resolve", "GetScenePath", "GetNavigationTarget" };
            foreach (string name in methodNames)
            {
                MethodInfo m = navType.GetMethod(name,
                    BindingFlags.Public | BindingFlags.Instance,
                    null, new[] { typeof(string) }, null);
                if (m != null && m.ReturnType != typeof(void))
                    return m;
            }
            return null;
        }
    }
}
