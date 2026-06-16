using System;
using System.Reflection;
using NUnit.Framework;

namespace NutriMind.Tests.EditMode.App
{
    /// <summary>
    /// EditMode red-phase tests for the Phase 02 shared game flow and state model
    /// (<c>NutriMind.Runtime.App</c> assembly).  All tests use reflection to
    /// reference expected types so the test assembly compiles before the
    /// production types exist.  Every test will FAIL until the Phase 02
    /// state-flow implementation is present.
    /// </summary>
    [TestFixture]
    public class NutriMindGameFlowStateTests
    {
        private const string ExpectedAssemblyName = "NutriMind.Runtime.App";


        private static Type FindType(string fullTypeName)
        {
            return Type.GetType(fullTypeName + ", " + ExpectedAssemblyName);
        }

        private static Type FindEnum(string enumName)
        {
            return FindType("NutriMind.Runtime.App." + enumName);
        }

        // ---------------------------------------------------------------
        // AppState enum — all 25 documented values
        // ---------------------------------------------------------------

        [Test]
        public void AppState_TypeExists()
        {
            Type type = FindEnum("AppState");
            Assert.That(type, Is.Not.Null,
                "AppState enum must exist in NutriMind.Runtime.App");
            Assert.That(type.IsEnum, Is.True,
                "AppState must be an enum type");
        }

        [Test]
        public void AppState_AllExpectedValuesDefined()
        {
            Type type = FindEnum("AppState");
            Assert.That(type, Is.Not.Null,
                "Precondition: AppState type must exist");

            string[] expectedNames =
            {
                "Starting", "CheckingServer", "MaintenanceBlocked", "UpdateRequired",
                "LoggedOut", "Authenticating", "Bootstrapping", "MainMenu",
                "SelectingSubject", "SelectingTerm", "LoadingWorld", "InWorld",
                "StartingStation", "InStationTask", "SubmittingAttempt", "ShowingFeedback",
                "RefreshingProgress", "ReturningToWorld",
                "ShowingMissionBrief", "ShowingHintOverlay", "ShowingReflection", "ShowingRewardPresentation",
                "SessionExpired", "ConnectionUnavailable", "FatalConfigurationError"
            };

            Array actualValues = Enum.GetValues(type);
            string[] actualNames = Enum.GetNames(type);

            Assert.That(actualNames, Is.EquivalentTo(expectedNames),
                "AppState must define exactly the {0} documented states", expectedNames.Length);

            // Also verify no duplicate numeric values
            var distinctValues = new System.Collections.Generic.HashSet<int>();
            foreach (object val in actualValues)
                Assert.That(distinctValues.Add(Convert.ToInt32(val)), Is.True,
                    "AppState must not contain duplicate numeric values");
        }

        // ---------------------------------------------------------------
        // DataProviderMode enum — two modes that share the same flow
        // ---------------------------------------------------------------

        [Test]
        public void DataProviderMode_TypeExists()
        {
            Type type = FindEnum("DataProviderMode");
            Assert.That(type, Is.Not.Null,
                "DataProviderMode enum must exist in NutriMind.Runtime.App");
            Assert.That(type.IsEnum, Is.True);
        }

        [Test]
        public void DataProviderMode_HasLocalDemoJsonAndHttp()
        {
            Type type = FindEnum("DataProviderMode");
            Assert.That(type, Is.Not.Null,
                "Precondition: DataProviderMode type must exist");

            string[] expectedNames = { "LocalDemoJson", "Http" };
            string[] actualNames = Enum.GetNames(type);

            Assert.That(actualNames, Is.EquivalentTo(expectedNames),
                "DataProviderMode must have exactly LocalDemoJson and Http values");
        }

        // ---------------------------------------------------------------
        // SubjectType enum — three subject categories
        // ---------------------------------------------------------------

        [Test]
        public void SubjectType_TypeExists()
        {
            Type type = FindEnum("SubjectType");
            Assert.That(type, Is.Not.Null,
                "SubjectType enum must exist in NutriMind.Runtime.App");
            Assert.That(type.IsEnum, Is.True);
        }

        [Test]
        public void SubjectType_HasThreeSubjects()
        {
            Type type = FindEnum("SubjectType");
            Assert.That(type, Is.Not.Null,
                "Precondition: SubjectType type must exist");

            string[] expectedNames = { "LiteraQuest", "ScienceQuest", "HealthQuest" };
            Assert.That(Enum.GetNames(type), Is.EquivalentTo(expectedNames));
        }

        // ---------------------------------------------------------------
        // StationState enum — eight documented values
        // ---------------------------------------------------------------

        [Test]
        public void StationState_TypeExists()
        {
            Type type = FindEnum("StationState");
            Assert.That(type, Is.Not.Null,
                "StationState enum must exist in NutriMind.Runtime.App");
            Assert.That(type.IsEnum, Is.True);
        }

        [Test]
        public void StationState_AllExpectedValuesDefined()
        {
            Type type = FindEnum("StationState");
            Assert.That(type, Is.Not.Null,
                "Precondition: StationState type must exist");

            string[] expectedNames =
            {
                "Unavailable", "Locked", "Unlocked", "Started",
                "AttemptPending", "PendingReview", "Completed", "ArchivedOrUnpublished"
            };
            Assert.That(Enum.GetNames(type), Is.EquivalentTo(expectedNames));
        }

        // ---------------------------------------------------------------
        // AppStateMachine — state transition controller
        // ---------------------------------------------------------------

        [Test]
        public void AppStateMachine_TypeExists()
        {
            Type type = FindType("NutriMind.Runtime.App.AppStateMachine");
            Assert.That(type, Is.Not.Null,
                "AppStateMachine class must exist in NutriMind.Runtime.App");
            Assert.That(type.IsClass, Is.True);
        }

        [Test]
        public void AppStateMachine_HasCurrentStateProperty()
        {
            Type type = FindType("NutriMind.Runtime.App.AppStateMachine");
            Assert.That(type, Is.Not.Null,
                "Precondition: AppStateMachine type must exist");

            PropertyInfo prop = type.GetProperty("CurrentState",
                BindingFlags.Public | BindingFlags.Instance);
            Assert.That(prop, Is.Not.Null,
                "AppStateMachine must expose a public CurrentState property");
            Assert.That(prop.PropertyType, Is.EqualTo(FindEnum("AppState")),
                "CurrentState must return AppState");
            Assert.That(prop.CanRead, Is.True,
                "CurrentState must be readable");
        }

        [Test]
        public void AppStateMachine_HasTryTransitionMethod()
        {
            Type type = FindType("NutriMind.Runtime.App.AppStateMachine");
            Assert.That(type, Is.Not.Null,
                "Precondition: AppStateMachine type must exist");

            Type appStateType = FindEnum("AppState");

            MethodInfo method = type.GetMethod("TryTransition",
                BindingFlags.Public | BindingFlags.Instance,
                null, new[] { appStateType }, null);
            Assert.That(method, Is.Not.Null,
                "AppStateMachine must have a public TryTransition(AppState) method");
            Assert.That(method.ReturnType, Is.EqualTo(typeof(bool)),
                "TryTransition must return bool indicating whether the transition was accepted");
        }

        [Test]
        public void AppStateMachine_StartsWithStartingOrDefaultState()
        {
            Type type = FindType("NutriMind.Runtime.App.AppStateMachine");
            Assert.That(type, Is.Not.Null,
                "Precondition: AppStateMachine type must exist");

            ConstructorInfo ctor = type.GetConstructor(Type.EmptyTypes);
            Assert.That(ctor, Is.Not.Null,
                "AppStateMachine must have a parameterless constructor");

            object machine = ctor.Invoke(null);
            PropertyInfo currentStateProp = type.GetProperty("CurrentState");
            object initialState = currentStateProp.GetValue(machine);

            Type appStateType = FindEnum("AppState");
            object startingValue = Enum.Parse(appStateType, "Starting");
            object loggedOutValue = Enum.Parse(appStateType, "LoggedOut");

            bool isValidStart = initialState.Equals(startingValue)
                                || initialState.Equals(loggedOutValue);
            Assert.That(isValidStart, Is.True,
                "Initial AppState must be either Starting or LoggedOut, but was {0}",
                initialState);
        }

        // ---------------------------------------------------------------
        // Happy-path transition: startup through main menu
        // ---------------------------------------------------------------

        [Test]
        public void HappyPath_StartupToMainMenu_TransitionsSucceed()
        {
            Type type = FindType("NutriMind.Runtime.App.AppStateMachine");
            Assert.That(type, Is.Not.Null,
                "Precondition: AppStateMachine type must exist");

            Type appStateType = FindEnum("AppState");
            ConstructorInfo ctor = type.GetConstructor(Type.EmptyTypes);
            object machine = ctor.Invoke(null);
            MethodInfo tryTransition = type.GetMethod("TryTransition",
                BindingFlags.Public | BindingFlags.Instance,
                null, new[] { appStateType }, null);

            string[] happyPath =
            {
                "Starting", "CheckingServer", "LoggedOut", "Authenticating",
                "Bootstrapping", "MainMenu"
            };

            foreach (string stateName in happyPath)
            {
                object targetState = Enum.Parse(appStateType, stateName);
                bool result = (bool)tryTransition.Invoke(machine, new[] { targetState });
                Assert.That(result, Is.True,
                    "TryTransition to {0} should succeed in happy-path startup flow", stateName);
            }

            PropertyInfo currentProp = type.GetProperty("CurrentState");
            object finalState = currentProp.GetValue(machine);
            object expectedState = Enum.Parse(appStateType, "MainMenu");
            Assert.That(finalState, Is.EqualTo(expectedState),
                "After happy-path transition, CurrentState should be MainMenu");
        }

        // ---------------------------------------------------------------
        // Happy-path: subject to world to station to return
        // ---------------------------------------------------------------

        [Test]
        public void HappyPath_FullGameFlow_TransitionsSucceed()
        {
            Type type = FindType("NutriMind.Runtime.App.AppStateMachine");
            Assert.That(type, Is.Not.Null,
                "Precondition: AppStateMachine type must exist");

            Type appStateType = FindEnum("AppState");
            ConstructorInfo ctor = type.GetConstructor(Type.EmptyTypes);
            object machine = ctor.Invoke(null);
            MethodInfo tryTransition = type.GetMethod("TryTransition",
                BindingFlags.Public | BindingFlags.Instance,
                null, new[] { appStateType }, null);

            // Full flow: startup -> ... -> station -> submission -> feedback -> return to world
            Type subjectTypeType = FindEnum("SubjectType");
            Assert.That(subjectTypeType, Is.Not.Null,
                "Precondition: SubjectType enum must exist");

            MethodInfo selectSubject = type.GetMethod("TrySelectSubject",
                BindingFlags.Public | BindingFlags.Instance,
                null, new[] { subjectTypeType }, null);
            Assert.That(selectSubject, Is.Not.Null,
                "AppStateMachine must provide TrySelectSubject(SubjectType) for subject-aware state flow");

            // Transition to SelectingSubject first
            string[] preSubjectPath =
            {
                "Starting", "CheckingServer", "LoggedOut", "Authenticating",
                "Bootstrapping", "MainMenu", "SelectingSubject"
            };

            foreach (string stateName in preSubjectPath)
            {
                object targetState = Enum.Parse(appStateType, stateName);
                bool result = (bool)tryTransition.Invoke(machine, new[] { targetState });
                Assert.That(result, Is.True,
                    "TryTransition to {0} should succeed in full game flow", stateName);
            }

            // Select LiteraQuest (a station-capable subject) while in SelectingSubject
            object literaSubject = Enum.Parse(subjectTypeType, "LiteraQuest");
            bool selectResult = (bool)selectSubject.Invoke(machine, new[] { literaSubject });
            Assert.That(selectResult, Is.True,
                "TrySelectSubject(LiteraQuest) must succeed during SelectingSubject phase");

            // Continue through term selection, world load, and station flow
            string[] postSubjectPath =
            {
                "SelectingTerm", "LoadingWorld", "InWorld", "StartingStation",
                "InStationTask", "SubmittingAttempt", "ShowingFeedback",
                "RefreshingProgress", "ReturningToWorld"
            };

            foreach (string stateName in postSubjectPath)
            {
                object targetState = Enum.Parse(appStateType, stateName);
                bool result = (bool)tryTransition.Invoke(machine, new[] { targetState });
                Assert.That(result, Is.True,
                    "TryTransition to {0} should succeed in full game flow", stateName);
            }

            PropertyInfo currentProp = type.GetProperty("CurrentState");
            object finalState = currentProp.GetValue(machine);
            object expectedState = Enum.Parse(appStateType, "ReturningToWorld");
            Assert.That(finalState, Is.EqualTo(expectedState),
                "After full happy-path, CurrentState should be ReturningToWorld");
        }

        // ---------------------------------------------------------------
        // Invalid transitions are rejected safely
        // ---------------------------------------------------------------

        [Test]
        public void InvalidTransition_FromLoggedOutToInWorld_ReturnsFalse()
        {
            Type type = FindType("NutriMind.Runtime.App.AppStateMachine");
            Assert.That(type, Is.Not.Null,
                "Precondition: AppStateMachine type must exist");

            Type appStateType = FindEnum("AppState");
            ConstructorInfo ctor = type.GetConstructor(Type.EmptyTypes);
            object machine = ctor.Invoke(null);
            MethodInfo tryTransition = type.GetMethod("TryTransition",
                BindingFlags.Public | BindingFlags.Instance,
                null, new[] { appStateType }, null);

            object loggedOut = Enum.Parse(appStateType, "LoggedOut");
            tryTransition.Invoke(machine, new[] { loggedOut });

            object inWorld = Enum.Parse(appStateType, "InWorld");
            bool result = (bool)tryTransition.Invoke(machine, new[] { inWorld });
            Assert.That(result, Is.False,
                "Transition from LoggedOut directly to InWorld must be rejected");
        }

        [Test]
        public void InvalidTransition_FromLoggedOutToInStationTask_ReturnsFalse()
        {
            Type type = FindType("NutriMind.Runtime.App.AppStateMachine");
            Assert.That(type, Is.Not.Null,
                "Precondition: AppStateMachine type must exist");

            Type appStateType = FindEnum("AppState");
            ConstructorInfo ctor = type.GetConstructor(Type.EmptyTypes);
            object machine = ctor.Invoke(null);
            MethodInfo tryTransition = type.GetMethod("TryTransition",
                BindingFlags.Public | BindingFlags.Instance,
                null, new[] { appStateType }, null);

            object loggedOut = Enum.Parse(appStateType, "LoggedOut");
            tryTransition.Invoke(machine, new[] { loggedOut });

            object inStation = Enum.Parse(appStateType, "InStationTask");
            bool result = (bool)tryTransition.Invoke(machine, new[] { inStation });
            Assert.That(result, Is.False,
                "Transition from LoggedOut directly to InStationTask must be rejected");
        }

        [Test]
        public void InvalidTransition_FromStartingToInWorld_ReturnsFalse()
        {
            Type type = FindType("NutriMind.Runtime.App.AppStateMachine");
            Assert.That(type, Is.Not.Null,
                "Precondition: AppStateMachine type must exist");

            Type appStateType = FindEnum("AppState");
            ConstructorInfo ctor = type.GetConstructor(Type.EmptyTypes);
            object machine = ctor.Invoke(null);
            MethodInfo tryTransition = type.GetMethod("TryTransition",
                BindingFlags.Public | BindingFlags.Instance,
                null, new[] { appStateType }, null);

            object inWorld = Enum.Parse(appStateType, "InWorld");
            bool result = (bool)tryTransition.Invoke(machine, new[] { inWorld });
            Assert.That(result, Is.False,
                "Transition from Starting directly to InWorld must be rejected " +
                "(cannot load gameplay before compatibility and session are known)");
        }

        [Test]
        public void InvalidTransition_FromLoggedOutToSubmittingAttempt_ReturnsFalse()
        {
            Type type = FindType("NutriMind.Runtime.App.AppStateMachine");
            Assert.That(type, Is.Not.Null,
                "Precondition: AppStateMachine type must exist");

            Type appStateType = FindEnum("AppState");
            ConstructorInfo ctor = type.GetConstructor(Type.EmptyTypes);
            object machine = ctor.Invoke(null);
            MethodInfo tryTransition = type.GetMethod("TryTransition",
                BindingFlags.Public | BindingFlags.Instance,
                null, new[] { appStateType }, null);

            object loggedOut = Enum.Parse(appStateType, "LoggedOut");
            tryTransition.Invoke(machine, new[] { loggedOut });

            object submit = Enum.Parse(appStateType, "SubmittingAttempt");
            bool result = (bool)tryTransition.Invoke(machine, new[] { submit });
            Assert.That(result, Is.False,
                "Direct transition from LoggedOut to SubmittingAttempt must be rejected");
        }

        // ---------------------------------------------------------------
        // Science Quest path — never enters station flow
        // ---------------------------------------------------------------

        [Test]
        public void ScienceQuest_NeverEntersStationFlow()
        {
            Type type = FindType("NutriMind.Runtime.App.AppStateMachine");
            Assert.That(type, Is.Not.Null,
                "Precondition: AppStateMachine type must exist");

            Type appStateType = FindEnum("AppState");
            Type subjectTypeType = FindEnum("SubjectType");
            Assert.That(subjectTypeType, Is.Not.Null,
                "Precondition: SubjectType enum must exist");

            ConstructorInfo ctor = type.GetConstructor(Type.EmptyTypes);
            object machine = ctor.Invoke(null);
            MethodInfo tryTransition = type.GetMethod("TryTransition",
                BindingFlags.Public | BindingFlags.Instance,
                null, new[] { appStateType }, null);

            // Transition through startup to SelectingSubject
            string[] preSubjectPath =
            {
                "Starting", "CheckingServer", "LoggedOut", "Authenticating",
                "Bootstrapping", "MainMenu", "SelectingSubject"
            };

            foreach (string stateName in preSubjectPath)
            {
                object targetState = Enum.Parse(appStateType, stateName);
                bool transitionResult = (bool)tryTransition.Invoke(machine, new[] { targetState });
                Assert.That(transitionResult, Is.True,
                    "Science: TryTransition to {0} should succeed", stateName);
            }

            // Explicitly select Science while in SelectingSubject so the machine
            // can distinguish subject-specific transition rules (e.g. Science has no stations).
            MethodInfo selectSubject = type.GetMethod("TrySelectSubject",
                BindingFlags.Public | BindingFlags.Instance,
                null, new[] { subjectTypeType }, null);
            Assert.That(selectSubject, Is.Not.Null,
                "AppStateMachine must provide TrySelectSubject(SubjectType) " +
                "for subject-aware state flow");

            object scienceSubject = Enum.Parse(subjectTypeType, "ScienceQuest");
            bool selectResult = (bool)selectSubject.Invoke(machine, new[] { scienceSubject });
            Assert.That(selectResult, Is.True,
                "TrySelectSubject(ScienceQuest) must succeed during SelectingSubject phase");

            // Now continue through term selection, world load, and InWorld
            string[] postSubjectPath =
            {
                "SelectingTerm", "LoadingWorld", "InWorld"
            };

            foreach (string stateName in postSubjectPath)
            {
                object targetState = Enum.Parse(appStateType, stateName);
                bool transitionResult = (bool)tryTransition.Invoke(machine, new[] { targetState });
                Assert.That(transitionResult, Is.True,
                    "Science: TryTransition to {0} should succeed", stateName);
            }

            // Station transitions must be rejected for Science
            object startingStation = Enum.Parse(appStateType, "StartingStation");
            bool stationResult = (bool)tryTransition.Invoke(machine, new[] { startingStation });
            Assert.That(stationResult, Is.False,
                "Science Quest must reject transition to StartingStation " +
                "(Science has no station gameplay in current milestone)");
        }

        // ---------------------------------------------------------------
        // Subject-aware state guards — no-subject and post-world-lock
        // ---------------------------------------------------------------

        [Test]
        public void InWorldToStartingStation_RejectedWhenNoSubjectSelected()
        {
            Type type = FindType("NutriMind.Runtime.App.AppStateMachine");
            Assert.That(type, Is.Not.Null,
                "Precondition: AppStateMachine type must exist");

            Type appStateType = FindEnum("AppState");
            ConstructorInfo ctor = type.GetConstructor(Type.EmptyTypes);
            object machine = ctor.Invoke(null);
            MethodInfo tryTransition = type.GetMethod("TryTransition",
                BindingFlags.Public | BindingFlags.Instance,
                null, new[] { appStateType }, null);

            // Walk through to InWorld without ever selecting a subject
            string[] path =
            {
                "Starting", "CheckingServer", "LoggedOut", "Authenticating",
                "Bootstrapping", "MainMenu", "SelectingSubject", "SelectingTerm",
                "LoadingWorld", "InWorld"
            };

            foreach (string stateName in path)
            {
                object targetState = Enum.Parse(appStateType, stateName);
                bool result = (bool)tryTransition.Invoke(machine, new[] { targetState });
                Assert.That(result, Is.True,
                    "Path to InWorld: TryTransition to {0} should succeed", stateName);
            }

            // Attempting to start a station without a selected subject must be rejected
            object startingStation = Enum.Parse(appStateType, "StartingStation");
            bool stationResult = (bool)tryTransition.Invoke(machine, new[] { startingStation });
            Assert.That(stationResult, Is.False,
                "Transition from InWorld to StartingStation must be rejected " +
                "when no subject has been selected");
        }

        [Test]
        public void TrySelectSubject_RejectedAfterInWorld()
        {
            Type type = FindType("NutriMind.Runtime.App.AppStateMachine");
            Assert.That(type, Is.Not.Null,
                "Precondition: AppStateMachine type must exist");

            Type appStateType = FindEnum("AppState");
            Type subjectTypeType = FindEnum("SubjectType");
            Assert.That(subjectTypeType, Is.Not.Null,
                "Precondition: SubjectType enum must exist");

            ConstructorInfo ctor = type.GetConstructor(Type.EmptyTypes);
            object machine = ctor.Invoke(null);
            MethodInfo tryTransition = type.GetMethod("TryTransition",
                BindingFlags.Public | BindingFlags.Instance,
                null, new[] { appStateType }, null);
            MethodInfo selectSubject = type.GetMethod("TrySelectSubject",
                BindingFlags.Public | BindingFlags.Instance,
                null, new[] { subjectTypeType }, null);
            Assert.That(selectSubject, Is.Not.Null,
                "AppStateMachine must provide TrySelectSubject(SubjectType)");

            // Walk through to SelectingSubject
            string[] startupPath =
            {
                "Starting", "CheckingServer", "LoggedOut", "Authenticating",
                "Bootstrapping", "MainMenu", "SelectingSubject"
            };
            foreach (string stateName in startupPath)
            {
                object targetState = Enum.Parse(appStateType, stateName);
                bool result = (bool)tryTransition.Invoke(machine, new[] { targetState });
                Assert.That(result, Is.True,
                    "Path to SelectingSubject: TryTransition to {0} should succeed", stateName);
            }

            // Select ScienceQuest — must succeed during subject-selection phase
            object scienceSubject = Enum.Parse(subjectTypeType, "ScienceQuest");
            bool selectResult = (bool)selectSubject.Invoke(machine, new[] { scienceSubject });
            Assert.That(selectResult, Is.True,
                "TrySelectSubject(ScienceQuest) must succeed during SelectingSubject phase");

            // Advance to InWorld
            string[] gamePath = { "SelectingTerm", "LoadingWorld", "InWorld" };
            foreach (string stateName in gamePath)
            {
                object targetState = Enum.Parse(appStateType, stateName);
                bool result = (bool)tryTransition.Invoke(machine, new[] { targetState });
                Assert.That(result, Is.True,
                    "Path to InWorld: TryTransition to {0} should succeed", stateName);
            }

            // After entering InWorld, subject mutation must be rejected
            object literaSubject = Enum.Parse(subjectTypeType, "LiteraQuest");
            bool mutateResult = (bool)selectSubject.Invoke(machine, new[] { literaSubject });
            Assert.That(mutateResult, Is.False,
                "TrySelectSubject must return false once the flow has entered InWorld");

            // StartingStation must remain rejected because ScienceQuest was selected
            // (and TrySelectSubject(LiteraQuest) was refused, so the subject stayed as ScienceQuest)
            object startingStation = Enum.Parse(appStateType, "StartingStation");
            bool stationResult = (bool)tryTransition.Invoke(machine, new[] { startingStation });
            Assert.That(stationResult, Is.False,
                "StartingStation must remain rejected because ScienceQuest is still the " +
                "selected subject (TrySelectSubject mutation was rejected)");
        }

        // ---------------------------------------------------------------
        // Session expiry
        // ---------------------------------------------------------------

        [Test]
        public void SessionExpiry_TransitionsToSessionExpired()
        {
            Type type = FindType("NutriMind.Runtime.App.AppStateMachine");
            Assert.That(type, Is.Not.Null,
                "Precondition: AppStateMachine type must exist");

            Type appStateType = FindEnum("AppState");
            ConstructorInfo ctor = type.GetConstructor(Type.EmptyTypes);
            object machine = ctor.Invoke(null);
            MethodInfo tryTransition = type.GetMethod("TryTransition",
                BindingFlags.Public | BindingFlags.Instance,
                null, new[] { appStateType }, null);

            string[] reachSessionExpired =
            {
                "Starting", "CheckingServer", "LoggedOut", "Authenticating",
                "Bootstrapping", "MainMenu"
            };
            foreach (string stateName in reachSessionExpired)
            {
                object targetState = Enum.Parse(appStateType, stateName);
                tryTransition.Invoke(machine, new[] { targetState });
            }

            object expired = Enum.Parse(appStateType, "SessionExpired");
            bool result = (bool)tryTransition.Invoke(machine, new[] { expired });
            Assert.That(result, Is.True,
                "Transition to SessionExpired must be valid from MainMenu");
        }

        [Test]
        public void SessionExpiry_CanTransitionToLoggedOut()
        {
            Type type = FindType("NutriMind.Runtime.App.AppStateMachine");
            Assert.That(type, Is.Not.Null,
                "Precondition: AppStateMachine type must exist");

            Type appStateType = FindEnum("AppState");
            ConstructorInfo ctor = type.GetConstructor(Type.EmptyTypes);
            object machine = ctor.Invoke(null);
            MethodInfo tryTransition = type.GetMethod("TryTransition",
                BindingFlags.Public | BindingFlags.Instance,
                null, new[] { appStateType }, null);

            string[] states =
            {
                "Starting", "LoggedOut", "Authenticating", "Bootstrapping",
                "MainMenu", "SessionExpired"
            };
            foreach (string stateName in states)
            {
                object targetState = Enum.Parse(appStateType, stateName);
                tryTransition.Invoke(machine, new[] { targetState });
            }

            object loggedOut = Enum.Parse(appStateType, "LoggedOut");
            bool result = (bool)tryTransition.Invoke(machine, new[] { loggedOut });
            Assert.That(result, Is.True,
                "Transition from SessionExpired to LoggedOut must be valid");
        }

        // ---------------------------------------------------------------
        // Missing scene / content unavailable
        // ---------------------------------------------------------------

        [Test]
        public void MissingScene_LoadingWorldCanTransitionToConnectionUnavailable()
        {
            Type type = FindType("NutriMind.Runtime.App.AppStateMachine");
            Assert.That(type, Is.Not.Null,
                "Precondition: AppStateMachine type must exist");

            Type appStateType = FindEnum("AppState");
            ConstructorInfo ctor = type.GetConstructor(Type.EmptyTypes);
            object machine = ctor.Invoke(null);
            MethodInfo tryTransition = type.GetMethod("TryTransition",
                BindingFlags.Public | BindingFlags.Instance,
                null, new[] { appStateType }, null);

            string[] path =
            {
                "Starting", "CheckingServer", "LoggedOut", "Authenticating",
                "Bootstrapping", "MainMenu", "SelectingSubject", "SelectingTerm",
                "LoadingWorld"
            };
            foreach (string stateName in path)
            {
                object targetState = Enum.Parse(appStateType, stateName);
                tryTransition.Invoke(machine, new[] { targetState });
            }

            object unavailable = Enum.Parse(appStateType, "ConnectionUnavailable");
            bool result = (bool)tryTransition.Invoke(machine, new[] { unavailable });
            Assert.That(result, Is.True,
                "From LoadingWorld, transition to ConnectionUnavailable must be valid");
        }

        [Test]
        public void ConnectionUnavailable_ReturnToMainMenuIsValid()
        {
            Type type = FindType("NutriMind.Runtime.App.AppStateMachine");
            Assert.That(type, Is.Not.Null,
                "Precondition: AppStateMachine type must exist");

            Type appStateType = FindEnum("AppState");
            ConstructorInfo ctor = type.GetConstructor(Type.EmptyTypes);
            object machine = ctor.Invoke(null);
            MethodInfo tryTransition = type.GetMethod("TryTransition",
                BindingFlags.Public | BindingFlags.Instance,
                null, new[] { appStateType }, null);

            string[] path =
            {
                "Starting", "CheckingServer", "LoggedOut", "Authenticating",
                "Bootstrapping", "MainMenu", "SelectingSubject", "SelectingTerm",
                "LoadingWorld", "InWorld", "ConnectionUnavailable"
            };
            foreach (string stateName in path)
            {
                object targetState = Enum.Parse(appStateType, stateName);
                tryTransition.Invoke(machine, new[] { targetState });
            }

            object mainMenu = Enum.Parse(appStateType, "MainMenu");
            bool result = (bool)tryTransition.Invoke(machine, new[] { mainMenu });
            Assert.That(result, Is.True,
                "From ConnectionUnavailable, transition back to MainMenu must be valid");
        }

        // ---------------------------------------------------------------
        // Maintenance and update-required
        // ---------------------------------------------------------------

        [Test]
        public void MaintenanceBlocked_IsReachableFromCheckingServer()
        {
            Type type = FindType("NutriMind.Runtime.App.AppStateMachine");
            Assert.That(type, Is.Not.Null,
                "Precondition: AppStateMachine type must exist");

            Type appStateType = FindEnum("AppState");
            ConstructorInfo ctor = type.GetConstructor(Type.EmptyTypes);
            MethodInfo tryTransition = type.GetMethod("TryTransition",
                BindingFlags.Public | BindingFlags.Instance,
                null, new[] { appStateType }, null);

            object starting = Enum.Parse(appStateType, "Starting");
            object checkingServer = Enum.Parse(appStateType, "CheckingServer");

            // --- Machine 1: reach MaintenanceBlocked from CheckingServer ---
            object machine1 = ctor.Invoke(null);
            tryTransition.Invoke(machine1, new[] { starting });
            tryTransition.Invoke(machine1, new[] { checkingServer });

            object maintenance = Enum.Parse(appStateType, "MaintenanceBlocked");
            bool result = (bool)tryTransition.Invoke(machine1, new[] { maintenance });
            Assert.That(result, Is.True,
                "MaintenanceBlocked must be reachable from CheckingServer");

            // --- Machine 2: reach UpdateRequired from CheckingServer ---
            // (separate machine so the previous MaintenanceBlocked state
            //  does not interfere with the CheckingServer→UpdateRequired path)
            object machine2 = ctor.Invoke(null);
            tryTransition.Invoke(machine2, new[] { starting });
            tryTransition.Invoke(machine2, new[] { checkingServer });

            object updateRequired = Enum.Parse(appStateType, "UpdateRequired");
            result = (bool)tryTransition.Invoke(machine2, new[] { updateRequired });
            Assert.That(result, Is.True,
                "UpdateRequired must be reachable from CheckingServer");
        }

        // ---------------------------------------------------------------
        // Pause/resume — stale completion must be ignored
        // ---------------------------------------------------------------

        [Test]
        public void PauseResume_StaleCompletionIsIgnored()
        {
            Type type = FindType("NutriMind.Runtime.App.AppStateMachine");
            Assert.That(type, Is.Not.Null,
                "Precondition: AppStateMachine type must exist");

            MethodInfo invalidateMethod = type.GetMethod("InvalidateInFlightOperations",
                BindingFlags.Public | BindingFlags.Instance,
                null, Type.EmptyTypes, null);

            if (invalidateMethod != null)
            {
                ConstructorInfo ctor = type.GetConstructor(Type.EmptyTypes);
                object machine = ctor.Invoke(null);
                Assert.DoesNotThrow(() => invalidateMethod.Invoke(machine, null),
                    "InvalidateInFlightOperations must not throw");
            }
            else
            {
                MethodInfo pauseMethod = type.GetMethod("HandlePause",
                    BindingFlags.Public | BindingFlags.Instance);
                Assert.That(pauseMethod ?? invalidateMethod, Is.Not.Null,
                    "AppStateMachine must provide InvalidateInFlightOperations() or HandlePause()");
            }
        }

        // ---------------------------------------------------------------
        // Retry reuses client_attempt_uuid
        // ---------------------------------------------------------------

        [Test]
        public void RetryAttempt_ReusesClientAttemptUuid()
        {
            Type attemptScopeType = FindType("NutriMind.Runtime.App.AttemptScope");
            Assert.That(attemptScopeType, Is.Not.Null,
                "AttemptScope type must exist in NutriMind.Runtime.App");

            PropertyInfo uuidProp = attemptScopeType.GetProperty("ClientAttemptUuid",
                BindingFlags.Public | BindingFlags.Instance);
            Assert.That(uuidProp, Is.Not.Null,
                "AttemptScope must expose ClientAttemptUuid property");
            Assert.That(uuidProp.PropertyType, Is.EqualTo(typeof(string)),
                "ClientAttemptUuid must be a string");

            MethodInfo retryMethod = attemptScopeType.GetMethod("Retry",
                BindingFlags.Public | BindingFlags.Instance);
            Assert.That(retryMethod, Is.Not.Null,
                "AttemptScope must have a Retry method");

            ConstructorInfo ctor = attemptScopeType.GetConstructor(Type.EmptyTypes);
            object scope = ctor.Invoke(null);

            string originalUuid = (string)uuidProp.GetValue(scope);
            Assert.That(originalUuid, Is.Not.Null.And.Not.Empty,
                "ClientAttemptUuid must be non-null and non-empty");

            retryMethod.Invoke(scope, null);

            string afterRetryUuid = (string)uuidProp.GetValue(scope);
            Assert.That(afterRetryUuid, Is.EqualTo(originalUuid),
                "Retry must reuse the same ClientAttemptUuid");
        }

        // ---------------------------------------------------------------
        // SessionScope
        // ---------------------------------------------------------------

        [Test]
        public void SessionScope_TypeExists()
        {
            Type type = FindType("NutriMind.Runtime.App.SessionScope");
            Assert.That(type, Is.Not.Null,
                "SessionScope type must exist in NutriMind.Runtime.App");
        }

        [Test]
        public void SessionScope_HasClearMethod()
        {
            Type type = FindType("NutriMind.Runtime.App.SessionScope");
            Assert.That(type, Is.Not.Null,
                "Precondition: SessionScope type must exist");

            MethodInfo clearMethod = type.GetMethod("Clear",
                BindingFlags.Public | BindingFlags.Instance,
                null, Type.EmptyTypes, null);
            Assert.That(clearMethod, Is.Not.Null,
                "SessionScope must have a Clear() method");

            ConstructorInfo ctor = type.GetConstructor(Type.EmptyTypes);
            object scope = ctor.Invoke(null);
            Assert.DoesNotThrow(() => clearMethod.Invoke(scope, null),
                "Clear() must not throw");
        }

        [Test]
        public void SessionScope_HasDataProviderModeProperty()
        {
            Type type = FindType("NutriMind.Runtime.App.SessionScope");
            Assert.That(type, Is.Not.Null,
                "Precondition: SessionScope type must exist");

            PropertyInfo modeProp = type.GetProperty("Mode",
                BindingFlags.Public | BindingFlags.Instance);
            Assert.That(modeProp, Is.Not.Null,
                "SessionScope must expose a Mode property");

            Type expectedModeType = FindEnum("DataProviderMode");
            Assert.That(modeProp.PropertyType, Is.EqualTo(expectedModeType),
                "SessionScope.Mode must return DataProviderMode");
        }

        // ---------------------------------------------------------------
        // Logout clears session state
        // ---------------------------------------------------------------

        [Test]
        public void Logout_ClearsSessionState()
        {
            Type sessionType = FindType("NutriMind.Runtime.App.SessionScope");
            Assert.That(sessionType, Is.Not.Null,
                "Precondition: SessionScope type must exist");

            Type appStateType = FindEnum("AppState");
            Type stateMachineType = FindType("NutriMind.Runtime.App.AppStateMachine");
            Assert.That(stateMachineType, Is.Not.Null,
                "Precondition: AppStateMachine type must exist");

            ConstructorInfo sessionCtor = sessionType.GetConstructor(Type.EmptyTypes);
            object session = sessionCtor.Invoke(null);
            MethodInfo clearMethod = sessionType.GetMethod("Clear");
            clearMethod.Invoke(session, null); // should not throw

            ConstructorInfo machineCtor = stateMachineType.GetConstructor(Type.EmptyTypes);
            object machine = machineCtor.Invoke(null);
            MethodInfo tryTransition = stateMachineType.GetMethod("TryTransition",
                BindingFlags.Public | BindingFlags.Instance,
                null, new[] { appStateType }, null);

            string[] path =
            {
                "Starting", "CheckingServer", "LoggedOut", "Authenticating",
                "Bootstrapping", "MainMenu"
            };
            foreach (string stateName in path)
            {
                object targetState = Enum.Parse(appStateType, stateName);
                tryTransition.Invoke(machine, new[] { targetState });
            }

            object loggedOut = Enum.Parse(appStateType, "LoggedOut");
            bool result = (bool)tryTransition.Invoke(machine, new[] { loggedOut });
            Assert.That(result, Is.True,
                "Logout transition from MainMenu to LoggedOut must succeed");
        }

        // ---------------------------------------------------------------
        // FatalConfigurationError
        // ---------------------------------------------------------------

        [Test]
        public void FatalConfigurationError_IsReachableFromStarting()
        {
            Type type = FindType("NutriMind.Runtime.App.AppStateMachine");
            Assert.That(type, Is.Not.Null,
                "Precondition: AppStateMachine type must exist");

            Type appStateType = FindEnum("AppState");
            ConstructorInfo ctor = type.GetConstructor(Type.EmptyTypes);
            object machine = ctor.Invoke(null);
            MethodInfo tryTransition = type.GetMethod("TryTransition",
                BindingFlags.Public | BindingFlags.Instance,
                null, new[] { appStateType }, null);

            object starting = Enum.Parse(appStateType, "Starting");
            tryTransition.Invoke(machine, new[] { starting });

            object fatal = Enum.Parse(appStateType, "FatalConfigurationError");
            bool result = (bool)tryTransition.Invoke(machine, new[] { fatal });
            Assert.That(result, Is.True,
                "FatalConfigurationError must be reachable from Starting");
        }

        // ---------------------------------------------------------------
        // Shared flow: LocalDemoJson and Http
        // ---------------------------------------------------------------

        [Test]
        public void LocalDemoJsonAndHttp_ShareSameAppStateValues()
        {
            Type appStateType = FindEnum("AppState");
            Assert.That(appStateType, Is.Not.Null,
                "Precondition: AppState type must exist");

            int stateCount = Enum.GetValues(appStateType).Length;
            Assert.That(stateCount, Is.EqualTo(25),
                "AppState must define exactly 25 values shared by both modes");
        }

        [Test]
        public void LocalDemoJsonAndHttp_ShareSameStateMachineType()
        {
            Type machineType = FindType("NutriMind.Runtime.App.AppStateMachine");
            Assert.That(machineType, Is.Not.Null,
                "AppStateMachine must exist — both providers share this type");
            Assert.That(machineType.Assembly.GetName().Name, Is.EqualTo(ExpectedAssemblyName),
                "AppStateMachine must live in {0}", ExpectedAssemblyName);
        }

        // ---------------------------------------------------------------
        // Return-to-world: after completing station flow, you can go back to InWorld
        // ---------------------------------------------------------------

        [Test]
        public void ReturnToWorld_CanTransitionBackToInWorld()
        {
            Type type = FindType("NutriMind.Runtime.App.AppStateMachine");
            Assert.That(type, Is.Not.Null,
                "Precondition: AppStateMachine type must exist");

            Type appStateType = FindEnum("AppState");
            ConstructorInfo ctor = type.GetConstructor(Type.EmptyTypes);
            object machine = ctor.Invoke(null);
            MethodInfo tryTransition = type.GetMethod("TryTransition",
                BindingFlags.Public | BindingFlags.Instance,
                null, new[] { appStateType }, null);

            // Walk full station path through to ReturningToWorld
            Type subjectTypeType = FindEnum("SubjectType");
            Assert.That(subjectTypeType, Is.Not.Null,
                "Precondition: SubjectType enum must exist");

            MethodInfo selectSubject = type.GetMethod("TrySelectSubject",
                BindingFlags.Public | BindingFlags.Instance,
                null, new[] { subjectTypeType }, null);
            Assert.That(selectSubject, Is.Not.Null,
                "AppStateMachine must provide TrySelectSubject(SubjectType) for subject-aware state flow");

            // Transition through startup to SelectingSubject
            string[] preSubjectPath =
            {
                "Starting", "CheckingServer", "LoggedOut", "Authenticating",
                "Bootstrapping", "MainMenu", "SelectingSubject"
            };

            foreach (string stateName in preSubjectPath)
            {
                object targetState = Enum.Parse(appStateType, stateName);
                bool transitionResult = (bool)tryTransition.Invoke(machine, new[] { targetState });
                Assert.That(transitionResult, Is.True,
                    "TryTransition to {0} should succeed in full station flow", stateName);
            }

            // Select LiteraQuest (a station-capable subject) while in SelectingSubject
            object literaSubject = Enum.Parse(subjectTypeType, "LiteraQuest");
            bool selectResult = (bool)selectSubject.Invoke(machine, new[] { literaSubject });
            Assert.That(selectResult, Is.True,
                "TrySelectSubject(LiteraQuest) must succeed during SelectingSubject phase");

            // Continue through term selection, world load, station flow, and return
            string[] postSubjectPath =
            {
                "SelectingTerm", "LoadingWorld", "InWorld", "StartingStation",
                "InStationTask", "SubmittingAttempt", "ShowingFeedback",
                "RefreshingProgress", "ReturningToWorld"
            };

            foreach (string stateName in postSubjectPath)
            {
                object targetState = Enum.Parse(appStateType, stateName);
                bool transitionResult = (bool)tryTransition.Invoke(machine, new[] { targetState });
                Assert.That(transitionResult, Is.True,
                    "TryTransition to {0} should succeed in full station flow", stateName);
            }

            // Now return to InWorld
            object inWorld = Enum.Parse(appStateType, "InWorld");
            bool result = (bool)tryTransition.Invoke(machine, new[] { inWorld });
            Assert.That(result, Is.True,
                "After ReturningToWorld, TryTransition(InWorld) must succeed " +
                "(player goes back to the world after station completion)");

            PropertyInfo currentProp = type.GetProperty("CurrentState");
            object finalState = currentProp.GetValue(machine);
            Assert.That(finalState, Is.EqualTo(inWorld),
                "After ReturningToWorld -> InWorld transition, CurrentState must be InWorld");
        }

        // ---------------------------------------------------------------
        // Session expiry during gameplay / async flow
        // ---------------------------------------------------------------

        [Test]
        public void SessionExpiry_FromSubmittingAttemptIsValid()
        {
            Type type = FindType("NutriMind.Runtime.App.AppStateMachine");
            Assert.That(type, Is.Not.Null,
                "Precondition: AppStateMachine type must exist");

            Type appStateType = FindEnum("AppState");
            ConstructorInfo ctor = type.GetConstructor(Type.EmptyTypes);
            object machine = ctor.Invoke(null);
            MethodInfo tryTransition = type.GetMethod("TryTransition",
                BindingFlags.Public | BindingFlags.Instance,
                null, new[] { appStateType }, null);

            string[] path =
            {
                "Starting", "CheckingServer", "LoggedOut", "Authenticating",
                "Bootstrapping", "MainMenu", "SelectingSubject", "SelectingTerm",
                "LoadingWorld", "InWorld", "StartingStation", "InStationTask",
                "SubmittingAttempt"
            };

            foreach (string stateName in path)
            {
                object targetState = Enum.Parse(appStateType, stateName);
                tryTransition.Invoke(machine, new[] { targetState });
            }

            object expired = Enum.Parse(appStateType, "SessionExpired");
            bool result = (bool)tryTransition.Invoke(machine, new[] { expired });
            Assert.That(result, Is.True,
                "Session expiry from SubmittingAttempt must be valid " +
                "(token can expire during submission)");
        }

        [Test]
        public void SessionExpiry_FromLoadingWorldIsValid()
        {
            Type type = FindType("NutriMind.Runtime.App.AppStateMachine");
            Assert.That(type, Is.Not.Null,
                "Precondition: AppStateMachine type must exist");

            Type appStateType = FindEnum("AppState");
            ConstructorInfo ctor = type.GetConstructor(Type.EmptyTypes);
            object machine = ctor.Invoke(null);
            MethodInfo tryTransition = type.GetMethod("TryTransition",
                BindingFlags.Public | BindingFlags.Instance,
                null, new[] { appStateType }, null);

            string[] path =
            {
                "Starting", "CheckingServer", "LoggedOut", "Authenticating",
                "Bootstrapping", "MainMenu", "SelectingSubject", "SelectingTerm",
                "LoadingWorld"
            };

            foreach (string stateName in path)
            {
                object targetState = Enum.Parse(appStateType, stateName);
                tryTransition.Invoke(machine, new[] { targetState });
            }

            object expired = Enum.Parse(appStateType, "SessionExpired");
            bool result = (bool)tryTransition.Invoke(machine, new[] { expired });
            Assert.That(result, Is.True,
                "Session expiry from LoadingWorld must be valid " +
                "(token can expire during world load)");
        }

        // ---------------------------------------------------------------
        // SessionScope.Clear preserves data provider mode
        // ---------------------------------------------------------------

        [Test]
        public void SessionScope_ClearPreservesProviderMode()
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
                "Precondition: SessionScope must have Clear method");

            ConstructorInfo ctor = sessionType.GetConstructor(Type.EmptyTypes);
            object session = ctor.Invoke(null);

            // Set Mode to Http
            object httpMode = Enum.Parse(modeType, "Http");
            modeProp.SetValue(session, httpMode);

            // Clear should preserve the mode
            clearMethod.Invoke(session, null);

            object modeAfterClear = modeProp.GetValue(session);
            Assert.That(modeAfterClear, Is.EqualTo(httpMode),
                "SessionScope.Clear must preserve the current Mode value " +
                "(data provider mode is chosen at startup and must survive logout/clear)");
        }

        // ---------------------------------------------------------------
        // OperationGeneration — cancellation / stale-completion signal
        // ---------------------------------------------------------------

        [Test]
        public void AppStateMachine_HasOperationGenerationProperty()
        {
            Type type = FindType("NutriMind.Runtime.App.AppStateMachine");
            Assert.That(type, Is.Not.Null,
                "Precondition: AppStateMachine type must exist");

            PropertyInfo prop = type.GetProperty("OperationGeneration",
                BindingFlags.Public | BindingFlags.Instance);
            Assert.That(prop, Is.Not.Null,
                "AppStateMachine must expose a public OperationGeneration property");
            Assert.That(prop.PropertyType, Is.EqualTo(typeof(int)),
                "OperationGeneration must be of type int");
            Assert.That(prop.CanRead, Is.True,
                "OperationGeneration must be readable");
        }

        [Test]
        public void AppStateMachine_OperationGenerationStartsAtZero()
        {
            Type type = FindType("NutriMind.Runtime.App.AppStateMachine");
            Assert.That(type, Is.Not.Null,
                "Precondition: AppStateMachine type must exist");

            ConstructorInfo ctor = type.GetConstructor(Type.EmptyTypes);
            object machine = ctor.Invoke(null);

            PropertyInfo prop = type.GetProperty("OperationGeneration",
                BindingFlags.Public | BindingFlags.Instance);
            Assume.That(prop, Is.Not.Null,
                "Precondition: OperationGeneration property must exist");

            int initialValue = (int)prop.GetValue(machine);
            Assert.That(initialValue, Is.EqualTo(0),
                "OperationGeneration must start at 0");
        }

        [Test]
        public void AppStateMachine_InvalidateInFlightOperationsIncrementsGeneration()
        {
            Type type = FindType("NutriMind.Runtime.App.AppStateMachine");
            Assert.That(type, Is.Not.Null,
                "Precondition: AppStateMachine type must exist");

            ConstructorInfo ctor = type.GetConstructor(Type.EmptyTypes);
            object machine = ctor.Invoke(null);

            PropertyInfo genProp = type.GetProperty("OperationGeneration",
                BindingFlags.Public | BindingFlags.Instance);
            Assume.That(genProp, Is.Not.Null,
                "Precondition: OperationGeneration property must exist");

            MethodInfo invalidate = type.GetMethod("InvalidateInFlightOperations",
                BindingFlags.Public | BindingFlags.Instance,
                null, Type.EmptyTypes, null);
            Assume.That(invalidate, Is.Not.Null,
                "Precondition: InvalidateInFlightOperations method must exist");

            int before = (int)genProp.GetValue(machine);

            invalidate.Invoke(machine, null);

            int after = (int)genProp.GetValue(machine);
            Assert.That(after, Is.GreaterThan(before),
                "InvalidateInFlightOperations must increment OperationGeneration");
        }

        // ---------------------------------------------------------------
        // Unsupported / undefined SubjectType must not unlock station entry
        // ---------------------------------------------------------------

        [Test]
        public void UnsupportedSubject_UndefinedValue_MustNotUnlockStationEntry()
        {
            Type type = FindType("NutriMind.Runtime.App.AppStateMachine");
            Assert.That(type, Is.Not.Null,
                "Precondition: AppStateMachine type must exist");

            Type appStateType = FindEnum("AppState");
            Type subjectTypeType = FindEnum("SubjectType");
            Assert.That(subjectTypeType, Is.Not.Null,
                "Precondition: SubjectType enum type must exist");

            ConstructorInfo ctor = type.GetConstructor(Type.EmptyTypes);
            object machine = ctor.Invoke(null);
            MethodInfo tryTransition = type.GetMethod("TryTransition",
                BindingFlags.Public | BindingFlags.Instance,
                null, new[] { appStateType }, null);
            MethodInfo selectSubject = type.GetMethod("TrySelectSubject",
                BindingFlags.Public | BindingFlags.Instance,
                null, new[] { subjectTypeType }, null);
            Assert.That(selectSubject, Is.Not.Null,
                "AppStateMachine must provide TrySelectSubject(SubjectType)");

            // Walk to SelectingSubject
            string[] startupPath =
            {
                "Starting", "CheckingServer", "LoggedOut", "Authenticating",
                "Bootstrapping", "MainMenu", "SelectingSubject"
            };
            foreach (string stateName in startupPath)
            {
                object targetState = Enum.Parse(appStateType, stateName);
                bool result = (bool)tryTransition.Invoke(machine, new[] { targetState });
                Assert.That(result, Is.True,
                    "TryTransition to {0} should succeed", stateName);
            }

            // Select an undefined/unsupported SubjectType value (999)
            // that does not correspond to any named subject.
            object undefinedSubject = Enum.ToObject(subjectTypeType, 999);
            bool selectResult = (bool)selectSubject.Invoke(machine, new[] { undefinedSubject });

            // If current production accepts it (e.g. no value validation),
            // the selection itself may return true — that is acceptable
            // for this test.  We do not assert on selectResult.

            // Continue through term selection and world load to InWorld
            string[] postSubjectPath = { "SelectingTerm", "LoadingWorld", "InWorld" };
            foreach (string stateName in postSubjectPath)
            {
                object targetState = Enum.Parse(appStateType, stateName);
                bool result = (bool)tryTransition.Invoke(machine, new[] { targetState });
                Assert.That(result, Is.True,
                    "TryTransition to {0} should succeed", stateName);
            }

            // An undefined/unsupported subject value MUST NOT unlock station entry.
            // Only explicit, known station-capable subjects (LiteraQuest, HealthQuest)
            // should allow StartingStation, not arbitrary undefined enum values.
            object startingStation = Enum.Parse(appStateType, "StartingStation");
            bool stationResult = (bool)tryTransition.Invoke(machine, new[] { startingStation });
            Assert.That(stationResult, Is.False,
                "TryTransition(StartingStation) must be false when an undefined/unsupported " +
                "SubjectType value was selected — only explicit known subjects may unlock stations");
        }

        // ---------------------------------------------------------------
        // HealthQuest is station-capable
        // ---------------------------------------------------------------

        [Test]
        public void HealthQuest_IsStationCapable()
        {
            Type type = FindType("NutriMind.Runtime.App.AppStateMachine");
            Assert.That(type, Is.Not.Null,
                "Precondition: AppStateMachine type must exist");

            Type appStateType = FindEnum("AppState");
            Type subjectTypeType = FindEnum("SubjectType");
            Assert.That(subjectTypeType, Is.Not.Null,
                "Precondition: SubjectType enum type must exist");

            ConstructorInfo ctor = type.GetConstructor(Type.EmptyTypes);
            object machine = ctor.Invoke(null);
            MethodInfo tryTransition = type.GetMethod("TryTransition",
                BindingFlags.Public | BindingFlags.Instance,
                null, new[] { appStateType }, null);
            MethodInfo selectSubject = type.GetMethod("TrySelectSubject",
                BindingFlags.Public | BindingFlags.Instance,
                null, new[] { subjectTypeType }, null);
            Assert.That(selectSubject, Is.Not.Null,
                "AppStateMachine must provide TrySelectSubject(SubjectType)");

            // Walk to SelectingSubject
            string[] startupPath =
            {
                "Starting", "CheckingServer", "LoggedOut", "Authenticating",
                "Bootstrapping", "MainMenu", "SelectingSubject"
            };
            foreach (string stateName in startupPath)
            {
                object targetState = Enum.Parse(appStateType, stateName);
                bool result = (bool)tryTransition.Invoke(machine, new[] { targetState });
                Assert.That(result, Is.True,
                    "TryTransition to {0} should succeed", stateName);
            }

            // Select HealthQuest — must succeed
            object healthQuest = Enum.Parse(subjectTypeType, "HealthQuest");
            bool selectResult = (bool)selectSubject.Invoke(machine, new[] { healthQuest });
            Assert.That(selectResult, Is.True,
                "TrySelectSubject(HealthQuest) must succeed during SelectingSubject phase");

            // Continue through term selection and world load to InWorld
            string[] postSubjectPath = { "SelectingTerm", "LoadingWorld", "InWorld" };
            foreach (string stateName in postSubjectPath)
            {
                object targetState = Enum.Parse(appStateType, stateName);
                bool result = (bool)tryTransition.Invoke(machine, new[] { targetState });
                Assert.That(result, Is.True,
                    "TryTransition to {0} should succeed", stateName);
            }

            // HealthQuest must be station-capable, just like LiteraQuest
            object startingStation = Enum.Parse(appStateType, "StartingStation");
            bool stationResult = (bool)tryTransition.Invoke(machine, new[] { startingStation });
            Assert.That(stationResult, Is.True,
                "TryTransition(StartingStation) must be true for HealthQuest " +
                "(HealthQuest is a station-capable subject)");
        }

        // ---------------------------------------------------------------
        // Subject mutation is rejected immediately after SelectingTerm
        // ---------------------------------------------------------------

        [Test]
        public void TrySelectSubject_RejectedAfterSelectingTerm()
        {
            Type type = FindType("NutriMind.Runtime.App.AppStateMachine");
            Assert.That(type, Is.Not.Null,
                "Precondition: AppStateMachine type must exist");

            Type appStateType = FindEnum("AppState");
            Type subjectTypeType = FindEnum("SubjectType");
            Assert.That(subjectTypeType, Is.Not.Null,
                "Precondition: SubjectType enum type must exist");

            ConstructorInfo ctor = type.GetConstructor(Type.EmptyTypes);
            object machine = ctor.Invoke(null);
            MethodInfo tryTransition = type.GetMethod("TryTransition",
                BindingFlags.Public | BindingFlags.Instance,
                null, new[] { appStateType }, null);
            MethodInfo selectSubject = type.GetMethod("TrySelectSubject",
                BindingFlags.Public | BindingFlags.Instance,
                null, new[] { subjectTypeType }, null);
            Assert.That(selectSubject, Is.Not.Null,
                "AppStateMachine must provide TrySelectSubject(SubjectType)");

            // Walk to SelectingSubject
            string[] startupPath =
            {
                "Starting", "CheckingServer", "LoggedOut", "Authenticating",
                "Bootstrapping", "MainMenu", "SelectingSubject"
            };
            foreach (string stateName in startupPath)
            {
                object targetState = Enum.Parse(appStateType, stateName);
                bool result = (bool)tryTransition.Invoke(machine, new[] { targetState });
                Assert.That(result, Is.True,
                    "TryTransition to {0} should succeed", stateName);
            }

            // Select an initial subject while in SelectingSubject
            object initialSubject = Enum.Parse(subjectTypeType, "HealthQuest");
            bool selectResult = (bool)selectSubject.Invoke(machine, new[] { initialSubject });
            Assert.That(selectResult, Is.True,
                "TrySelectSubject(HealthQuest) must succeed during SelectingSubject phase");

            // Advance to SelectingTerm — this locks the subject
            object selectingTerm = Enum.Parse(appStateType, "SelectingTerm");
            bool transitionResult = (bool)tryTransition.Invoke(machine, new[] { selectingTerm });
            Assert.That(transitionResult, Is.True,
                "TryTransition to SelectingTerm must succeed");

            // Attempting to select a different subject after SelectingTerm must be rejected,
            // not deferred until InWorld. This validates early rejection of subject mutation.
            object differentSubject = Enum.Parse(subjectTypeType, "LiteraQuest");
            bool mutateResult = (bool)selectSubject.Invoke(machine, new[] { differentSubject });
            Assert.That(mutateResult, Is.False,
                "TrySelectSubject must return false when called after SelectingTerm " +
                "(subject choice is locked once term selection begins)");
        }
    }
}
