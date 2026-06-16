# Unity Shared Client Systems

## Purpose

This document defines reusable Unity systems required across all scenes and stations. It describes responsibilities and boundaries, not mandatory class names.

## Application Composition

The client needs one coherent application lifetime for services that must survive scene changes:

- app/session coordinator
- network/API client
- auth session store
- configuration and compatibility service
- bootstrap/profile/settings store
- subject/term/station catalog store
- sync revision service
- optional realtime metadata service
- scene loader and scene registry
- audio/localization/accessibility settings
- UI Toolkit application shell, screen navigation, modal layer, and notification layer
- shared `PanelSettings`, UXML templates, USS themes, and reusable VisualElement components
- logging and safe diagnostics
- selected game-data provider: local demo JSON or HTTP

Scene-owned gameplay objects must not become alternate global service instances.

## Game Data Provider Boundary

All application and gameplay systems consume a shared provider contract rather than reading files or sending HTTP directly. The logical provider surface covers:

- ping/config
- login/logout
- bootstrap/profile/settings
- subjects, terms, stations, and station content
- station start/resume
- attempts and completion
- progress and rewards
- sync status

Two implementations are required during development:

```txt
LocalJsonGameDataProvider -> development/demo fixture source
HttpGameDataProvider      -> real /api/v1 source
```

Names may follow existing project conventions, but the boundary is required. Both providers must return the same DTO types and provider-level result/error abstractions.

UI, scenes, stores, and station scripts must not use scattered `if (demoMode)` behavior. They call the provider and handle the same success, retryable error, unsupported content, and session state.

## Local Demo State

The local provider loads an immutable full-demo fixture and creates a session-scoped mutable state for:

- active fake session
- station start/resume state
- accepted attempt IDs
- station completion
- progress revisions
- reward wallet changes
- settings changes

This state is simulation only. It must be resettable and must never be treated as production authority.


## UI Toolkit Application Layer

All runtime application UI uses UI Toolkit. Shared UI responsibilities include:

- root `UIDocument` composition
- screen activation and navigation
- modal/dialog layer
- loading and blocking-operation layer
- toast/notification layer
- connection and compatibility banners
- safe-area application
- focus and back-navigation behavior
- accessibility and text-size classes
- background asset resolution with a neutral fallback

UXML defines structure, USS defines visual styling, and C# controllers/presenters bind application state and events. Visual elements must not send raw HTTP requests, read demo JSON directly, calculate official results, or own authentication state.

Screen controllers must register and unregister callbacks predictably so reopening a screen does not duplicate actions. Cached `VisualElement` references may be used after the visual tree is created; repeated full-tree queries or tree reconstruction during every frame are not allowed.

Application stores and the selected game-data provider remain independent from UI Toolkit. Switching between `LocalDemoJson` and `Http` must not require different UXML or USS files.

Background images are local presentation assets only. The agent creates the folder and binding point, but the user supplies the actual files under `Assets/_Project/Nutrimind/UI/Backgrounds/`. Missing assets use a safe placeholder and must not prevent interaction.

## Local Asset and Prefab Catalog

The client needs a project-owned catalog or registry that maps stable presentation keys to local Unity assets without making local asset presence authoritative for content availability.

The catalog may resolve:

- scene keys
- prefab keys
- interactable families
- environment/prop keys
- character or NPC presentation keys
- icon and sprite keys
- audio and effect keys
- UI background and illustration keys

Asset resolution order:

```txt
approved provided asset/prefab
-> approved project-owned variant or adapter
-> project-created/generated asset
-> explicit placeholder/fallback
-> safe unavailable state
```

The resolver must not choose unrelated assets silently. Missing required keys produce a development diagnostic and a student-safe unavailable or placeholder state.

Project-created/generated assets must be clearly project-owned and stored separately from imported source packages where practical. Prefab variants should be preferred when a supplied prefab only needs project-specific components, colliders, materials, bindings, or optimization settings.

The asset catalog does not determine whether a subject, term, station, or task is available. Availability still comes from the active game-data provider.

## Network Client

The shared client must provide:

- base URL and API version configuration
- JSON request/response handling
- bearer token attachment
- timeout and cancellation
- content-type validation
- safe error parsing
- retry classification
- request IDs in support messages
- no automatic retry for unsafe mutations unless the operation is idempotent
- no direct Unity object mutation from background callbacks

## Auth Session

The auth layer owns:

- active bearer session
- login/logout
- expiration and revocation handling
- session cleanup
- cancellation of authenticated work on logout
- no PIN persistence
- no logging of tokens or PINs

## DTO and Contract Layer

DTOs used by local JSON and HTTP must be the same transport models. DTOs must:

- separate transport models from scene objects
- tolerate unknown optional fields
- validate required identifiers before use
- map unknown enums to safe unsupported states
- avoid depending on field order
- avoid assuming every nullable field exists
- prevent answer keys or private data from becoming gameplay dependencies

## Data Stores

Client-side stores may hold current-session snapshots for presentation, but the server remains authoritative.

Suggested logical stores:

- connection/config state
- student/profile state
- settings state
- subject and term state
- station status state
- active station session
- progress state
- reward wallet state
- sync revisions

Stores must be cleared or re-scoped on logout so one student cannot see another student's cached data.

## No Hardcoded Catalog Data

Unity must not hardcode canonical:

- subject availability
- term availability
- station list
- unlock state
- official content
- rewards or prices
- curriculum text
- score values

Unity may register local presentation mappings such as:

- scene keys
- prefab keys
- interactable families
- icons
- audio cues
- environment assets
- fallback labels

A local asset does not make a server subject or station available. In demo mode, availability comes from the fabricated contract fixture rather than from scene presence.

## Scene Registry

Each local world mapping contains:

```txt
unity_scene_key
unity_scene_name
scene_address_key
subject_slug
grade_level
term_number
world_theme_key
local_scene_reference
availability_state
```

Registry validation should detect:

- duplicate stable keys
- missing local scene references
- mismatched subject/grade/term metadata
- unsupported address keys
- scenes excluded from the build/addressable catalog

## Interaction Framework

Shared gameplay contracts should support these families:

- inspect
- collect
- place
- sort
- match
- NPC/dialogue choice
- sequence/order
- evidence board
- scenario/path choice
- short reading panel
- fill-blank/text input
- short response/revision panel

Every task instance must expose or resolve:

- stable task key
- challenge ID
- mechanic family
- interaction state
- answer builder
- completion trigger
- reset/cancel behavior
- accessibility fallback

## Attempt Coordinator

The attempt system owns:

- answer serialization by challenge type
- `client_attempt_uuid`
- submit state
- duplicate-submit prevention
- retry of the same payload
- response correlation
- safe error handling
- server feedback delivery
- progress/reward refresh trigger

Individual station scripts must not independently grant rewards or update official progress.

## Main-Thread Dispatch

Network and realtime callbacks must enqueue data for application on Unity's main thread before touching:

- GameObjects
- components
- transforms
- UI elements
- scenes
- ScriptableObjects used by active gameplay

Destroyed or unloaded scene owners must not receive late callbacks.

## Settings Services

Settings include:

```txt
language_preference: en or tl
master_volume: 0.0 to 1.0
music_volume: 0.0 to 1.0
sfx_volume: 0.0 to 1.0
mute_all: boolean
subtitles_enabled: boolean
text_size: small, medium, large
camera_sensitivity: 0.0 to 1.0
joystick_opacity: 0.0 to 1.0
reduced_motion: boolean
show_hints: boolean
```

Unity may apply temporary local values immediately and then save to the server. Save failure must be shown without corrupting the server-backed state.

## Diagnostics

Diagnostics may include:

- safe request ID
- current client/API/contract version
- current scene key
- connection state
- last successful sync time
- non-sensitive error category

Diagnostics must not include PINs, bearer tokens, answer keys, raw server exceptions, or private student data. A development diagnostic may show `LocalDemoJson` or `Http` as the selected provider, but it must never print the fake demo PIN or private evaluator rules.
