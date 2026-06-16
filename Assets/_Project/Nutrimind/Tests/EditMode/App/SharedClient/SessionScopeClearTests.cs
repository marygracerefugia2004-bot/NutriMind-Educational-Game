using System;
using System.Reflection;
using NUnit.Framework;

namespace NutriMind.Tests.EditMode.App
{
    /// <summary>
    /// RED-phase tests for <c>SessionScope.Clear()</c> behaviour
    /// (Phase 03 requirement: Clear resets auth/profile/settings/subject/progress/
    ///  reward/interaction session-owned state but preserves <see cref="DataProviderMode"/>).
    ///
    /// The <c>Mode</c> preservation test is expected to pass with the Phase 02
    /// implementation.  The session-owned state tests will FAIL until Phase 03
    /// adds those fields and the corresponding clear logic.
    /// </summary>
    [TestFixture]
    public class SessionScopeClearTests
    {
        private const string AssemblyName = "NutriMind.Runtime.App";

        private static Type FindType(string fullTypeName)
        {
            return Type.GetType(fullTypeName + ", " + AssemblyName);
        }

        private static Type FindEnum(string enumName)
        {
            return FindType("NutriMind.Runtime.App." + enumName);
        }

        // ---------------------------------------------------------------
        // Mode preservation (existing Phase 02 — expected to PASS)
        // ---------------------------------------------------------------

        [Test]
        public void Mode_SurvivesClear()
        {
            Type sessionType = FindType("NutriMind.Runtime.App.SessionScope");
            Assert.That(sessionType, Is.Not.Null,
                "Precondition: SessionScope type must exist");

            Type modeType = FindEnum("DataProviderMode");
            Assert.That(modeType, Is.Not.Null,
                "Precondition: DataProviderMode type must exist");

            PropertyInfo modeProp = sessionType.GetProperty("Mode",
                BindingFlags.Public | BindingFlags.Instance);
            Assert.That(modeProp, Is.Not.Null,
                "Precondition: SessionScope must have Mode property");

            MethodInfo clearMethod = sessionType.GetMethod("Clear",
                BindingFlags.Public | BindingFlags.Instance,
                null, Type.EmptyTypes, null);
            Assert.That(clearMethod, Is.Not.Null,
                "Precondition: SessionScope must have Clear() method");

            ConstructorInfo ctor = sessionType.GetConstructor(Type.EmptyTypes);
            object session = ctor.Invoke(null);

            // Set Mode to Http before clearing
            object httpMode = Enum.Parse(modeType, "Http");
            modeProp.SetValue(session, httpMode);

            // Act
            clearMethod.Invoke(session, null);

            // Assert — Mode preserved
            object modeAfterClear = modeProp.GetValue(session);
            Assert.That(modeAfterClear, Is.EqualTo(httpMode),
                "SessionScope.Clear must preserve the current Mode value " +
                "(data provider mode is chosen at startup and must survive logout/clear)");
        }

        [Test]
        public void Mode_DefaultIsLocalDemoJson()
        {
            Type sessionType = FindType("NutriMind.Runtime.App.SessionScope");
            Assert.That(sessionType, Is.Not.Null,
                "Precondition: SessionScope type must exist");

            Type modeType = FindEnum("DataProviderMode");
            Assert.That(modeType, Is.Not.Null,
                "Precondition: DataProviderMode type must exist");

            PropertyInfo modeProp = sessionType.GetProperty("Mode",
                BindingFlags.Public | BindingFlags.Instance);
            Assert.That(modeProp, Is.Not.Null,
                "Precondition: SessionScope must have Mode property");

            ConstructorInfo ctor = sessionType.GetConstructor(Type.EmptyTypes);
            object session = ctor.Invoke(null);

            object defaultMode = modeProp.GetValue(session);
            object expectedDefault = Enum.Parse(modeType, "LocalDemoJson");
            Assert.That(defaultMode, Is.EqualTo(expectedDefault),
                "SessionScope.Mode must default to LocalDemoJson");
        }

        // ---------------------------------------------------------------
        // Session-owned state fields — Phase 03 additions
        // These will FAIL because the fields do not exist yet (RED).
        // ---------------------------------------------------------------

        private static readonly string[] s_sessionStateFieldNames =
        {
            "AuthToken",
            "ProfileData",
            "Settings",
            "SelectedSubject",
            "ProgressData",
            "RewardState",
            "InteractionState"
        };

        [Test]
        public void SessionScope_HasAuthTokenProperty([ValueSource(nameof(s_sessionStateFieldNames))] string fieldName)
        {
            Type sessionType = FindType("NutriMind.Runtime.App.SessionScope");
            Assert.That(sessionType, Is.Not.Null,
                "Precondition: SessionScope type must exist");

            PropertyInfo prop = sessionType.GetProperty(fieldName,
                BindingFlags.Public | BindingFlags.Instance);
            Assert.That(prop, Is.Not.Null,
                "SessionScope must expose a public instance property named '{0}' " +
                "for session-scoped state that Clear() resets", fieldName);
        }

        [Test]
        public void Clear_ResetsAllSessionOwnedStateToDefault()
        {
            Type sessionType = FindType("NutriMind.Runtime.App.SessionScope");
            Assert.That(sessionType, Is.Not.Null,
                "Precondition: SessionScope type must exist");

            MethodInfo clearMethod = sessionType.GetMethod("Clear",
                BindingFlags.Public | BindingFlags.Instance,
                null, Type.EmptyTypes, null);
            Assert.That(clearMethod, Is.Not.Null,
                "Precondition: SessionScope must have Clear() method");

            ConstructorInfo ctor = sessionType.GetConstructor(Type.EmptyTypes);
            object session = ctor.Invoke(null);

            // Verify each expected session-owned property is null/default after Clear()
            foreach (string fieldName in s_sessionStateFieldNames)
            {
                PropertyInfo prop = sessionType.GetProperty(fieldName,
                    BindingFlags.Public | BindingFlags.Instance);
                if (prop == null)
                {
                    // Property not yet implemented — expected for Phase 03 (RED)
                    continue;
                }

                object value = prop.GetValue(session);
                Type propType = prop.PropertyType;

                // Nullable reference types or string should be null
                // Value types (e.g. enum SubjectType?) should be default
                if (!propType.IsValueType || Nullable.GetUnderlyingType(propType) != null)
                {
                    Assert.That(value, Is.Null,
                        "SessionScope.{0} must be null after Clear() / on fresh scope", fieldName);
                }
                else
                {
                    Assert.That(value, Is.EqualTo(Activator.CreateInstance(propType)),
                        "SessionScope.{0} must be default({1}) after Clear()", fieldName, propType.Name);
                }
            }

            // Double-check Mode wasn't accidentally reset
            Type modeType = FindEnum("DataProviderMode");
            if (modeType != null)
            {
                PropertyInfo modeProp = sessionType.GetProperty("Mode",
                    BindingFlags.Public | BindingFlags.Instance);
                if (modeProp != null)
                {
                    object modeValue = modeProp.GetValue(session);
                    Assert.That(modeValue, Is.Not.Null,
                        "Mode must never be null — it is preserved across Clear()");
                }
            }
        }

        // ---------------------------------------------------------------
        // Guard: Clear does not throw (already passes)
        // ---------------------------------------------------------------

        [Test]
        public void Clear_DoesNotThrow()
        {
            Type sessionType = FindType("NutriMind.Runtime.App.SessionScope");
            Assert.That(sessionType, Is.Not.Null,
                "Precondition: SessionScope type must exist");

            MethodInfo clearMethod = sessionType.GetMethod("Clear",
                BindingFlags.Public | BindingFlags.Instance,
                null, Type.EmptyTypes, null);
            Assert.That(clearMethod, Is.Not.Null,
                "Precondition: SessionScope must have Clear() method");

            ConstructorInfo ctor = sessionType.GetConstructor(Type.EmptyTypes);
            object session = ctor.Invoke(null);

            Assert.DoesNotThrow(() => clearMethod.Invoke(session, null),
                "Clear() must not throw under normal conditions");
        }

        // ---------------------------------------------------------------
        // Clear is idempotent
        // ---------------------------------------------------------------

        [Test]
        public void Clear_IsIdempotent()
        {
            Type sessionType = FindType("NutriMind.Runtime.App.SessionScope");
            Assert.That(sessionType, Is.Not.Null,
                "Precondition: SessionScope type must exist");

            MethodInfo clearMethod = sessionType.GetMethod("Clear",
                BindingFlags.Public | BindingFlags.Instance,
                null, Type.EmptyTypes, null);
            Assert.That(clearMethod, Is.Not.Null,
                "Precondition: SessionScope must have Clear() method");

            ConstructorInfo ctor = sessionType.GetConstructor(Type.EmptyTypes);
            object session = ctor.Invoke(null);

            Assert.DoesNotThrow(() =>
            {
                clearMethod.Invoke(session, null);
                clearMethod.Invoke(session, null);
                clearMethod.Invoke(session, null);
            }, "Clear() must be idempotent — calling it multiple times must not throw");
        }
    }
}
