# Phase 09 Task: UI Toolkit, Controls, Accessibility, and Presentation

## Mandatory Companion and Sequence

This phase prompt is not standalone. In every fresh session, load `agent-prompts/unity/00_MASTER_ORCHESTRATOR.md` together with this file. Follow the master's sequence, global rules, one-unit limit, and stop/report requirements.

**Prerequisite:** For UI foundation, Phase 01 project root and asset intake must be approved. For a screen, all shared foundation stages and prior screens in sequence must be approved.

Use `agent-prompts/unity/SESSION_START_TEMPLATE.md` to identify the selected unit and approved prerequisites.

## Authority

Read `docs/unity/09_UI_CONTROLS_ACCESSIBILITY_AND_PRESENTATION.md` and the approved UI foundation.

## One-Scope Rule

Complete exactly one selected UI scope per cycle:

- shared UI Toolkit foundation component set
- Bootstrap/Application Root presentation and root UI layers
- Splash
- Login
- Main Interface
- Profile
- Settings
- Subject Selection
- Term Selection
- Loading/Transition
- World HUD
- one station UI scope
- one accessibility/control improvement scope

Do not build the next screen.

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

## UI Asset Rules

Use provided icons, fonts, illustrations, UI textures, UXML, USS, and visual assets first. Prefer UXML/USS for scalable panels and states rather than full-screen raster art. If a needed UI asset is missing, create an original project-owned asset using project references for direction. Do not bake live data or editable text into images.

Final background images remain user-supplied unless explicitly authorized. Use a neutral placeholder that can be replaced without logic changes.

## Acceptance

- UI Toolkit only for new runtime UI
- safe area and landscape layout work
- loading/error/disabled/focus states work
- event callbacks clean up
- local demo and HTTP share the screen
- asset paths and provenance are reported

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
