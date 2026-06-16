using System;
using System.Linq;
using NUnit.Framework;
using NutriMind.Runtime.App;

namespace NutriMind.Tests.EditMode.App
{
    /// <summary>
    /// EditMode tests for Phase 04A State-Flow Compatibility.
    /// Covers the four new AppState values, LearningCyclePhase enum,
    /// and all new transition paths added to AppStateMachine.
    /// </summary>
    [TestFixture]
    public class NutriMindPhase04AStateFlowTests
    {
        // ---------------------------------------------------------------
        // New AppState values exist and are discoverable
        // ---------------------------------------------------------------

        [Test]
        public void AppState_HasFourNewValues()
        {
            string[] newNames =
            {
                "ShowingMissionBrief",
                "ShowingHintOverlay",
                "ShowingReflection",
                "ShowingRewardPresentation"
            };

            string[] allNames = Enum.GetNames(typeof(AppState));

            foreach (string name in newNames)
            {
                Assert.That(allNames, Does.Contain(name),
                    "AppState must contain new value: {0}", name);
            }
        }

        [Test]
        public void AppState_ContainsAtLeastBaseline25Values()
        {
            string[] baselineNames =
            {
                "Starting", "CheckingServer", "MaintenanceBlocked", "UpdateRequired",
                "LoggedOut", "Authenticating", "Bootstrapping", "MainMenu",
                "SelectingSubject", "SelectingTerm", "LoadingWorld", "InWorld",
                "StartingStation", "InStationTask", "SubmittingAttempt", "ShowingFeedback",
                "RefreshingProgress", "ReturningToWorld",
                "ShowingMissionBrief", "ShowingHintOverlay", "ShowingReflection", "ShowingRewardPresentation",
                "SessionExpired", "ConnectionUnavailable", "FatalConfigurationError"
            };

            string[] allNames = Enum.GetNames(typeof(AppState));
            int count = allNames.Length;

            Assert.That(count, Is.GreaterThanOrEqualTo(baselineNames.Length),
                "AppState must define at least {0} baseline values", baselineNames.Length);

            foreach (string name in baselineNames)
            {
                Assert.That(allNames, Does.Contain(name),
                    "AppState must contain baseline value: {0}", name);
            }
        }

        [Test]
        public void AppState_NewValuesExistAndHaveUniqueOrdinals()
        {
            string[] newNames =
            {
                "ShowingMissionBrief",
                "ShowingHintOverlay",
                "ShowingReflection",
                "ShowingRewardPresentation"
            };

            string[] allNames = Enum.GetNames(typeof(AppState));
            var seen = new System.Collections.Generic.HashSet<int>();

            foreach (string name in newNames)
            {
                Assert.That(allNames, Does.Contain(name),
                    "AppState must contain new Phase 4A value: {0}", name);
            }

            foreach (int value in Enum.GetValues(typeof(AppState)))
            {
                Assert.That(seen.Add(value), Is.True,
                    "AppState value {0} is duplicated; enum ordinals must be unique", value);
            }
        }

        // ---------------------------------------------------------------
        // LearningCyclePhase enum
        // ---------------------------------------------------------------

        [Test]
        public void LearningCyclePhase_TypeExists()
        {
            Type type = typeof(LearningCyclePhase);
            Assert.That(type, Is.Not.Null,
                "LearningCyclePhase enum must exist in NutriMind.Runtime.App");
            Assert.That(type.IsEnum, Is.True,
                "LearningCyclePhase must be an enum type");
        }

        [Test]
        public void LearningCyclePhase_HasFourPhases()
        {
            string[] expectedNames = { "Discover", "Practice", "Apply", "Review" };
            string[] actualNames = Enum.GetNames(typeof(LearningCyclePhase));

            Assert.That(actualNames, Is.EquivalentTo(expectedNames),
                "LearningCyclePhase must define exactly Discover, Practice, Apply, Review");
        }

        // ---------------------------------------------------------------
        // Mission brief path
        // ---------------------------------------------------------------

        [Test]
        public void MissionBriefPath_StartsFromStartingStation()
        {
            var machine = new AppStateMachine();
            WalkToState(machine, AppState.SelectingSubject);
            machine.TrySelectSubject(SubjectType.LiteraQuest);
            WalkToState(machine, AppState.StartingStation);

            Assert.That(machine.TryTransition(AppState.ShowingMissionBrief), Is.True,
                "StartingStation -> ShowingMissionBrief must be valid");
        }

        [Test]
        public void MissionBriefPath_SelfLoopIsValid()
        {
            var machine = new AppStateMachine();
            WalkToState(machine, AppState.SelectingSubject);
            machine.TrySelectSubject(SubjectType.LiteraQuest);
            WalkToState(machine, AppState.StartingStation);
            machine.TryTransition(AppState.ShowingMissionBrief);

            Assert.That(machine.TryTransition(AppState.ShowingMissionBrief), Is.True,
                "ShowingMissionBrief self-loop must be valid");
        }

        [Test]
        public void MissionBriefPath_CanEnterInStationTask()
        {
            var machine = new AppStateMachine();
            WalkToState(machine, AppState.SelectingSubject);
            machine.TrySelectSubject(SubjectType.LiteraQuest);
            WalkToState(machine, AppState.StartingStation);
            machine.TryTransition(AppState.ShowingMissionBrief);

            Assert.That(machine.TryTransition(AppState.InStationTask), Is.True,
                "ShowingMissionBrief -> InStationTask must be valid");
        }

        [Test]
        public void MissionBriefPath_CanExpireSession()
        {
            var machine = new AppStateMachine();
            WalkToState(machine, AppState.SelectingSubject);
            machine.TrySelectSubject(SubjectType.LiteraQuest);
            WalkToState(machine, AppState.StartingStation);
            machine.TryTransition(AppState.ShowingMissionBrief);

            Assert.That(machine.TryTransition(AppState.SessionExpired), Is.True,
                "ShowingMissionBrief -> SessionExpired must be valid");
        }

        [Test]
        public void MissionBriefPath_DirectToInStationTask_StillValid()
        {
            var machine = new AppStateMachine();
            WalkToState(machine, AppState.SelectingSubject);
            machine.TrySelectSubject(SubjectType.LiteraQuest);
            WalkToState(machine, AppState.StartingStation);

            Assert.That(machine.TryTransition(AppState.InStationTask), Is.True,
                "StartingStation -> InStationTask direct path must still be valid");
        }

        // ---------------------------------------------------------------
        // Hint overlay path
        // ---------------------------------------------------------------

        [Test]
        public void HintOverlayPath_StartsFromInStationTask()
        {
            var machine = new AppStateMachine();
            WalkToState(machine, AppState.SelectingSubject);
            machine.TrySelectSubject(SubjectType.LiteraQuest);
            WalkToState(machine, AppState.InStationTask);

            Assert.That(machine.TryTransition(AppState.ShowingHintOverlay), Is.True,
                "InStationTask -> ShowingHintOverlay must be valid");
        }

        [Test]
        public void HintOverlayPath_SelfLoopIsValid()
        {
            var machine = new AppStateMachine();
            WalkToState(machine, AppState.SelectingSubject);
            machine.TrySelectSubject(SubjectType.LiteraQuest);
            WalkToState(machine, AppState.InStationTask);
            machine.TryTransition(AppState.ShowingHintOverlay);

            Assert.That(machine.TryTransition(AppState.ShowingHintOverlay), Is.True,
                "ShowingHintOverlay self-loop must be valid");
        }

        [Test]
        public void HintOverlayPath_CanReturnToInStationTask()
        {
            var machine = new AppStateMachine();
            WalkToState(machine, AppState.SelectingSubject);
            machine.TrySelectSubject(SubjectType.LiteraQuest);
            WalkToState(machine, AppState.InStationTask);
            machine.TryTransition(AppState.ShowingHintOverlay);

            Assert.That(machine.TryTransition(AppState.InStationTask), Is.True,
                "ShowingHintOverlay -> InStationTask must be valid");
        }

        [Test]
        public void HintOverlayPath_CanExpireSession()
        {
            var machine = new AppStateMachine();
            WalkToState(machine, AppState.SelectingSubject);
            machine.TrySelectSubject(SubjectType.LiteraQuest);
            WalkToState(machine, AppState.InStationTask);
            machine.TryTransition(AppState.ShowingHintOverlay);

            Assert.That(machine.TryTransition(AppState.SessionExpired), Is.True,
                "ShowingHintOverlay -> SessionExpired must be valid");
        }

        // ---------------------------------------------------------------
        // Retry path: ShowingFeedback -> InStationTask
        // ---------------------------------------------------------------

        [Test]
        public void RetryPath_ShowingFeedbackToInStationTask()
        {
            var machine = new AppStateMachine();
            WalkToState(machine, AppState.SelectingSubject);
            machine.TrySelectSubject(SubjectType.LiteraQuest);
            WalkToState(machine, AppState.ShowingFeedback);

            Assert.That(machine.TryTransition(AppState.InStationTask), Is.True,
                "ShowingFeedback -> InStationTask retry path must be valid");
        }

        // ---------------------------------------------------------------
        // Reflection / reward path
        // ---------------------------------------------------------------

        [Test]
        public void ReflectionPath_ShowingFeedbackToShowingReflection()
        {
            var machine = new AppStateMachine();
            WalkToState(machine, AppState.SelectingSubject);
            machine.TrySelectSubject(SubjectType.LiteraQuest);
            WalkToState(machine, AppState.ShowingFeedback);

            Assert.That(machine.TryTransition(AppState.ShowingReflection), Is.True,
                "ShowingFeedback -> ShowingReflection must be valid");
        }

        [Test]
        public void ReflectionPath_SelfLoopIsValid()
        {
            var machine = new AppStateMachine();
            WalkToState(machine, AppState.SelectingSubject);
            machine.TrySelectSubject(SubjectType.LiteraQuest);
            WalkToState(machine, AppState.ShowingReflection);

            Assert.That(machine.TryTransition(AppState.ShowingReflection), Is.True,
                "ShowingReflection self-loop must be valid");
        }

        [Test]
        public void ReflectionPath_CanGoToRewardPresentation()
        {
            var machine = new AppStateMachine();
            WalkToState(machine, AppState.SelectingSubject);
            machine.TrySelectSubject(SubjectType.LiteraQuest);
            WalkToState(machine, AppState.ShowingReflection);

            Assert.That(machine.TryTransition(AppState.ShowingRewardPresentation), Is.True,
                "ShowingReflection -> ShowingRewardPresentation must be valid");
        }

        [Test]
        public void ReflectionPath_CanGoToRefreshingProgress()
        {
            var machine = new AppStateMachine();
            WalkToState(machine, AppState.SelectingSubject);
            machine.TrySelectSubject(SubjectType.LiteraQuest);
            WalkToState(machine, AppState.ShowingReflection);

            Assert.That(machine.TryTransition(AppState.RefreshingProgress), Is.True,
                "ShowingReflection -> RefreshingProgress must be valid");
        }

        [Test]
        public void ReflectionPath_CanExpireSession()
        {
            var machine = new AppStateMachine();
            WalkToState(machine, AppState.SelectingSubject);
            machine.TrySelectSubject(SubjectType.LiteraQuest);
            WalkToState(machine, AppState.ShowingReflection);

            Assert.That(machine.TryTransition(AppState.SessionExpired), Is.True,
                "ShowingReflection -> SessionExpired must be valid");
        }

        [Test]
        public void RewardPresentationPath_SelfLoopIsValid()
        {
            var machine = new AppStateMachine();
            WalkToState(machine, AppState.SelectingSubject);
            machine.TrySelectSubject(SubjectType.LiteraQuest);
            WalkToState(machine, AppState.ShowingReflection);
            machine.TryTransition(AppState.ShowingRewardPresentation);

            Assert.That(machine.TryTransition(AppState.ShowingRewardPresentation), Is.True,
                "ShowingRewardPresentation self-loop must be valid");
        }

        [Test]
        public void RewardPresentationPath_CanGoToRefreshingProgress()
        {
            var machine = new AppStateMachine();
            WalkToState(machine, AppState.SelectingSubject);
            machine.TrySelectSubject(SubjectType.LiteraQuest);
            WalkToState(machine, AppState.ShowingReflection);
            machine.TryTransition(AppState.ShowingRewardPresentation);

            Assert.That(machine.TryTransition(AppState.RefreshingProgress), Is.True,
                "ShowingRewardPresentation -> RefreshingProgress must be valid");
        }

        [Test]
        public void RewardPresentationPath_CanExpireSession()
        {
            var machine = new AppStateMachine();
            WalkToState(machine, AppState.SelectingSubject);
            machine.TrySelectSubject(SubjectType.LiteraQuest);
            WalkToState(machine, AppState.ShowingReflection);
            machine.TryTransition(AppState.ShowingRewardPresentation);

            Assert.That(machine.TryTransition(AppState.SessionExpired), Is.True,
                "ShowingRewardPresentation -> SessionExpired must be valid");
        }

        [Test]
        public void FeedbackPath_DirectToRefreshingProgress_StillValid()
        {
            var machine = new AppStateMachine();
            WalkToState(machine, AppState.SelectingSubject);
            machine.TrySelectSubject(SubjectType.LiteraQuest);
            WalkToState(machine, AppState.ShowingFeedback);

            Assert.That(machine.TryTransition(AppState.RefreshingProgress), Is.True,
                "ShowingFeedback -> RefreshingProgress direct path must still be valid");
        }

        // ---------------------------------------------------------------
        // Invalid shortcuts are rejected
        // ---------------------------------------------------------------

        [Test]
        public void InvalidShortcut_ShowingMissionBriefToShowingFeedback()
        {
            var machine = new AppStateMachine();
            WalkToState(machine, AppState.SelectingSubject);
            machine.TrySelectSubject(SubjectType.LiteraQuest);
            WalkToState(machine, AppState.StartingStation);
            machine.TryTransition(AppState.ShowingMissionBrief);

            Assert.That(machine.TryTransition(AppState.ShowingFeedback), Is.False,
                "ShowingMissionBrief -> ShowingFeedback must be rejected");
        }

        [Test]
        public void InvalidShortcut_ShowingHintOverlayToShowingReflection()
        {
            var machine = new AppStateMachine();
            WalkToState(machine, AppState.SelectingSubject);
            machine.TrySelectSubject(SubjectType.LiteraQuest);
            WalkToState(machine, AppState.InStationTask);
            machine.TryTransition(AppState.ShowingHintOverlay);

            Assert.That(machine.TryTransition(AppState.ShowingReflection), Is.False,
                "ShowingHintOverlay -> ShowingReflection must be rejected");
        }

        [Test]
        public void InvalidShortcut_ShowingReflectionToInStationTask()
        {
            var machine = new AppStateMachine();
            WalkToState(machine, AppState.SelectingSubject);
            machine.TrySelectSubject(SubjectType.LiteraQuest);
            WalkToState(machine, AppState.ShowingReflection);

            Assert.That(machine.TryTransition(AppState.InStationTask), Is.False,
                "ShowingReflection -> InStationTask must be rejected");
        }

        [Test]
        public void InvalidShortcut_ShowingRewardPresentationToInStationTask()
        {
            var machine = new AppStateMachine();
            WalkToState(machine, AppState.SelectingSubject);
            machine.TrySelectSubject(SubjectType.LiteraQuest);
            WalkToState(machine, AppState.ShowingReflection);
            machine.TryTransition(AppState.ShowingRewardPresentation);

            Assert.That(machine.TryTransition(AppState.InStationTask), Is.False,
                "ShowingRewardPresentation -> InStationTask must be rejected");
        }

        [Test]
        public void InvalidShortcut_ShowingRewardPresentationToShowingHintOverlay()
        {
            var machine = new AppStateMachine();
            WalkToState(machine, AppState.SelectingSubject);
            machine.TrySelectSubject(SubjectType.LiteraQuest);
            WalkToState(machine, AppState.ShowingReflection);
            machine.TryTransition(AppState.ShowingRewardPresentation);

            Assert.That(machine.TryTransition(AppState.ShowingHintOverlay), Is.False,
                "ShowingRewardPresentation -> ShowingHintOverlay must be rejected");
        }

        [Test]
        public void InvalidShortcut_ShowingRewardPresentationToShowingReflection()
        {
            var machine = new AppStateMachine();
            WalkToState(machine, AppState.SelectingSubject);
            machine.TrySelectSubject(SubjectType.LiteraQuest);
            WalkToState(machine, AppState.ShowingReflection);
            machine.TryTransition(AppState.ShowingRewardPresentation);

            Assert.That(machine.TryTransition(AppState.ShowingReflection), Is.False,
                "ShowingRewardPresentation -> ShowingReflection must be rejected");
        }

        // ---------------------------------------------------------------
        // ScienceQuest still cannot enter station flow
        // ---------------------------------------------------------------

        [Test]
        public void ScienceQuest_StillCannotEnterStartingStation()
        {
            var machine = new AppStateMachine();
            WalkToState(machine, AppState.SelectingSubject);
            machine.TrySelectSubject(SubjectType.ScienceQuest);
            WalkToState(machine, AppState.InWorld);

            Assert.That(machine.TryTransition(AppState.StartingStation), Is.False,
                "ScienceQuest must still reject StartingStation transition");
        }

        [Test]
        public void ScienceQuest_CannotEnterShowingMissionBrief()
        {
            var machine = new AppStateMachine();
            WalkToState(machine, AppState.SelectingSubject);
            machine.TrySelectSubject(SubjectType.ScienceQuest);
            WalkToState(machine, AppState.InWorld);

            Assert.That(machine.TryTransition(AppState.ShowingMissionBrief), Is.False,
                "ScienceQuest must reject ShowingMissionBrief transition " +
                "(cannot reach it without passing StartingStation gate)");
        }

        // ---------------------------------------------------------------
        // Full flow with all new states
        // ---------------------------------------------------------------

        [Test]
        public void FullFlow_WithAllNewStates_LiteraQuest()
        {
            var machine = new AppStateMachine();
            WalkToState(machine, AppState.SelectingSubject);
            machine.TrySelectSubject(SubjectType.LiteraQuest);

            // World entry
            AssertTransition(machine, AppState.SelectingTerm);
            AssertTransition(machine, AppState.LoadingWorld);
            AssertTransition(machine, AppState.InWorld);

            // Station entry with mission brief
            AssertTransition(machine, AppState.StartingStation);
            AssertTransition(machine, AppState.ShowingMissionBrief);
            AssertTransition(machine, AppState.InStationTask);

            // Hint overlay during task
            AssertTransition(machine, AppState.ShowingHintOverlay);
            AssertTransition(machine, AppState.InStationTask);

            // Submit and get feedback
            AssertTransition(machine, AppState.SubmittingAttempt);
            AssertTransition(machine, AppState.ShowingFeedback);

            // Reflection and reward
            AssertTransition(machine, AppState.ShowingReflection);
            AssertTransition(machine, AppState.ShowingRewardPresentation);

            // Progress refresh and return
            AssertTransition(machine, AppState.RefreshingProgress);
            AssertTransition(machine, AppState.ReturningToWorld);
            AssertTransition(machine, AppState.InWorld);

            Assert.That(machine.CurrentState, Is.EqualTo(AppState.InWorld),
                "Full flow with all new states must end in InWorld");
        }

        [Test]
        public void FullFlow_WithRetryPath_LiteraQuest()
        {
            var machine = new AppStateMachine();
            WalkToState(machine, AppState.SelectingSubject);
            machine.TrySelectSubject(SubjectType.LiteraQuest);

            AssertTransition(machine, AppState.SelectingTerm);
            AssertTransition(machine, AppState.LoadingWorld);
            AssertTransition(machine, AppState.InWorld);
            AssertTransition(machine, AppState.StartingStation);
            AssertTransition(machine, AppState.InStationTask);
            AssertTransition(machine, AppState.SubmittingAttempt);
            AssertTransition(machine, AppState.ShowingFeedback);

            // Retry: go back to task
            AssertTransition(machine, AppState.InStationTask);

            // Re-submit
            AssertTransition(machine, AppState.SubmittingAttempt);
            AssertTransition(machine, AppState.ShowingFeedback);

            // Now continue to reflection
            AssertTransition(machine, AppState.ShowingReflection);
            AssertTransition(machine, AppState.RefreshingProgress);
            AssertTransition(machine, AppState.ReturningToWorld);

            Assert.That(machine.CurrentState, Is.EqualTo(AppState.ReturningToWorld),
                "Retry flow must end in ReturningToWorld");
        }

        [Test]
        public void FullFlow_SkipOptionalStates_DirectPathStillWorks()
        {
            var machine = new AppStateMachine();
            WalkToState(machine, AppState.SelectingSubject);
            machine.TrySelectSubject(SubjectType.LiteraQuest);

            // Old direct path without optional states
            AssertTransition(machine, AppState.SelectingTerm);
            AssertTransition(machine, AppState.LoadingWorld);
            AssertTransition(machine, AppState.InWorld);
            AssertTransition(machine, AppState.StartingStation);
            AssertTransition(machine, AppState.InStationTask);
            AssertTransition(machine, AppState.SubmittingAttempt);
            AssertTransition(machine, AppState.ShowingFeedback);
            AssertTransition(machine, AppState.RefreshingProgress);
            AssertTransition(machine, AppState.ReturningToWorld);

            Assert.That(machine.CurrentState, Is.EqualTo(AppState.ReturningToWorld),
                "Direct path without optional states must still work");
        }

        // ---------------------------------------------------------------
        // Helpers
        // ---------------------------------------------------------------

        /// <summary>
        /// Walks the state machine along its canonical happy path
        /// (<see cref="NextOnHappyPath"/>) until it reaches <paramref name="target"/>.
        /// This follows only the direct progression — it does not branch into
        /// optional states that are not on the happy path chain.
        /// Branch-only states such as <see cref="AppState.ShowingRewardPresentation"/>
        /// should be reached by transitioning from their parent state after
        /// WalkToState completes (e.g. WalkToState to ShowingReflection, then
        /// Transition to ShowingRewardPresentation).
        /// </summary>
        private static void WalkToState(AppStateMachine machine, AppState target)
        {
            AppState current = machine.CurrentState;
            int guard = 0;
            while (current != target)
            {
                if (++guard > 100)
                    throw new InvalidOperationException(
                        $"Cycle guard triggered: cannot reach {target} from {current} after 100 steps. " +
                        $"The target may be on an optional branch not covered by NextOnHappyPath.");

                bool moved = machine.TryTransition(target);
                if (moved)
                    break;

                // Advance one step along the canonical happy path
                AppState next = NextOnHappyPath(current);
                if (next == current)
                    throw new InvalidOperationException(
                        $"Cannot reach {target} from {current}: no forward transition defined.");

                bool ok = machine.TryTransition(next);
                if (!ok)
                    throw new InvalidOperationException(
                        $"Transition from {current} to {next} was rejected.");

                current = machine.CurrentState;
            }
        }

        private static AppState NextOnHappyPath(AppState current)
        {
            return current switch
            {
                AppState.Starting => AppState.CheckingServer,
                AppState.CheckingServer => AppState.LoggedOut,
                AppState.LoggedOut => AppState.Authenticating,
                AppState.Authenticating => AppState.Bootstrapping,
                AppState.Bootstrapping => AppState.MainMenu,
                AppState.MainMenu => AppState.SelectingSubject,
                AppState.SelectingSubject => AppState.SelectingTerm,
                AppState.SelectingTerm => AppState.LoadingWorld,
                AppState.LoadingWorld => AppState.InWorld,
                AppState.InWorld => AppState.StartingStation,
                AppState.StartingStation => AppState.InStationTask,
                AppState.InStationTask => AppState.SubmittingAttempt,
                AppState.SubmittingAttempt => AppState.ShowingFeedback,
                AppState.ShowingFeedback => AppState.RefreshingProgress,
                AppState.RefreshingProgress => AppState.ReturningToWorld,
                AppState.ReturningToWorld => AppState.InWorld,
                _ => current
            };
        }

        private static void AssertTransition(AppStateMachine machine, AppState target)
        {
            bool result = machine.TryTransition(target);
            Assert.That(result, Is.True,
                "Transition to {0} should succeed", target);
        }
    }
}
