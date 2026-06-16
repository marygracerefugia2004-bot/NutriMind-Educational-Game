# Unity Testing, Integration, and Release Requirements

## Purpose

Testing follows the same one-scene-at-a-time development model. A scene is not approved because it merely opens.

## Test Layers

- Edit Mode tests for DTOs, providers, stores, registry validation, stable keys, and pure gameplay state
- Play Mode tests for scene loading, UI Toolkit interaction, player/world flow, station mechanics, and cleanup
- contract tests proving local demo JSON deserializes through the same DTOs as HTTP
- integration tests against the development server when available
- Android landscape smoke tests
- manual curriculum and presentation review

## Foundation Tests

Before scene development:

- project assemblies compile
- UI Toolkit `PanelSettings`, base UXML, and USS load
- shared components can be instantiated
- safe-area/layout checks pass for representative landscape sizes
- provider mode is explicit
- production configuration rejects `LocalDemoJson`
- fixture JSON is valid
- scene registry rejects duplicate/missing mappings
- navigation does not load unrelated fallback scenes
- auth/session/profile/settings stores reset safely
- attempt coordinator preserves `client_attempt_uuid`
- main-thread dispatch and cancellation behavior are covered

## Asset and Prefab Validation

For every foundation or scene checkpoint, verify:

- the asset inventory was performed before new asset creation
- required stable asset/prefab keys resolve
- provided source assets were not overwritten destructively
- prefab variants preserve their source relationship where intended
- generated/project-owned assets are stored in approved project folders
- imported materials and shaders work on the target render pipeline
- textures and meshes are reasonable for Android memory and rendering constraints
- required colliders, interaction components, anchors, and accessibility fallbacks exist
- placeholders are explicitly identified and do not masquerade as final art
- missing assets fail safely
- no unrelated substitute asset is loaded silently
- the scene report lists reused, adapted, created/generated, placeholder, and blocked assets

Asset creation is not considered successful only because the file exists. It must be referenced by the intended scene/prefab/UI, load without errors, fit the stable-key catalog, and pass the current scene's functional checks.

## Per-Scene Acceptance Checklist

Every application, world, and station scene/screen unit must be tested and reviewed separately.

Required checks:

- correct stable scene/screen key
- correct previous and next navigation
- UI Toolkit assets load
- no unintended Canvas/uGUI runtime UI
- final or placeholder background is safe
- loading/error/missing-data behavior is clear
- event callbacks do not duplicate after re-entry
- scene unload removes subscriptions and cancels work
- Android landscape touch/keyboard behavior is acceptable
- the next scene was not modified in the same cycle

The agent must stop after reporting these checks.

## Application Flow Tests

Cover:

```txt
bootstrap
-> splash
-> login/demo login
-> main interface
-> profile/settings
-> subject selection
-> term selection
-> loading transition
-> world
```

Test:

- invalid login
- demo login blocked outside development mode
- session expiration
- missing/unsupported config
- back navigation
- repeated scene entry
- missing background/icon fallback
- local demo reset

## Current Demo Fixture Tests

The fixture must contain:

- one fabricated student
- all three subject summaries
- all nine term/world records
- six LiteraQuest station summaries/content/start/attempt/completion records
- six PE/Health station summaries/content/start/attempt/completion records
- empty Science station lists for all three terms
- no Science attempt, completion, score, or reward records
- valid stable world, portal, interactable, prefab, challenge, and scene keys
- safe error fixtures

Expected counts:

```txt
9 world records
12 station summaries
12 station content records
12 start responses
12 simulated attempt results
12 completion results
12 demo evaluation records
```

## Learning-Through-Gameplay Design Tests

For every playable station, verify:

- the learning skill is required by the world/UI mechanic rather than only by a detached quiz panel
- Discover, Practice, Apply, and Review stages are reachable
- mission/NPC dialogue is concise and has a UI fallback
- a failed attempt preserves safe progress and shows approved feedback/hints
- hint tiers do not expose hidden answer data incorrectly
- optional discoveries are not required for completion
- reward and world-restoration presentation occurs only after an accepted provider result
- repeated or failed requests do not duplicate coins, crystals, badges, restoration, or progress
- unknown or missing optional narrative fields do not crash the station
- Science preview worlds do not gain missions, collectibles, station rewards, or completion effects

## LiteraQuest Tests

Required station scenes:

- Vocabulary Clue Trail
- Sequence Path
- Inference Investigation
- Fact or Opinion Market
- Sentence Repair Workshop
- Paragraph Order Bridge

For each station test:

- portal and scene resolution
- session start/resume
- task interaction
- answer shape
- duplicate-submit protection
- result presentation
- one-time progress/reward update
- return to correct term world

## PE/Health Tests

Required station scenes:

- Wellness Choice Trail
- Sanitation Sorting Center
- First-Aid Sequence Station
- Safety Decision Branch
- Medicine Label Safety Check
- Healthy Habit and Advertisement Truth Board

Additionally verify:

- no personalized medical advice
- content is non-graphic
- fabricated labels/products are used in demo data
- missing approved content produces unavailable behavior rather than invented instructions

## Science Exploration Tests

For each Science term world:

- scene loads and returns correctly
- movement, camera, HUD, and preview label work
- station list is empty without error
- no station portal exists
- no attempt request occurs
- no score, progress, mastery, or reward mutation occurs
- no unfinished interaction appears usable

## API and Provider Integration

When the development server is available:

- select `Http` explicitly
- verify config/login/bootstrap/profile/settings
- verify LiteraQuest and PE/Health station endpoints
- compare HTTP DTOs with fixture contract tests
- verify idempotent attempt behavior
- verify server-authoritative results
- verify safe errors and retry actions
- verify polling fallback
- keep WSS optional

Do not add scene-specific HTTP branches to fix integration differences.

## Performance and Reliability

Check representative devices for:

- scene load time
- UI Toolkit layout/repaint behavior
- large texture/background memory
- repeated callbacks
- frame spikes during portal/station transitions
- unbounded local-demo state
- cancellation on scene unload
- duplicate requests/rewards
- logs without PII/secrets

Optimize measured problems only. Do not introduce speculative complexity.

## Android Release Checks

Before a release candidate:

- supported Unity version recorded
- Android landscape build succeeds
- all current required scenes/assets are included
- data source is `Http` for real integration/release
- local demo mode is rejected or excluded
- no secrets or real student data included
- UI Toolkit safe areas and touch zones checked
- supplied backgrounds are included intentionally or approved placeholders remain
- text clipping and accessibility settings checked
- pause/resume and network recovery checked
- logs are safe

## Current Completion Matrix

| Area | Current completion requirement |
|---|---|
| Foundation | Shared project/bootstrap, providers, stores, UI Toolkit system, scene registry, world/station templates, tests |
| Application | Bootstrap plus Splash, Login, Main Interface, Profile, Settings, Subject Selection, Term Selection, and Loading units |
| Worlds | Nine term worlds registered and loadable |
| LiteraQuest | Six of six current station scenes |
| PE/Health | Six of six current station scenes |
| Science | Three exploration-preview worlds and zero station gameplay |
| Demo data | Twelve playable station contracts plus empty Science station lists |
| Reliability | No duplicate attempts/rewards, safe cleanup, provider parity, safe API evolution |
| Release | Android smoke checks and synchronized docs |

Do not claim the current demo is complete until every scene has been separately approved and the end-to-end flow passes.
