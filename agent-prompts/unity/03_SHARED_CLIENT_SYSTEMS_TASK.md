# Phase 03 Task: Shared Client Systems

## Mandatory Companion and Sequence

This phase prompt is not standalone. In every fresh session, load `agent-prompts/unity/00_MASTER_ORCHESTRATOR.md` together with this file. Follow the master's sequence, global rules, one-unit limit, and stop/report requirements.

**Prerequisite:** Phase 02 state model must be approved. Phase 01 project and asset foundations must remain valid.

Use `agent-prompts/unity/SESSION_START_TEMPLATE.md` to identify the selected unit and approved prerequisites.

## Authority

Read `docs/unity/03_SHARED_CLIENT_SYSTEMS.md` and approved Phase 01/02 outputs.

## Scope

Create or review shared systems only:

- application lifetime/composition
- game-data provider interface
- auth/session and student stores
- DTO-to-domain mapping boundary
- navigation and scene registry
- interaction contracts
- attempt coordinator
- main-thread dispatch
- settings/accessibility services
- safe diagnostics
- local asset and prefab catalog/resolver

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

## Asset Catalog Requirements

Implement a stable-key catalog that resolves provided assets first, then approved variants, then project-created/generated assets, then explicit placeholders. Local assets must not determine server/demo content availability.

Do not create individual world or station assets in this phase. Create catalog structure, validation, and shared adapters only.

## Acceptance

- no duplicate global services
- stores reset safely on logout
- local JSON and HTTP use the same DTO/provider surface
- missing asset keys produce safe diagnostics and fallback behavior
- asset catalog tests cover duplicate and missing mappings

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
