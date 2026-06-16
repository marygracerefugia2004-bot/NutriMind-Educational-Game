# Unity Agent Master Orchestrator

This prompt is the permanent operating contract for Unity development. It must be loaded together with exactly one selected Unity phase prompt in every fresh agent session.

The detailed requirement documents under `docs/unity/` are authoritative. Phase prompts are focused operational wrappers; they do not replace requirements.

## How to Start a Session

For a fresh session, instruct the agent to read:

1. `agent-prompts/unity/00_MASTER_ORCHESTRATOR.md`
2. one selected phase prompt, for example `agent-prompts/unity/06_LITERAQUEST_GAMEPLAY_TASK.md`
3. `docs/UNITY_REQUIREMENTS.md`
4. `docs/SERVER_REQUIREMENTS.md` when the selected work touches API contracts, content data, authentication, scoring, rewards, or progress
5. the numbered Unity requirement file mapped to the selected phase
6. earlier approved requirement files and implementation dependencies

Then provide the selected unit and approved prerequisites using `agent-prompts/unity/SESSION_START_TEMPLATE.md`.

In a continuing session, the master remains active. After the user says `approved, continue`, load exactly one next phase/unit. Do not start the next phase automatically.

## Current Product Scope

- LiteraQuest: three term worlds and two complete playable stations per term, six stations total
- PE/Health Quest: three term worlds and two complete playable stations per term, six stations total
- Science Quest: three exploration-preview term worlds with no station gameplay in the current milestone
- UI: UI Toolkit for all new runtime game UI
- Data: `LocalDemoJson` for development/demo and `Http` for integration/staging/production through the same DTO and provider boundary
- Assets: user-provided assets and prefabs first; project-owned variants second; newly created/generated assets only when required assets are missing

## Authoritative Execution Sequence

The numbered requirement files are read in order, but implementation uses the dependency sequence below. Every listed unit is a separate checkpoint unless the user explicitly combines units.

### Stage 1 — Project and Asset Foundation

1. Phase 01: project root and project-owned folders
2. Phase 01: provided asset, prefab, UI reference, world reference, and station reference inventory
3. Phase 09: shared UI Toolkit foundation only—`PanelSettings`, root `UIDocument`, UXML/USS conventions, theme tokens, common components, safe-area strategy, and background placeholder strategy

### Stage 2 — Shared Application and Data Foundation

4. Phase 02: shared game flow and state model
5. Phase 03: shared client systems, stores, navigation, scene registry, attempt coordination, and asset catalog
6. Phase 04A: additive learning-through-gameplay compatibility review for story, NPCs, hints, discoveries, rewards, and world restoration
7. Phase 04: production DTOs, HTTP client, authentication contract, errors, retries, polling, and optional metadata-only realtime configuration
8. Phase 11: full fabricated student demo fixture and `LocalDemoJson` provider using the production DTOs
9. Phase 05: reusable application shell, world template, station template, player/camera baseline, portals, station-session framework, feedback, and return flow

Do not begin application screens until Stages 1 and 2 are approved.

### Stage 2A — Gameplay Design Amendment

Use Phase 04A after the completed logical Phases 1–4 (project foundation, UI foundation, game flow, and shared client systems) and before API/demo/framework work continues. This is an additive retrofit, not a foundation rewrite. Complete one compatibility unit at a time and stop for review.

The approved design pillars are:

- learning is performed through gameplay
- Discover, Practice, Apply, and Review
- concise NPC mission guidance
- safe mistakes and tiered hints
- optional discoveries and fun facts
- provider-confirmed coins/crystals/badges and world restoration
- no new Science missions or station rewards in the current milestone

### Stage 3 — Application Screens, One at a Time

Use Phase 09 for each unit, in this order:

1. Bootstrap/Application Root presentation and root UI layers
2. Splash
3. Login
4. Main Interface
5. Profile
6. Settings
7. Subject Selection
8. Term Selection
9. Loading/Transition

Each screen uses the approved shared state, provider, navigation, UI Toolkit system, and assets. Stop after each screen.

### Stage 4 — LiteraQuest, One Unit at a Time

Use Phase 06:

1. Term 1 world
2. Vocabulary Clue Trail
3. Sequence Path
4. Term 1 integration check
5. Term 2 world
6. Inference Investigation
7. Fact or Opinion Market
8. Term 2 integration check
9. Term 3 world
10. Sentence Repair Workshop
11. Paragraph Order Bridge
12. Term 3 integration check

Do not begin PE/Health until all LiteraQuest units are approved.

### Stage 5 — PE/Health, One Unit at a Time

Use Phase 08:

1. Term 1 world
2. Wellness Choice Trail
3. Sanitation Sorting Center
4. Term 1 integration check
5. Term 2 world
6. First-Aid Sequence Station
7. Safety Decision Branch
8. Term 2 integration check
9. Term 3 world
10. Medicine Label Safety Check
11. Healthy Habit and Advertisement Truth Board
12. Term 3 integration check

### Stage 6 — Science Exploration Preview

Use Phase 07, one scene at a time:

1. Science Term 1 world
2. Science Term 2 world
3. Science Term 3 world

Do not add Science station portals, challenges, attempts, scoring, rewards, or completion state.

### Stage 7 — Final Integration and Release

Use Phase 10 for focused checks throughout development and for these final units:

1. local-demo full regression
2. HTTP contract/integration verification when the server is available
3. Android landscape smoke and performance checks
4. final approved-scope regression and release-readiness report

## Phase-to-Requirement Map

| Phase prompt | Authoritative requirement | Main purpose |
|---|---|---|
| 01 | `docs/unity/01_FOUNDATION_AND_DELIVERY_ORDER.md` | project root, asset intake, dependencies, sequence |
| 02 | `docs/unity/02_GAME_FLOW_AND_STATE_MODEL.md` | state transitions and navigation behavior |
| 03 | `docs/unity/03_SHARED_CLIENT_SYSTEMS.md` | reusable client architecture |
| 04 | `docs/unity/04_SERVER_CONNECTION_AND_UNITY_API.md` | DTO, HTTP, auth, error, retry, polling contract |
| 04A | `docs/unity/04A_LEARNING_THROUGH_GAMEPLAY_STORY_AND_REWARDS.md` | additive gameplay-design compatibility and shared extensions |
| 05 | `docs/unity/05_SCENES_WORLDS_AND_STATION_FRAMEWORK.md` | reusable scene/world/station framework |
| 06 | `docs/unity/06_LITERAQUEST_GAMEPLAY.md` | LiteraQuest worlds and stations |
| 07 | `docs/unity/07_SCIENCE_QUEST_GAMEPLAY.md` | Science exploration-preview worlds |
| 08 | `docs/unity/08_HEALTH_QUEST_GAMEPLAY.md` | PE/Health worlds and stations |
| 09 | `docs/unity/09_UI_CONTROLS_ACCESSIBILITY_AND_PRESENTATION.md` | UI Toolkit foundation and one UI scope at a time |
| 10 | `docs/unity/10_TESTING_INTEGRATION_AND_RELEASE.md` | focused and final verification |
| 11 | `docs/unity/11_DEMO_DATA_AND_LOCAL_PROVIDER.md` | full fake student fixture and local provider |

## Global Learning-Through-Gameplay Rule

Every LiteraQuest and PE/Health station must make the learning concept part of the world/UI action, follow Discover–Practice–Apply–Review, support safe mistakes and approved hints, and show provider-confirmed rewards/restoration. Do not create a decorative walk followed by a generic quiz. Keep optional discoveries optional and Science exploration-only.

## Global Asset-First Rule

Before creating an asset or prefab for the selected unit:

1. inspect all relevant user-provided/imported assets, prefabs, materials, models, textures, sprites, icons, audio, animations, UXML, USS, and reference images
2. reuse a suitable provided asset first
3. adapt non-destructively through a project-owned prefab variant, material, wrapper, binding, collider, LOD, or import adjustment
4. create or generate a new original project-owned asset only when no suitable supplied asset exists
5. use project image references as design direction for newly created original assets
6. do not generate final screen backgrounds unless explicitly authorized; use the supplied background or a neutral placeholder
7. never overwrite vendor/package/user source assets destructively
8. report asset provenance and status

If source, license, suitability, or intended use is unclear, stop and report it.

## Global Engineering Rules

- Use Build mode, but inspect and plan the selected unit before editing.
- Do not create a nested Unity project.
- Do not develop multiple screens, worlds, or stations in parallel.
- Do not hide missing shared infrastructure inside a feature scene.
- Do not create scene-specific HTTP clients, JSON readers, auth stores, scoring, reward, progress, or navigation systems.
- Do not hardcode canonical subject, term, station, curriculum, content, reward, score, unlock, or availability data.
- Do not use Canvas/uGUI for new runtime game UI.
- Do not make Science stations functional in the current milestone.
- Do not silently fall back from HTTP to fake data in integration or production.
- Preserve stable server keys and API compatibility.
- Use the same DTOs, stores, UI, scenes, and gameplay systems for `LocalDemoJson` and `Http`.
- Reuse the same `client_attempt_uuid` when retrying an attempt.
- Official correctness, scoring, progress, rewards, unlocks, and completion come from the server in HTTP mode.
- Run focused tests for every unit.

## Mandatory Stop

Complete exactly one selected phase unit. Save the work, run focused checks, provide the report below, and stop. Do not begin the next phase, screen, world, station, integration unit, or documentation file until the user explicitly approves continuation.

## Required Checkpoint Report

- **Scope completed:** exact phase and unit
- **Prerequisites verified:** approved dependencies used
- **Files changed:** every created or modified file
- **Assets:** reused, adapted as variants, created/generated, placeholders, and blocked items
- **Behavior completed:** what now works
- **Stable keys/contracts used:** scene, prefab, portal, interactable, DTO, and provider keys
- **Verification:** exact commands/tests/checks and results
- **Known gaps:** verified gaps in the selected unit only
- **Pending cross-unit work:** work intentionally deferred
- **Proposed next task:** name it, but do not start it
