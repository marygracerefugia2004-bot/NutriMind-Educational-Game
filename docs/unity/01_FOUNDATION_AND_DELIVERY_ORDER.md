# Unity Foundation Dependencies and Scene-by-Scene Delivery Order

## Purpose

This document defines the required development flow. Work begins with shared foundation/bootstrap dependencies and UI Toolkit assets. After foundation approval, development proceeds one scene or screen at a time.

The objective is to prevent scattered scripts, duplicate systems, conflicting UI, and several unfinished scenes being developed in parallel.

## Phase Prompt Mapping

The foundation is completed through connected phase prompts rather than one oversized agent run:

- Phase 01 owns project-root creation and provided-asset/prefab intake.
- Phase 09 owns the shared UI Toolkit design foundation before any application screen.
- Phase 02 owns the shared game-flow/state model.
- Phase 03 owns reusable client services and stores.
- Phase 04A owns the additive learning-through-gameplay compatibility review for story missions, NPC guidance, safe mistakes, hints, discoveries, rewards, and world restoration.
- Phase 04 owns HTTP/API DTO contracts and connection behavior.
- Phase 11 owns the full local-demo fixture/provider using the production DTOs.
- Phase 05 owns reusable scene, world, and station templates.

These outputs together satisfy the foundation dependencies described below. Do not duplicate a provider, store, UI system, or scene framework inside Phase 01 merely because the requirements are summarized here.

## Rule: Foundation First, Then One Scene at a Time

There are only two development layers:

1. **Foundation dependencies/bootstrap** shared across the whole project.
2. **Per-scene development** performed in the exact approved order.

During per-scene development, the agent must modify only the current scene scope, complete its tests and review report, then stop. The next scene begins only after explicit user approval.

## Foundation Dependency A: Unity Project and Owned Folders

Before UI, scenes, prefabs, demo fixtures, or gameplay scripts:

- locate or create the valid Unity project root containing `Assets/`, `Packages/`, and `ProjectSettings/`
- do not create a nested second Unity project
- preserve the configured Unity version and packages unless a verified compatibility issue exists
- configure Android landscape target assumptions
- create or confirm the project-owned root:

```txt
Assets/_Project/Nutrimind/
```

Recommended structure:

```txt
Assets/_Project/Nutrimind/
  Runtime/
    App/
    Auth/
    Data/
    Networking/
    Navigation/
    Scenes/
    UI/
    Gameplay/
    Stations/
    Subjects/
    Accessibility/
  UI/
    Documents/
      AppShell/
      Screens/
      Components/
      Overlays/
      Stations/
    Styles/
      Base/
      Components/
      Screens/
      Accessibility/
    Themes/
    PanelSettings/
    Controllers/
    Bindings/
    Icons/
    Backgrounds/
    Placeholders/
  Scenes/
    App/
    Worlds/
      LiteraQuest/
      HealthQuest/
      ScienceQuest/
    Stations/
      LiteraQuest/
      HealthQuest/
  Art/
    Provided/
    Generated/
    Materials/
    Textures/
    Models/
    Audio/
  References/
    UI/
    Worlds/
    Stations/
  Prefabs/
    Variants/
    Generated/
  ScriptableObjects/
  DemoData/
  Tests/
    EditMode/
    PlayMode/
```

Checkpoint: stop after project/folder creation and report the exact paths.

## Foundation Dependency A2: Provided Asset and Prefab Intake

After the project-owned folders exist and before UI or scene creation, inspect the Unity project for assets supplied by the user or already imported by the project.

The inventory includes, where present:

- prefabs and prefab variants
- 3D models, meshes, rigs, animations, and characters
- materials, shaders, textures, terrain assets, skyboxes, and lighting profiles
- sprites, icons, fonts, audio, particles, and visual effects
- UXML, USS, `PanelSettings`, UI templates, and UI textures
- scene templates, world props, station props, portals, interactables, and cameras
- image references stored in the project for UI, worlds, and stations

For every relevant asset, classify it as:

```txt
ready_to_use
usable_with_variant_or_adapter
usable_after_import_or_mobile_adjustment
reference_only
missing
blocked_or_unclear
```

Rules:

- Use a supplied asset or prefab when it meets the requirement.
- Do not edit package/vendor source assets destructively. Create a project-owned prefab variant, material, wrapper, or derived asset when modification is needed.
- Preserve GUIDs and existing references. Do not move or rename large imported asset trees without a verified reason.
- Do not generate a duplicate replacement before searching the project.
- Verify mobile suitability, including texture size, material/shader support, collider cost, animation complexity, and polygon/LOD needs.
- Record the stable local key or scene role that will use the asset.
- If the source or permitted use is unclear, mark it blocked and request review.

Recommended project-owned organization for derived work:

```txt
Assets/_Project/Nutrimind/Art/Provided/       # optional project-owned organization or adapters
Assets/_Project/Nutrimind/Art/Generated/      # newly created/generated project assets
Assets/_Project/Nutrimind/Prefabs/Variants/   # variants of suitable supplied prefabs
Assets/_Project/Nutrimind/Prefabs/Generated/  # new project-owned prefabs
Assets/_Project/Nutrimind/References/         # project reference images by UI/world/station
```

Do not copy or relocate imported assets merely to match this structure when doing so would break package ownership or GUID references. The folders are for project-owned assets, variants, generated work, and approved copies.

Checkpoint: report the asset inventory, reusable assets, required adaptations, missing assets, blocked assets, and likely generated assets. Stop for user review before creating the UI Toolkit foundation.

## Foundation Dependency B: UI Toolkit Design and Asset Bootstrap

Before creating the Splash scene or any other screen, establish the reusable visual system.

Required foundation:

- shared runtime `PanelSettings`
- root `UIDocument`/application-shell strategy
- UXML and USS conventions
- typography scale
- spacing and sizing tokens
- panel, card, button, input, tab, badge, modal, toast, loading, error, and progress components
- focus, disabled, pressed, selected, validation, success, and warning states
- safe-area and Android landscape scaling
- shared icon placement and naming rules
- neutral placeholder backgrounds
- controller/presenter boundary so UXML/USS do not perform networking or domain logic

Use approved project reference images and the completed asset inventory for:

- screen hierarchy
- component layout
- panel proportions
- spacing
- typography emphasis
- navigation placement
- icon placement
- border, surface, and button direction

Do not copy, extract, or generate the reference background image unless the user separately and explicitly authorizes background generation. The user will place final backgrounds in:

```txt
Assets/_Project/Nutrimind/UI/Backgrounds/
```

Until then, use a readable neutral placeholder from `UI/Placeholders/`. Replacing the background must not require controller or gameplay changes.

Checkpoint: stop after the UI Toolkit design foundation and shared components are ready. Do not create Splash in the same cycle.

## Foundation Dependency B2: Learning-Through-Gameplay Compatibility

When the project folders, UI Toolkit foundation, game-flow model, and shared client systems are already approved, run Phase 04A once before continuing to API/demo/framework and subject scenes. Do not rebuild completed phases. Add only missing reusable support for mission briefings, NPC dialogue, Discover–Practice–Apply–Review, safe mistakes, hint progression, optional discoveries, provider-confirmed rewards, and world-restoration state.

Checkpoint: stop after each selected Phase 04A retrofit unit and report compatibility changes.

## Foundation Dependency C: Shared Application Bootstrap

Before individual application scenes:

- composition root/application lifetime
- selected data source: `LocalDemoJson` or `Http`
- shared DTOs and JSON parsing
- local demo provider and HTTP provider behind the same interfaces
- configuration and compatibility handling
- authentication/session store
- profile/settings/subject/term/progress/reward stores
- safe error/result abstraction
- timeout, cancellation, retry, and main-thread dispatch
- navigation service
- scene registry and stable scene keys
- common loading, unavailable, modal, and notification services
- logging without secrets or real student data

Checkpoint: run Edit Mode tests for provider selection, DTO parsing, navigation keys, and shared state. Stop for review.

## Foundation Dependency D: Shared World and Station Bootstrap

Before subject world or station scenes:

- player movement and camera baseline
- world HUD through UI Toolkit
- pause/back/exit behavior
- world spawn and return-point registry
- portal status and interaction contract
- station session start/resume contract
- challenge presenter registry
- answer-state contract
- attempt coordinator with persistent `client_attempt_uuid`
- feedback/result presentation
- local-demo progress and reward mutation
- server-authoritative HTTP result path
- scene cancellation and cleanup
- reusable world-scene template
- reusable station-scene template

Checkpoint: create templates and tests only. Do not begin a subject world in the same cycle.

## Foundation Acceptance

Foundation is approved only when a new scene can reuse:

- one app/bootstrap lifetime
- one data-provider boundary
- one auth/session store
- one navigation/scene loader
- one UI Toolkit visual system
- one error/loading system
- one world bootstrap
- one station session/attempt/result flow

No scene may create a second API client, second JSON reader, second auth system, independent scoring, or separate demo-only gameplay implementation.

# Per-Scene Development Sequence

## Scene Cycle Rules

For every scene or screen development unit:

1. Read its approved requirements.
2. Confirm dependencies are already approved.
3. Inventory the provided assets/prefabs relevant to that scene, then create or modify only that scene and directly owned assets/scripts/tests.
4. Reuse suitable provided assets and prefabs first; create project-owned variants or new assets only when necessary.
5. Use shared UI Toolkit components and services.
6. Connect `LocalDemoJson` first when the server is unavailable.
7. Validate navigation into and out of the scene.
8. Validate loading, empty, error, and missing-asset behavior where applicable.
9. Run focused Edit Mode/Play Mode checks.
10. Report files changed, assets reused/adapted/created, behavior completed, tests, gaps, and screenshots available for review.
11. Stop. Do not touch the next scene until the user explicitly approves continuation.

If a shared dependency is missing, stop and fix that dependency as a separate approved foundation cycle. Do not hide shared infrastructure inside the current scene.

## Application Scene Order

Treat each item as a separate checkpoint, even when the project uses a persistent application shell and swaps UXML screens rather than loading a new Unity scene.

1. **Bootstrap/Application Root** — persistent services, provider selection, scene loader, root UI layers.
2. **Splash Screen** — branding, initialization, compatibility/demo status, safe transition.
3. **Login Screen** — LRN/PIN and explicit development-only demo login.
4. **Main Interface** — primary student dashboard and play/profile/settings navigation.
5. **Profile Screen** — student-safe profile and progress summary.
6. **Settings Screen** — supported local/server settings, accessibility, audio, language.
7. **Subject Selection Screen** — LiteraQuest, PE/Health Quest, and Science Quest cards from provider data.
8. **Term Selection Screen** — three terms, availability, progress, and world scene resolution.
9. **Loading/Transition Screen** — scene-loading progress, cancellation policy, and safe errors.

Do not begin subject worlds until all application scene units above are approved.

## LiteraQuest Scene Order

Complete one term before beginning the next.

### LiteraQuest Term 1

1. `g5_literaquest_t1_world`
2. Vocabulary Clue Trail station scene
3. Sequence Path station scene
4. Term 1 integration check: both portals, return points, progress, and rewards

### LiteraQuest Term 2

1. `g5_literaquest_t2_world`
2. Inference Investigation station scene
3. Fact or Opinion Market station scene
4. Term 2 integration check

### LiteraQuest Term 3

1. `g5_literaquest_t3_world`
2. Sentence Repair Workshop station scene
3. Paragraph Order Bridge station scene
4. Term 3 integration check

Do not begin PE/Health Quest until all six LiteraQuest station scenes and three LiteraQuest worlds are approved.

## PE/Health Quest Scene Order

The display label is **PE/Health Quest**. The server contract may continue using the stable slug `health_quest`.

### PE/Health Term 1

1. `g5_health_quest_t1_world`
2. Wellness Choice Trail station scene
3. Sanitation Sorting Center station scene
4. Term 1 integration check

### PE/Health Term 2

1. `g5_health_quest_t2_world`
2. First-Aid Sequence Station scene
3. Safety Decision Branch station scene
4. Term 2 integration check

### PE/Health Term 3

1. `g5_health_quest_t3_world`
2. Medicine Label Safety Check station scene
3. Healthy Habit and Advertisement Truth Board station scene
4. Term 3 integration check

## Science Quest Exploration Scene Order

Science Quest is intentionally non-functional beyond exploration in the current milestone.

1. `g5_science_quest_t1_world`
2. `g5_science_quest_t2_world`
3. `g5_science_quest_t3_world`

Each Science scene is a separate checkpoint. It may include movement, camera, ambient presentation, landmarks, UI Toolkit HUD, an exploration-preview label, and a return action.

Do not add:

- station portals
- challenge panels
- task interactions
- collectibles that imply progress
- attempt requests
- scoring
- rewards
- completion state

## Scope Protection

Current required gameplay is:

```txt
LiteraQuest: 2 stations x 3 terms = 6
PE/Health:   2 stations x 3 terms = 6
Science:     3 exploration-only worlds, 0 stations
```

Deferred station ideas may remain documented as future possibilities, but they must not be implemented before the current 12-station demo is complete and approved.
