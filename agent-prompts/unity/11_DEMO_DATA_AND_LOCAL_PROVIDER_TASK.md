# Phase 11 Task: Demo Data and Local Provider

## Mandatory Companion and Sequence

This phase prompt is not standalone. In every fresh session, load `agent-prompts/unity/00_MASTER_ORCHESTRATOR.md` together with this file. Follow the master's sequence, global rules, one-unit limit, and stop/report requirements.

**Prerequisite:** Production DTO/provider interfaces from Phases 03-04 must be approved before finalizing the fixture/provider.

Use `agent-prompts/unity/SESSION_START_TEMPLATE.md` to identify the selected unit and approved prerequisites.

## Authority

Read `docs/unity/11_DEMO_DATA_AND_LOCAL_PROVIDER.md`, the example fixture, shared DTO/provider requirements, and current demo scope.

## Scope

Select one unit per cycle:

- fixture schema and validation
- fake login/bootstrap/profile/settings
- subject/term/world records
- one term's LiteraQuest station records
- one term's PE/Health station records
- Science empty station behavior
- local mutable attempt/progress/reward state
- provider reset and production protection
- stable asset/prefab key mapping validation

Do not rewrite gameplay scenes in this phase.

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

Demo keys must map through the asset catalog. Use provided assets first, variants second, generated assets only when missing, and placeholders only when documented. Do not rename stable contract keys to match asset filenames.

## Acceptance

The selected fixture/provider unit deserializes through production DTOs, behaves deterministically, resets safely, contains no real student data/secrets, and passes focused contract tests.

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
