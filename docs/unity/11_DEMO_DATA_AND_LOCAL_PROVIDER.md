# Unity Full Demo Data and Local Provider Requirements

## Purpose

This document defines a complete development/demo student session while the server is unavailable.

The local dataset must imitate the real student API contract so changing from JSON to HTTP does not require rewriting UI, scenes, station mechanics, stores, or navigation.

Reference fixture:

```txt
docs/unity/examples/full-demo-student-data.json
```

## Provider Boundary

```txt
IGameDataProvider or equivalent
  |- LocalJsonGameDataProvider
  `- HttpGameDataProvider
```

Required behavior:

- same DTOs
- same result/error abstractions
- same stores
- same UI Toolkit screens
- same world/station scenes
- centralized JSON access
- centralized HTTP access
- no scene reads JSON directly
- no station sends raw HTTP directly
- no scattered gameplay-changing `if (demoMode)` branches

## Current Local Demo Coverage

The full fake student bundle covers:

- fake config and development-only login
- bootstrap, profile, classroom, and settings
- LiteraQuest, PE/Health Quest, and Science Quest subject cards
- all three terms for each subject
- all nine term world scene keys
- six LiteraQuest playable stations
- six PE/Health playable stations
- empty station lists for all three Science terms
- station start/resume responses for the 12 playable stations
- fabricated approved content for the 12 playable stations
- simulated attempts and completion for the 12 playable stations
- progress, rewards, and sync revisions
- safe error fixtures

Science worlds are exploration-only. The fixture must not create Science station content, attempts, completion, scores, or rewards in this milestone.

## Demo Story and Learning-Gameplay Coverage

The full demo fixture should include fabricated, student-safe examples for story context, mission summaries, NPC guides, the four-step learning cycle, tiered hints, optional discoveries, reflection, reward previews, and world-restoration states for all 12 playable stations. These fields use the same DTO structure expected from HTTP and remain optional for compatibility.

Demo state may simulate discoveries, hint use, coins/stars, Language/Wellness Crystals, badges, and world restoration. It must preserve idempotency and reset to the immutable source fixture. Science fixture data must not simulate station missions, station rewards, or station completion.

## Required Fixture Shape

```txt
fixture_format_version
fixture_id
mode
notice
demo_auth
responses
terms_by_subject
stations_by_scope
station_content_by_id
station_start_by_id
attempt_result_by_challenge_id
completion_result_by_station_id
demo_only_evaluation
error_fixtures
```

The outer fixture is a development container. Nested response objects should mirror actual endpoint DTOs.

## Fake Student Rules

The fixture represents one fabricated student and may include:

- fake student ID
- clearly fake 12-digit demo LRN
- fake display name
- grade level
- fake section/classroom and teacher display name
- language/accessibility settings
- subject/term availability
- progress summary
- reward wallet
- revision values

Never include real student information, real credentials, real access tokens, provider secrets, teacher/admin private data, or production answer keys.

## Required Station Coverage

```txt
LiteraQuest: 6
PE/Health:   6
Science:     0
Total:      12 playable stations
```

Each playable station requires:

- station ID and stable station key
- subject slug, grade, and term
- station scene key
- title and description
- state and progress
- portal/interactable/prefab keys
- completion rule
- at least one world task
- at least one challenge
- valid answer shape
- simulated attempt result
- simulated completion result

Science `stations_by_scope` entries must exist for each term but contain empty `stations` arrays. This proves the UI handles intentional no-station scope safely.

## Demo Fixture Asset-Key Compatibility

The local demo fixture must use the same stable local presentation keys expected by the HTTP contract, including scene, portal, interactable, prefab, icon, and optional environment keys.

Before adding a new demo key:

1. resolve it to a suitable provided asset/prefab when available
2. otherwise resolve it to an approved project-owned variant or newly created/generated asset
3. otherwise use a documented placeholder or unsupported state

Do not change fixture keys merely to match arbitrary asset filenames. Maintain a catalog mapping between stable contract keys and local asset references.

## Local Attempts and Mutable State

The local provider accepts the same attempt DTO used by HTTP, including `client_attempt_uuid`.

It must:

- produce one result per unique attempt
- return the same result for an identical retry
- reject/safely handle the same UUID with a different payload
- update progress once
- grant a simulated reward once
- use the HTTP-compatible safe error shape

The source fixture remains immutable. Runtime uses a resettable session copy for:

- settings
- station start state
- accepted attempt UUIDs
- station completion
- progress
- rewards
- revisions

## Storage and Configuration

Recommended project path:

```txt
Assets/_Project/Nutrimind/DemoData/full-demo-student-data.json
```

Configuration is explicit:

```txt
data_source: LocalDemoJson | Http
api_base_url: required for Http
full_demo_fixture_key: required for LocalDemoJson
show_demo_indicator: true for LocalDemoJson
```

Never infer local demo mode only because HTTP failed.

## Production Protection

- production selects `Http`
- release builds reject `LocalDemoJson`
- CI/build validation detects demo mode in production settings
- demo fixtures are excluded from release packaging where practical
- no automatic fallback from real HTTP failure to fake data

## Switch to HTTP

Switching is complete when:

1. composition selects `HttpGameDataProvider`
2. the same DTOs/stores continue working
3. the same UI/world/station scenes continue working
4. local answer evaluator is not used
5. login, attempts, progress, rewards, and completion use real endpoints
6. fixture contract tests remain as regression tests
7. integration tests pass against the server

Fix differences in contract mapping/provider layers, not inside individual scenes.

## Demo Acceptance

```txt
start app
-> demo login
-> load fake profile/settings
-> view three subjects and nine terms
-> enter nine registered worlds
-> complete 6 LiteraQuest stations
-> complete 6 PE/Health stations
-> enter 3 Science exploration-only worlds with no stations
-> submit idempotent simulated attempts
-> see progress/rewards update once
-> reset fake student state
```

The local demo proves scene and contract-shaped behavior. It is not proof of final server integration.
