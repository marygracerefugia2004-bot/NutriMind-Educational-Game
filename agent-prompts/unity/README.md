# Unity Agent Phase Prompts

These prompts split Unity development into reviewable units while preserving one connected dependency sequence.

## Does the Master Prompt Always Accompany a Phase Prompt?

Yes for every fresh session. Load:

- `00_MASTER_ORCHESTRATOR.md`
- exactly one selected phase prompt
- the selected phase's authoritative requirement file

In a continuing session where the master is already in context, send only the approved next phase/unit. For a new session or when context is uncertain, reload the master and selected phase.

A phase prompt is not a standalone replacement for the master. Every phase file contains a companion notice for this reason.

## Session Start

Use `SESSION_START_TEMPLATE.md` to state:

- selected phase
- selected unit
- prior approved checkpoints
- server availability
- data provider mode
- available asset/reference locations
- explicit restrictions

## Connected Sequence

1. Phase 01 project root
2. Phase 01 asset/prefab intake
3. Phase 09 shared UI Toolkit foundation
4. Phase 02 state model
5. Phase 03 shared client systems
6. Phase 04A learning-through-gameplay compatibility review
7. Phase 04 HTTP/API contract
8. Phase 11 full demo data/provider
9. Phase 05 shared scene/world/station framework
10. Phase 09 application screens one at a time
11. Phase 06 LiteraQuest one unit at a time
12. Phase 08 PE/Health one unit at a time
13. Phase 07 Science worlds one at a time
14. Phase 10 final integration/release checks

Focused Phase 10 tests are also applied after every implementation unit.

## Prompt Files

| Prompt | Main requirement | Purpose |
|---|---|---|
| 01 | `01_FOUNDATION_AND_DELIVERY_ORDER.md` | project folders, asset intake, dependency sequencing |
| 02 | `02_GAME_FLOW_AND_STATE_MODEL.md` | navigation and state logic |
| 03 | `03_SHARED_CLIENT_SYSTEMS.md` | providers, stores, navigation, attempts, asset catalog |
| 04 | `04_SERVER_CONNECTION_AND_UNITY_API.md` | DTO, HTTP, auth, error and compatibility contract |
| 04A | `04A_LEARNING_THROUGH_GAMEPLAY_STORY_AND_REWARDS.md` | gameplay-design compatibility, story, hints, rewards, restoration |
| 05 | `05_SCENES_WORLDS_AND_STATION_FRAMEWORK.md` | reusable world/station framework |
| 06 | `06_LITERAQUEST_GAMEPLAY.md` | one LiteraQuest unit |
| 07 | `07_SCIENCE_QUEST_GAMEPLAY.md` | one exploration-only Science world |
| 08 | `08_HEALTH_QUEST_GAMEPLAY.md` | one PE/Health unit |
| 09 | `09_UI_CONTROLS_ACCESSIBILITY_AND_PRESENTATION.md` | UI Toolkit foundation or one screen/UI scope |
| 10 | `10_TESTING_INTEGRATION_AND_RELEASE.md` | one focused/final verification scope |
| 11 | `11_DEMO_DATA_AND_LOCAL_PROVIDER.md` | one demo fixture/provider scope |

The requirement documents are authoritative. The prompts operationalize them and enforce stopping points.
