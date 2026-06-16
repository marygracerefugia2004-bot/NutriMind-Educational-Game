# LiteraQuest Gameplay Requirements

## Current Scope

LiteraQuest is the first complete playable subject for the demo.

```txt
3 term exploration worlds
2 station gameplay scenes per term
6 complete stations total
```

Every station combines world interaction with UI Toolkit support. It must not be only a disconnected quiz panel, and it must not use unrelated combat, platforming, or collection mechanics.

## LiteraQuest Asset Direction and Reuse

For each LiteraQuest term world and station, search for provided fantasy-literacy assets and prefabs before creating new work. Relevant roles include paths, books, signs, clue props, cards, market stalls, boards, workshops, bridges, writing desks, characters, portals, and interactable markers.

If suitable assets exist, reuse or adapt them through prefab variants. If an asset role is missing, create an original low/mid-poly, mobile-appropriate asset guided by the approved LiteraQuest project references. Do not bake question text, answer content, station availability, or correct answers into textures or meshes; those values come from the data provider.

Each term checkpoint must report the provided assets reused, variants created, new assets generated, and placeholders remaining.

## LiteraQuest Story and Learning Cycle

LiteraQuest uses a configurable story in which language landmarks or Language Crystals have lost their meaning and structure. The student helps short, child-friendly NPC guides restore each term world by using reading and language skills. Story text, NPC dialogue, hints, discoveries, questions, feedback, and rewards remain provider-driven.

Every station follows Discover, Practice, Apply, and Review and must provide safe, encouraging retries. Completing both stations in a term may restore the term landmark and award a provider-confirmed Language Crystal or badge.

## Term 1: Vocabulary and Sequence

### World Scene

`g5_literaquest_t1_world`

The world may use a story trail, reading grove, village path, or camp theme. It contains exactly two current-demo portals.

### Station 1: Vocabulary Clue Trail

Mission framing: help a guide restore a dim word lantern or Language Crystal by investigating environmental context clues. The selected meaning is the action that restores it.

Stable station scene key:

```txt
g5_literaquest_t1_vocabulary_clue_trail
```

Learning focus:

- determine word meaning through context clues
- connect a word to evidence from the surrounding sentence or scene

World mechanic:

- walk through a short clue trail
- inspect signs, objects, or NPC clue markers
- collect or activate only the clues relevant to the target word

UI Toolkit mechanic:

- show the passage and highlighted word
- show selected clue evidence
- choose or match the supported meaning
- present server/local-demo result and explanation

Completion:

- required clue interactions are complete
- one idempotent attempt is accepted
- result is shown
- student returns to the Term 1 world

### Station 2: Sequence Path

Mission framing: rebuild a broken story path by discovering and placing events in logical order. Accepted sequencing visibly completes the route.

Stable station scene key:

```txt
g5_literaquest_t1_sequence_path
```

Learning focus:

- identify beginning, middle, and ending events
- arrange events in logical order

World mechanic:

- locate event markers along a broken path
- bring or activate markers at the sequence area

UI Toolkit mechanic:

- arrange event keys in order using accessible controls
- provide tap/select alternatives to precision dragging
- show the ordered sequence and feedback

Visual result:

- the path or bridge becomes complete only after the accepted result

## Term 2: Inference and Fact/Opinion

### World Scene

`g5_literaquest_t2_world`

The world may use an investigation district, evidence market, or town-square theme. It contains exactly two current-demo portals.

### Station 1: Inference Investigation

Mission framing: help an NPC solve a small mystery by inspecting the scene, gathering evidence, and applying an inference.

Stable station scene key:

```txt
g5_literaquest_t2_inference_investigation
```

Learning focus:

- infer information not stated directly
- support an inference with observed clues

World mechanic:

- inspect a compact scene
- activate relevant clue objects
- avoid presenting unrelated collection as progress

UI Toolkit mechanic:

- review collected evidence
- choose the supported inference
- optionally connect the inference to one or more evidence keys

### Station 2: Fact or Opinion Market

Mission framing: help reopen the market information board by identifying which claims are verifiable facts and which are opinions.

Stable station scene key:

```txt
g5_literaquest_t2_fact_opinion_market
```

Learning focus:

- distinguish verifiable facts from opinions

World mechanic:

- visit market stalls or speakers
- gather statement cards from approved interactables

UI Toolkit mechanic:

- sort statements into Fact and Opinion
- show why a statement is verifiable or preference-based
- provide non-drag selection controls

## Term 3: Revision and Paragraph Order

### World Scene

`g5_literaquest_t3_world`

The world may use a writing workshop, scribe district, bridge workshop, or revision garden theme. It contains exactly two current-demo portals.

### Station 1: Sentence Repair Workshop

Mission framing: restart workshop devices by repairing the sentences attached to their control stations.

Stable station scene key:

```txt
g5_literaquest_t3_sentence_repair_workshop
```

Learning focus:

- correct grammar, punctuation, usage, or sentence clarity using approved content

World mechanic:

- inspect workbenches or correction stations
- gather or activate correction tools/tokens

UI Toolkit mechanic:

- select or place the correct revision
- compare original and revised sentence
- show a clear explanation after submission

### Station 2: Paragraph Order Bridge

Mission framing: restore the bridge by arranging paragraph parts into a coherent path from beginning to conclusion.

Stable station scene key:

```txt
g5_literaquest_t3_paragraph_order_bridge
```

Learning focus:

- arrange sentences or paragraph parts into coherent order

World mechanic:

- discover paragraph blocks around a bridge-work area
- bring or activate blocks at the ordering station

UI Toolkit mechanic:

- order the paragraph keys
- support buttons/tap movement in addition to drag
- animate the bridge/path completion after accepted result

## LiteraQuest Mistakes, Discoveries, and Rewards

- Failed attempts provide an approved clue, evidence reminder, or ordering scaffold instead of only showing `Incorrect`.
- Optional hidden vocabulary words, story notes, and fun facts may award small provider-confirmed bonus coins but are not required for station completion.
- Station completion may restore lanterns, paths, market signs, workshop devices, or bridges only after the accepted result.
- Completing both stations in a term may award a Language Crystal, badge, coins/stars, or a cosmetic unlock from the provider.
- No question, correct answer, hint, dialogue, or reward grant may be baked into scene art or local-only station code.

## Deferred LiteraQuest Stations

The following earlier ideas are not required for the current demo:

- Character and Setting Camp
- Point-of-View Crossroads
- Main Idea Beacon
- Evidence Trail Board
- Media Truth Check
- Figurative Language Forge
- Supporting Details Garden
- Short Response Scribe
- Final Revision Quest

They may remain server-supported or future scope. Do not implement them before all six current LiteraQuest and six current PE/Health stations are complete and approved.

## LiteraQuest Acceptance

LiteraQuest is complete for the current demo when:

- all three worlds are independently approved
- each world exposes exactly two working current-demo portals
- all six station scenes use shared provider/session/attempt/result systems
- local demo content can be replaced by HTTP without scene rewrites
- attempts are idempotent
- accepted results update progress/rewards once
- each station returns to the correct world and portal state
- every station has focused Play Mode coverage
