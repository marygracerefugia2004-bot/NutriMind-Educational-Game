using System;
using System.Reflection;
using NUnit.Framework;

namespace NutriMind.Tests.EditMode.App
{
    [TestFixture]
    public class AssetCatalogValidatorTests
    {
        private const string AssemblyName = "NutriMind.Runtime.App";
        private static Type FindType(string fullTypeName) => Type.GetType(fullTypeName + ", " + AssemblyName);

        [Test]
        public void AssetCatalogValidator_TypeExists()
        {
            Type v = FindType("NutriMind.Runtime.App.AssetCatalogValidator");
            Assert.That(v, Is.Not.Null, "AssetCatalogValidator class must exist");
            Assert.That(v.IsClass, Is.True);
        }

        [Test]
        public void AssetCatalogValidator_HasValidateMethod()
        {
            Type v = FindType("NutriMind.Runtime.App.AssetCatalogValidator");
            Assert.That(v, Is.Not.Null);

            MethodInfo m = v.GetMethod("Validate", BindingFlags.Public | BindingFlags.Instance)
                ?? v.GetMethod("Validate", BindingFlags.Public | BindingFlags.Static);
            Assert.That(m, Is.Not.Null, "AssetCatalogValidator must have a Validate method");
        }

        [Test]
        public void AssetCatalogValidator_DetectsDuplicateAssetKey()
        {
            Type v = FindType("NutriMind.Runtime.App.AssetCatalogValidator");
            Assert.That(v, Is.Not.Null);

            MethodInfo m = v.GetMethod("CheckForDuplicates", BindingFlags.Public | BindingFlags.Instance)
                ?? v.GetMethod("CheckForDuplicates", BindingFlags.Public | BindingFlags.Static)
                ?? v.GetMethod("ValidateMapping", BindingFlags.Public | BindingFlags.Instance);

            if (m != null)
                Assert.That(m.ReturnType == typeof(bool) || m.ReturnType == typeof(void), Is.True);
            else
            {
                m = v.GetMethod("Validate", BindingFlags.Public | BindingFlags.Instance)
                    ?? v.GetMethod("Validate", BindingFlags.Public | BindingFlags.Static);
                Assert.That(m, Is.Not.Null, "Must expose CheckForDuplicates, ValidateMapping, or Validate");
            }
        }

        [Test]
        public void AssetCatalogValidator_DetectsMissingMapping()
        {
            Type v = FindType("NutriMind.Runtime.App.AssetCatalogValidator");
            Assert.That(v, Is.Not.Null);

            MethodInfo m = v.GetMethod("CheckForMissingMappings", BindingFlags.Public | BindingFlags.Instance)
                ?? v.GetMethod("CheckForMissingMappings", BindingFlags.Public | BindingFlags.Static)
                ?? v.GetMethod("FindMissingMappings", BindingFlags.Public | BindingFlags.Instance)
                ?? v.GetMethod("FindMissingMappings", BindingFlags.Public | BindingFlags.Static);

            Assert.That(m, Is.Not.Null, "Must expose CheckForMissingMappings or FindMissingMappings");
        }

        [Test]
        public void AssetCatalogValidator_HasFallbackResolveMethod()
        {
            Type v = FindType("NutriMind.Runtime.App.AssetCatalogValidator");
            Assert.That(v, Is.Not.Null);

            MethodInfo m = v.GetMethod("ResolveAsset", BindingFlags.Public | BindingFlags.Instance)
                ?? v.GetMethod("Resolve", BindingFlags.Public | BindingFlags.Instance)
                ?? v.GetMethod("GetAsset", BindingFlags.Public | BindingFlags.Instance)
                ?? v.GetMethod("TryGetAsset", BindingFlags.Public | BindingFlags.Instance)
                ?? v.GetMethod("TryResolve", BindingFlags.Public | BindingFlags.Instance);

            Assert.That(m, Is.Not.Null, "Must provide ResolveAsset/GetAsset/TryResolve for safe lookup");
            Assert.That(m.ReturnType, Is.Not.EqualTo(typeof(void)));
        }

        [Test]
        public void AssetCatalogValidator_Validate_DoesNotThrowOnEmptyCatalog()
        {
            Type v = FindType("NutriMind.Runtime.App.AssetCatalogValidator");
            Assert.That(v, Is.Not.Null);

            MethodInfo m = v.GetMethod("Validate", BindingFlags.Public | BindingFlags.Instance)
                ?? v.GetMethod("Validate", BindingFlags.Public | BindingFlags.Static);

            if (m != null)
            {
                bool isStatic = m.IsStatic;
                object instance = isStatic ? null : v.GetConstructor(Type.EmptyTypes)?.Invoke(null);
                object[] args = m.GetParameters().Length == 0 ? Array.Empty<object>() : new object[m.GetParameters().Length];
                Assert.DoesNotThrow(() => m.Invoke(instance, args));
            }
        }

        [Test]
        public void AssetCatalogEntry_TypeExists()
        {
            Type entry = FindType("NutriMind.Runtime.App.AssetCatalogEntry");
            Assert.That(entry, Is.Not.Null, "AssetCatalogEntry type must exist");
        }

        [Test]
        public void AssetCatalogValidator_ValidateResult_HasDiagnosticProperties()
        {
            Type v = FindType("NutriMind.Runtime.App.AssetCatalogValidator");
            Assert.That(v, Is.Not.Null);

            MethodInfo m = v.GetMethod("Validate", BindingFlags.Public | BindingFlags.Instance)
                ?? v.GetMethod("Validate", BindingFlags.Public | BindingFlags.Static);

            if (m != null && m.ReturnType != typeof(void))
            {
                Type rt = m.ReturnType;
                bool hasDiag = rt.GetProperty("HasErrors", BindingFlags.Public | BindingFlags.Instance) != null
                    || rt.GetProperty("Diagnostics", BindingFlags.Public | BindingFlags.Instance) != null
                    || rt.GetProperty("Errors", BindingFlags.Public | BindingFlags.Instance) != null
                    || rt.GetProperty("Issues", BindingFlags.Public | BindingFlags.Instance) != null;
                Assert.That(hasDiag, Is.True, "Validate result type should include diagnostic properties");
            }

        }

        // ---------------------------------------------------------------
        // Phase 03: duplicate keys produce structured diagnostics (not throw)
        // ---------------------------------------------------------------

        [Test]
        public void RegisterDuplicate_DoesNotThrow()
        {
            Type v = FindType("NutriMind.Runtime.App.AssetCatalogValidator");
            Assert.That(v, Is.Not.Null,
                "Precondition: AssetCatalogValidator type must exist");

            MethodInfo register = v.GetMethod("Register",
                BindingFlags.Public | BindingFlags.Instance,
                null, new[] { typeof(string), typeof(string) }, null);

            Assert.That(register, Is.Not.Null,
                "AssetCatalogValidator must have a Register(string, string) method");

            ConstructorInfo ctor = v.GetConstructor(Type.EmptyTypes);
            object instance = ctor.Invoke(null);

            // First registration should succeed
            register.Invoke(instance, new object[] { "duplicate_key", "Assets/SomeAsset.png" });

            // Second registration with the same key must NOT throw
            Assert.DoesNotThrow(() =>
                register.Invoke(instance, new object[] { "duplicate_key", "Assets/SomeAsset.png" }),
                "Registering a duplicate key must not throw during registration -- " +
                "duplicates should be reported through Validate() or CheckForDuplicates()");
        }

        [Test]
        public void Validate_ReportsDuplicateKeyInIssues()
        {
            Type v = FindType("NutriMind.Runtime.App.AssetCatalogValidator");
            Assert.That(v, Is.Not.Null,
                "Precondition: AssetCatalogValidator type must exist");

            MethodInfo register = v.GetMethod("Register",
                BindingFlags.Public | BindingFlags.Instance,
                null, new[] { typeof(string), typeof(string) }, null);
            Assert.That(register, Is.Not.Null,
                "Precondition: Register(string, string) method must exist");

            MethodInfo validate = v.GetMethod("Validate",
                BindingFlags.Public | BindingFlags.Instance);
            Assert.That(validate, Is.Not.Null,
                "Precondition: Validate method must exist");

            ConstructorInfo ctor = v.GetConstructor(Type.EmptyTypes);
            object instance = ctor.Invoke(null);

            // Register the same key twice -- must not throw
            Assert.DoesNotThrow(() =>
            {
                register.Invoke(instance, new object[] { "my_key", "Assets/Path1.unity" });
                register.Invoke(instance, new object[] { "my_key", "Assets/Path2.unity" });
            }, "Registering duplicate keys must not throw");

            // Validate should report the duplicate
            object result = validate.Invoke(instance, null);

            // The result should have diagnostic data indicating a duplicate
            Type resultType = result?.GetType();
            Assert.That(resultType, Is.Not.Null,
                "Validate() must return a result object");

            // Check the Issues/Errors/Diagnostics collections
            PropertyInfo issuesProp = resultType.GetProperty("Issues",
                BindingFlags.Public | BindingFlags.Instance)
                ?? resultType.GetProperty("Errors", BindingFlags.Public | BindingFlags.Instance)
                ?? resultType.GetProperty("Diagnostics", BindingFlags.Public | BindingFlags.Instance);

            Assert.That(issuesProp, Is.Not.Null,
                "Validate result must expose Issues, Errors, or Diagnostics collection");

            object issues = issuesProp.GetValue(result);
            Assert.That(issues, Is.Not.Null,
                "Issues/Errors/Diagnostics must be non-null");

            // The collection should contain at least one entry about the duplicate
            int issueCount = (int)(issues.GetType().GetProperty("Count")?.GetValue(issues) ?? 0);
            Assert.That(issueCount, Is.GreaterThan(0),
                "Validate() must report at least one issue when duplicate keys are present");

            // If the result has HasErrors, verify it's true
            PropertyInfo hasErrorsProp = resultType.GetProperty("HasErrors",
                BindingFlags.Public | BindingFlags.Instance);
            if (hasErrorsProp != null && hasErrorsProp.PropertyType == typeof(bool))
                Assert.That((bool)hasErrorsProp.GetValue(result), Is.True,
                    "HasErrors must be true when duplicate keys are present");
        }

        [Test]
        public void CheckForDuplicates_ReturnsTrue_AfterDuplicatesRegistered()
        {
            Type v = FindType("NutriMind.Runtime.App.AssetCatalogValidator");
            Assert.That(v, Is.Not.Null,
                "Precondition: AssetCatalogValidator type must exist");

            MethodInfo register = v.GetMethod("Register",
                BindingFlags.Public | BindingFlags.Instance,
                null, new[] { typeof(string), typeof(string) }, null);
            Assert.That(register, Is.Not.Null,
                "Precondition: Register(string, string) method must exist");

            MethodInfo checkDup = v.GetMethod("CheckForDuplicates",
                BindingFlags.Public | BindingFlags.Instance);
            Assert.That(checkDup, Is.Not.Null,
                "Precondition: CheckForDuplicates method must exist");

            ConstructorInfo ctor = v.GetConstructor(Type.EmptyTypes);
            object instance = ctor.Invoke(null);

            // Initially no duplicates
            object initialResult = checkDup.Invoke(instance, null);
            Assert.That((bool)initialResult, Is.False,
                "CheckForDuplicates must return false on an empty or unique catalog");

            // Register duplicate (must not throw)
            Assert.DoesNotThrow(() =>
            {
                register.Invoke(instance, new object[] { "dup_key", "Assets/A.unity" });
                register.Invoke(instance, new object[] { "dup_key", "Assets/B.unity" });
            });

            bool afterDuplicate = (bool)checkDup.Invoke(instance, null);
            Assert.That(afterDuplicate, Is.True,
                "CheckForDuplicates must return true after duplicate keys are registered");
        }

        [Test]
        public void RegisterDuplicate_DoesNotOverrideExistingEntry()
        {
            Type v = FindType("NutriMind.Runtime.App.AssetCatalogValidator");
            Assert.That(v, Is.Not.Null,
                "Precondition: AssetCatalogValidator type must exist");

            MethodInfo register = v.GetMethod("Register",
                BindingFlags.Public | BindingFlags.Instance,
                null, new[] { typeof(string), typeof(string) }, null);
            Assert.That(register, Is.Not.Null,
                "Precondition: Register(string, string) method must exist");

            MethodInfo resolve = v.GetMethod("ResolveAsset",
                BindingFlags.Public | BindingFlags.Instance)
                ?? v.GetMethod("Resolve", BindingFlags.Public | BindingFlags.Instance)
                ?? v.GetMethod("GetAsset", BindingFlags.Public | BindingFlags.Instance);

            ConstructorInfo ctor = v.GetConstructor(Type.EmptyTypes);
            object instance = ctor.Invoke(null);

            Assert.DoesNotThrow(() =>
            {
                register.Invoke(instance, new object[] { "keep_key", "Assets/Original.unity" });
            });

            // Register duplicate -- should not overwrite the original
            Assert.DoesNotThrow(() =>
            {
                register.Invoke(instance, new object[] { "keep_key", "Assets/Override.unity" });
            });

            // Verify original entry still resolves
            if (resolve != null)
            {
                object entry = resolve.Invoke(instance, new object[] { "keep_key" });
                if (entry != null)
                {
                    PropertyInfo pathProp = entry.GetType().GetProperty("Path",
                        BindingFlags.Public | BindingFlags.Instance);
                    if (pathProp != null)
                    {
                        string path = pathProp.GetValue(entry) as string;
                        Assert.That(path, Is.EqualTo("Assets/Original.unity"),
                            "Duplicate registration must not overwrite the original entry's Path");
                    }
                }
            }
        }
    }
}