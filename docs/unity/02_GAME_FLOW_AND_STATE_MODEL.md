# Unity Game Flow and State Model

## Purpose

This document defines the logical flow of the game and the state transitions shared by every subject, term, world, and station.

## Application States

The client should represent these logical states even if implementation uses scenes, screens, state machines, or a combination:

```txt
Starting
CheckingServer
MaintenanceBlocked
UpdateRequired
LoggedOut
Authenticating
Bootstrapping
MainMenu
SelectingSubject
SelectingTerm
LoadingWorld
InWorld
StartingStation
InStationTask
SubmittingAttempt
ShowingFeedback
RefreshingProgress
ReturningToWorld
SessionExpired
ConnectionUnavailable
FatalConfigurationError
```

Transitions must be explicit enough that duplicate requests, scene changes, logout, or a data-source switch between builds cannot leave the client in an undefined state.

## Learning Mission States

The existing state model may be extended additively with these logical substates or equivalent flags:

```txt
ShowingMissionBriefing
DiscoveringProblem
PracticingConcept
ApplyingSolution
ShowingHint
ShowingReflection
PresentingReward
ApplyingWorldRestoration
```

Completed Phase 3 code does not need a rewrite when equivalent states already exist. Add only the missing transitions and tests. Mission, hint, reward, and restoration substates must not bypass `SubmittingAttempt`, `ShowingFeedback`, or provider-confirmed completion.

## Data Source Modes

The logical game states are identical in both modes:

```txt
LocalDemoJson -> reads fabricated endpoint payloads and simulates a local student session
Http          -> sends real HTTPS requests and uses server-authoritative results
```

The selected provider is chosen during application composition before login/bootstrap. Scenes, screens, stores, and station mechanics consume common DTOs and must not branch on demo mode.

In local demo mode:

- startup reads the local ping/config fixture instead of testing an external server
- a clearly labeled demo login may authenticate only the fabricated demo student
- bootstrap and catalog data come from the full demo bundle
- attempt and completion calls return simulated, non-authoritative demo results
- progress, wallet, and revisions may update in memory for the current demo session
- restarting/resetting demo mode restores the deterministic source fixture
- the UI may display a non-production demo indicator

In HTTP mode, all identity, content, scoring, progress, completion, and rewards come from the server.

## Startup Flow

1. Initialize local configuration and services.
2. Apply safe local presentation settings.
3. Resolve ping/config from the selected provider: local fixture for `LocalDemoJson`, real endpoint for `Http`.
4. Validate API version, contract compatibility, maintenance mode, and minimum client version.
5. If a valid active session exists under the selected security design, validate or bootstrap it.
6. Otherwise show login.
7. Never load a gameplay scene before compatibility and session state are known.

## Login Flow

```txt
enter LRN + PIN
-> validate local input format
-> disable duplicate submit
-> POST login
-> parse safe error or token response
-> bootstrap student state
-> enter main menu
```

On authentication failure, do not reveal whether an LRN exists beyond the provider-approved safe message. Local demo authentication must accept only explicitly fabricated credentials or a development-only Demo Student action. On expired or revoked session, clear active session data and return to login.

## Main Menu Flow

The main menu provides:

- Play
- Profile
- Settings
- student-safe summary
- connection state
- logout

Play enters subject selection. Subject cards and availability come from the server bootstrap/subject endpoints.

## Subject and Term Flow

1. Show only server-available subject platforms.
2. Show student-friendly labels and progress.
3. Load terms for the selected subject.
4. Show term world title, description, availability, completion, and scene availability.
5. Validate the local scene registry before starting a world load.
6. If a world is unavailable locally, display a safe message and keep the student in term selection.

## World Flow

On entering a term world:

1. Resolve registered scene and world metadata.
2. Load the scene through the shared loader.
3. Spawn the player at the correct safe spawn.
4. Fetch or apply the current station list and revisions.
5. For LiteraQuest and PE/Health, bind the two current-demo station states to registered portals/interactables.
6. For Science Quest, accept an intentionally empty station list, show exploration-preview state, and do not create station portals or gameplay side effects.
7. Enable movement only after critical setup succeeds.
8. Keep an exit path to term selection or main menu.

Worlds must not calculate unlocks locally. LiteraQuest and PE/Health render provider-supplied station states. Science exploration worlds treat an empty station list as valid current scope rather than a content error.

## Station State Model

Supported station states:

```txt
Unavailable
Locked
Unlocked
Started
AttemptPending
PendingReview
Completed
ArchivedOrUnpublished
```

Typical state flow:

```txt
Locked
-> Unlocked
-> Started
-> AttemptPending
-> Completed
```

A short-response or teacher-reviewed task may use:

```txt
Started
-> AttemptPending
-> PendingReview
-> Completed after server-approved resolution
```

Unity must handle a station becoming locked, unpublished, or changed while the student is in the world by showing a safe refresh/return flow. The local demo bundle must include at least one safe fixture for these states even if the main demo student has access to all required stations.

## Station Session Flow

1. Student interacts with an unlocked portal.
2. Unity requests station start/resume.
3. Server returns session and approved content.
4. Unity validates required task data and stable mappings.
5. Unity starts the interaction.
6. Local answer state is kept until a definitive server response.
7. Unity submits one answer using a generated `client_attempt_uuid`.
8. Network retry of the same answer reuses that UUID.
9. Server response determines correctness, points, rewards, progress, and completion.
10. Unity presents feedback and refreshes changed state.
11. Unity returns to the world and updates the portal.

## Learning Cycle Flow

Every playable station uses this flow:

```txt
mission briefing / NPC request
-> Discover the problem in the station environment
-> Practice through concept-related interactions
-> Apply the concept to solve the mission
-> submit the official answer
-> provider-confirmed feedback
-> Review and reflection
-> provider-confirmed reward/progress presentation
-> visible world restoration
-> return to the term world
```

Optional discoveries may occur during world or station exploration, but they cannot replace required learning interactions or block station completion.

## Safe Mistake Flow

A failed attempt returns to a recoverable station state. Unity keeps valid local interaction progress, displays provider-approved encouragement, and offers the next hint tier when allowed. Ordinary wrong attempts must not cause harsh loss of progress, access, or earned rewards. The client must not reveal hidden answer data or invent its own hint text in HTTP mode.

## Failure and Recovery Flow

### Timeout While Answering

- Keep local answer state.
- Show retry or return option according to error action.
- Reuse the same attempt UUID when retrying the same submitted answer.

### Session Expired

- Stop polling/realtime.
- Clear active session credentials.
- Return to login.
- Do not continue showing authoritative gameplay state as active.

### Content Changed or Unpublished

- Stop the affected station safely.
- Refresh sync status/content.
- Return to world or term selection with a friendly message.

### Scene Missing

- Do not load a different scene.
- Show world unavailable.
- Include safe support details such as request ID or scene key where appropriate.

### App Pause or Scene Unload

- Cancel scene-owned requests.
- Pause polling as required.
- Do not allow callbacks from destroyed objects to update UI.
- Resume through a validated session and scene state.

## Story and World Restoration Flow

Term worlds may show a provider-driven story objective and restoration progress. An accepted station completion may update a world landmark, crystal, path, lighting state, information board, community area, or other approved visual state. Unity may play restoration animation only after the provider confirms completion.

NPC dialogue and story text use provider/demo stable keys and have a UI Toolkit fallback if a character asset is unavailable.

## Progress and Reward Presentation

Unity may animate expected outcomes only after the server returns the official result. It must distinguish:

- local interaction feedback
- server-confirmed correctness
- server-confirmed score
- server-confirmed reward/wallet change
- pending teacher review

## Offline Scope

Full offline progression is not required. When the server is unavailable, Unity shows an unavailable/offline placeholder and a safe retry route. It must not create unsynchronized official attempts, progress, rewards, or unlocks.
