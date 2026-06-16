# NutriMind Development Requirements and Agent Prompts

This package is the final documentation and agent-prompt set for the server/web app and Unity client.

## Authoritative Requirements

- `docs/SERVER_REQUIREMENTS.md` — server, dashboard, database, security, testing, operations, and Unity-facing API authority
- `docs/UNITY_REQUIREMENTS.md` — Unity authority index
- `docs/unity/01...11` — detailed Unity architecture, flow, API, gameplay, UI Toolkit, testing, and demo-data requirements

When requirements conflict:

1. Server docs control API paths, authentication, payloads, validation, scoring, rewards, progress, compatibility, and student-safe data.
2. Unity docs control scene architecture, presentation, controls, local assets, gameplay interactions, and crash-safe integration.
3. Subject gameplay files control the approved current demo mechanics and scene scope.

## Agent Prompts

### Server

Use:

- `agent-prompts/server/SERVER_MASTER_REVIEW_AND_IMPLEMENTATION.md`
- optional `agent-prompts/server/SESSION_START_TEMPLATE.md`

The server prompt covers implementation gaps, Admin/Teacher management actions, manual and prompt-based AI authoring, Unity API, security, caching, rate limiting, React/TanStack and database performance, tests, GitLab CI with inactive CD, README, and documentation synchronization.

### Unity

For every fresh Unity-agent session, load:

1. `agent-prompts/unity/00_MASTER_ORCHESTRATOR.md`
2. exactly one selected phase prompt
3. `agent-prompts/unity/SESSION_START_TEMPLATE.md` values
4. the mapped Unity requirement file

The master prompt is always required in a fresh session. In a continuing conversation, it does not need to be pasted again if it remains in context, but the agent must still follow it.

## Unity Sequence Summary

1. project root and folders
2. provided asset/prefab/reference inventory
3. shared UI Toolkit foundation
4. game-flow/state model
5. shared client systems
6. HTTP/API DTO contract
7. full local demo fixture/provider
8. shared scene/world/station framework
9. application screens one at a time
10. LiteraQuest worlds/stations one at a time
11. PE/Health worlds/stations one at a time
12. Science exploration worlds one at a time
13. final integration and Android release checks

Every Unity phase unit stops for owner review before the next unit.

## Current Unity Demo Scope

- LiteraQuest: six playable stations, two per term
- PE/Health: six playable stations, two per term
- Science: three exploration-preview worlds and no station gameplay
- UI: UI Toolkit only for new runtime UI
- Development data: full fabricated student fixture through `LocalDemoJson`
- Production/integration data: HTTPS JSON through `Http`
- WSS: optional metadata only

## Asset Policy

Use user-provided assets and prefabs first. Create project-owned variants when adaptation is needed. Create or generate original assets only when a required asset is missing after inventory. Use project image references as direction. Final UI background images remain user-provided unless explicitly authorized.

## Important Safety and Consistency Rules

- Do not hardcode canonical catalog/content/reward/progress data in Unity.
- Do not hide production catalog data in server seed scripts or source arrays.
- Manual teacher content creation must work without AI.
- AI assistance is optional, prompt-based, and returns editable drafts.
- Core Unity gameplay must work through HTTPS REST without WSS.
- `LocalDemoJson` and `Http` use the same DTOs and gameplay systems.
- Production builds must not silently fall back to fake data.
- Server-side contract changes must update the relevant Unity contract documents.
