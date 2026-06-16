# Unity Scenes, Worlds, and Station Framework

## Purpose

This document defines scene ownership, registration, loading, return flow, and the shared framework used by the current demo. Development is strictly one scene at a time.

## Scene Categories

### Persistent Bootstrap

A bootstrap/application-root scene owns shared services and survives scene transitions where the selected architecture requires it.

It may own:

- composition root
- active game-data provider
- session and shared stores
- navigation/scene loading
- shared UI Toolkit layers
- loading/error/modal services
- audio/settings lifetime

Feature scenes must not duplicate these services.

### Application UI Scenes or Screen Units

Required application units:

```txt
app_bootstrap
splash
login
main_interface
profile
settings
subject_selection
term_selection
loading_transition
```

The implementation may use dedicated Unity scenes or a stable application shell that swaps UXML screens. For development and review, each unit remains an independent checkpoint and must not be bundled with the next unit.

All runtime UI uses UI Toolkit.

### Exploration World Scenes

Required current world scene keys:

```txt
g5_literaquest_t1_world
g5_literaquest_t2_world
g5_literaquest_t3_world

g5_health_quest_t1_world
g5_health_quest_t2_world
g5_health_quest_t3_world

g5_science_quest_t1_world
g5_science_quest_t2_world
g5_science_quest_t3_world
```

Each world scene must provide:

- registered stable scene key
- player spawn point
- safe return/exit action
- movement and camera baseline
- UI Toolkit world HUD
- pause/settings access where approved
- loading and unload cleanup
- missing-data and missing-asset safety
- term/world title and current mode indicator

LiteraQuest and PE/Health worlds additionally provide exactly two current-demo station portals per term.

Science worlds provide no functional station portal in this milestone.

### Station Gameplay Scenes

The current demo uses dedicated station gameplay scene units. A station portal starts/resumes a station session, records the originating world and return anchor, then loads the registered station scene.

Required LiteraQuest station scenes:

```txt
g5_literaquest_t1_vocabulary_clue_trail
g5_literaquest_t1_sequence_path
g5_literaquest_t2_inference_investigation
g5_literaquest_t2_fact_opinion_market
g5_literaquest_t3_sentence_repair_workshop
g5_literaquest_t3_paragraph_order_bridge
```

Required PE/Health station scenes:

```txt
g5_health_quest_t1_wellness_choice_trail
g5_health_quest_t1_sanitation_sorting_center
g5_health_quest_t2_first_aid_sequence
g5_health_quest_t2_safety_decision_branch
g5_health_quest_t3_medicine_label_safety
g5_health_quest_t3_habit_advertisement_truth_board
```

The exact Unity scene asset name may differ, but the registry must keep stable keys and document the mapping.

## Learning-Through-Gameplay Scene Contract

A term world is a mission hub, not only a portal menu. It should establish the term's story problem, show the two station objectives, contain optional approved discoveries, and visibly reflect completed station restoration states.

A playable station scene must include or support:

- a concise mission briefing or NPC request
- a world problem directly connected to the learning skill
- Discover, Practice, Apply, and Review stages
- provider-approved safe-mistake feedback and tiered hints
- a meaningful environmental result after accepted completion
- optional discoveries that are not required for completion
- provider-confirmed reward/progress presentation
- a short return dialogue or reflection where appropriate

Do not implement a decorative walk followed by a generic batch of questions. World interactions must generate evidence, arrangement, classification, choices, revisions, or other data needed by the official station answer.

## Scene Asset and Prefab Workflow

Before creating or modifying each application screen, world, or station scene:

1. List the asset roles required by the current scope.
2. Search the project for user-provided assets and prefabs that satisfy those roles.
3. Reuse suitable assets directly when safe.
4. Create project-owned prefab variants or adapters for required components, materials, colliders, bindings, or mobile adjustments.
5. Create or generate a new asset only for a verified missing role.
6. Use project image references for original visual direction when a new asset is required.
7. Use a clear placeholder when a final asset cannot be created safely in the current cycle.
8. Update the local asset/prefab catalog and focused tests.

A scene review report must include an asset manifest with these columns:

| Asset role | Stable/local key | Source | Action | Final path | Status |
|---|---|---|---|---|---|
| Example: station portal | `litera_t1_portal` | user-provided prefab | prefab variant | project prefab path | ready |

Allowed `Action` values:

```txt
reused
adapted_as_variant
created
externally_generated
placeholder
blocked
```

Do not use generated assets to conceal a missing functional dependency. A generated prop does not replace missing gameplay logic, stable-key mapping, data contracts, or tests.

## Scene Registry Contract

A scene registry entry should contain, as applicable:

```txt
scene_key
scene_type
unity_scene_name or address_key
subject_slug
grade_level
term_number
station_key
origin/return behavior
required build inclusion
```

The registry must:

- reject duplicate stable keys
- detect missing scene assets
- never silently load an unrelated fallback scene
- expose a safe unavailable result
- be testable without manually opening every scene

## World-to-Station Flow

```txt
enter term world
-> resolve station summaries from active provider
-> bind two approved portals by stable portal/station key
-> player interacts with portal
-> start or resume station session
-> preserve origin world and return anchor
-> load dedicated station scene
-> run world/UI learning task
-> submit attempt
-> present result
-> complete or continue station
-> return to originating world
-> refresh portal/progress/reward state
```

The station scene must not directly parse the source JSON or call a raw URL. It uses shared stores, provider interfaces, and attempt services.

## Shared Narrative, NPC, Discovery, Hint, and Reward Components

The shared framework should provide reusable, data-driven support for:

- mission briefing panels
- NPC dialogue triggers and UI fallback
- discovery/fun-fact presentation
- hint progression
- retry-safe station state
- reflection and feedback panels
- reward preview and provider-confirmed reward presentation
- world restoration state bindings

These components must use stable keys and shared provider DTOs. Subject scenes configure them; they must not create separate story, hint, reward, or discovery subsystems.

## Shared World Template

The reusable world template should provide:

- player controller and camera hooks
- input enable/disable lifecycle
- spawn/return-point registry
- world HUD document
- term title and progress area
- portal anchor registry
- interaction prompt service
- scene-ready and scene-exit lifecycle
- ambient audio hook
- safe missing-background/environment behavior

A subject world extends the template with its own terrain, landmarks, visual theme, and approved portal placements.

## Shared Station Template

The reusable station template should provide:

- station context and session ID
- instructions/introduction panel
- world-task registration
- challenge presenter resolution
- local answer state
- submit/confirm behavior
- loading and duplicate-submit protection
- feedback/result panel
- reward/progress presentation
- retry and safe exit
- return-to-world behavior
- cleanup and cancellation

A station scene adds only the mechanic and presentation unique to that station.

## Current Mechanic Families

The 12 current station scenes use reusable families:

- inspect and answer
- sequence/order
- clue/evidence selection
- sorting/classification
- scenario/path choice
- label inspection and matching
- sentence repair/revision

New station scenes should extend these families instead of copying networking and completion code.

## Science Exploration-Only Rules

Science term worlds may contain:

- traversable environment
- landmarks
- ambient effects and animations
- decorative educational objects
- non-scoring informational labels if explicitly approved
- exploration-preview status
- return button/portal

Science term worlds must not contain unfinished buttons, disabled station portals that appear usable, fake scores, fake rewards, or hidden gameplay triggers. The student should clearly understand that this subject is an exploration preview in the current demo.

## Scene Completion Checkpoint

A scene is ready for review only when:

- its stable key resolves
- it opens from the approved previous scene
- it returns to the correct destination
- shared services are reused
- UI Toolkit assets load
- final or placeholder background behavior is safe
- loading/error/missing-data states are handled
- no unrelated next scene was modified
- focused tests pass
- the agent stops and reports the completed scene
