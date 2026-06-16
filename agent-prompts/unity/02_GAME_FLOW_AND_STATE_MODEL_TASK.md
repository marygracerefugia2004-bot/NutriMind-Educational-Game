# Phase 02 Task: Game Flow and State Model

## Mandatory Companion and Sequence

This phase prompt is not standalone. In every fresh session, load `agent-prompts/unity/00_MASTER_ORCHESTRATOR.md` together with this file. Follow the master's sequence, global rules, one-unit limit, and stop/report requirements.

**Prerequisite:** Phase 01 project root, asset intake, and shared UI Toolkit foundation must be approved.

Use `agent-prompts/unity/SESSION_START_TEMPLATE.md` to identify the selected unit and approved prerequisites.

## Authority

Read `docs/unity/02_GAME_FLOW_AND_STATE_MODEL.md`, Phase 01 approvals, and the current navigation/session code.

## Scope

Review or implement the shared logical state flow only:

- startup/configuration
- demo or student authentication
- bootstrap
- main interface
- subject and term selection
- world loading
- station entry, attempt, result, and return
- session expiration
- missing content/scene
- app pause/resume
- offline/unavailable behavior
- Science exploration-only path

Do not build several screens or worlds in this phase. The output is the shared state/navigation behavior and tests needed by later scene units.

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

Asset work in this phase is limited to existing transition/loading/error presentation dependencies. Reuse the UI foundation; do not create subject-world art.

## Acceptance

- state transitions are explicit and testable
- invalid transitions fail safely
- cancellation and re-entry are handled
- demo and HTTP modes share the same flow
- Science never enters a station flow

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
