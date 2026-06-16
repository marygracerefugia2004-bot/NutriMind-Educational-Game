# Phase 04A Task: Learning-Through-Gameplay Design Amendment

## Mandatory Companion and Authority

In a fresh session, load `agent-prompts/unity/00_MASTER_ORCHESTRATOR.md` with this prompt. Read `docs/UNITY_REQUIREMENTS.md` and `docs/unity/04A_LEARNING_THROUGH_GAMEPLAY_STORY_AND_REWARDS.md`. Read the completed Phase 1–4 implementation before editing.

## Purpose

Phases 1–4 are already approved. Do not rebuild them. Perform one additive compatibility review so future worlds and stations support learning-through-gameplay, story missions, NPC guidance, safe mistakes, tiered hints, optional discoveries, provider-confirmed rewards, and visible world restoration.

## Selected Unit

Complete only one selected retrofit unit per cycle:

1. state-flow compatibility
2. shared UI Toolkit mission/NPC/hint/discovery/reward components
3. shared client-store/service compatibility
4. DTO/provider compatibility for optional narrative fields
5. demo fixture compatibility
6. gameplay-design integration audit

Do not begin a subject world or station.

## Rules

- Preserve approved Phase 1–4 architecture and public interfaces when possible.
- Add only missing additive states, optional fields, shared components, services, and tests.
- Learning must be performed through the station mechanic, not a decorative walk followed by a detached quiz.
- Use Discover, Practice, Apply, and Review.
- Use concise NPC mission guidance, safe retry feedback, and approved tiered hints.
- Keep discoveries optional and provider-driven.
- Present official rewards and world restoration only after accepted provider results.
- Keep Science exploration-only.
- Stop after the selected retrofit unit and report all files, tests, gaps, and the proposed next unit.
