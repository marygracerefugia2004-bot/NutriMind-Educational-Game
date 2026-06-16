# Phase 04 Unity API Client State

Status: Phase 04 implementation validated on 2026-06-14; awaiting owner approval before moving to the next Unity phase.

## Implemented

- Shared async `IGameDataProvider` contract for the student API surface.
- HTTPS `HttpProvider` with config validation, bearer auth, safe error mapping, retry/idempotency policy, and all documented Phase 04 REST endpoints.
- DTO coverage for config, auth, bootstrap/profile/settings, subjects/terms/world/stations/content, station start/resume, attempts, completion, progress, rewards, sync status, and optional student-safe narrative/reward/restoration metadata.
- `AuthSessionState` stores token type and student identity from login and clears all state on logout/reset.
- `SyncPollingService` supports opaque sync revision polling with main-thread event dispatch.
- `NoOpRealtimeService` and realtime event DTOs represent optional metadata-only WSS behavior without making WSS authoritative.
- Release-build guard prevents silent `LocalDemoJson` fallback outside editor/development builds.

## Not implemented in this phase

- Final demo fixtures or LocalDemoJson data bundles.
- UI screens, worlds, stations, or gameplay mechanics.
- Live server integration test.
- Real WSS transport.

## Validation

- Unity compilation: 0 errors.
- EditMode tests: job `236819d56669`, 505/505 passed.

## Server contract gaps to confirm

- Subject path parameter naming: `subject_slug` vs `subject_id`.
- Station list endpoint authority: server docs prefer `/student/subjects/{subject_slug}/terms/{term_number}/stations`.
- Attempt endpoint authority: server docs prefer `/student/challenges/{challenge_id}/attempts`.
- Station completion path/parameter: clarify whether `{station_id}` path plus optional `station_session_id` body is canonical.
