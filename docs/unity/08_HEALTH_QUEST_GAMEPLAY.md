# PE/Health Quest Gameplay Requirements

## Naming and Scope

The player-facing subject label is **PE/Health Quest**. The stable server/API slug may remain:

```txt
health_quest
```

Current demo delivery:

```txt
3 term exploration worlds
2 station gameplay scenes per term
6 complete stations total
```

Content must be age-appropriate, teacher-approved, non-graphic, and educational. Unity must not invent medical instructions, personalized dosage, diagnosis, or treatment advice.

## PE/Health Asset Direction and Reuse

For each PE/Health world and station, search for provided school-safe wellness, sanitation, safety, first-aid simulation, label-inspection, path-choice, sorting, and information-board assets before creating new work.

Reuse or adapt supplied prefabs through project-owned variants. New assets must be original, age-appropriate, non-graphic, generic, and mobile-appropriate. Do not reproduce real medicine branding, personalized dosage information, graphic injuries, or unapproved medical instructions in textures, labels, props, or animations.

Each term checkpoint must report the provided assets reused, variants created, new assets generated, and placeholders remaining.

## PE/Health Story and Learning Cycle

PE/Health uses a configurable community-restoration story. The student helps child-safe NPC guides restore wellness trails, sanitation areas, safety routes, and trustworthy information boards by applying approved health and safety concepts. Story, NPC dialogue, hints, discoveries, feedback, and rewards remain provider-driven.

Every station follows Discover, Practice, Apply, and Review. Mistakes use encouraging, approved educational hints and never invent medical guidance. Completing both stations in a term may restore the term community landmark and award a provider-confirmed Wellness Crystal or badge.

## Term 1: Wellness and Sanitation

### World Scene

`g5_health_quest_t1_world`

The world may use a wellness park, school/community trail, hygiene center, or healthy-habits area. It contains exactly two current-demo portals.

### Station 1: Wellness Choice Trail

Mission framing: help an NPC complete a healthy daily route; each supported choice restores part of the wellness trail after accepted completion.

Stable scene key:

```txt
g5_health_quest_t1_wellness_choice_trail
```

Learning focus:

- choose age-appropriate actions that support daily wellness

World mechanic:

- move through a short scenario path
- interact with decision points or NPC prompts
- choose a route/action at each required point

UI Toolkit mechanic:

- review the scenario and choices
- confirm the selected safe/healthy action
- show approved explanation and result

### Station 2: Sanitation Sorting Center

Mission framing: clean and reopen a community area by classifying sanitation practices and objects correctly.

Stable scene key:

```txt
g5_health_quest_t1_sanitation_sorting_center
```

Learning focus:

- classify sanitation, disposal, hygiene, and unsafe practices

World mechanic:

- inspect or gather fabricated action/object cards
- bring them to the sorting area

UI Toolkit mechanic:

- sort items into approved categories such as Safe Practice and Unsafe Practice
- provide accessible tap-based sorting
- show explanation after submission

## Term 2: First Aid and Safety Decisions

### World Scene

`g5_health_quest_t2_world`

The world may use a safety training camp, school safety area, community center, or first-aid learning zone. It contains exactly two current-demo portals.

### Station 1: First-Aid Sequence Station

Mission framing: organize approved response markers and guide the character toward trusted-adult or emergency help when the scenario requires it.

Stable scene key:

```txt
g5_health_quest_t2_first_aid_sequence
```

Learning focus:

- arrange approved, age-appropriate response steps
- recognize when to call an adult or emergency service

World mechanic:

- activate step markers in a safe simulated learning area

UI Toolkit mechanic:

- order server/local-demo provided step keys
- never display invented medical instructions
- show approved explanation and escalation guidance

### Station 2: Safety Decision Branch

Mission framing: guide an NPC through a safe route by applying age-appropriate safety decisions at each branch.

Stable scene key:

```txt
g5_health_quest_t2_safety_decision_branch
```

Learning focus:

- choose safe responses in common age-appropriate situations

World mechanic:

- navigate a branching scenario environment
- select a path at each risk point

UI Toolkit mechanic:

- explain the scenario and choice
- confirm selection
- display non-graphic consequence/feedback

## Term 3: Label Awareness and Healthy Messages

### World Scene

`g5_health_quest_t3_world`

The world may use a health-information center, safe home/community setting, pharmacy-label education area, or media-awareness plaza. It contains exactly two current-demo portals.

### Station 1: Medicine Label Safety Check

Mission framing: inspect a fabricated label and use approved warning, storage, expiry, and adult-supervision information to unlock a safe-storage area.

Stable scene key:

```txt
g5_health_quest_t3_medicine_label_safety
```

Learning focus:

- identify warnings, storage, expiry, and adult-supervision information on a fabricated label

World mechanic:

- inspect a simulated package and approved label hotspots

UI Toolkit mechanic:

- zoom/read approved sections
- match label parts to meanings
- choose the safe action

Restrictions:

- no personalized dosage
- no diagnosis
- no treatment advice
- no real product endorsement

### Station 2: Healthy Habit and Advertisement Truth Board

Mission framing: restore a trustworthy community information board by evaluating healthy-habit and advertisement claims against approved evidence.

Stable scene key:

```txt
g5_health_quest_t3_habit_advertisement_truth_board
```

Learning focus:

- identify healthy habits
- evaluate simple health-message or advertisement claims using approved evidence

World mechanic:

- inspect habit markers, posters, or advertisement boards
- collect only the required fabricated claim/evidence cards

UI Toolkit mechanic:

- sort healthy/unhealthy or supported/unsupported claims
- connect a claim to the correct evidence
- show an age-appropriate explanation

## PE/Health Mistakes, Discoveries, and Rewards

- Failed attempts provide approved, age-appropriate reminders or safer-choice scaffolding rather than only showing `Incorrect`.
- Hints must not become diagnosis, personalized treatment, dosage, or invented first-aid instructions.
- Optional wellness facts, story notes, and non-required discovery areas may appear without changing official station completion.
- Station completion may restore trails, sanitation areas, safety routes, storage areas, and information boards only after the accepted result.
- Completing both stations in a term may award a Wellness Crystal, badge, coins/stars, or a cosmetic unlock from the provider.
- No official health answer, hint, dialogue, or reward grant may be hardcoded into scene art or local-only code.

## Deferred PE/Health Scope

Additional stations are not required until the current six are complete and approved. Do not add extra portals merely because the server supports more station slots.

## PE/Health Acceptance

PE/Health is complete for the current demo when:

- all three worlds are approved one at a time
- each world exposes exactly two current-demo portals
- all six station scenes complete the local-demo attempt/result/return loop
- content remains approved, non-graphic, and student-safe
- no official result is calculated by scene code in HTTP mode
- duplicate attempt/reward side effects are prevented
- focused tests cover each station and term return flow
