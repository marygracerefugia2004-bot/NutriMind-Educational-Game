# Phase 10 Task: Testing, Integration, and Release

## Mandatory Companion and Sequence

This phase prompt is not standalone. In every fresh session, load `agent-prompts/unity/00_MASTER_ORCHESTRATOR.md` together with this file. Follow the master's sequence, global rules, one-unit limit, and stop/report requirements.

**Prerequisite:** Test only a selected approved implementation scope. Final release checks require all current-milestone units to be approved.

Use `agent-prompts/unity/SESSION_START_TEMPLATE.md` to identify the selected unit and approved prerequisites.

## Authority

Also read `docs/unity/04A_LEARNING_THROUGH_GAMEPLAY_STORY_AND_REWARDS.md` and apply its learning-cycle, story, NPC, safe-mistake, discovery, reward, and restoration rules.

Read `docs/unity/10_TESTING_INTEGRATION_AND_RELEASE.md` and test only the currently approved implementation scope.

## Scope

Select one test checkpoint per cycle:

- foundation tests
- one application screen acceptance test set
- one world scene acceptance test set
- one station acceptance test set
- one term integration test set
- local demo fixture/provider contract tests
- HTTP integration tests
- Android smoke/performance checks
- final approved-scope regression pass

Do not implement unrelated features during a test phase. Fix only defects required to make the selected approved scope correct, then report them.

## Global Asset-First Rule

Before creating any asset or prefab for the current scope:

1. inspect the Unity project for user-provided or already imported assets, prefabs, materials, models, textures, sprites, icons, audio, animations, UXML, USS, and project reference images
2. use a suitable provided asset first
3. adapt it non-destructively through a project-owned prefab variant, material, wrapper, binding, collider, LOD, or import adjustment when needed
4. create or generate a new original project-owned asset only when no suitable provided asset exists
5. use project image references as visual direction for newly created assets
6. do not generate final screen backgrounds unless the user explicitly requests it; use the user-supplied background or a neutral placeholder
7. do not overwrite vendor/package/user source assets destructively
8. report every asset as reused, adapted, created/generated, placeholder, or blocked

If an asset's source, license, suitability, or intended use is unclear, stop and report it. Do not silently ship it.

## Asset Validation

Verify asset inventory evidence, stable-key mappings, provided source preservation, prefab variants, render-pipeline compatibility, Android texture/mesh suitability, missing-asset fallbacks, placeholder labeling, and asset provenance reports.

## Acceptance

Report exact tests, devices/configuration, results, defects fixed, failures remaining, and whether the selected scope is approved.

## Mandatory Stop

Complete only the scope selected for this phase. Save the work, run focused checks, provide the required report, and stop. Do not begin the next phase, scene, station, or documentation file until the user explicitly says `approved, continue` or selects another task.

## Required Report

- **Scope completed:** exact phase, checkpoint, screen, world, station, or integration unit
- **Files changed:** every created or modified file
- **Assets:** provided assets reused, prefab variants/adaptations, new/generated assets, placeholders, and blocked items
- **Behavior completed:** what now works
- **Stable keys/contracts used:** relevant scene, prefab, interactable, portal, DTO, or data-provider keys
- **Verification:** exact commands/tests/checks and results
- **Known gaps:** verified gaps for the current scope only
- **Pending work:** intentionally deferred work
- **Proposed next task:** name it, but do not start it
