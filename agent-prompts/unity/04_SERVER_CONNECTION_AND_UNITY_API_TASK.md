# Phase 04 Task: Server Connection and Unity API

## Mandatory Companion and Sequence

This phase prompt is not standalone. In every fresh session, load `agent-prompts/unity/00_MASTER_ORCHESTRATOR.md` together with this file. Follow the master's sequence, global rules, one-unit limit, and stop/report requirements.

**Prerequisite:** Phase 03 provider/service boundaries and the server contract documents must be available.

Use `agent-prompts/unity/SESSION_START_TEMPLATE.md` to identify the selected unit and approved prerequisites.

## Authority

Read `docs/unity/04_SERVER_CONNECTION_AND_UNITY_API.md`, `docs/SERVER_REQUIREMENTS.md`, and approved shared client systems.

## Scope

Implement or review:

- configuration and API versioning
- login/session handling
- bootstrap/profile/settings
- subjects/terms/stations/content
- station start/resume
- attempts/completion
- progress/rewards/sync
- safe error envelope/actions
- retry, timeout, cancellation, and idempotency
- polling and optional metadata-only WSS

Do not change gameplay scenes to work around API differences. Fix the shared provider/DTO layer.

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

Asset work is limited to validating that server/demo stable keys can resolve through the local asset catalog. Do not rename contract keys to match filenames.

## Acceptance

- HTTPS REST supports the complete required loop
- `client_attempt_uuid` retries are safe
- production does not silently use demo data
- unknown optional fields/enums do not crash Unity
- student-safe data boundaries are preserved

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
