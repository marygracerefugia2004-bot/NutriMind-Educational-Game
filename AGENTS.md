# NutriMind — OpenCode Agent Instructions

## Purpose

This file defines repository-wide rules for OpenCode agents working on the NutriMind Unity project.

These rules apply in addition to:

- `agent-prompts/unity/00_MASTER_ORCHESTRATOR.md`
- the selected Unity phase prompt
- `docs/UNITY_REQUIREMENTS.md`
- the matching file under `docs/unity/`
- `docs/SERVER_REQUIREMENTS.md` when work affects the server contract

When instructions conflict, use this priority:

1. `docs/SERVER_REQUIREMENTS.md` for server-owned API, auth, scoring, rewards, progress, compatibility, and student-safe data.
2. `docs/UNITY_REQUIREMENTS.md` and the numbered Unity requirement files.
3. `agent-prompts/unity/00_MASTER_ORCHESTRATOR.md`.
4. The selected phase prompt.
5. This `AGENTS.md`.
6. Local implementation conventions inferred from the codebase.

Do not recreate deleted task folders, old phase documents, or obsolete documentation.

---

## Project Shape

- **Unity 6000.4.10f1** is declared in `ProjectSettings/ProjectVersion.txt`.
- The repository uses the standard Unity layout: `Assets/`, `Packages/`, and `ProjectSettings/`.
- Generated `.csproj`, `.sln`, and `.slnx` files are not authoritative source files.
- Project-owned code and assets live under:

```text
Assets/_Project/Nutrimind/
```

Current project-owned assemblies and locations include:

- `NutriMind.Runtime.App`
    - `Runtime/App/`
    - Core systems such as `CompositionRoot`, `AppStateMachine`, `SessionScope`, `SceneRegistry`, `AssetCatalogValidator`, `AttemptCoordinator`, `MainThreadDispatcher`, `HttpProvider`, `LocalDemoJsonProvider`, and `IGameDataProvider`.
- `NutriMind.Runtime.UI`
    - `Runtime/UI/`
    - Shared UI Toolkit helpers, controllers, presenters, and bindings.
- UI Toolkit assets:
    - UXML under `UI/Documents/`
    - USS under `UI/Styles/`
    - themes under `UI/Themes/`
    - shared `PanelSettings` under `UI/PanelSettings/`
- Scenes:
    - application scenes under `Scenes/App/`
    - world scenes under their approved subject/term folders
    - station scenes under their approved subject/term folders
- Tests:
    - Edit Mode tests under `Tests/EditMode/`
    - Play Mode tests under `Tests/PlayMode/`

Do not create a nested Unity project inside the repository.

---

## Third-Party and Unity Store Assets

The project owner stores Unity Asset Store and other third-party content under:

```text
Assets/_Project/Nutrimind/ThirdParty/
```

There may also be legacy or imported third-party folders elsewhere under `Assets/`. Always inspect the actual project tree before assuming a single vendor location.

### Third-Party Protection Rules

- Treat all content under `ThirdParty/` as vendor-owned source material.
- Do not destructively edit vendor prefabs, scenes, models, textures, materials, animations, scripts, shaders, UXML, USS, or package metadata.
- Do not rename, move, or delete vendor assets unless the owner explicitly requests it.
- Do not modify third-party `.meta` files manually.
- Do not add project-specific scripts directly to vendor prefabs.
- Do not overwrite vendor materials or textures to make them project-specific.
- Do not edit imported package code merely to bypass an integration issue.

When a third-party asset needs adaptation, create a project-owned derivative in an approved folder, such as:

```text
Assets/_Project/Nutrimind/Prefabs/Variants/
Assets/_Project/Nutrimind/Art/Materials/
Assets/_Project/Nutrimind/Art/Textures/
Assets/_Project/Nutrimind/UI/
```

Prefer prefab variants, wrapper prefabs, project-owned materials, project-owned controllers, adapters, bindings, and composition rather than vendor modification.

Record the source asset path and the project-owned derivative path in the asset manifest or completion report.

---

## Asset-First Rule

The project owner frequently adds new game assets, prefabs, UI references, icons, textures, models, audio, animations, and Unity Store packages.

Asset discovery is therefore not a one-time Phase 1 activity.

### Required Asset Check Before Every Visual Task

Before creating or generating any visual asset, UI Toolkit asset, prefab, material, texture, icon, model, animation, audio clip, world prop, or station prop:

1. Re-scan the relevant project asset folders.
2. Re-scan `Assets/_Project/Nutrimind/ThirdParty/`.
3. Re-scan project-owned provided-asset folders.
4. Re-scan the relevant reference-image folders.
5. Check whether the owner recently added a suitable asset.
6. Check whether an existing prefab can be reused or adapted safely.
7. Check whether a project-owned variant already exists.
8. Create or generate a new asset only when no suitable reusable option exists.

Never rely only on an old asset inventory when starting a new screen, world, station, or UI component.

### Asset Selection Order

Use this strict priority:

1. Suitable owner-provided project asset or prefab.
2. Suitable third-party asset used without destructive modification.
3. Existing project-owned prefab variant or adapter.
4. New project-owned variant, wrapper, material, controller, or binding.
5. Newly created or generated asset based on approved references.
6. Explicit placeholder when the final asset is unavailable or blocked.

Do not skip directly to generation.

### Required Asset Status Reporting

For every completed visual unit, classify relevant assets as:

- `reused`
- `adapted_as_variant`
- `created`
- `generated`
- `placeholder`
- `blocked`
- `waiting_for_owner_asset`

Report the asset role, source path, final project-owned path, stable key when applicable, action taken, and whether it is safe for production use.

---

## Image Reference Rules

Image references inside the project may guide layout, spacing, component proportions, typography hierarchy, button shapes, panel shapes, border treatment, icon style, navigation structure, visual hierarchy, and world or station mood.

Do not assume a reference image is a shippable asset.

Do not:

- copy a reference image wholesale into the game
- extract copyrighted elements from a reference
- embed interactive labels into a background image
- make layout logic depend on a flattened screenshot
- generate a replacement for a final background that the owner said they will provide

When the final background is not available, use a neutral project-owned placeholder that can be replaced without changing UI logic.

---

## Canvas/uGUI to UI Toolkit Migration

The project may contain existing Canvas/uGUI prefabs that the owner wants converted to UI Toolkit.

All new runtime UI must use UXML, USS, `UIDocument`, shared `PanelSettings`, reusable `VisualTreeAsset` components, and C# controllers, presenters, and bindings.

Do not create new runtime Canvas/uGUI screens, HUDs, dialogs, station panels, or menus unless the owner explicitly approves a documented exception.

### Conversion Rules

A Canvas prefab is a visual and behavioral reference, not a file that can be converted automatically without review.

For each Canvas prefab selected for migration:

1. Inspect the prefab hierarchy, anchors, layout groups, fonts, sprites, textures, animations, event handlers, accessibility behavior, and runtime bindings.
2. Identify reusable non-uGUI assets such as sprites, icons, textures, fonts, and audio.
3. Recreate the visual hierarchy as UXML.
4. Recreate styling and responsive layout as USS.
5. Recreate behavior using UI Toolkit controllers, presenters, bindings, and events.
6. Map Canvas concepts to UI Toolkit equivalents rather than imitating RectTransform logic.
7. Preserve safe-area, Android landscape, keyboard/controller, and touch behavior.
8. Preserve loading, disabled, focus, error, empty, and accessibility states.
9. Add tests for loading, callbacks, navigation, scaling, and cleanup.
10. Compare the UI Toolkit result with the approved Canvas/reference design.
11. Keep the original Canvas prefab untouched until UI Toolkit parity is reviewed and approved.
12. After approval, mark the Canvas prefab as legacy, archived, or removable according to owner instructions.

Do not:

- edit the original Canvas prefab destructively
- mix Canvas and UI Toolkit for the same screen without an approved migration reason
- retain duplicated business logic in both implementations
- make the UI Toolkit version read JSON or call HTTP directly
- delete the old prefab before parity and reference checks pass

### Migration Output

For every migrated Canvas prefab, report:

- original Canvas prefab path
- new UXML path
- new USS path
- controller or presenter path
- reused sprites, fonts, textures, and icons
- behaviors migrated
- behaviors intentionally omitted
- visual or functional differences
- tests run
- legacy-prefab disposition

---

## Authoritative Requirements

| Source                                                          | Controls                                                                                                          |
| --------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------- |
| `docs/SERVER_REQUIREMENTS.md`                                   | API paths, authentication, payloads, validation, scoring, rewards, progress, compatibility, and student-safe data |
| `docs/UNITY_REQUIREMENTS.md`                                    | Unity scope and requirement index                                                                                 |
| `docs/unity/01_FOUNDATION_AND_DELIVERY_ORDER.md`                | Approved implementation order and prerequisites                                                                   |
| `docs/unity/02_GAME_FLOW_AND_STATE_MODEL.md`                    | Game states, navigation, recovery, and transitions                                                                |
| `docs/unity/03_SHARED_CLIENT_SYSTEMS.md`                        | Shared stores, services, composition, navigation, attempts, and diagnostics                                       |
| `docs/unity/04_SERVER_CONNECTION_AND_UNITY_API.md`              | Unity DTOs, provider interfaces, HTTP behavior, and API compatibility                                             |
| `docs/unity/04A_LEARNING_THROUGH_GAMEPLAY_STORY_AND_REWARDS.md` | Learning-through-gameplay, story, NPCs, hints, rewards, discoveries, and restoration                              |
| `docs/unity/05_SCENES_WORLDS_AND_STATION_FRAMEWORK.md`          | Shared scene, world, portal, and station architecture                                                             |
| `docs/unity/06_LITERAQUEST_GAMEPLAY.md`                         | Approved LiteraQuest demo mechanics and scene scope                                                               |
| `docs/unity/07_SCIENCE_QUEST_GAMEPLAY.md`                       | Science exploration-preview scope                                                                                 |
| `docs/unity/08_HEALTH_QUEST_GAMEPLAY.md`                        | Approved PE/Health mechanics, scope, and safety                                                                   |
| `docs/unity/09_UI_CONTROLS_ACCESSIBILITY_AND_PRESENTATION.md`   | UI Toolkit, controls, accessibility, responsive layout, and presentation                                          |
| `docs/unity/10_TESTING_INTEGRATION_AND_RELEASE.md`              | Test, integration, performance, Android, and release requirements                                                 |
| `docs/unity/11_DEMO_DATA_AND_LOCAL_PROVIDER.md`                 | Local demo fixture, provider parity, and production safeguards                                                    |

Server-owned contract decisions override Unity assumptions.

---

## Required Reading for Every Fresh Unity Session

Read:

1. `AGENTS.md`
2. `agent-prompts/unity/00_MASTER_ORCHESTRATOR.md`
3. exactly one selected phase prompt under `agent-prompts/unity/`
4. `docs/UNITY_REQUIREMENTS.md`
5. the mapped requirement file for that phase
6. related requirement files explicitly referenced by the selected phase
7. `docs/SERVER_REQUIREMENTS.md` when work affects the server contract

Do not skip the master prompt. Do not auto-start the next phase or unit. Complete one approved unit, test it, report it, and stop for owner review.

---

## Approved Logical Sequence

1. Project root and project-owned folders.
2. Current asset, prefab, third-party, and reference inventory.
3. Shared UI Toolkit foundation.
4. Game-flow and state model.
5. Shared client systems.
6. Phase 04A gameplay-design compatibility.
7. HTTP/API DTO contract.
8. Full local demo fixture and provider.
9. Shared application/world/station framework.
10. Application screens one at a time.
11. LiteraQuest worlds and stations one unit at a time.
12. PE/Health worlds and stations one unit at a time.
13. Science exploration worlds one at a time.
14. Final integration, Android, performance, and release verification.

Focused Phase 10 checks apply after every implementation unit.

---

## Current Demo Scope

### LiteraQuest

Six playable stations:

- Vocabulary Clue Trail
- Sequence Path
- Inference Investigation
- Fact or Opinion Market
- Sentence Repair Workshop
- Paragraph Order Bridge

### PE/Health

Six playable stations:

- Wellness Choice Trail
- Sanitation Sorting Center
- First-Aid Sequence Station
- Safety Decision Branch
- Medicine Label Safety Check
- Healthy Habit and Advertisement Truth Board

### Science

Three exploration-preview worlds and zero playable stations in the current milestone.

Do not add Science station gameplay unless the requirements are explicitly changed.

---

## Non-Negotiable Unity Rules

- Use UI Toolkit for all new runtime UI.
- Re-scan provided and third-party assets before generating anything.
- Protect vendor assets from destructive modification.
- Convert approved Canvas/uGUI prefabs into project-owned UI Toolkit equivalents.
- Keep the original Canvas prefab until migration parity is approved.
- `LocalDemoJson` and `Http` must share DTOs, stores, UI, scenes, and gameplay systems.
- Consumers must not branch on `DataProviderMode`.
- Individual screens and scenes must not read demo JSON directly.
- Individual screens and scenes must not create their own HTTP clients.
- Production must never silently fall back to fake data.
- The server decides official identity, content, availability, scoring, rewards, progress, mastery, and reports.
- Unity must not hardcode canonical catalog, curriculum, station, reward, score, or availability data.
- Preserve stable server keys and API compatibility.
- Retry attempts must reuse `client_attempt_uuid`.
- Unknown optional fields and enum values must fail safely.
- Network callbacks must not mutate Unity objects off the main thread.
- WSS is optional metadata only.
- HTTPS REST must support the complete core game flow.
- Learning must be part of gameplay, not a quiz added after unrelated exploration.
- Wrong answers must use safe, encouraging feedback and approved hint progression.
- Science remains exploration-only for the current milestone.
- Health content must remain age-appropriate, non-graphic, and free from personalized diagnosis, dosage, or treatment advice.

---

## Data Provider Selection

`CompositionRoot` selects the provider according to the approved session or environment mode. Consumers should depend on `IGameDataProvider` and shared stores.

They must not contain logic such as:

```csharp
if (mode == DataProviderMode.LocalDemoJson)
{
    // separate screen or gameplay behavior
}
```

Provider differences belong inside provider implementations and composition.

---

## Scene and Prefab Editing

Prefer Unity MCP tools for scene creation, scene inspection, component changes, prefab creation, prefab variants, asset references, screenshots, editor state, and test runner operations.

Avoid direct serialized YAML editing of `.unity`, `.prefab`, and `.asset` files unless no safe Unity MCP or Editor operation exists, the change is small and understood, serialized references are preserved, the file is backed up or committed, and Unity is reopened and validated afterward.

Do not run multiple agents or Unity Editor instances against the same project directory.

---

## Testing and Validation

| Layer       | Required focus                                                                                    |
| ----------- | ------------------------------------------------------------------------------------------------- |
| Edit Mode   | DTOs, providers, stores, registries, stable keys, state transitions, and pure gameplay logic      |
| Play Mode   | Scene loading, UI Toolkit interaction, player/world flow, station mechanics, and callback cleanup |
| Contract    | Local JSON deserializes through the same DTOs as HTTP                                             |
| Integration | Development server behavior when available                                                        |
| Android     | Landscape smoke, safe areas, touch controls, memory, and loading                                  |
| Manual      | Curriculum, presentation, references, and asset suitability                                       |

After every implementation unit:

1. Check Unity compilation.
2. Run focused Edit Mode tests.
3. Run focused Play Mode tests when the unit has scene or UI behavior.
4. Validate missing references.
5. Validate callbacks and cleanup.
6. Validate Android landscape behavior when relevant.
7. Report checks that could not run.

Do not claim a check passed when it was not run.

---

## OpenCode and MCP Notes

- `opencode.json` configures a local Unity MCP server named `unity` and a file-system MCP.
- Prefer Unity MCP for Editor-owned operations.
- Use file-system tools for Markdown, JSON, C#, UXML, USS, and other text files when safe.
- Do not create hidden automation that edits future phases without owner approval.
- Do not begin the next scene or phase automatically.
- Stop after the selected phase unit and provide the required completion report.

---

## Required Completion Report

After every approved unit, report:

1. Completed scope.
2. Files created or changed.
3. Provided assets reused.
4. Third-party assets reused.
5. Assets adapted as project-owned variants.
6. Assets created or generated.
7. Canvas prefabs migrated or pending migration.
8. Placeholder, blocked, or owner-needed assets.
9. Shared systems used.
10. Behavior implemented.
11. Tests and checks run.
12. Known gaps.
13. Pending cross-document or cross-scene changes.
14. Proposed next unit.

Stop after the report and wait for explicit owner approval.
