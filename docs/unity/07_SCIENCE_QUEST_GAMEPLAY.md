# Science Quest Exploration Preview Requirements

## Current Scope

Science Quest is an exploration-only preview in the current Unity demo.

Required delivery:

```txt
3 term world scenes
0 functional station scenes
0 station portals
0 challenge submissions
0 scoring or rewards
```

Science gameplay stations are deferred until LiteraQuest and PE/Health Quest are complete and the project owner explicitly expands the scope.

## Science Preview Asset Direction and Reuse

Science currently needs exploration-world presentation only. Reuse provided terrain, rocks, plants, weather props, field equipment, measurement props, ecosystem decorations, skyboxes, materials, ambient effects, and environment prefabs before creating anything new.

When an environment role is missing, create only the minimum original mobile-appropriate preview asset needed to make the world coherent. Do not create station portals, challenge props, scoring devices, task interactions, or misleading gameplay assets for Science in this milestone.

## Shared Science World Rules

Each Science world must provide:

- registered world scene key
- player spawn
- movement and camera
- safe terrain and navigation
- representative environment theme
- ambient effects/animation where available
- UI Toolkit world HUD
- clear `Exploration Preview` or equivalent status
- safe return to term or subject selection
- placeholder/background behavior that does not crash
- no misleading functional controls

Each Science world must not provide:

- station portals
- station task prompts
- answer panels
- collectibles that affect progress
- attempt submission
- station completion
- scores
- rewards
- mastery changes
- hidden triggers that imply unfinished gameplay

Decorative educational labels may be used only when clearly non-interactive and age-appropriate.

## Term 1 World: Observation Environment

Scene key:

```txt
g5_science_quest_t1_world
```

Visual direction may include:

- field observation camp
- plants, rocks, safe specimens, or nature landmarks
- observation tables and field equipment as decorative assets
- clear paths and environmental storytelling

There is no Field Observation station in the current milestone.

## Term 2 World: Measurement Environment

Scene key:

```txt
g5_science_quest_t2_world
```

Visual direction may include:

- outdoor measurement camp
- simple lab or workshop area
- rulers, containers, scales, or chart props as decoration
- safe navigation and landmarks

There is no Measurement Mission station in the current milestone.

## Term 3 World: Weather and Ecosystem Environment

Scene key:

```txt
g5_science_quest_t3_world
```

Visual direction may include:

- weather observation landscape
- ecosystem zones
- clouds, wind, water, plants, and habitat landmarks
- ambient environmental effects

There is no Weather/Ecosystem station in the current milestone.

## Science Data Behavior

The active data provider should return:

- Science subject availability
- three Science terms
- valid world scene keys
- zero current station records for each Science term
- no fake Science station progress or rewards

The UI must treat an empty Science station list as intentional preview scope, not as an error.

## Science Acceptance

Each Science world is reviewed separately. Science preview completion requires:

- three world scenes load and return correctly
- movement/camera/HUD work
- the preview state is clear
- station lists are empty without crashes
- no station or reward side effects occur
- no next Science world is modified before the current one is approved
