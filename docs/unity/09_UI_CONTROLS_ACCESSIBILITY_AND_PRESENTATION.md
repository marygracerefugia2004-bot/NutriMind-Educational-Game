# Unity UI Toolkit, Controls, Accessibility, and Presentation

## UI Technology Decision

All runtime game UI must use Unity UI Toolkit.

Required UI Toolkit assets and concepts:

- UXML for screen and component structure
- USS for styles, themes, responsive layout, and visual states
- `UIDocument` for runtime presentation
- shared `PanelSettings` for scale, text, and panel behavior
- C# controllers or presenters for event handling and state binding
- reusable `VisualTreeAsset` templates for shared components

Do not create new uGUI Canvas-based:

- application screens
- HUDs
- dialogs
- loading overlays
- station answer panels
- feedback/reward panels
- touch controls
- profile or settings screens

A legacy or third-party Canvas may remain only when removal is unsafe and the user explicitly approves the exception. The exception must be isolated, documented, and must not become the pattern for new UI.

## Project Folder Must Exist First

Before creating any UI screen, confirm a valid Unity project root containing:

```txt
Assets/
Packages/
ProjectSettings/
```

Then create or confirm the project-owned UI structure:

```txt
Assets/_Project/Nutrimind/
  Runtime/
    UI/
  UI/
    Documents/
      AppShell/
      Screens/
      Components/
      Overlays/
      Stations/
    Styles/
      Base/
      Components/
      Screens/
      Accessibility/
    Themes/
    PanelSettings/
    Controllers/
    Bindings/
    Icons/
    Backgrounds/
    Placeholders/
  Tests/
    EditMode/
    PlayMode/
```

Do not create a second nested Unity project if a project already exists. Do not begin building screen UXML before the project root, project-owned folders, UI Toolkit panel strategy, and naming conventions are established.

The agent must stop after the project-folder foundation is created so the user can review it before UI implementation continues.

## Provided UI Asset Priority

Before creating any icon, illustration, UI texture, button artwork, frame, panel ornament, or UI prefab-like `VisualTreeAsset`, inspect the project for user-provided assets that already match the approved visual direction.

Priority:

```txt
provided UI asset
-> adapted project-owned copy/variant/style
-> newly created original UI asset based on project references
-> neutral placeholder
```

Use UXML and USS for layout and scalable interface surfaces whenever possible instead of rasterizing entire panels or screens into images. Do not bake editable text, live progress, scores, subject availability, or navigation state into images.

When a new UI asset is required:

- use the project reference images for visual direction
- create an original asset rather than copying the reference pixel-for-pixel
- keep source and exported files organized under project-owned UI folders
- optimize texture dimensions and import settings for Android
- provide normal, selected, disabled, loading, and error states through UI Toolkit styling when possible
- document the local key and screen/component that consumes the asset

Final background images remain user-supplied unless the user explicitly authorizes their creation.

## Visual Reference Policy

User-provided reference images may guide:

- layout hierarchy
- panel placement and proportions
- navigation structure
- button and card treatment
- spacing rhythm
- typography scale and emphasis
- icon placement
- selected, disabled, loading, error, and success states
- overall clean fantasy/educational presentation

Reference images are design guidance, not files to copy blindly.

The agent must not:

- copy or extract the reference background image
- generate a replacement background without being asked
- bake labels or gameplay text into images
- reproduce confusing decorative numbers, stars, markers, or world-task graphics as permanent interface decoration
- make the interface dependent on an unavailable reference file

The user will provide the real background image or place it in:

```txt
Assets/_Project/Nutrimind/UI/Backgrounds/
```

Until then, use a neutral solid, gradient, or clearly named placeholder from `UI/Placeholders/`. The screen must remain readable and fully usable without the final background.

Background binding should use a documented asset reference or stable local key so replacing the placeholder does not require changing screen logic.

## Target Experience

The client targets Android phones in 16:9 landscape-first presentation and must remain usable on supported landscape aspect ratios through safe-area-aware responsive layout.

Visual direction:

- clean modern wooden-fantasy educational UI
- UI Toolkit panel surfaces rather than Canvas prefabs
- low/mid-poly environments and characters behind or around the UI
- readable student-facing labels
- clear subject identity without relying only on color
- consistent feedback, connection, progress, and reward presentation
- quiet backgrounds that do not compete with controls or task text

## UI Toolkit Architecture

Use a shared application shell rather than one unrelated `UIDocument` implementation per screen.

Recommended logical layers:

```txt
Application Root
  Background Layer
  Screen Layer
  World HUD Layer
  Modal Layer
  Loading/Blocking Layer
  Toast/Notification Layer
  Accessibility/Diagnostics Layer
```

A project may use multiple `UIDocument` components where sorting, lifetime, or performance justifies it, but they must share a documented `PanelSettings` and visual language unless a specific layer requires a separate panel.

UXML owns hierarchy. USS owns presentation. C# owns:

- state subscription
- data binding
- event registration
- navigation requests
- form submission
- validation display
- lifecycle cleanup

UXML and USS must not contain product data. Controllers must not hardcode subject, term, station, reward, score, or curriculum availability.

## Shared UI Components

Create reusable UI Toolkit components or templates for:

- primary, secondary, back, and destructive buttons
- icon button
- navigation header
- subject card
- term card
- station status card
- profile summary
- settings row
- labeled text field and PIN field
- selection control
- progress indicator
- connection badge
- loading overlay
- confirmation dialog
- error panel
- empty/unavailable panel
- feedback panel
- reward presentation
- interaction prompt
- touch joystick
- primary and secondary touch action buttons

Shared components must expose clear states:

- normal
- focused
- pressed
- selected
- disabled
- loading
- error
- success
- locked
- completed

## Required Screens

UI Toolkit screens must cover:

- Splash and initialization
- Login
- Main Menu
- Profile
- Settings
- Subject Selection
- Term Selection
- Loading and transition failure
- World HUD
- Station introduction
- Station task panel
- Reading and writing panel
- Attempt pending
- Feedback and rewards
- Offline/unavailable
- Session expired
- Unsupported client/maintenance

Screen controllers bind to shared stores and providers. They must not read local JSON or invoke HTTP directly.

## Controls

Required touch controls:

```txt
fixed left circular joystick
right-side primary interact button
secondary context button where needed
confirm/back controls where needed
```

Implement touch controls with UI Toolkit pointer events, manipulators, or a project-approved custom `VisualElement`. Do not use uGUI Canvas controls.

The joystick remains fixed. Students may adjust joystick opacity and camera sensitivity.

Controls must:

- respect safe areas
- avoid covering task text
- expose clear pressed and disabled states
- avoid tiny targets
- support contextual labels/icons
- stop movement during blocking dialogs where appropriate
- release pointer capture safely on cancel, focus loss, pause, screen change, or destroyed UI
- be verified on Android touch input, not only mouse input in the Editor

World-object prompts should normally be rendered in the screen-space UI Toolkit HUD and positioned from world data when needed. Do not create a Canvas for every interactable.

## Main Menu

The main menu includes:

- Play
- Profile
- Settings
- connection status
- student-safe summary
- logout

The layout must remain clear when the final background is absent or replaced.

## Profile

Profile is read-only and may show:

```txt
display name
masked LRN
grade level
section name
classroom name
assigned teacher name when allowed
account status
level/rank label
badges/reward summary
active subject progress
language preference
last sync time
```

Never show PIN, token, private teacher/admin notes, AI settings, answer keys, or raw audit data.

## Settings

Required areas:

- language
- master/music/SFX volume
- mute
- subtitles
- text size
- camera sensitivity
- joystick opacity
- reduced motion
- hints
- connection information

Settings should apply locally where safe, then synchronize to the server. Save failure must be visible.

## Reading and Writing UX

Literacy and content-heavy tasks require:

- readable font size
- text-size setting support
- scrollable long content
- clear contrast
- paragraph spacing
- visible selected answer state
- keyboard-safe layout for text input
- preserved draft answer during retryable failure
- character or length guidance when server-defined
- appropriate focus movement when the on-screen keyboard opens or closes

Use UI Toolkit scrolling and text input controls. Long lists should use an appropriate virtualized control such as `ListView` when supported by the design.

## Safe Area and Responsive Layout

The UI must:

- account for Android display cutouts and system-safe areas
- use the selected `PanelSettings` scale mode consistently
- avoid assuming one exact pixel resolution
- preserve minimum touch target sizes
- keep critical controls visible at supported landscape aspect ratios
- prevent text clipping at supported text-size settings
- avoid absolute positioning unless the element genuinely needs fixed overlay placement

Safe-area padding must be recalculated when the display, orientation state, or relevant screen metrics change.

## Accessibility

At minimum:

- subtitles
- adjustable text size
- reduced motion
- volume controls
- language preference `en` and `tl`
- non-color-only status cues
- clear focus/selection state
- accessible alternative for precise drag interactions when practical
- readable error and retry messages
- keyboard/gamepad focus behavior where supported

## Feedback

Differentiate:

- local interaction response
- request pending
- server-confirmed correct/incorrect result
- pending teacher review
- reward granted
- reward use rejected
- connection issue

Do not show official points or reward animations before server confirmation.

## UI Toolkit Performance Requirements

The client must avoid avoidable UI stalls and visual-tree churn:

- create stable screen hierarchies rather than reconstructing the entire tree every frame
- cache frequently used element references after tree creation
- unregister callbacks and data subscriptions when screens detach or close
- avoid per-frame full-tree queries
- avoid per-frame style-sheet replacement
- prefer class changes or targeted property updates over rebuilding large sections
- use virtualized list controls for large datasets
- keep hidden/inactive screen trees intentional and bounded
- avoid loading full-resolution backgrounds when a smaller mobile-appropriate texture is sufficient
- define dynamic-atlas and texture choices deliberately
- use efficient translation/transform techniques for frequently moving UI Toolkit elements
- profile on representative Android hardware before adding speculative optimization

Network calls, JSON parsing, and expensive content preparation must not block the Unity main thread.

## Missing Asset and Failure Behavior

If a background, icon, UXML, USS, or UI component mapping is missing:

1. show the neutral fallback where possible
2. keep navigation and important actions usable
3. log a non-sensitive development diagnostic
4. do not show raw paths or exceptions to students
5. do not load an unrelated asset as a silent substitute
6. do not crash the active scene

## UI Completion Criteria

UI is not complete merely because a visual mockup exists. Completion requires:

- project and project-owned UI folders established first
- UI Toolkit used for all new runtime game UI
- no unintended uGUI Canvas dependency
- UXML/USS assets load correctly
- shared components and visual states exist
- navigation and back behavior work
- provider/store data binds correctly
- local-demo and HTTP modes use the same screens
- final or placeholder backgrounds can be exchanged without code changes
- safe areas and Android touch controls are verified
- accessibility settings affect the UI
- loading, empty, error, offline, and session-expired states work
- callbacks and subscriptions clean up correctly
- representative device profiling shows acceptable behavior
