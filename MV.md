# Final Project Code Map

This file explains the authored code in this Unity project as one connected system. It focuses on the game flow, the runtime dependencies, the editor/build pipeline, and the role of each script under `Assets/Scripts` and `Assets/Editor`.

## High-Level Structure

The project is organized around four main layers:

- `Assets/Editor`
  - Editor-only tooling that assembles scenes and prefabs from code.
- `Assets/Scripts/Building`
  - Runtime and editor helpers that build the environment, enemies, props, and scene content.
- `Assets/Scripts/Core`
  - Game lifecycle, scene bootstrap, audio, particles, and pause state.
- `Assets/Scripts/Controls`, `Assets/Scripts/AI`, `Assets/Scripts/UI`
  - Interactive gameplay systems, enemy behavior, and screen/UI effects.

The project is heavily code-driven. A large part of the map, menu, UI, and enemy setup is created procedurally rather than being hand-authored entirely in the Unity editor.

## Main Flow

The normal gameplay path is:

1. The player opens the main menu scene.
2. `SimpleMainMenu` or related menu code presents the title screen and start controls.
3. The game scene loads.
4. `GameManager.Start()` calls `SceneSetup.EnsureSceneIsBuilt(this)`.
5. `SceneSetup` checks whether essential scene systems already exist and creates any missing ones.
6. `WorldBuilder` builds the office, hallways, props, enemies, and supporting objects when geometry is missing.
7. `SecurityCamera`, `HallwayLightSystem`, `AudioManager`, `PauseMenu`, `ParticleSystems`, and `DeskAnimations` are ensured or spawned.
8. `GameManager` initializes night state, difficulty, the clock display, and win/lose logic.
9. `EnemyAI` runs the actual threat loop and calls back into `GameManager` on failure conditions.
10. `SceneSetup` and `JumpscareEffect` drive the lose and win screens.

## Core Runtime Integrations

### `GameManager`

`Assets/Scripts/Core/GameManager.cs` is the central runtime coordinator.

It is responsible for:

- Loading and validating the saved night number from `PlayerPrefs`.
- Bootstrapping the scene through `SceneSetup`.
- Tracking the night timer from start to finish.
- Updating the office monitor clock text.
- Applying night difficulty values to all `EnemyAI` instances.
- Handling restart and next-night input after win/loss states.
- Triggering game over and win screens.

Key integrations:

- Reads and writes `PlayerPrefs` using `CurrentNight`.
- Calls `SceneSetup.EnsureSceneIsBuilt`.
- Locates or creates the monitor clock through `SceneSetup.FindOrCreateClockText`.
- Adjusts all live `EnemyAI` instances in `ApplyDifficulty`.
- Creates `JumpscareEffect` on death and passes `SceneSetup.ShowGameOverScreen` as the callback.
- Calls `SceneSetup.ShowWinScreen` and `SceneSetup.ShowFinalWinScreen`.

### `SceneSetup`

`Assets/Scripts/Core/SceneSetup.cs` is the runtime bootstrap utility.

It is responsible for:

- Ensuring the main camera exists.
- Ensuring world geometry exists.
- Ensuring control systems exist.
- Ensuring support systems exist.
- Ensuring a second enemy exists when only one is present.
- Creating runtime UI objects for win and loss screens.

Key integrations:

- Spawns `WorldBuilder` if the office geometry is missing.
- Spawns `SecurityCamera`, `HallwayLightSystem`, `AudioManager`, `DeskAnimations`, `ParticleSystems`, and `PauseMenu` when absent.
- Works with `GameManager` by receiving the manager instance and assigning it to new enemy instances.
- Adds `AnimatronicAnimator` to enemies that do not already have one.
- Removes physics collider behavior where AI navigation should not use standard collision.

### `AudioManager`

`Assets/Scripts/Core/AudioManager.cs` is the shared sound hub.

It appears to expose a global `Instance` and is consumed by other systems for event audio such as camera clicks. The most visible runtime dependency is `SecurityCamera`, which checks `AudioManager.Instance` before playing feedback sounds.

### `PauseMenu`

`Assets/Scripts/Core/PauseMenu.cs` controls the paused state and likely affects time scale and player input availability. It is runtime-ensured by `SceneSetup`, which means the project expects pausing to be available even when the scene was not fully pre-authored in the editor.

### `ParticleSystems`

`Assets/Scripts/Core/ParticleSystems.cs` is another support subsystem that is auto-created by `SceneSetup`. It likely owns ambient visual effects such as dust, fog, or atmosphere, and exists independently from the scene file.

## Building and Scene Authoring

### `SceneBuilder`

`Assets/Editor/SceneBuilder.cs` is the editor-side construction entry point.

It adds custom Unity menu items:

- `Night Shift/View Map`
- `Night Shift/View Menu`
- `Night Shift/Build`

Its main job is to rebuild both the menu scene and the gameplay scene entirely from code.

The `Build()` method:

1. Opens the main menu scene.
2. Calls `BuildMainMenuScene()`.
3. Saves that scene.
4. Opens the game scene.
5. Calls `BuildGameScene()`.
6. Saves that scene.
7. Reopens the menu scene for testing.

`BuildGameScene()` orchestrates the gameplay content build:

- `ClearScene()`
- `CreateMaterials()`
- `BuildOffice()`
- `BuildHallways()`
- `BuildLighting()`
- `BuildNavMesh()`
- `BuildDoors()`
- `BuildSpots()`
- `BuildEnemy()`
- `BuildClown()`
- `BuildMonitorClock()`
- `BuildGameManager()`
- `TestNavMesh()`

This means the playable map is not just loaded from static assets. It is assembled through code with generated geometry, materials, lighting, navigation, doors, enemies, and UI anchor objects.

### `WorldBuilder`

`Assets/Scripts/Building/WorldBuilder.cs` is the large runtime/editor construction helper used by both `SceneBuilder` and `SceneSetup`.

It is the main environment factory and likely owns logic for:

- Office geometry.
- Hallway/classroom/bathroom layout.
- Props and decorative objects.
- Door structures and placements.
- Enemy model or prefab construction.
- Secondary enemy creation such as the clown.

Key integrations:

- Called by `SceneSetup.EnsureSceneGeometry()` via `BuildAll()`.
- Called by `SceneSetup.EnsureSecondAnimatronic()` to build a clown enemy when only one enemy exists.
- Used by `SceneBuilder` during full scene reconstruction.

### `MainMenuSceneBuilder`

`Assets/Scripts/Building/MainMenuSceneBuilder.cs` is the menu-scene construction helper. It complements `SimpleMainMenu` by handling the environment or layout of the main menu side of the project.

## Controls and Interaction Systems

### `SecurityCamera`

`Assets/Scripts/Controls/SecurityCamera.cs` is one of the main gameplay mechanics.

It is responsible for:

- Opening and closing the security camera monitor.
- Creating six security camera viewpoints in the world.
- Building an on-screen camera monitor UI overlay.
- Switching between cameras with keys and UI buttons.
- Managing the battery resource.
- Draining battery from camera use and from closed doors.
- Recharging battery when the player holds `F`.
- Forcing doors open when battery reaches zero.
- Updating on-screen battery and state text.

Key integrations:

- Reads all `DoorController` instances every frame for battery drain logic.
- Calls `DoorController.OpenDoor()` when battery depletes.
- Uses `AudioManager.Instance.PlayCameraClick()` for feedback.
- Creates its own render texture and routes camera output into a `RawImage`.

This system is deeply connected to the threat loop because battery pressure interacts with both surveillance and defense.

### `DoorController`

`Assets/Scripts/Controls/DoorController.cs` governs the office doors.

It is a shared dependency for:

- `SecurityCamera`, which uses door state to increase power drain and to force doors open on power loss.
- `EnemyAI`, which likely checks left and right doors before entering or attacking.

The class exposes at least:

- An `isClosed` state.
- An `OpenDoor()` method.

### `HallwayLightSystem`

`Assets/Scripts/Controls/HallwayLight.cs` manages hallway lighting behavior. It is runtime-created by `SceneSetup`, so the game expects it to exist as an active system rather than as a passive scene object.

It likely participates in:

- Player visibility tools.
- Horror timing and threat signaling.
- Office-side left/right interaction cues.

## AI and Threat Systems

### `EnemyAI`

`Assets/Scripts/AI/EnemyAI.cs` is the main enemy state machine.

It contains an internal `State` enum:

- `Hiding`
- `Waiting`
- `Attacking`
- `BreakingIn`

This implies a staged behavior loop rather than constant direct pursuit.

Based on its integrations and how other code references it, `EnemyAI` is responsible for:

- Moving between AI spots or zones.
- Scaling threat through `moveSpeed` and `aggressionGrowth`.
- Checking door state and office access.
- Coordinating with `GameManager` for end-game outcomes.
- Possibly using navmesh movement to travel between map regions.

Key integrations:

- `GameManager.ApplyDifficulty()` modifies `moveSpeed` and `aggressionGrowth`.
- `SceneBuilder` and `WorldBuilder` create enemy instances.
- `SceneSetup` injects the `GameManager` reference into newly created enemies.
- `SceneSetup` ensures enemies have `AnimatronicAnimator`.
- `DoorController` references are assigned to the enemy build path.

### `AnimatronicAnimator`

`Assets/Scripts/AI/AnimatronicAnimator.cs` handles visual motion and presentation for enemies. It exists as a companion to `EnemyAI`, separating the animation/display layer from the state and navigation layer.

### `DeskAnimations`

`Assets/Scripts/AI/DeskAnimations.cs` is a support animation system for office props or environmental motion. It is runtime-ensured by `SceneSetup`, so it likely animates office items such as fans or desk objects for atmosphere.

## UI and Screen Logic

### `SimpleMainMenu`

`Assets/Scripts/UI/SimpleMainMenu.cs` appears to be the main playable menu implementation. `SceneBuilder.BuildMainMenuScene()` creates a single `MainMenu` object and adds this component, which suggests the menu UI is generated from code in `Start()` or a similar lifecycle method.

### `MainMenuController`

`Assets/Scripts/UI/MainMenuController.cs` is another menu-related controller. Depending on how the scene is currently composed, it may represent a more traditional scene-authored menu path while `SimpleMainMenu` is the code-driven path.

### `JumpscareEffect`

`Assets/Scripts/UI/JumpscareEffect.cs` owns the death transition effect.

Known integration:

- `GameManager.TriggerGameOver()` creates a `JumpscareEffect` object at runtime.
- `JumpscareEffect.Play(...)` is passed a callback to `SceneSetup.ShowGameOverScreen`.

This means the lose flow is:

1. Enemy catches player.
2. `GameManager.TriggerGameOver()` runs.
3. A jumpscare effect object is spawned.
4. The jumpscare plays.
5. The callback shows the final game-over UI.

### `GameOverController`

`Assets/Scripts/UI/GameOverController.cs` is a dedicated lose-state UI controller. It may be used for behavior attached to the game-over screen, restart handling, or additional screen animation.

## Persistent Data

The project uses `PlayerPrefs` for a small amount of persistent state:

- `CurrentNight`

This value is:

- Read by `GameManager.Start()`.
- Reset to `1` on death.
- Incremented on next-night load.
- Reset to `1` after the final win.

There is no sign in the inspected core flow of larger save data, inventory, or serialized progression beyond the current-night number.

## Scene Generation Versus Scene Dependency

The codebase is designed to tolerate incomplete scenes.

That is a major architectural choice.

Rather than assuming every object already exists in the `.unity` scene files, the game uses:

- `SceneBuilder` to explicitly rebuild scenes in the editor.
- `SceneSetup` to repair or complete scenes at runtime.

This makes the project resilient in two ways:

- The editor can regenerate the intended test/play environment.
- The runtime can still produce a functional experience if a required manager or object is missing.

## Integration Summary

The most important code connections are:

- `GameManager -> SceneSetup`
  - Runtime bootstrap.
- `SceneSetup -> WorldBuilder`
  - Scene geometry and enemy creation.
- `SceneSetup -> SecurityCamera / HallwayLightSystem / AudioManager / PauseMenu / ParticleSystems / DeskAnimations`
  - Guaranteed subsystem creation.
- `GameManager -> EnemyAI`
  - Difficulty scaling and game-state gating.
- `EnemyAI -> GameManager`
  - Loss condition callback.
- `EnemyAI <-> DoorController`
  - Office attack logic and blocking.
- `SecurityCamera -> DoorController`
  - Power drain and forced open behavior.
- `SecurityCamera -> AudioManager`
  - Camera click audio.
- `GameManager -> JumpscareEffect -> SceneSetup.ShowGameOverScreen`
  - Death presentation pipeline.
- `GameManager -> SceneSetup.ShowWinScreen / ShowFinalWinScreen`
  - Victory presentation pipeline.
- `SceneBuilder -> MainMenuSceneBuilder / WorldBuilder / GameManager / enemy setup`
  - Full editor reconstruction path.

## File-by-File Responsibility Map

### `Assets/Editor`

- `SceneBuilder.cs`
  - Editor menu commands for opening and rebuilding scenes.

### `Assets/Scripts/Building`

- `MainMenuSceneBuilder.cs`
  - Main menu scene construction support.
- `WorldBuilder.cs`
  - Procedural construction of the playable world and enemy-related scene content.

### `Assets/Scripts/Core`

- `AudioManager.cs`
  - Shared audio playback hub.
- `GameManager.cs`
  - Main gameplay loop, timer, night progression, win/loss logic.
- `ParticleSystems.cs`
  - Ambient particle or visual support system.
- `PauseMenu.cs`
  - Pause-state UI and time/input control.
- `SceneSetup.cs`
  - Runtime bootstrap and fallback scene assembly.

### `Assets/Scripts/Controls`

- `DoorController.cs`
  - Door state, opening/closing, and defensive interaction.
- `HallwayLight.cs`
  - Hallway light system and light-based player interaction.
- `SecurityCamera.cs`
  - Camera monitor UI, camera switching, battery mechanics, and surveillance.

### `Assets/Scripts/AI`

- `AnimatronicAnimator.cs`
  - Visual/animation behavior for enemies.
- `DeskAnimations.cs`
  - Office prop or desk animation support.
- `EnemyAI.cs`
  - Main enemy logic and attack-state machine.

### `Assets/Scripts/UI`

- `GameOverController.cs`
  - Game-over screen behavior.
- `JumpscareEffect.cs`
  - Jumpscare effect and transition into loss UI.
- `MainMenuController.cs`
  - Menu interaction/controller logic.
- `SimpleMainMenu.cs`
  - Code-generated main menu entry point.

## Practical Reading Order

If you want to understand the project quickly, read the code in this order:

1. `Assets/Scripts/Core/GameManager.cs`
2. `Assets/Scripts/Core/SceneSetup.cs`
3. `Assets/Scripts/Controls/SecurityCamera.cs`
4. `Assets/Scripts/AI/EnemyAI.cs`
5. `Assets/Scripts/Building/WorldBuilder.cs`
6. `Assets/Editor/SceneBuilder.cs`
7. `Assets/Scripts/Controls/DoorController.cs`
8. `Assets/Scripts/UI/JumpscareEffect.cs`
9. `Assets/Scripts/UI/SimpleMainMenu.cs`

That order gives the clearest picture of the runtime loop, scene bootstrap path, player mechanics, enemy behavior, and code-driven content build.
