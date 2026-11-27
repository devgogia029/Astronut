# Astronut

A 2D gravity-flip platformer made in Unity 6. One button flips gravity, at any time, even mid-air. Use it to dodge spikes, cross pits and collect every diamond before the portal opens.

**[Play it in your browser on itch.io](https://devgogia.itch.io/astronut)**

![Main menu](media/main-menu.gif)

![Level 1](media/level1-run.gif)

![Level 2](media/level2-run.gif)

---

## About

Built for Github's Game Off 2026 (theme : SHIFT).

Astronut is a solo project: all the code, levels and game design were made by me. The astronaut can flip gravity whenever he likes. Each flip keeps a little of his vertical speed, so timing a flip in mid-air is the core skill. Every level has diamonds to collect. The portal stays locked until you have all of them.

Dying sends you back to the last checkpoint, but the clock keeps running, so every mistake counts if you are going for a fast time.

## How to play

| Key | Action |
|---|---|
| **A / D** or **Left / Right** | Move |
| **Space** | Levels 1-2: flip gravity. Levels 3-5: turn the level 90° right |
| **Shift** | Levels 3-5: turn the level 90° left |
| **R** | Restart the level |
| **Esc** | Pause |

**Goal:** collect every diamond, then enter the portal.

In the browser, click the game once first so it receives keyboard input and the music can start.

## Features

- Two gravity mechanics: a classic mid-air flip in Levels 1-2, then 90° level turns in Levels 3-5, where the camera rotates and zooms with the astronaut
- 5 hand-built levels, each split into two screens with a smooth camera glide and an automatic checkpoint
- Hazards: static spikes, patrolling spike strips, moving platforms, and pits (floor and ceiling holes in Level 5)
- Diamond counter that fills in automatically, with a portal that stays locked until all diamonds are collected
- Best times saved per level and shown in the HUD and on the results screen
- Pause menu, level restart, main menu, and rolling end credits
- Pixel-art visuals, music, and sound effects
- Runs in the browser as a WebGL build
- In-game hint bubbles that follow the astronaut

## Built with

- Unity 6 (6000.4), 2D URP
- C#
- TextMeshPro

## Run it locally

1. Clone the repo: `git clone https://github.com/devgogia029/Astronut.git`
2. Open it in Unity Hub with a Unity 6000.4 editor.
3. Open `Assets/Scenes/MainMenu` and press Play.

**To make a WebGL build:** File → Build Profiles → Web, with the scenes in order (MainMenu first), Compression Format set to Disabled, and the canvas at 1280 × 720.

## Project structure

```
Assets/
  Scenes/     MainMenu, Level1-Level5, LevelComplete, Credits, LevelTemplate
  Scripts/    game code (see below)
  Prefabs/    Player, LevelSystems, Portal, Diamond, Block, spike strips, moving platforms, and more
  Audio/      music and sound effects
```

**Main scripts**

| Script | What it does |
|---|---|
| `Player` | Movement, ground check, gravity flip, moving-platform riding |
| `PlayerRespawn` | Hazard detection, death sequence, respawn at the last checkpoint |
| `CameraController` / `ExitTrigger` | Moves the camera between the two parts of a level and sets checkpoints |
| `LevelManager` / `GameData` | Diamond count, timer, and carrying results between scenes |
| `LevelPortal` / `Diamond` | Pickups and the locked portal |
| `MovingPlatform` | Moves platforms and patrolling spike strips along a direction and range |
| `TiledBlock` | Keeps a tiled block's collider matched to its size |
| `AudioManager` | Persistent music and sound effects |
| `ResultsScreen` / `ResultsButtons` | Level complete screen, Next Level or Continue, and menu buttons |
| `CreditsScroll` | The rolling end credits |

## Credits and licenses

- **Characters, diamond and tiles:** Free Space Runner Pack by MattWalkden (CC0)
- **Portal animation:** FREE 2D Spaceships Pack by https://luissangueve.github.io/Panks-Creations/
- **Space background:** https://edwardtheskull.itch.io/
- **Font:** Public Pixel Font by GGBotNet
- **Music and sound effects:** https://itch.io/profile/ci

Everything else was made by me. Thank you to the creators of the assets above.

## Author

Made by **Dev**, [@devgogia029](https://github.com/devgogia029).

## License

This project's code is released under the MIT License. See the [LICENSE](LICENSE) file. Third-party assets keep their own licenses, listed in the Credits section above.
