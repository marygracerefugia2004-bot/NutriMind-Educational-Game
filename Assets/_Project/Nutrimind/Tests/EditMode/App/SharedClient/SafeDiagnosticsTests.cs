using System;
using System.Reflection;
using NUnit.Framework;

namespace NutriMind.Tests.EditMode.App
{
    [TestFixture]
    public class SafeDiagnosticsTests
    {
        private const string AssemblyName = "NutriMind.Runtime.App";
        private static Type FindType(string fullTypeName) => Type.GetType(fullTypeName + ", " + AssemblyName);

        [Test]
        public void SafeDiagnostics_TypeExists()
        {
            Type t = FindType("NutriMind.Runtime.App.SafeDiagnostics");
            Assert.That(t, Is.Not.Null, "SafeDiagnostics static class must exist");
        }

        [Test]
        public void SafeDiagnostics_IsStaticClass()
        {
            Type t = FindType("NutriMind.Runtime.App.SafeDiagnostics");
            Assert.That(t, Is.Not.Null);
            Assert.That(t.IsAbstract && t.IsSealed, Is.True, "SafeDiagnostics should be a static class");
        }

        [Test]
        public void SafeDiagnostics_HasRedactMethods()
        {
            Type t = FindType("NutriMind.Runtime.App.SafeDiagnostics");
            Assert.That(t, Is.Not.Null);

            bool hasRedact = false;
            foreach (MethodInfo m in t.GetMethods(BindingFlags.Public | BindingFlags.Static))
            {
                if (m.Name.StartsWith("Redact") && m.ReturnType == typeof(string))
                {
                    ParameterInfo[] ps = m.GetParameters();
                    if (ps.Length >= 1 && ps[0].ParameterType == typeof(string))
                        hasRedact = true;
                }
            }
            Assert.That(hasRedact, Is.True,
                "SafeDiagnostics should have at least one static Redact*(string)->string method");
        }

        [Test]
        public void SafeDiagnostics_Redact_DoesNotThrowOnNull()
        {
            Type t = FindType("NutriMind.Runtime.App.SafeDiagnostics");
            Assert.That(t, Is.Not.Null);

            bool anyTested = false;
            foreach (MethodInfo m in t.GetMethods(BindingFlags.Public | BindingFlags.Static))
            {
                if (m.Name.StartsWith("Redact") && m.ReturnType == typeof(string))
                {
                    ParameterInfo[] ps = m.GetParameters();
                    if (ps.Length >= 1 && ps[0].ParameterType == typeof(string))
                    {
                        Assert.DoesNotThrow(() => m.Invoke(null, new object[] { null }),
                            "{0} must not throw on null input", m.Name);
                        anyTested = true;
                    }
                }
            }
            Assert.That(anyTested, Is.True, "No Redact methods found");
        }

        [Test]
        public void SafeDiagnostics_Redact_ReturnsNonEmptyForToken()
        {
            Type t = FindType("NutriMind.Runtime.App.SafeDiagnostics");
            Assert.That(t, Is.Not.Null);

            MethodInfo m = t.GetMethod("RedactToken", BindingFlags.Public | BindingFlags.Static, null, new[] { typeof(string) }, null);
            if (m == null)
            {
                foreach (MethodInfo mi in t.GetMethods(BindingFlags.Public | BindingFlags.Static))
                {
                    if (mi.Name.StartsWith("Redact") && mi.ReturnType == typeof(string))
                    {
                        ParameterInfo[] ps = mi.GetParameters();
                        if (ps.Length == 1 && ps[0].ParameterType == typeof(string))
                        { m = mi; break; }
                    }
                }
            }

            if (m != null)
            {
                string input = "eyJhbGciOiJIUzI1NiJ9.dGVzdHRva2Vu.test";
                string result = (string)m.Invoke(null, new object[] { input });
                Assert.That(result, Is.Not.Null, "Redact must return non-null");
                Assert.That(result, Is.Not.EqualTo(input), "Redact must alter the sensitive input");
            }
        }

        [Test]
        public void SafeDiagnostics_HasComprehensiveRedactMethod()
        {
            Type t = FindType("NutriMind.Runtime.App.SafeDiagnostics");
            Assert.That(t, Is.Not.Null);

            MethodInfo m = t.GetMethod("RedactAll", BindingFlags.Public | BindingFlags.Static, null, new[] { typeof(string) }, null)
                ?? t.GetMethod("SanitizeDiagnostics", BindingFlags.Public | BindingFlags.Static, null, new[] { typeof(string) }, null);

            if (m != null)
                Assert.That(m.ReturnType, Is.EqualTo(typeof(string)));
            else
                Assert.Ignore("Phase 03 should provide RedactAll or SanitizeDiagnostics");
        }

        [Test]
        public void SafeDiagnostics_HasLogSafeMethod()
        {
            Type t = FindType("NutriMind.Runtime.App.SafeDiagnostics");
            Assert.That(t, Is.Not.Null);

            MethodInfo m = t.GetMethod("LogSafe", BindingFlags.Public | BindingFlags.Static)
                ?? t.GetMethod("LogDiagnostic", BindingFlags.Public | BindingFlags.Static);

            if (m != null)
                Assert.That(m.ReturnType == typeof(void), Is.True, "LogSafe should return void");
        }

        // ---------------------------------------------------------------
        // Phase 03: RedactAll / SanitizeDiagnostics redacts answer-key labels,
        //           stack-trace/file-path snippets in addition to JWT/PIN
        // ---------------------------------------------------------------

        [Test]
        public void RedactAll_RedactsAnswerKeyLabel()
        {
            Type t = FindType("NutriMind.Runtime.App.SafeDiagnostics");
            Assert.That(t, Is.Not.Null,
                "Precondition: SafeDiagnostics type must exist");

            MethodInfo redactAll = t.GetMethod("RedactAll",
                BindingFlags.Public | BindingFlags.Static,
                null, new[] { typeof(string) }, null);

            if (redactAll == null)
            {
                Assert.Ignore("RedactAll method not available");
                return;
            }

            // Input containing an answer_key label -- should be redacted
            string input = "The answer_key is correctAnswer and the user answered correctly.";
            string result = (string)redactAll.Invoke(null, new object[] { input });

            Assert.That(result, Is.Not.Null,
                "RedactAll must return non-null");
            Assert.That(result, Is.Not.EqualTo(input),
                "RedactAll must alter input containing 'answer_key'");
            Assert.That(result.ToLowerInvariant().Contains("answer_key"), Is.False,
                "RedactAll must redact 'answer_key' label from the output");
        }

        [Test]
        public void RedactAll_RedactsCorrectAnswerLabel()
        {
            Type t = FindType("NutriMind.Runtime.App.SafeDiagnostics");
            Assert.That(t, Is.Not.Null,
                "Precondition: SafeDiagnostics type must exist");

            MethodInfo redactAll = t.GetMethod("RedactAll",
                BindingFlags.Public | BindingFlags.Static,
                null, new[] { typeof(string) }, null);

            if (redactAll == null)
            {
                Assert.Ignore("RedactAll method not available");
                return;
            }

            string input = "Submitted: correct_answer = 42, expected correct_answer";
            string result = (string)redactAll.Invoke(null, new object[] { input });

            Assert.That(result, Is.Not.Null,
                "RedactAll must return non-null");
            Assert.That(result, Is.Not.EqualTo(input),
                "RedactAll must alter input containing 'correct_answer'");
            Assert.That(result.ToLowerInvariant().Contains("correct_answer"), Is.False,
                "RedactAll must redact 'correct_answer' label from the output");
        }

        [Test]
        public void RedactAll_RedactsStackTraceSnippet()
        {
            Type t = FindType("NutriMind.Runtime.App.SafeDiagnostics");
            Assert.That(t, Is.Not.Null,
                "Precondition: SafeDiagnostics type must exist");

            MethodInfo redactAll = t.GetMethod("RedactAll",
                BindingFlags.Public | BindingFlags.Static,
                null, new[] { typeof(string) }, null);

            if (redactAll == null)
            {
                Assert.Ignore("RedactAll method not available");
                return;
            }

            // Typical stack-trace snippet
            string input = "at NutriMind.GameLogic.ScoreCalculator.Calculate() in C:\\Projects\\NutriMind\\Assets\\Scripts\\GameLogic.cs:line 42";
            string result = (string)redactAll.Invoke(null, new object[] { input });

            Assert.That(result, Is.Not.Null,
                "RedactAll must return non-null");
            Assert.That(result, Is.Not.EqualTo(input),
                "RedactAll must alter input containing a stack trace");
            Assert.That(result.Contains("GameLogic.cs"), Is.False,
                "RedactAll must redact file-path snippets from stack traces");
        }

        [Test]
        public void RedactAll_RedactsFilePathInStackTrace()
        {
            Type t = FindType("NutriMind.Runtime.App.SafeDiagnostics");
            Assert.That(t, Is.Not.Null,
                "Precondition: SafeDiagnostics type must exist");

            MethodInfo redactAll = t.GetMethod("RedactAll",
                BindingFlags.Public | BindingFlags.Static,
                null, new[] { typeof(string) }, null);

            if (redactAll == null)
            {
                Assert.Ignore("RedactAll method not available");
                return;
            }

            // Input with a file path that looks like a stack-trace file reference
            string input = "Something failed in C:/Users/test/Assets/Scripts/Helper.cs:line 99";
            string result = (string)redactAll.Invoke(null, new object[] { input });

            Assert.That(result, Is.Not.Null,
                "RedactAll must return non-null");
            Assert.That(result, Is.Not.EqualTo(input),
                "RedactAll must alter input containing file paths from stack traces");
            Assert.That(result.Contains("Helper.cs"), Is.False,
                "RedactAll must redact file names from stack-trace-like snippets");
        }

        [Test]
        public void RedactAll_StillRedactsJwtToken()
        {
            Type t = FindType("NutriMind.Runtime.App.SafeDiagnostics");
            Assert.That(t, Is.Not.Null,
                "Precondition: SafeDiagnostics type must exist");

            MethodInfo redactAll = t.GetMethod("RedactAll",
                BindingFlags.Public | BindingFlags.Static,
                null, new[] { typeof(string) }, null);

            if (redactAll == null)
            {
                Assert.Ignore("RedactAll method not available");
                return;
            }

            // JWT-like token that RedactToken should already catch
            string input = "eyJhbGciOiJIUzI1NiJ9.dGVzdHRva2Vu.test";
            string result = (string)redactAll.Invoke(null, new object[] { input });

            Assert.That(result, Is.Not.Null,
                "RedactAll must return non-null");
            Assert.That(result, Is.Not.EqualTo(input),
                "RedactAll must still redact JWT-like tokens");
            Assert.That(result.Contains("REDACTED"), Is.True,
                "RedactAll output should contain a REDACTED placeholder for tokens");
        }

        [Test]
        public void RedactAll_StillRedactsPin()
        {
            Type t = FindType("NutriMind.Runtime.App.SafeDiagnostics");
            Assert.That(t, Is.Not.Null,
                "Precondition: SafeDiagnostics type must exist");

            MethodInfo redactAll = t.GetMethod("RedactAll",
                BindingFlags.Public | BindingFlags.Static,
                null, new[] { typeof(string) }, null);

            if (redactAll == null)
            {
                Assert.Ignore("RedactAll method not available");
                return;
            }

            string input = "My PIN is 1234 and also 98765432";
            string result = (string)redactAll.Invoke(null, new object[] { input });

            Assert.That(result, Is.Not.Null,
                "RedactAll must return non-null");
            Assert.That(result, Is.Not.EqualTo(input),
                "RedactAll must alter input containing PIN-like sequences");
        }

        [Test]
        public void SanitizeDiagnostics_RedactsAnswerKeyLabel()
        {
            Type t = FindType("NutriMind.Runtime.App.SafeDiagnostics");
            Assert.That(t, Is.Not.Null,
                "Precondition: SafeDiagnostics type must exist");

            MethodInfo sanitize = t.GetMethod("SanitizeDiagnostics",
                BindingFlags.Public | BindingFlags.Static,
                null, new[] { typeof(string) }, null);

            if (sanitize == null)
            {
                Assert.Ignore("SanitizeDiagnostics method not available");
                return;
            }

            string input = "answer_key = secretValue";
            string result = (string)sanitize.Invoke(null, new object[] { input });

            Assert.That(result, Is.Not.Null,
                "SanitizeDiagnostics must return non-null");
            Assert.That(result.ToLowerInvariant().Contains("answer_key"), Is.False,
                "SanitizeDiagnostics must redact 'answer_key' label");
        }

        [Test]
        public void SanitizeDiagnostics_RedactsCorrectAnswerLabel()
        {
            Type t = FindType("NutriMind.Runtime.App.SafeDiagnostics");
            Assert.That(t, Is.Not.Null,
                "Precondition: SafeDiagnostics type must exist");

            MethodInfo sanitize = t.GetMethod("SanitizeDiagnostics",
                BindingFlags.Public | BindingFlags.Static,
                null, new[] { typeof(string) }, null);

            if (sanitize == null)
            {
                Assert.Ignore("SanitizeDiagnostics method not available");
                return;
            }

            string input = "correct_answer = B";
            string result = (string)sanitize.Invoke(null, new object[] { input });

            Assert.That(result, Is.Not.Null,
                "SanitizeDiagnostics must return non-null");
            Assert.That(result.ToLowerInvariant().Contains("correct_answer"), Is.False,
                "SanitizeDiagnostics must redact 'correct_answer' label");
        }

        // ---------------------------------------------------------------
        // Answer VALUE redaction — both the label AND the associated
        // sensitive value must be removed from sanitized output.
        // Currently only labels are redacted; values leak through.
        // These tests expose the gap (RED).
        // ---------------------------------------------------------------

        [Test]
        public void RedactAll_RedactsCorrectAnswerValue_IntegerExample()
        {
            Type t = FindType("NutriMind.Runtime.App.SafeDiagnostics");
            Assert.That(t, Is.Not.Null,
                "Precondition: SafeDiagnostics type must exist");

            MethodInfo redactAll = t.GetMethod("RedactAll",
                BindingFlags.Public | BindingFlags.Static,
                null, new[] { typeof(string) }, null);

            if (redactAll == null)
            {
                Assert.Ignore("RedactAll method not available");
                return;
            }

            // Example: correct_answer = 42
            // Both the label "correct_answer" and the value "42" must be redacted.
            string input = "correct_answer = 42";
            string result = (string)redactAll.Invoke(null, new object[] { input });

            Assert.That(result, Is.Not.Null,
                "RedactAll must return non-null");
            Assert.That(result.ToLowerInvariant().Contains("correct_answer"), Is.False,
                "RedactAll must redact 'correct_answer' label");
            Assert.That(result.Contains("42"), Is.False,
                "RedactAll must redact the value '42' associated with correct_answer");
        }

        [Test]
        public void RedactAll_RedactsAnswerKeyValue_AlphanumericExample()
        {
            Type t = FindType("NutriMind.Runtime.App.SafeDiagnostics");
            Assert.That(t, Is.Not.Null,
                "Precondition: SafeDiagnostics type must exist");

            MethodInfo redactAll = t.GetMethod("RedactAll",
                BindingFlags.Public | BindingFlags.Static,
                null, new[] { typeof(string) }, null);

            if (redactAll == null)
            {
                Assert.Ignore("RedactAll method not available");
                return;
            }

            // Example: answer_key: "B12C"
            // Both the label "answer_key" and the value "B12C" must be redacted.
            string input = "answer_key: \"B12C\"";
            string result = (string)redactAll.Invoke(null, new object[] { input });

            Assert.That(result, Is.Not.Null,
                "RedactAll must return non-null");
            Assert.That(result.ToLowerInvariant().Contains("answer_key"), Is.False,
                "RedactAll must redact 'answer_key' label");
            Assert.That(result.Contains("B12C"), Is.False,
                "RedactAll must redact the value 'B12C' associated with answer_key");
        }

        [Test]
        public void RedactAll_RedactsCorrectAnswerValue_JsonExample()
        {
            Type t = FindType("NutriMind.Runtime.App.SafeDiagnostics");
            Assert.That(t, Is.Not.Null,
                "Precondition: SafeDiagnostics type must exist");

            MethodInfo redactAll = t.GetMethod("RedactAll",
                BindingFlags.Public | BindingFlags.Static,
                null, new[] { typeof(string) }, null);

            if (redactAll == null)
            {
                Assert.Ignore("RedactAll method not available");
                return;
            }

            // Example: "correct_answer":"Paris"
            // Both the label "correct_answer" and the value "Paris" must be redacted.
            string input = "\"correct_answer\":\"Paris\"";
            string result = (string)redactAll.Invoke(null, new object[] { input });

            Assert.That(result, Is.Not.Null,
                "RedactAll must return non-null");
            Assert.That(result.ToLowerInvariant().Contains("correct_answer"), Is.False,
                "RedactAll must redact 'correct_answer' label");
            Assert.That(result.Contains("Paris"), Is.False,
                "RedactAll must redact the value 'Paris' associated with correct_answer");
        }

        [Test]
        public void SanitizeDiagnostics_RedactsCorrectAnswerValue_IntegerExample()
        {
            Type t = FindType("NutriMind.Runtime.App.SafeDiagnostics");
            Assert.That(t, Is.Not.Null,
                "Precondition: SafeDiagnostics type must exist");

            MethodInfo sanitize = t.GetMethod("SanitizeDiagnostics",
                BindingFlags.Public | BindingFlags.Static,
                null, new[] { typeof(string) }, null);

            if (sanitize == null)
            {
                Assert.Ignore("SanitizeDiagnostics method not available");
                return;
            }

            string input = "correct_answer = 42";
            string result = (string)sanitize.Invoke(null, new object[] { input });

            Assert.That(result, Is.Not.Null,
                "SanitizeDiagnostics must return non-null");
            Assert.That(result.ToLowerInvariant().Contains("correct_answer"), Is.False,
                "SanitizeDiagnostics must redact 'correct_answer' label");
            Assert.That(result.Contains("42"), Is.False,
                "SanitizeDiagnostics must redact the value '42' associated with correct_answer");
        }

        [Test]
        public void SanitizeDiagnostics_RedactsAnswerKeyValue_AlphanumericExample()
        {
            Type t = FindType("NutriMind.Runtime.App.SafeDiagnostics");
            Assert.That(t, Is.Not.Null,
                "Precondition: SafeDiagnostics type must exist");

            MethodInfo sanitize = t.GetMethod("SanitizeDiagnostics",
                BindingFlags.Public | BindingFlags.Static,
                null, new[] { typeof(string) }, null);

            if (sanitize == null)
            {
                Assert.Ignore("SanitizeDiagnostics method not available");
                return;
            }

            string input = "answer_key: \"B12C\"";
            string result = (string)sanitize.Invoke(null, new object[] { input });

            Assert.That(result, Is.Not.Null,
                "SanitizeDiagnostics must return non-null");
            Assert.That(result.ToLowerInvariant().Contains("answer_key"), Is.False,
                "SanitizeDiagnostics must redact 'answer_key' label");
            Assert.That(result.Contains("B12C"), Is.False,
                "SanitizeDiagnostics must redact the value 'B12C' associated with answer_key");
        }

        [Test]
        public void SanitizeDiagnostics_RedactsCorrectAnswerValue_JsonExample()
        {
            Type t = FindType("NutriMind.Runtime.App.SafeDiagnostics");
            Assert.That(t, Is.Not.Null,
                "Precondition: SafeDiagnostics type must exist");

            MethodInfo sanitize = t.GetMethod("SanitizeDiagnostics",
                BindingFlags.Public | BindingFlags.Static,
                null, new[] { typeof(string) }, null);

            if (sanitize == null)
            {
                Assert.Ignore("SanitizeDiagnostics method not available");
                return;
            }

            string input = "\"correct_answer\":\"Paris\"";
            string result = (string)sanitize.Invoke(null, new object[] { input });

            Assert.That(result, Is.Not.Null,
                "SanitizeDiagnostics must return non-null");
            Assert.That(result.ToLowerInvariant().Contains("correct_answer"), Is.False,
                "SanitizeDiagnostics must redact 'correct_answer' label");
            Assert.That(result.Contains("Paris"), Is.False,
                "SanitizeDiagnostics must redact the value 'Paris' associated with correct_answer");
        }

        [Test]
        public void SanitizeDiagnostics_RedactsStackTraceSnippet()
        {
            Type t = FindType("NutriMind.Runtime.App.SafeDiagnostics");
            Assert.That(t, Is.Not.Null,
                "Precondition: SafeDiagnostics type must exist");

            MethodInfo sanitize = t.GetMethod("SanitizeDiagnostics",
                BindingFlags.Public | BindingFlags.Static,
                null, new[] { typeof(string) }, null);

            if (sanitize == null)
            {
                Assert.Ignore("SanitizeDiagnostics method not available");
                return;
            }

            string input = "at SomeClass.Method() in E:\\Projects\\App\\Model.cs:line 77";
            string result = (string)sanitize.Invoke(null, new object[] { input });

            Assert.That(result, Is.Not.Null,
                "SanitizeDiagnostics must return non-null");
            Assert.That(result.Contains("Model.cs"), Is.False,
                "SanitizeDiagnostics must redact file-path snippets from stack traces");
        }

        // ---------------------------------------------------------------
        // Multi-word answer VALUE redaction — the current regex
        // [^\s,""}] + captures only the first token.  A quoted value
        // containing spaces (e.g. "New York" or "Fact or Opinion")
        // leaks every token after the first.  (RED — these prove the gap.)
        // ---------------------------------------------------------------

        [Test]
        public void RedactAll_RedactsMultiWordQuotedAnswer_JsonStyle()
        {
            Type t = FindType("NutriMind.Runtime.App.SafeDiagnostics");
            Assert.That(t, Is.Not.Null,
                "Precondition: SafeDiagnostics type must exist");

            MethodInfo redactAll = t.GetMethod("RedactAll",
                BindingFlags.Public | BindingFlags.Static,
                null, new[] { typeof(string) }, null);

            if (redactAll == null)
            {
                Assert.Ignore("RedactAll method not available");
                return;
            }

            // JSON-style value with a space in the quoted answer.
            //   "[correct_answer]":"[New] York"   ← only "New" is captured
            string input = "\"correct_answer\":\"New York\"";
            string result = (string)redactAll.Invoke(null, new object[] { input });

            Assert.That(result, Is.Not.Null,
                "RedactAll must return non-null");
            Assert.That(result.ToLowerInvariant().Contains("correct_answer"), Is.False,
                "RedactAll must redact 'correct_answer' label");
            // RED — the trailing word 'York' leaks past the first-token capture
            Assert.That(result.Contains("York"), Is.False,
                "RedactAll must redact the trailing word 'York' in a multi-word quoted answer value");
        }

        [Test]
        public void RedactAll_RedactsMultiWordAnswerKey_EqualsStyle()
        {
            Type t = FindType("NutriMind.Runtime.App.SafeDiagnostics");
            Assert.That(t, Is.Not.Null,
                "Precondition: SafeDiagnostics type must exist");

            MethodInfo redactAll = t.GetMethod("RedactAll",
                BindingFlags.Public | BindingFlags.Static,
                null, new[] { typeof(string) }, null);

            if (redactAll == null)
            {
                Assert.Ignore("RedactAll method not available");
                return;
            }

            // Equals-style with a space-separated multi-word value.
            //   answer_key = "[Fact] or Opinion"   ← only "Fact" is captured
            string input = "answer_key = \"Fact or Opinion\"";
            string result = (string)redactAll.Invoke(null, new object[] { input });

            Assert.That(result, Is.Not.Null,
                "RedactAll must return non-null");
            Assert.That(result.ToLowerInvariant().Contains("answer_key"), Is.False,
                "RedactAll must redact 'answer_key' label");
            // RED — 'Opinion' leaks (only 'Fact' is captured before the space)
            Assert.That(result.Contains("Opinion"), Is.False,
                "RedactAll must redact the trailing word 'Opinion' in a multi-word answer value");
        }

        // ---------------------------------------------------------------
        // Stack-trace path redaction with hyphens and spaces in directory
        // names.  The current regex uses \w+ which does not match '-'
        // or ' ', so any directory containing those characters breaks
        // the match entirely.  (RED.)
        // ---------------------------------------------------------------

        [Test]
        public void RedactAll_RedactsHyphenatedDir_InStackTrace()
        {
            Type t = FindType("NutriMind.Runtime.App.SafeDiagnostics");
            Assert.That(t, Is.Not.Null,
                "Precondition: SafeDiagnostics type must exist");

            MethodInfo redactAll = t.GetMethod("RedactAll",
                BindingFlags.Public | BindingFlags.Static,
                null, new[] { typeof(string) }, null);

            if (redactAll == null)
            {
                Assert.Ignore("RedactAll method not available");
                return;
            }

            // \w+ stops at the hyphen in 'Dev-Env' — entire match fails.
            string input = "at Helper.Run() in C:\\Projects\\Dev-Env\\Sub\\Module.cs:line 88";
            string result = (string)redactAll.Invoke(null, new object[] { input });

            Assert.That(result, Is.Not.Null,
                "RedactAll must return non-null");
            Assert.That(result.Contains("Dev-Env"), Is.False,
                "RedactAll must redact hyphenated directory names like 'Dev-Env' from stack traces");
            Assert.That(result.Contains("Module.cs"), Is.False,
                "RedactAll must redact file name when the path contains hyphenated directory segments");
        }

        [Test]
        public void RedactAll_RedactsSpacedDir_InStackTrace()
        {
            Type t = FindType("NutriMind.Runtime.App.SafeDiagnostics");
            Assert.That(t, Is.Not.Null,
                "Precondition: SafeDiagnostics type must exist");

            MethodInfo redactAll = t.GetMethod("RedactAll",
                BindingFlags.Public | BindingFlags.Static,
                null, new[] { typeof(string) }, null);

            if (redactAll == null)
            {
                Assert.Ignore("RedactAll method not available");
                return;
            }

            // \w+ stops at the space after 'NutriMind' — entire match fails.
            string input = "at Worker.Execute() in C:\\My Project\\NutriMind Game\\Scripts\\Runner.cs:line 22";
            string result = (string)redactAll.Invoke(null, new object[] { input });

            Assert.That(result, Is.Not.Null,
                "RedactAll must return non-null");
            Assert.That(result.Contains("NutriMind Game"), Is.False,
                "RedactAll must redact directory names containing spaces from stack traces");
            Assert.That(result.Contains("Runner.cs"), Is.False,
                "RedactAll must redact file name when the path contains spaced directory segments");
        }

        [Test]
        public void RedactAll_RedactsFullRepoPath_InStackTrace()
        {
            Type t = FindType("NutriMind.Runtime.App.SafeDiagnostics");
            Assert.That(t, Is.Not.Null,
                "Precondition: SafeDiagnostics type must exist");

            MethodInfo redactAll = t.GetMethod("RedactAll",
                BindingFlags.Public | BindingFlags.Static,
                null, new[] { typeof(string) }, null);

            if (redactAll == null)
            {
                Assert.Ignore("RedactAll method not available");
                return;
            }

            // Exact repo path — contains both hyphens AND spaces.
            string input = "at SafeDiagnostics.RedactAll() in C:\\Users\\Kingk\\Dev-Environment\\Capstone-Project\\NutriMind Educational Game\\Assets\\_Project\\Nutrimind\\Runtime\\App\\SafeDiagnostics.cs:line 42";
            string result = (string)redactAll.Invoke(null, new object[] { input });

            Assert.That(result, Is.Not.Null,
                "RedactAll must return non-null");
            Assert.That(result.Contains("Dev-Environment"), Is.False,
                "RedactAll must redact 'Dev-Environment' from the repo path in stack traces");
            Assert.That(result.Contains("Capstone-Project"), Is.False,
                "RedactAll must redact 'Capstone-Project' from the repo path in stack traces");
            Assert.That(result.Contains("NutriMind Educational Game"), Is.False,
                "RedactAll must redact 'NutriMind Educational Game' from the repo path in stack traces");
        }

        [Test]
        public void SanitizeDiagnostics_RedactsMultiWordQuotedAnswer_JsonStyle()
        {
            Type t = FindType("NutriMind.Runtime.App.SafeDiagnostics");
            Assert.That(t, Is.Not.Null,
                "Precondition: SafeDiagnostics type must exist");

            MethodInfo sanitize = t.GetMethod("SanitizeDiagnostics",
                BindingFlags.Public | BindingFlags.Static,
                null, new[] { typeof(string) }, null);

            if (sanitize == null)
            {
                Assert.Ignore("SanitizeDiagnostics method not available");
                return;
            }

            string input = "\"correct_answer\":\"New York\"";
            string result = (string)sanitize.Invoke(null, new object[] { input });

            Assert.That(result, Is.Not.Null,
                "SanitizeDiagnostics must return non-null");
            Assert.That(result.ToLowerInvariant().Contains("correct_answer"), Is.False,
                "SanitizeDiagnostics must redact 'correct_answer' label");
            Assert.That(result.Contains("York"), Is.False,
                "SanitizeDiagnostics must redact the trailing word 'York' in a multi-word quoted answer value");
        }
    }
}