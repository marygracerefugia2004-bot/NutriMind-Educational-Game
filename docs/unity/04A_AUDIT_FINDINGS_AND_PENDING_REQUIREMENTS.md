# Phase 4A Audit Findings and Pending Requirements

**Status**: Audit findings recorded; source/test fixes applied. Static build validation (`dotnet build`) passed with 0 errors for `NutriMind.Runtime.App.csproj`, `NutriMind.Tests.EditMode.App.csproj`, and `NutriMind.Tests.EditMode.UI.csproj`. Unity MCP compilation check (`unity_get_compilation_errors`) confirmed zero errors (count=0, isCompiling=false). Unity Test Runner EditMode execution succeeded across two runs (both 434/434 passed, 0 failed, 0 skipped; durations 4.56s and 4.1s). PlayMode tests and Coplay visual smoke were not run and remain manual/pending.

**Date**: 2026-06-13

---

## 1. Scope Statement

This document records the verified audit findings, fixes applied, and pending requirements from the **Phase 4A learning-through-gameplay design integration audit** only.

The following were not created by this unit and remain out of scope:

- Application scenes beyond `Splash.unity`
- Final UI layouts for worlds, stations, or screens
- 3D world environments, station scene content, or NPC models
- Visual assets (textures, icons, sprites, character models, animations, audio)
- `HttpProvider` implementation (placeholder remains)
- `LocalDemoJsonProvider` implementation (placeholder remains)
- Production JSON parser infrastructure
- Demo JSON fixtures or content files

---

## 2. Compatibility Verdict Table

| Area | Verdict | Evidence |
|------|---------|----------|
| **Game flow / state machine** | Compatible with additive mission/hint/reflection/reward transitions. | `AppStateMachine` defines `ShowingMissionBrief`, `ShowingHintOverlay`, `ShowingReflection`, `ShowingRewardPresentation` with forward/backward/skip/self-loop/session-expiry transitions. Full-flow test covers all new state transitions (`NutriMindPhase04AStateFlowTests.FullFlow_WithAllNewStates_LiteraQuest`); `dotnet build` passed with 0 errors; EditMode Test Runner passed (434/434). |
| **ScienceQuest cannot enter station branch** | Confirmed blocked by subject guard. | `AppStateMachine.TryTransition` rejects `StartingStation` when `_selectedSubject` is `ScienceQuest`. Tests `ScienceQuest_StillCannotEnterStartingStation` and `ScienceQuest_CannotEnterShowingMissionBrief` cover this guard; `dotnet build` passed with 0 errors; EditMode Test Runner passed (434/434). |
| **LearningCyclePhase representation** | Discover/Practice/Apply/Review represented as enum, not duplicated app states. | `LearningCyclePhase` enum in `AppEnums.cs` has exactly four values. `AppStateMachine` does not contain equivalent states. `InteractionStateStore.CurrentPhase` stores the active phase locally. |
| **Attempt / retry authority** | Compatible — `AttemptCoordinator`/`AttemptScope` preserve `client_attempt_uuid` on retry and do not grant scoring/progress/rewards. | `AttemptScope.Retry()` intentionally leaves `ClientAttemptUuid` unchanged. `AttemptCoordinator` is pure lifecycle coordination; no scoring authority. Test `AttemptScope_ClientAttemptUuid_IsStableAcrossRetry` covers UUID stability; `dotnet build` passed with 0 errors; EditMode Test Runner passed (434/434). |
| **Provider ownership** | Compatible — `CompositionRoot` centralizes provider selection. `IGameDataProvider` remains a narrow placeholder pending Phase 04/11. | `CompositionRoot.CreateForMode(mode)` selects `HttpProvider` or `LocalDemoJsonProvider`. Both providers return `DataResult<object>.Fail(...)` for every method. `IGameDataProvider` exposes 5 methods: `GetSubjects`, `GetSubjectProgress`, `SubmitAttempt`, `GetRewards`, `GetProfile`. |
| **DTO / story metadata** | Compatible — `StationNarrative` and all child types are optional/default-safe. Actual JSON parser and tolerant deserialisation pending Phase 04/11. | `StationNarrative` fields are all nullable `string?`, nullable reference types, or default-empty `List<T>`. Tests verify null defaults for optional fields (`StationNarrativeDtoTests`). No production JSON parser exists yet — both providers are placeholders. |
| **Stores** | Compatible — `InteractionStateStore` owns transient narrative/current phase/feedback. `ProgressRewardStore` owns provider-presented progress/reward previews/world restoration state and does not grant official rewards. | `InteractionStateStore` has `CurrentStationNarrative`, `CurrentPhase`, `LastFeedback` with `Reset()` behaviour tested (`InteractionStateStoreExtendedResetTests`). `ProgressRewardStore` has `RestorationState`, `RewardPreviews` with `Reset()` behaviour tested (`ProgressRewardStoreExtendedResetTests`). |
| **UI Toolkit foundation** | Compatible — existing UXML assets and tests cover mission brief, hint, reflection, reward, NPC dialogue, discovery card, feedback panel. No uGUI additions. | All 7 UXML templates exist under `UI/Documents/Overlays/` or `UI/Documents/Components/`. Tests cover asset loading (`NutriMindAssetPathTests`), required named elements (`NutriMindComponentTemplateTests`), and focusable roots for interactive components. |
| **Scene dependencies / cleanup** | Compatible — `SceneRegistry` key-based navigation with null-safe `TryGetScene`. `MainThreadDispatcher` generation invalidation covers late callback cleanup. | `SceneRegistry.TryGetScene(null, out path)` returns `false`/`null` without throwing, covered by test `SceneRegistry_TryGetScene_NullKeyReturnsFalse`. `MainThreadDispatcher.Invalidate()` discards queued callbacks and increments generation counter; generation-bound `Post(int, Action)` drops stale callbacks (covered by `MainThreadDispatcherThreadingTests`); `dotnet build` passed with 0 errors; EditMode Test Runner passed (434/434). |

---

## 3. Fixes Applied During This Audit

### 3.1 AppState tests — removed exact-count and ordinal-locking checks

**Problem**: `NutriMindPhase04AStateFlowTests` contained two tests that created brittle dependencies on the precise shape of the `AppState` enum:
- `AppState_TotalCount_Is25` asserted an exact enum-value count of 25, locking the enum to its current size.
- `AppState_NewValuesPreserveExistingOrdinals` verified specific integer ordinals for the four new `AppState` values (`ShowingMissionBrief`, `ShowingHintOverlay`, `ShowingReflection`, `ShowingRewardPresentation`), contradicting the `AppEnums.cs` contract which explicitly states that `AppState` ordinals are NOT a stable serialization or persistence contract.

**Fix**: Replaced both tests with contract-aligned replacements:

1. **`AppState_ContainsAtLeastBaseline25Values`** asserts that the number of `AppState` values is at least the baseline count of 25 and that all 25 baseline names are present in the enum via `Enum.GetNames`. This avoids an exact-count lock while still guarding against accidental removal of known states.

2. **Combined new-state assertion** verifies that `ShowingMissionBrief`, `ShowingHintOverlay`, `ShowingReflection`, and `ShowingRewardPresentation` exist in `AppState` and that all `AppState` enum numeric values are unique. It does **not** assert specific ordinal values.

Aligned to the documented contract: consumers must persist or refer to states by `Enum.ToString`/`Enum.Parse` or explicit external keys, never by numeric value.

### 3.2 SceneRegistry.TryGetScene(null) defensive guard

**Problem**: `SceneRegistry.TryGetScene(null, out string path)` passed a null key to `Dictionary.TryGetValue`, which would throw `ArgumentNullException`. The registry was designed to handle null gracefully in `GetScene` (returns null) but `TryGetScene` lacked the same guard.

**Fix**: Added explicit null-key guard to `TryGetScene`: when `key == null`, sets `scenePath = null` and returns `false` without calling `_scenes.TryGetValue`. Test `SceneRegistry_TryGetScene_NullKeyReturnsFalse` verifies the behaviour.

### 3.3 NutriMindAssetPaths.OverlayReward and related tests

**Addition**: Added `NutriMindAssetPaths.OverlayReward` constant pointing to `UI/Documents/Overlays/RewardOverlay.uxml`. Asset-load test `RewardOverlay_LoadsAtExpectedPath` verifies the UXML exists. Named-element test `RewardOverlay_HasRequiredNamedElements` verifies `#reward-icon`, `#reward-title`, `#reward-subtitle`, `#reward-amount`, and `#reward-continue-button`.

---

## 4. Pending Phase 04/11 API and Demo-Data Requirements

The following requirements were identified during the audit but are **not satisfied** in the current codebase. They must be addressed in Phase 04 (HTTP/API contract) and Phase 11 (demo data/local provider).

### 4.1 Expand IGameDataProvider surface

The current `IGameDataProvider` has only 5 methods (matching the Phase 03 skeleton). Per `docs/SERVER_REQUIREMENTS.md` the provider must ultimately support:

- **Ping** — connectivity check
- **Config** — fetch client configuration
- **Login / Logout** — authentication lifecycle
- **Bootstrap** — initial session data
- **Settings** — student preferences
- **Subjects** — available subjects and term structure (partial — current `GetSubjects` exists)
- **Terms** — term/world metadata per subject
- **Stations** — station summaries and content for a term
- **Content** — station task content/data (separate from summary)
- **Start** — initialise a station attempt
- **Attempts** — submit attempt payloads (partial — current `SubmitAttempt` exists)
- **Complete** — finalise station completion
- **Progress** — fetch student progress (partial — current `GetSubjectProgress` exists)
- **Rewards** — fetch reward state (partial — current `GetRewards` exists)
- **Sync** — synchronise offline/local progress

Design: add methods incrementally, keep `DataResult<T>` wrapping, avoid adding `DataProviderMode` branches in consumers.

### 4.2 Choose or implement production DTO JSON parser

No JSON serialisation or deserialisation code exists in the project. Requirements:

- Must support **tolerance for unknown optional fields** (server may add new fields without the client crashing).
- Must support **tolerance for unknown enum values** (server may add new station state, subject, or phase values without crashing the client — map to a safe default or ignore).
- **Do not use `UnityEngine.JsonUtility`** with the current property-style DTOs unless the DTO design changes to support `[Serializable]` fields-only classes. `JsonUtility` cannot deserialise into C# auto-properties with `{ get; set; }`.
- Preferred options: `Newtonsoft.Json` (already present in many Unity projects), `System.Text.Json` (with AOT considerations), or a lightweight custom reader that deserialises only known fields.

### 4.3 Populate LocalDemoJson and demo fixtures through shared DTOs

The `LocalDemoJsonProvider` currently returns "not yet implemented" for every call. Phase 11 must:

- Create demo JSON fixture files under `Assets/_Project/Nutrimind/DemoData/` (or the equivalent path used by the provider).
- Deserialise fixtures through the **same DTOs** used by `HttpProvider`.
- Use the **same stores, UI components, scenes, and gameplay systems** as the HTTP path.
- **No production fake fallback** — `LocalDemoJson` must be an explicit development/testing mode, never a silent fallback in release builds.

### 4.4 Ensure server-sourced narrative fields contain only student-safe published metadata

All `StationNarrative` fields (story context, NPC dialogue, hints, discoveries, reflection prompts, reward previews, world restoration state) originate from the active data provider. Unity is a presentation and interaction client. Fields must:

- Contain only student-safe, curriculum-approved published content.
- Not expose answer keys, teacher notes, grading rubrics, or admin data.
- Fail gracefully (null UI, safe placeholder) when the provider returns null/empty narrative data.

---

## 5. Phase 5 World/Station Framework Requirements

The Phase 05 scene/world/station framework must consume the Phase 04A additive types:

- **World restoration**: The world/station framework must read `ProgressRewardStore.RestorationState` after a station completes and apply the visible world state changes (e.g. restored paths, lit lanterns, unlocked portals) only after the provider has confirmed progress/completion.
- **Reward previews**: `ProgressRewardStore.RewardPreviews` may be used for motivational display in-world (e.g. a "rewards earned here" signpost). These are not earned rewards — they are presentation hints.
- **No duplicate stores**: Scene-local reward, progress, or restoration stores must not be created. The shared `ProgressRewardStore` in the composition root is the single source of provider-presented state.
- **Science exploration-preview constraint**: Science quest worlds must not include station portals, station gameplay, station-completion rewards, or collectible progress tracking. The `AppStateMachine` subject guard already blocks the transition; the scene framework must also not register station keys or instantiate station interactables for Science terms.

---

## 6. Phase 6/8 Station Requirements

Every playable LiteraQuest (Phase 06) and PE/Health (Phase 08) station must:

- Implement the four-step **learning-through-gameplay cycle** using `LearningCyclePhase` / `InteractionStateStore.CurrentPhase` and `StationNarrative` metadata:
  1. **Discover** — student encounters the problem.
  2. **Practice** — student performs scaffolded concept interactions.
  3. **Apply** — student uses the concept to complete the station mission.
  4. **Review** — provider returns feedback; Unity shows result, reflection, and reward.
- Use **NPC guidance/hints/reflection** that is concise, provider-approved, and safe-retry oriented. NPC dialogue must not deliver long lectures before gameplay.
- Support **optional discoveries** — provider-driven, never required for station completion. Failure or unavailability must not block the station.
- Submit **attempts through the shared attempt/provider flow** (`AttemptCoordinator` + `IGameDataProvider.SubmitAttempt`). Official correctness, scoring, progress, and rewards are determined only after the provider result is returned.
- Follow the **safe-mistake and hint progression** rules from `docs/unity/04A_LEARNING_THROUGH_GAMEPLAY_STORY_AND_REWARDS.md`.

---

## 7. Play Mode Visual Smoke Checklist

These checks are not automated tests (no Play Mode test infrastructure exists yet). They are intended for the Foreman's final verification report when all phases are complete.

| Check | Instruction |
|-------|-------------|
| Splash scene load | Open `Scenes/App/Splash.unity`, enter Play Mode with `LocalDemoJson` provider. |
| UI Toolkit shell renders | Verify AppShell UXML loads with no Console errors. Confirm base styles are applied. |
| Subject cards display | Navigate to subject selection; verify three subject cards render without missing-reference errors. |
| Science term world has no station gameplay | Enter any Science quest term world. Verify no station portal, no collectible reward interaction, no attempt submission. |
| LiteraQuest/PE world station affordances | Enter a LiteraQuest or HealthQuest term world. When stations are later implemented, verify only appropriate station interactables appear per the demo scope (2 per term). |
| Mission/hint/reflection/reward templates | Verify each overlay (`MissionBriefOverlay`, `HintOverlay`, `ReflectionOverlay`, `RewardOverlay`) instantiates without missing-reference errors when triggered from station flow. |
| No unhandled console errors | After all above checks, the Console must contain zero `LogException` or `LogError` entries except deliberate provider-not-implemented messages. |

---

## 8. Evidence Index

Key files examined or changed during the audit:

### Source files (Runtime)

| Path | Role |
|------|------|
| `Assets/_Project/Nutrimind/Runtime/App/AppStateMachine.cs` | Canonical state machine with 25 states including 4 Phase 04A additions; subject-aware station guard |
| `Assets/_Project/Nutrimind/Runtime/App/AppEnums.cs` | `AppState` (25 values, non-stable ordinal contract), `LearningCyclePhase` (4 phases), `SubjectType`, `StationState` |
| `Assets/_Project/Nutrimind/Runtime/App/AttemptCoordinator.cs` | Attempt lifecycle coordinator; rejects duplicate in-flight submissions; preserves UUID on retry |
| `Assets/_Project/Nutrimind/Runtime/App/AttemptScope.cs` | Single attempt context with `ClientAttemptUuid` stable across retries |
| `Assets/_Project/Nutrimind/Runtime/App/CompositionRoot.cs` | Singleton composition root; selects `HttpProvider` or `LocalDemoJsonProvider` by mode |
| `Assets/_Project/Nutrimind/Runtime/App/IGameDataProvider.cs` | Narrow 5-method provider interface (placeholder) |
| `Assets/_Project/Nutrimind/Runtime/App/HttpProvider.cs` | Placeholder — all methods return "not yet implemented" |
| `Assets/_Project/Nutrimind/Runtime/App/LocalDemoJsonProvider.cs` | Placeholder — all methods return "not yet implemented" |
| `Assets/_Project/Nutrimind/Runtime/App/InteractionStateStore.cs` | Typed store for transient narrative/current phase/feedback state; resets on station transitions |
| `Assets/_Project/Nutrimind/Runtime/App/ProgressRewardStore.cs` | Typed store for provider-presented progress/reward previews/world restoration state |
| `Assets/_Project/Nutrimind/Runtime/App/StationNarrative.cs` | Provider-agnostic DTOs: `StationNarrative`, `NpcGuide`, `HintPolicy`, `HintTier`, `DiscoveryEntry`, `RewardPreview`, `WorldRestorationState`, `SuccessFeedback`, `MistakeFeedback` — all fields optional/default-safe |
| `Assets/_Project/Nutrimind/Runtime/App/SceneRegistry.cs` | Key-to-path scene registry; null-safe `TryGetScene` (fixed in this audit) |
| `Assets/_Project/Nutrimind/Runtime/App/MainThreadDispatcher.cs` | Main-thread callback dispatcher with generation-based invalidation |
| `Assets/_Project/Nutrimind/Runtime/UI/NutriMindAssetPaths.cs` | Asset path constants; `OverlayReward` added in this audit |

### UXML overlay templates

| Path | Named elements covered by tests |
|------|--------------------------------|
| `Assets/_Project/Nutrimind/UI/Documents/Overlays/MissionBriefOverlay.uxml` | `#mission-npc-name`, `#mission-title`, `#mission-description`, `#mission-objective`, `#mission-skip-button`, `#mission-begin-button` |
| `Assets/_Project/Nutrimind/UI/Documents/Overlays/HintOverlay.uxml` | `#hint-tier-label`, `#hint-title`, `#hint-text`, `#hint-try-again-button` |
| `Assets/_Project/Nutrimind/UI/Documents/Overlays/ReflectionOverlay.uxml` | `#reflection-title`, `#reflection-question`, `#reflection-skip-button`, `#reflection-continue-button` |
| `Assets/_Project/Nutrimind/UI/Documents/Overlays/RewardOverlay.uxml` | `#reward-icon`, `#reward-title`, `#reward-subtitle`, `#reward-amount`, `#reward-continue-button` |
| `Assets/_Project/Nutrimind/UI/Documents/Components/NpcDialogue.uxml` | `#npc-name`, `#npc-portrait`, `#npc-dialogue-text`, `#npc-dismiss-button` |
| `Assets/_Project/Nutrimind/UI/Documents/Components/DiscoveryCard.uxml` | `#discovery-icon`, `#discovery-title`, `#discovery-text`, `#discovery-dismiss-button` |
| `Assets/_Project/Nutrimind/UI/Documents/Components/FeedbackPanel.uxml` | `#feedback-icon`, `#feedback-title`, `#feedback-message`, `#feedback-retry-button`, `#feedback-continue-button` |

### Test files

| Path | Coverage |
|------|----------|
| `Tests/EditMode/App/NutriMindPhase04AStateFlowTests.cs` | Phase 04A state machine paths: mission brief, hint overlay, reflection, reward presentation, retry, full flow, invalid shortcuts, ScienceQuest rejection |
| `Tests/EditMode/App/SharedClient/StationNarrativeDtoTests.cs` | All `StationNarrative` child DTOs: type existence, string/nullable properties, collection defaults, mutable collections, phase defaults |
| `Tests/EditMode/App/SharedClient/InteractionStateStoreExtendedResetTests.cs` | Phase 04A fields on `InteractionStateStore`: defaults and reset behaviour (narrative, phase, feedback, backward compat) |
| `Tests/EditMode/App/SharedClient/ProgressRewardStoreExtendedResetTests.cs` | Phase 04A fields on `ProgressRewardStore`: defaults and reset behaviour (restoration state, reward previews, backward compat) |
| `Tests/EditMode/App/SharedClient/SceneRegistryTests.cs` | Includes `SceneRegistry_TryGetScene_NullKeyReturnsFalse` (added in this audit) |
| `Tests/EditMode/App/SharedClient/AttemptCoordinatorTests.cs` | Attempt lifecycle: type existence, submit/reject-duplicate, `ClientAttemptUuid` stability across retry |
| `Tests/EditMode/UI/NutriMindAssetPathTests.cs` | Includes `RewardOverlay_LoadsAtExpectedPath` (added in this audit), plus all other overlay/component asset path tests |
| `Tests/EditMode/UI/NutriMindComponentTemplateTests.cs` | Named element verification for all overlay and component templates; focusable-root checks for interactive components |
| `Tests/EditMode/App/SharedClient/MainThreadDispatcherThreadingTests.cs` | Thread safety and generation-based stale callback suppression |

---

## 9. Assumptions and Blockers

- **No architecture blocker identified** for additive Phase 04A compatibility; static build validation (`dotnet build`) passed and EditMode Test Runner passed (434/434). PlayMode execution and Coplay visual smoke verification remain manual/pending.
- **Blocker for station production**: `IGameDataProvider` must be expanded (Phase 04) and `LocalDemoJsonProvider` must be populated with demo fixtures (Phase 11) before any station can complete an end-to-end attempt/feedback/ reward flow.
- **Blocker for JSON deserialisation**: No production JSON parser exists. Do not use `JsonUtility` with property-style DTOs. The parser choice (Newtonsoft.Json, System.Text.Json, or custom reader) is a Phase 04/11 decision.
- **Assumption**: UXML templates under `UI/Documents/Overlays/` and `UI/Documents/Components/` are placeholder UI only. Final visual design (spacing, colours, typography, safe-area handling) will be refined during Phase 09 and per-screen implementation units.
