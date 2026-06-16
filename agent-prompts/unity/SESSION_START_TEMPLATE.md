# Unity Agent Session Start Template

Load these files before working:

- `agent-prompts/unity/00_MASTER_ORCHESTRATOR.md`
- `agent-prompts/unity/<SELECTED_PHASE_PROMPT>.md`
- `docs/UNITY_REQUIREMENTS.md`
- the selected phase's numbered requirement document
- `docs/SERVER_REQUIREMENTS.md` when the unit touches server contracts or server-owned data

## Selected Work

- **Phase:** `<01-11 or 04A>`
- **Selected unit:** `<one checkpoint, screen, world, station, fixture unit, or test unit>`
- **Mode:** `Build`
- **Data provider:** `LocalDemoJson | Http | contract-only`
- **Server availability:** `unavailable | available at <environment>`

## Approved Prerequisites

List only checkpoints already approved by the project owner:

- `<approved checkpoint>`
- `<approved checkpoint>`

If a required prerequisite is not approved or cannot be verified, stop before implementation and report the dependency gap.

## Available Assets and References

- Provided asset/prefab roots: `<paths>`
- UI references: `<paths>`
- World references: `<paths>`
- Station references: `<paths>`
- User-supplied backgrounds: `<paths or not yet available>`

Use supplied assets first. Generate/create only verified missing assets. Do not inspect attachments the user explicitly asked to ignore.

## Gameplay Design Amendment

- Phase 04A compatibility approved: `<yes/no/not-applicable>`
- Current mission/story scope: `<selected scope>`
- Safe-mistake/hint expectations: `<selected policy or provider-driven>`

## Restrictions for This Cycle

- Complete exactly one selected unit.
- Do not touch the next scene or phase.
- Do not change server contracts unless the selected phase requires it and the change is documented.
- Do not create final background images without explicit authorization.
- Stop with the master checkpoint report.
