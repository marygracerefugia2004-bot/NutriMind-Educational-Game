# Phase 01 Task: Foundation, Asset Intake, and Delivery Order

## Mandatory Companion and Sequence

This phase prompt is not standalone. In every fresh session, load `agent-prompts/unity/00_MASTER_ORCHESTRATOR.md` together with this file. Follow the master's sequence, global rules, one-unit limit, and stop/report requirements.

**Prerequisite:** No implementation phase is assumed complete. Verify the Unity project root and current repository state first.

Use `agent-prompts/unity/SESSION_START_TEMPLATE.md` to identify the selected unit and approved prerequisites.

## Authority

Read `docs/unity/01_FOUNDATION_AND_DELIVERY_ORDER.md` and the Unity requirements index. This prompt owns only the initial project-and-asset foundation units. Other foundation dependencies are implemented by their mapped phase prompts.

## One-Unit Rule

Complete exactly one selected unit per cycle:

1. Unity project root and project-owned folder structure
2. provided asset, prefab, UI-reference, world-reference, and station-reference intake/classification
3. foundation dependency audit after Phases 02, 03, 04, 05, 09, and 11 have completed their mapped shared systems

Do not implement the UI Toolkit foundation, game-flow state model, client services, HTTP provider, demo provider, or scene framework inside Phase 01. Use their mapped phase prompts:

- Phase 09 for UI Toolkit foundation
- Phase 02 for game flow/state model
- Phase 03 for shared client systems
- Phase 04 for HTTP/API contract
- Phase 11 for demo data/provider
- Phase 05 for shared scene/world/station framework

## Unit Requirements

### Project Root and Folders

Confirm `Assets/`, `Packages/`, and `ProjectSettings/`. Create or confirm `Assets/_Project/Nutrimind/` and the approved project-owned subfolders. Do not create a nested Unity project or move imported packages merely for organization.

### Asset and Prefab Intake

Inventory provided assets and classify each relevant item as ready, adaptable, needs mobile/import adjustment, reference-only, missing, or blocked. Identify stable roles/keys, likely prefab variants, final-background availability, and missing assets. Stop before creating new art.

### Foundation Dependency Audit

After the mapped phases have been completed, verify that later scenes can reuse one approved UI system, data-provider boundary, state model, client service layer, scene loader, world template, station framework, attempt flow, and demo fixture. Do not create missing systems during the audit; report the owning phase that must be revisited.

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

## Acceptance

The completed checkpoint must be reusable by later scenes and must not hide missing shared dependencies inside a feature scene.

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
