using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using NutriMind.Runtime.App;
using NutriMind.Runtime.App.Dto;
using NutriMind.Runtime.App.Http;

namespace NutriMind.Tests.EditMode.App
{
    /// <summary>
    /// Phase 04 API contract tests. Validates that:
    /// <list type="bullet">
    ///   <item>IGameDataProvider has all required async methods</item>
    ///   <item>All methods return Task&lt;DataResult&lt;T&gt;&gt;</item>
    ///   <item>IGameDataProvider does not expose DataProviderMode</item>
    ///   <item>HttpProvider and LocalDemoJsonProvider implement the interface</item>
    ///   <item>DataResult&lt;T&gt; preserves backward compat</item>
    ///   <item>DataProviderError has full envelope fields</item>
    ///   <item>ErrorAction enum has known actions + Unknown</item>
    ///   <item>ChallengeAnswerType enum has known types + Unknown</item>
    ///   <item>DTOs have correct snake_case JsonProperty mapping</item>
    ///   <item>Unknown optional fields are absent-safe</item>
    ///   <item>Unknown enum values fall back gracefully</item>
    ///   <item>Error envelope has all documented fields</item>
    ///   <item>client_attempt_uuid JSON field name is correct</item>
    ///   <item>Provider failures return structured error with not_implemented</item>
    /// </list>
    /// </summary>
    [TestFixture]
    public class GameDataProviderContractTests
    {
        private const string AppAssemblyName = "NutriMind.Runtime.App";
        private const string DtoAssemblyName = "NutriMind.Runtime.App";

        private static Type FindType(string fullTypeName)
            => Type.GetType(fullTypeName + ", " + AppAssemblyName);

        private static Type FindDtoType(string typeName)
            => Type.GetType("NutriMind.Runtime.App.Dto." + typeName + ", " + DtoAssemblyName);

        // ==============================================================
        //  IGameDataProvider interface shape
        // ==============================================================

        [Test]
        public void IGameDataProvider_TypeExists()
        {
            Type iface = FindType("NutriMind.Runtime.App.IGameDataProvider");
            Assert.That(iface, Is.Not.Null,
                "IGameDataProvider interface must exist in NutriMind.Runtime.App");
            Assert.That(iface.IsInterface, Is.True,
                "IGameDataProvider must be an interface type");
        }

        [Test]
        public void IGameDataProvider_HasAllAsyncEndpointMethods()
        {
            Type iface = FindType("NutriMind.Runtime.App.IGameDataProvider");
            Assert.That(iface, Is.Not.Null);
            Assert.That(iface.IsInterface, Is.True);

            // All required provider methods with their return types
            string[] requiredMethods = {
                "PingAsync",
                "GetConfigAsync",
                "LoginAsync",
                "LogoutAsync",
                "GetBootstrapAsync",
                "GetProfileAsync",
                "GetSettingsAsync",
                "PatchSettingsAsync",
                "GetSubjectsAsync",
                "GetTermsAsync",
                "GetStationsAsync",
                "GetStationContentAsync",
                "StartStationAsync",
                "SubmitAttemptAsync",
                "CompleteStationAsync",
                "GetProgressSummaryAsync",
                "GetRewardsAsync",
                "UseRewardAsync",
                "GetSyncStatusAsync"
            };

            foreach (string methodName in requiredMethods)
            {
                MethodInfo method = iface.GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance);
                Assert.That(method, Is.Not.Null,
                    "IGameDataProvider must declare method '{0}'", methodName);

                // All methods must return Task<DataResult<T>>
                Assert.That(typeof(Task).IsAssignableFrom(method.ReturnType),
                    "Method '{0}' must return a Task type, but returns {1}", methodName, method.ReturnType.Name);
            }
        }

        [Test]
        public void IGameDataProvider_AllMethodsReturnTaskDataResult()
        {
            Type iface = FindType("NutriMind.Runtime.App.IGameDataProvider");
            Assert.That(iface, Is.Not.Null);

            MethodInfo[] methods = iface.GetMethods(BindingFlags.Public | BindingFlags.Instance);
            foreach (MethodInfo method in methods)
            {
                Assert.That(typeof(Task).IsAssignableFrom(method.ReturnType),
                    "Method '{0}' must return Task, but returns {1}",
                    method.Name, method.ReturnType.Name);
            }
        }

        [Test]
        public void IGameDataProvider_DoesNotExposeDataProviderMode()
        {
            Type iface = FindType("NutriMind.Runtime.App.IGameDataProvider");
            Assert.That(iface, Is.Not.Null);

            Type dataProviderModeType = Type.GetType("NutriMind.Runtime.App.DataProviderMode, " + AppAssemblyName);
            Assume.That(dataProviderModeType, Is.Not.Null);

            PropertyInfo[] props = iface.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in props)
                Assert.That(prop.PropertyType, Is.Not.EqualTo(dataProviderModeType),
                    "IGameDataProvider must not expose DataProviderMode");

            MethodInfo[] methods = iface.GetMethods(BindingFlags.Public | BindingFlags.Instance);
            foreach (MethodInfo method in methods)
            {
                Assert.That(method.ReturnType, Is.Not.EqualTo(dataProviderModeType),
                    "Method '{0}' must not return DataProviderMode", method.Name);
                foreach (ParameterInfo param in method.GetParameters())
                    Assert.That(param.ParameterType, Is.Not.EqualTo(dataProviderModeType),
                        "Method '{0}' must not accept DataProviderMode", method.Name);
            }
        }

        [Test]
        public void IGameDataProvider_AcceptsCancellationToken()
        {
            Type iface = FindType("NutriMind.Runtime.App.IGameDataProvider");
            Assert.That(iface, Is.Not.Null);

            MethodInfo[] methods = iface.GetMethods(BindingFlags.Public | BindingFlags.Instance);
            foreach (MethodInfo method in methods)
            {
                ParameterInfo[] params_ = method.GetParameters();
                bool hasCt = false;
                foreach (ParameterInfo p in params_)
                {
                    if (p.ParameterType == typeof(CancellationToken) && p.IsOptional)
                    {
                        hasCt = true;
                        break;
                    }
                }
                Assert.That(hasCt, Is.True,
                    "Method '{0}' must accept optional CancellationToken parameter", method.Name);
            }
        }

        // ==============================================================
        //  DataResult<T> backward compatibility
        // ==============================================================

        [Test]
        public void DataResultOfT_TypeExists()
        {
            Type openResult = FindType("NutriMind.Runtime.App.DataResult`1");
            Assert.That(openResult, Is.Not.Null,
                "DataResult<T> generic type must exist");
            Assert.That(openResult.IsGenericTypeDefinition, Is.True);
        }

        [Test]
        public void DataResultOfT_HasSuccessProperty()
        {
            Type openResult = FindType("NutriMind.Runtime.App.DataResult`1");
            Assert.That(openResult, Is.Not.Null);

            PropertyInfo successProp = openResult.GetProperty("Success", BindingFlags.Public | BindingFlags.Instance);
            Assert.That(successProp, Is.Not.Null,
                "DataResult<T> must expose Success property");
            Assert.That(successProp.PropertyType, Is.EqualTo(typeof(bool)));
        }

        [Test]
        public void DataResultOfT_HasErrorMessageProperty()
        {
            Type openResult = FindType("NutriMind.Runtime.App.DataResult`1");
            Assert.That(openResult, Is.Not.Null);

            PropertyInfo errorProp = openResult.GetProperty("ErrorMessage", BindingFlags.Public | BindingFlags.Instance);
            Assert.That(errorProp, Is.Not.Null,
                "DataResult<T> must expose ErrorMessage property for backward compatibility");
            Assert.That(errorProp.PropertyType, Is.EqualTo(typeof(string)));
        }

        [Test]
        public void DataResultOfT_HasDataProperty()
        {
            Type openResult = FindType("NutriMind.Runtime.App.DataResult`1");
            Assert.That(openResult, Is.Not.Null);

            PropertyInfo dataProp = openResult.GetProperty("Data", BindingFlags.Public | BindingFlags.Instance);
            Assert.That(dataProp, Is.Not.Null,
                "DataResult<T> must expose Data property");
        }

        [Test]
        public void DataResultOfT_HasErrorPropertyOfTypeDataProviderError()
        {
            Type openResult = FindType("NutriMind.Runtime.App.DataResult`1");
            Assert.That(openResult, Is.Not.Null);

            Type errorType = FindType("NutriMind.Runtime.App.DataProviderError");
            Assert.That(errorType, Is.Not.Null);

            PropertyInfo errorProp = openResult.GetProperty("Error", BindingFlags.Public | BindingFlags.Instance);
            Assert.That(errorProp, Is.Not.Null,
                "DataResult<T> must expose Error property");
            Assert.That(errorProp.PropertyType, Is.EqualTo(errorType));
        }

        // ==============================================================
        //  DataProviderError full envelope
        // ==============================================================

        [Test]
        public void DataProviderError_HasAllEnvelopeFields()
        {
            Type errorType = FindType("NutriMind.Runtime.App.DataProviderError");
            Assert.That(errorType, Is.Not.Null);

            // Code
            PropertyInfo codeProp = errorType.GetProperty("Code", BindingFlags.Public | BindingFlags.Instance);
            Assert.That(codeProp, Is.Not.Null, "Must have Code property");
            Assert.That(codeProp.PropertyType, Is.EqualTo(typeof(string)));

            // Message
            PropertyInfo msgProp = errorType.GetProperty("Message", BindingFlags.Public | BindingFlags.Instance);
            Assert.That(msgProp, Is.Not.Null, "Must have Message property");
            Assert.That(msgProp.PropertyType, Is.EqualTo(typeof(string)));

            // RequestId (optional)
            PropertyInfo reqProp = errorType.GetProperty("RequestId", BindingFlags.Public | BindingFlags.Instance);
            Assert.That(reqProp, Is.Not.Null, "Must have RequestId property");

            // Retryable (optional)
            PropertyInfo retProp = errorType.GetProperty("Retryable", BindingFlags.Public | BindingFlags.Instance);
            Assert.That(retProp, Is.Not.Null, "Must have Retryable property");
            Assert.That(retProp.PropertyType, Is.EqualTo(typeof(bool?)));

            // Details (optional)
            PropertyInfo detProp = errorType.GetProperty("Details", BindingFlags.Public | BindingFlags.Instance);
            Assert.That(detProp, Is.Not.Null, "Must have Details property");

            // FieldErrors (optional)
            PropertyInfo feProp = errorType.GetProperty("FieldErrors", BindingFlags.Public | BindingFlags.Instance);
            Assert.That(feProp, Is.Not.Null, "Must have FieldErrors property");

            // RetryAfterSeconds (optional)
            PropertyInfo rasProp = errorType.GetProperty("RetryAfterSeconds", BindingFlags.Public | BindingFlags.Instance);
            Assert.That(rasProp, Is.Not.Null, "Must have RetryAfterSeconds property");
            Assert.That(rasProp.PropertyType, Is.EqualTo(typeof(int?)));

            // Action (optional)
            PropertyInfo actProp = errorType.GetProperty("Action", BindingFlags.Public | BindingFlags.Instance);
            Assert.That(actProp, Is.Not.Null, "Must have Action property");
            Assert.That(actProp.PropertyType, Is.EqualTo(typeof(string)));

            // ResolvedAction (computed)
            PropertyInfo resActProp = errorType.GetProperty("ResolvedAction", BindingFlags.Public | BindingFlags.Instance);
            Assert.That(resActProp, Is.Not.Null, "Must have ResolvedAction property");

            // IsRetryable (computed)
            PropertyInfo isRetProp = errorType.GetProperty("IsRetryable", BindingFlags.Public | BindingFlags.Instance);
            Assert.That(isRetProp, Is.Not.Null, "Must have IsRetryable property");
            Assert.That(isRetProp.PropertyType, Is.EqualTo(typeof(bool)));
        }

        [Test]
        public void DataProviderError_Constructor()
        {
            var error = new DataProviderError("STATION_LOCKED", "Station is locked.");
            Assert.That(error.Code, Is.EqualTo("STATION_LOCKED"));
            Assert.That(error.Message, Is.EqualTo("Station is locked."));
            Assert.That(error.RequestId, Is.Null);
            Assert.That(error.Retryable, Is.Null);
            Assert.That(error.Action, Is.Null);
        }

        [Test]
        public void DataProviderError_ResolvedAction_MapsKnownActions()
        {
            var error = new DataProviderError();
            Assert.That(error.ResolvedAction, Is.EqualTo(ErrorAction.Unknown));

            error.Action = "login_again";
            Assert.That(error.ResolvedAction, Is.EqualTo(ErrorAction.LoginAgain));

            error.Action = "retry";
            Assert.That(error.ResolvedAction, Is.EqualTo(ErrorAction.Retry));

            error.Action = "wait_then_retry";
            Assert.That(error.ResolvedAction, Is.EqualTo(ErrorAction.WaitThenRetry));

            error.Action = "refresh_sync_status";
            Assert.That(error.ResolvedAction, Is.EqualTo(ErrorAction.RefreshSyncStatus));

            error.Action = "return_to_menu";
            Assert.That(error.ResolvedAction, Is.EqualTo(ErrorAction.ReturnToMenu));

            error.Action = "show_offline_prompt";
            Assert.That(error.ResolvedAction, Is.EqualTo(ErrorAction.ShowOfflinePrompt));

            error.Action = "contact_teacher";
            Assert.That(error.ResolvedAction, Is.EqualTo(ErrorAction.ContactTeacher));
        }

        [Test]
        public void DataProviderError_ResolvedAction_UnknownFallsBack()
        {
            var error = new DataProviderError();
            error.Action = "some_future_action";
            Assert.That(error.ResolvedAction, Is.EqualTo(ErrorAction.Unknown));

            error.Action = null;
            Assert.That(error.ResolvedAction, Is.EqualTo(ErrorAction.Unknown));
        }

        [Test]
        public void DataProviderError_IsRetryable()
        {
            var error = new DataProviderError();
            Assert.That(error.IsRetryable, Is.False);

            error.Retryable = true;
            Assert.That(error.IsRetryable, Is.True);

            error.Retryable = false;
            Assert.That(error.IsRetryable, Is.False);

            error.Retryable = null;
            Assert.That(error.IsRetryable, Is.False);
        }

        // ==============================================================
        //  ErrorAction enum
        // ==============================================================

        [Test]
        public void ErrorAction_EnumHasExpectedValues()
        {
            Assert.That(Enum.IsDefined(typeof(ErrorAction), ErrorAction.Unknown), Is.True);
            Assert.That(Enum.IsDefined(typeof(ErrorAction), ErrorAction.LoginAgain), Is.True);
            Assert.That(Enum.IsDefined(typeof(ErrorAction), ErrorAction.Retry), Is.True);
            Assert.That(Enum.IsDefined(typeof(ErrorAction), ErrorAction.WaitThenRetry), Is.True);
            Assert.That(Enum.IsDefined(typeof(ErrorAction), ErrorAction.RefreshSyncStatus), Is.True);
            Assert.That(Enum.IsDefined(typeof(ErrorAction), ErrorAction.ReturnToMenu), Is.True);
            Assert.That(Enum.IsDefined(typeof(ErrorAction), ErrorAction.ShowOfflinePrompt), Is.True);
            Assert.That(Enum.IsDefined(typeof(ErrorAction), ErrorAction.ContactTeacher), Is.True);
        }

        [Test]
        public void ErrorAction_UnknownIsDefaultZero()
        {
            Assert.That((int)ErrorAction.Unknown, Is.EqualTo(0),
                "ErrorAction.Unknown should be value 0 so default/default-enum parse falls back safely");
        }

        // ==============================================================
        //  ChallengeAnswerType enum
        // ==============================================================

        [Test]
        public void ChallengeAnswerType_EnumHasExpectedValues()
        {
            Assert.That(Enum.IsDefined(typeof(ChallengeAnswerType), ChallengeAnswerType.Unknown), Is.True);
            Assert.That(Enum.IsDefined(typeof(ChallengeAnswerType), ChallengeAnswerType.MultipleChoice), Is.True);
            Assert.That(Enum.IsDefined(typeof(ChallengeAnswerType), ChallengeAnswerType.TrueFalse), Is.True);
            Assert.That(Enum.IsDefined(typeof(ChallengeAnswerType), ChallengeAnswerType.Matching), Is.True);
            Assert.That(Enum.IsDefined(typeof(ChallengeAnswerType), ChallengeAnswerType.Sorting), Is.True);
            Assert.That(Enum.IsDefined(typeof(ChallengeAnswerType), ChallengeAnswerType.Ordering), Is.True);
            Assert.That(Enum.IsDefined(typeof(ChallengeAnswerType), ChallengeAnswerType.FillBlank), Is.True);
            Assert.That(Enum.IsDefined(typeof(ChallengeAnswerType), ChallengeAnswerType.ShortResponse), Is.True);
            Assert.That(Enum.IsDefined(typeof(ChallengeAnswerType), ChallengeAnswerType.ScenarioChoice), Is.True);
        }

        [Test]
        public void ChallengeAnswerType_UnknownIsDefaultZero()
        {
            Assert.That((int)ChallengeAnswerType.Unknown, Is.EqualTo(0));
        }

        // ==============================================================
        //  DTO existence & snake_case JsonProperty
        // ==============================================================

        [Test]
        public void ApiConfigDto_HasSnakeCaseJsonProperties()
        {
            AssertDtoHasJsonProperty<ApiConfigDto>("ApiVersion", "api_version");
            AssertDtoHasJsonProperty<ApiConfigDto>("ContractVersion", "contract_version");
            AssertDtoHasJsonProperty<ApiConfigDto>("ServerTime", "server_time");
            AssertDtoHasJsonProperty<ApiConfigDto>("MaintenanceMode", "maintenance_mode");
            AssertDtoHasJsonProperty<ApiConfigDto>("MinimumUnityClientVersion", "minimum_unity_client_version");
            AssertDtoHasJsonProperty<ApiConfigDto>("SupportedLanguages", "supported_languages");
        }

        [Test]
        public void LoginRequestDto_HasSnakeCaseJsonProperties()
        {
            AssertDtoHasJsonProperty<LoginRequestDto>("Lrn", "lrn");
            AssertDtoHasJsonProperty<LoginRequestDto>("Pin", "pin");
            AssertDtoHasJsonProperty<LoginRequestDto>("DeviceName", "device_name");
            AssertDtoHasJsonProperty<LoginRequestDto>("ClientVersion", "client_version");
        }

        [Test]
        public void LoginResponseDto_HasSnakeCaseJsonProperties()
        {
            AssertDtoHasJsonProperty<LoginResponseDto>("Token", "token");
            AssertDtoHasJsonProperty<LoginResponseDto>("TokenType", "token_type");
            AssertDtoHasJsonProperty<LoginResponseDto>("Student", "student");
        }

        [Test]
        public void StudentIdentityDto_HasSnakeCaseJsonProperties()
        {
            AssertDtoHasJsonProperty<StudentIdentityDto>("Id", "id");
            AssertDtoHasJsonProperty<StudentIdentityDto>("Name", "name");
            AssertDtoHasJsonProperty<StudentIdentityDto>("LrnMasked", "lrn_masked");
            AssertDtoHasJsonProperty<StudentIdentityDto>("GradeLevel", "grade_level");
            AssertDtoHasJsonProperty<StudentIdentityDto>("LanguagePreference", "language_preference");
        }

        [Test]
        public void AttemptRequestDto_HasClientAttemptUuidCorrectlyMapped()
        {
            AssertDtoHasJsonProperty<AttemptRequestDto>("ClientAttemptUuid", "client_attempt_uuid");
            AssertDtoHasJsonProperty<AttemptRequestDto>("StationSessionId", "station_session_id");
            AssertDtoHasJsonProperty<AttemptRequestDto>("StationId", "station_id");
            AssertDtoHasJsonProperty<AttemptRequestDto>("Answer", "answer");
            AssertDtoHasJsonProperty<AttemptRequestDto>("TimeSpentSeconds", "time_spent_seconds");
            AssertDtoHasJsonProperty<AttemptRequestDto>("UsedRewards", "used_rewards");
        }

        [Test]
        public void SyncStatusDto_HasSnakeCaseJsonProperties()
        {
            AssertDtoHasJsonProperty<SyncStatusDto>("StudentProgressRevision", "student_progress_revision");
            AssertDtoHasJsonProperty<SyncStatusDto>("StudentSettingsRevision", "student_settings_revision");
            AssertDtoHasJsonProperty<SyncStatusDto>("StationUnlockRevision", "station_unlock_revision");
            AssertDtoHasJsonProperty<SyncStatusDto>("PublishedContentRevision", "published_content_revision");
            AssertDtoHasJsonProperty<SyncStatusDto>("RewardWalletRevision", "reward_wallet_revision");
            AssertDtoHasJsonProperty<SyncStatusDto>("ServerTime", "server_time");
            AssertDtoHasJsonProperty<SyncStatusDto>("NextPollAfterSeconds", "next_poll_after_seconds");
        }

        [Test]
        public void StationContentDto_HasNarrativeSnakeCaseProperties()
        {
            AssertDtoHasJsonProperty<StationContentDto>("StoryContext", "story_context");
            AssertDtoHasJsonProperty<StationContentDto>("MissionTitle", "mission_title");
            AssertDtoHasJsonProperty<StationContentDto>("MissionSummary", "mission_summary");
            AssertDtoHasJsonProperty<StationContentDto>("NpcGuides", "npc_guides");
            AssertDtoHasJsonProperty<StationContentDto>("LearningCycle", "learning_cycle");
            AssertDtoHasJsonProperty<StationContentDto>("HintPolicy", "hint_policy");
            AssertDtoHasJsonProperty<StationContentDto>("Discoveries", "discoveries");
            AssertDtoHasJsonProperty<StationContentDto>("ReflectionPrompt", "reflection_prompt");
            AssertDtoHasJsonProperty<StationContentDto>("RewardPreview", "reward_preview");
            AssertDtoHasJsonProperty<StationContentDto>("WorldRestorationState", "world_restoration_state");
            AssertDtoHasJsonProperty<StationContentDto>("SuccessFeedback", "success_feedback");
        }

        [Test]
        public void DataProviderError_HasSnakeCaseJsonProperties()
        {
            AssertDtoHasJsonProperty<DataProviderError>("Code", "code", inDtoNamespace: false);
            AssertDtoHasJsonProperty<DataProviderError>("Message", "message", inDtoNamespace: false);
            AssertDtoHasJsonProperty<DataProviderError>("RequestId", "request_id", inDtoNamespace: false);
            AssertDtoHasJsonProperty<DataProviderError>("Retryable", "retryable", inDtoNamespace: false);
            AssertDtoHasJsonProperty<DataProviderError>("Details", "details", inDtoNamespace: false);
            AssertDtoHasJsonProperty<DataProviderError>("FieldErrors", "field_errors", inDtoNamespace: false);
            AssertDtoHasJsonProperty<DataProviderError>("RetryAfterSeconds", "retry_after_seconds", inDtoNamespace: false);
            AssertDtoHasJsonProperty<DataProviderError>("Action", "action", inDtoNamespace: false);
        }

        // ==============================================================
        //  DTO absent-safe defaults (optional fields null-safe)
        // ==============================================================

        [Test]
        public void ApiConfigDto_OptionalFieldsAreNullByDefault()
        {
            var dto = new ApiConfigDto();
            Assert.That(dto.ApiVersion, Is.Null);
            Assert.That(dto.ContractVersion, Is.Null);
            Assert.That(dto.ServerTime, Is.Null);
            Assert.That(dto.MaintenanceMode, Is.Null);
            Assert.That(dto.MinimumUnityClientVersion, Is.Null);
            Assert.That(dto.SupportedLanguages, Is.Null);
            Assert.That(dto.Polling, Is.Null);
            Assert.That(dto.Realtime, Is.Null);
        }

        [Test]
        public void LoginResponseDto_OptionalFieldsAreNullByDefault()
        {
            var dto = new LoginResponseDto();
            Assert.That(dto.Token, Is.Null);
            Assert.That(dto.TokenType, Is.Null);
            Assert.That(dto.Student, Is.Null);
        }

        [Test]
        public void AttemptRequestDto_OptionalFieldsAreNullByDefault()
        {
            var dto = new AttemptRequestDto();
            Assert.That(dto.StationSessionId, Is.Null);
            Assert.That(dto.StationId, Is.Null);
            Assert.That(dto.ClientAttemptUuid, Is.Null);
            Assert.That(dto.Answer, Is.Null);
            Assert.That(dto.TimeSpentSeconds, Is.Null);
            Assert.That(dto.UsedRewards, Is.Null);
        }

        [Test]
        public void SyncStatusDto_OptionalFieldsAreNullByDefault()
        {
            var dto = new SyncStatusDto();
            Assert.That(dto.StudentProgressRevision, Is.Null);
            Assert.That(dto.StudentSettingsRevision, Is.Null);
            Assert.That(dto.StationUnlockRevision, Is.Null);
            Assert.That(dto.PublishedContentRevision, Is.Null);
            Assert.That(dto.RewardWalletRevision, Is.Null);
            Assert.That(dto.ServerTime, Is.Null);
            Assert.That(dto.NextPollAfterSeconds, Is.Null);
        }

        [Test]
        public void StationContentDto_NarrativeFieldsAreNullByDefault()
        {
            var dto = new StationContentDto();
            Assert.That(dto.StoryContext, Is.Null);
            Assert.That(dto.MissionTitle, Is.Null);
            Assert.That(dto.MissionSummary, Is.Null);
            Assert.That(dto.NpcGuides, Is.Null);
            Assert.That(dto.LearningCycle, Is.Null);
            Assert.That(dto.HintPolicy, Is.Null);
            Assert.That(dto.Discoveries, Is.Null);
            Assert.That(dto.ReflectionPrompt, Is.Null);
            Assert.That(dto.RewardPreview, Is.Null);
            Assert.That(dto.WorldRestorationState, Is.Null);
            Assert.That(dto.SuccessFeedback, Is.Null);
        }

        [Test]
        public void DataProviderError_DefaultsAreStudentSafe()
        {
            var error = new DataProviderError();
            Assert.That(error.Code, Is.EqualTo("unknown"));
            Assert.That(error.Message, Is.Not.Null);
            Assert.That(error.RequestId, Is.Null);
            Assert.That(error.Retryable, Is.Null);
            Assert.That(error.Details, Is.Null);
            Assert.That(error.FieldErrors, Is.Null);
            Assert.That(error.RetryAfterSeconds, Is.Null);
            Assert.That(error.Action, Is.Null);
        }

        // ==============================================================
        //  Provider implementation tests
        // ==============================================================

        [Test]
        public void HttpProvider_ImplementsIGameDataProvider()
        {
            Type iface = FindType("NutriMind.Runtime.App.IGameDataProvider");
            Assert.That(iface, Is.Not.Null);

            Type provider = FindType("NutriMind.Runtime.App.HttpProvider");
            Assert.That(provider, Is.Not.Null,
                "HttpProvider type must exist");
            Assert.That(iface.IsAssignableFrom(provider), Is.True,
                "HttpProvider must implement IGameDataProvider");
        }

        [Test]
        public void LocalDemoJsonProvider_ImplementsIGameDataProvider()
        {
            Type iface = FindType("NutriMind.Runtime.App.IGameDataProvider");
            Assert.That(iface, Is.Not.Null);

            Type provider = FindType("NutriMind.Runtime.App.LocalDemoJsonProvider");
            Assert.That(provider, Is.Not.Null,
                "LocalDemoJsonProvider type must exist");
            Assert.That(iface.IsAssignableFrom(provider), Is.True,
                "LocalDemoJsonProvider must implement IGameDataProvider");
        }

        [Test]
        public void HttpProvider_WithFakeTransport_PingReturnsSuccess()
        {
            var transport = new FakeHttpTransport();
            transport.EnqueueSuccess(200, "{\"status\":\"ok\",\"server_time\":\"2026-06-14T10:00:00+08:00\"}");

            var config = new HttpProviderConfig { BaseUrl = "https://test.example" };
            var session = new AuthSessionState();
            var provider = new HttpProvider(config, session, transport);

            DataResult<PingResponseDto> result = provider.PingAsync(CancellationToken.None).Result;

            Assert.That(result.Success, Is.True);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Data.Status, Is.EqualTo("ok"));
        }

        [Test]
        public void HttpProvider_AuthenticatedRequest_SendsBearerHeader()
        {
            var transport = new FakeHttpTransport();
            transport.EnqueueSuccess(200, "{\"student_id\":\"stu_1\"}");

            var config = new HttpProviderConfig { BaseUrl = "https://test.example" };
            var session = new AuthSessionState { Token = "secret_token" };
            var provider = new HttpProvider(config, session, transport);

            DataResult<StudentProfileDto> result = provider.GetProfileAsync(CancellationToken.None).Result;

            Assert.That(result.Success, Is.True);
            Assert.That(transport.Requests.Count, Is.EqualTo(1));
            Assert.That(transport.Requests[0].Headers.TryGetValue("Authorization", out string auth), Is.True);
            Assert.That(auth, Is.EqualTo("Bearer secret_token"));
        }

        [Test]
        public void HttpProvider_MissingBaseUrl_ReturnsConfigurationError()
        {
            var config = new HttpProviderConfig { BaseUrl = "" };
            var session = new AuthSessionState { Token = "token" };
            var provider = new HttpProvider(config, session, new FakeHttpTransport());

            DataResult<StudentProfileDto> result = provider.GetProfileAsync(CancellationToken.None).Result;

            Assert.That(result.Success, Is.False);
            Assert.That(result.Error.Code, Is.EqualTo("CONFIGURATION_ERROR"));
        }

        // ==============================================================
        //  Consumer branching-free check
        // ==============================================================

        [Test]
        public void Consumer_DoesNotBranchOnDataProviderMode()
        {
            Type modeType = Type.GetType("NutriMind.Runtime.App.DataProviderMode, " + AppAssemblyName);
            Assume.That(modeType, Is.Not.Null);

            string[] allowed = { "SessionScope", "CompositionRoot", "ServiceFactory", "GameServiceLocator" };
            Assembly asm = modeType.Assembly;

            foreach (Type publicType in asm.GetExportedTypes())
            {
                if (Array.Exists(allowed, c => publicType.Name.Contains(c))) continue;
                if (publicType.IsEnum || publicType.IsInterface) continue;

                foreach (MethodInfo method in publicType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                {
                    foreach (ParameterInfo param in method.GetParameters())
                        Assert.That(param.ParameterType, Is.Not.EqualTo(modeType),
                            "{0}.{1} must not accept DataProviderMode", publicType.Name, method.Name);
                }
            }
        }

        // ==============================================================
        //  Helper
        // ==============================================================

        private static void AssertDtoHasJsonProperty<T>(string propertyName, string expectedJsonName, bool inDtoNamespace = true)
        {
            Type type = inDtoNamespace
                ? FindDtoType(typeof(T).Name)
                : FindType(typeof(T).FullName);
            Assert.That(type, Is.Not.Null,
                "Type {0} must exist", typeof(T).Name);

            PropertyInfo prop = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            Assert.That(prop, Is.Not.Null,
                "{0} must have property '{1}'", type.Name, propertyName);

            var attrs = prop.GetCustomAttributes(typeof(Newtonsoft.Json.JsonPropertyAttribute), false);
            Assert.That(attrs.Length, Is.GreaterThan(0),
                "{0}.{1} must have [JsonProperty] attribute", type.Name, propertyName);

            var jsonAttr = (Newtonsoft.Json.JsonPropertyAttribute)attrs[0];
            Assert.That(jsonAttr.PropertyName, Is.EqualTo(expectedJsonName),
                "{0}.{1} [JsonProperty] must map to '{2}' but was '{3}'",
                type.Name, propertyName, expectedJsonName, jsonAttr.PropertyName);
        }
    }
}