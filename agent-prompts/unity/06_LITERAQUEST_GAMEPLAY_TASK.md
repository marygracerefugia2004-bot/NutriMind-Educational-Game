# Phase 06 Task: LiteraQuest Gameplay

## Mandatory Companion and Sequence

This phase prompt is not standalone. In every fresh session, load `agent-prompts/unity/00_MASTER_ORCHESTRATOR.md` together with this file. Follow the master's sequence, global rules, one-unit limit, and stop/report requirements.

**Prerequisite:** All shared foundation stages and all application screens must be approved. Complete LiteraQuest units in the order defined by the master.

Use `agent-prompts/unity/SESSION_START_TEMPLATE.md` to identify the selected unit and approved prerequisites.

## Authority

Also read `docs/unity/04A_LEARNING_THROUGH_GAMEPLAY_STORY_AND_REWARDS.md` and apply its learning-cycle, story, NPC, safe-mistake, discovery, reward, and restoration rules.

Read `docs/unity/06_LITERAQUEST_GAMEPLAY.md` and approved shared scene/framework requirements.

## One-Unit Rule

Complete exactly one selected unit per cycle:

1. Term 1 world
2. Vocabulary Clue Trail
3. Sequence Path
4. Term 1 integration
5. Term 2 world
6. Inference Investigation
7. Fact or Opinion Market
8. Term 2 integration
9. Term 3 world
10. Sentence Repair Workshop
11. Paragraph Order Bridge
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

## LiteraQuest Asset Direction

Search supplied fantasy-literacy assets first: books, paths, clue signs, cards, market stalls, boards, workshops, bridges, desks, characters, portals, and interactables. Use prefab variants for project components and mobile optimization. Generate an original low/mid-poly asset from project references only when the role is missing.

Never bake questions, answers, correct order, score, progress, or availability into textures or meshes.

## Acceptance

The selected unit must load from the approved previous flow, use provider data and stable keys, complete its intended interaction if it is a station, return correctly, pass focused tests, and include an asset provenance report.

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
