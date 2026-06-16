# Phase 08 Task: PE/Health Quest Gameplay

## Mandatory Companion and Sequence

This phase prompt is not standalone. In every fresh session, load `agent-prompts/unity/00_MASTER_ORCHESTRATOR.md` together with this file. Follow the master's sequence, global rules, one-unit limit, and stop/report requirements.

**Prerequisite:** All shared foundations, application screens, and all LiteraQuest units must be approved before PE/Health begins.

Use `agent-prompts/unity/SESSION_START_TEMPLATE.md` to identify the selected unit and approved prerequisites.

## Authority

Also read `docs/unity/04A_LEARNING_THROUGH_GAMEPLAY_STORY_AND_REWARDS.md` and apply its learning-cycle, story, NPC, safe-mistake, discovery, reward, and restoration rules.

Read `docs/unity/08_HEALTH_QUEST_GAMEPLAY.md` and approved shared framework work.

## One-Unit Rule

Complete exactly one selected unit per cycle:

1. Term 1 world
2. Wellness Choice Trail
3. Sanitation Sorting Center
4. Term 1 integration
5. Term 2 world
6. First-Aid Sequence Station
7. Safety Decision Branch
8. Term 2 integration
9. Term 3 world
10. Medicine Label Safety Check
11. Healthy Habit and Advertisement Truth Board
12. Term 3 integration

Do not touch the next unit.

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

## PE/Health Asset Direction

Reuse supplied school-safe wellness, sanitation, first-aid simulation, safety, label-inspection, sorting, path-choice, and information-board assets first. New assets must be original, generic, age-appropriate, and non-graphic. Do not reproduce real medicine branding, personalized dosage, diagnosis, graphic injury, or unapproved medical instructions.

## Acceptance

The selected unit must use approved content, stable keys, shared attempts/results, safe educational presentation, focused tests, and an asset provenance report.

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
