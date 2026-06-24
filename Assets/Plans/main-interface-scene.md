# Project Overview
- **Game Title**: NutriMind Educational Game
- **High-Level Concept**: A child-safe, interactive educational game that teaches nutrition, mindfulness, and cognitive growth, currently running on a quiz-first milestone where the main interface launches the Quiz Portal / Assessment Room.
- **Players**: Single player.
- **Inspiration / Reference Games**: Mobile Legends / Honor of Kings layout style (as provided in the user's attached reference image): a premium central gameplay prompt, clean corner-anchored profile info, top resource indicators, left vertically stacked secondary activity buttons, and a clean bottom navigation bar.
- **Tone / Art Direction**: Vibrant, clean, child-friendly, and highly readable. Uses a soft visual language with distinct, un-cluttered buttons and panels.
- **Target Platform**: StandaloneWindows64 (PC) and Android.
- **Screen Orientation / Resolution**: Landscape 1920x1080.
- **Render Pipeline**: Universal Render Pipeline (URP).

# Game Mechanics
## Core Gameplay Loop
The player logs in, arrives at the Main Interface displaying their student identity, selects the primary assessment activity (Play/Classic), navigates to the Subject selection hub, starts quizzes, completes questions, and submits attempts to receive feedback.

## Controls and Input Methods
Standard mouse click or touch tap gestures. UI layouts must feature comfortable touch targets (minimum 44x44 pixels) and clear highlighted states to accommodate Android landscape and PC gameplay seamlessly.

# UI
The Main Interface is built on a high-fidelity Canvas/uGUI setup. To match the attached reference image's elegant aesthetics, the layout will be restructured:
1. **Top-Left Student Profile Panel**: Replaces the reference image's character avatar. Shows a clean profile container binding the student's authenticated Name, Grade level, and Section (e.g., "Grade 6 - Mabini") from the `AuthSessionState` and `StudentProfileStore`. **This entire panel functions as a clickable touch target. Clicking the top-left Profile Panel will navigate directly to the Profile scene, keeping the interface clean and intuitive.**
2. **Top Resources Bar**: Displays read-only indicators for Energy, Coins, and Diamonds mirroring the top resource bar in the reference image. Since the economy is deferred, these are marked as read-only/decorative panels showing static achievements or mock values, strictly preventing any economic functionality.
3. **Center-Bottom CLASSIC Button**: The core call-to-action button, replacing the existing static `play` Image. Styled like the gold-framed "CLASSIC" play button in the reference image, it acts as the gateway to the Quiz Portal (`Worldhub` scene).
4. **Left-Side Vertical Button Container**: Replaces the vertical left-side buttons from the reference image ("Shop", "Activity"). In our game, these are secondary links:
   - **Settings Button**: Consolidates settings access, redirects to the `Settings` scene.
   - **Adventure/World Hub Button**: In this milestone, any Adventure/Open-world button (`world hub` element) is disabled, hidden, or clearly marked with a padlock overlay as locked/unavailable to preserve the quiz-first scope.
5. **Mascot Helper**: A decorative character graphic in the bottom right, mirroring the cute rabbit helper in the reference image, providing friendly encouragement.

# UI & Scene Performance Optimization
To ensure maximum performance, smooth transitions, and high frame rates especially on mobile/Android landscape targets, the following best practices will be strictly applied:
1. **Canvas Subdivision (Batching)**:
   - Separate the dynamic parts of the UI (such as active resource indicators, ticking animations, or profile updates) into nested or sibling Canvases. This isolates Canvas rebuilding to only the dirty elements rather than rebuilding the entire screen.
2. **GraphicRaycaster Optimization (Input Sweep)**:
   - Uncheck `Raycast Target` on all static images, backgrounds, icons, and TMPro text components that do not require click/touch interaction. This drastically reduces the search space for the `GraphicRaycaster` during touch sweeps.
3. **Overdraw Minimization**:
   - Avoid nesting multiple transparent/semi-transparent layers on top of each other. Background elements will be set to solid/opaque where applicable, and inactive overlays will be completely disabled (`SetActive(false)`) rather than set to alpha=0.
4. **Smooth & Safe Scene Transitions**:
   - Introduce a lightweight `CanvasGroup` fader inside `MainMenuController` to manage transitions smoothly.
   - During transitions, disable the `GraphicRaycaster` on the Canvas to prevent double-click or multi-tap inputs from queuing duplicate scene-load operations.
   - Gracefully stop the `VideoPlayer` component on the background before loading new scenes to immediately release video decoding memory and GPU compute resources.
5. **Allocation-Free Scripting**:
   - Avoid using `GetComponent` or `Find` operations in `Update()` loops. Cache all components and UI element references inside `Awake()` or serialization bindings.
   - Use non-allocating coroutines or state updates for periodic checks.

# Key Asset & Context
### New Scripts
1. **`Assets/_Project/Nutrimind/Runtime/App/MainMenuController.cs`**:
   - Thin MonoBehaviour coordinating the Main Menu Canvas elements.
   - Binds UI TextMeshPro labels with student profile data upon startup.
   - Connects button onClick events to scene transition commands.
   - **Clicking the Top-Left Profile Panel (`panel`) triggers navigation to the Profile scene.**
   - Safe-guards against in-flight transition errors and manages CancellationTokenSource for async scene preloading.

### Modified Assets
1. **`Assets/_Project/Nutrimind/Scenes/App/MainMenu.unity`**:
   - Canvas hierarchy restructuring:
     - Convert static Image objects `play` and `Settings` into interactable `Button` components.
     - **Add a `Button` component directly to the `panel` (Top-Left Student Profile Panel) to make it clickable.**
     - Remove/resolve redundant Settings images and streamline layout.
     - Reposition player info panel to Top-Left and resources to Top-Center/Right to match the reference layout elegantly.
     - Hide or overlay padlocks on deferred adventure buttons (`world hub`).
     - Attach `MainMenuController` and bind serialized fields.

# Implementation Steps

### Step 1: Create the MainMenuController script
- **Description**: Implement `MainMenuController.cs` under the NutriMind Runtime assembly. Design the script to bind current user session details (Student Name, Grade, Section) to TMP text components, add click handlers to buttons (`play` -> navigates to "Worldhub" key, `settings` -> navigates to "Settings" key, and clicking the Profile Panel `panel` -> navigates to "Profile" key), and handle safe async scene loading.
- **Assigned role**: developer
- **Dependencies**: None
- **Parallelizable**: Yes

### Step 2: REST hierarchy and Convert static Images to Buttons in Scene
- **Description**: Open `MainMenu.unity` scene. Restructure the Canvas to align with the elegant reference layout. Add `Button` components to:
  - `play` (Center gold CLASSIC button)
  - `panel` (Top-Left Student Profile Panel container, enabling it to act as the Profile Scene navigation button)
  - `Settings` (Settings navigation)
  Consolidate redundant settings images, disable or pad-lock the `world hub` Adventure button, and map the top resources (`energy`, `coins`, `diamond`) to decorative display-only states.
- **Assigned role**: developer
- **Dependencies**: Step 1
- **Parallelizable**: No

### Step 3: Bind and Wire Controller in the Scene
- **Description**: Add the `MainMenuController` script component to the root `MainMenu` GameObject in the scene. Bind all serialized fields (Profile Text labels, Play Button, Profile Panel Button, Settings Button, etc.) in the Inspector. Save the scene.
- **Assigned role**: developer
- **Dependencies**: Step 2
- **Parallelizable**: No

### Step 4: Verification and Polish
- **Description**: Run play mode in Editor. Verify correct binding of authenticated student info (from login), successful navigation on button clicks, safe-area boundary scaling on different landscape resolutions, and check that the Console is clean of any NullReferenceExceptions or warnings.
- **Assigned role**: developer
- **Dependencies**: Step 3
- **Parallelizable**: No

# Verification & Testing
1. **Layout & Alignment Check**: Manual inspection in Editor at resolutions 1920x1080, 16:9, and mobile aspects to confirm safe-area scaling, readable text, elegant positioning, and no overlapping.
2. **Data Binding Check**: Simulate login using a mock student (e.g. Grade 6, Mabini) and verify that the Top-Left profile panel correctly displays the student name, grade, and section.
3. **Button Navigation Check**:
   - Click "CLASSIC/PLAY" -> loads the Worldhub scene.
   - Click "Top-Left Profile Panel" -> loads the Profile scene.
   - Click "Settings" -> loads the Settings scene.
4. **Console Integrity**: Verify that zero exceptions, missing assembly references, or broken asset paths occur during play.
