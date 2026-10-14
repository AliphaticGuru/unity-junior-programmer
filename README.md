# Unity Junior Programmer — Completed Pathway & Project Portfolio

**Status: Unity Learn Junior Programmer Pathway completed** 🎉

A repository of Unity and C# projects developed throughout the [Unity Learn Junior Programmer Pathway](https://learn.unity.com/pathway/junior-programmer). It contains course prototypes, exercises, starter projects, and a larger **2.5D wave-based survival game** that brings together the skills learned across the pathway.

**🎮 [PLAY LIGHT FURY IN YOUR BROWSER ON UNITY PLAY](https://play.unity.com/en/games/b84ca2e7-8dc9-438a-8c25-f764539218af/light-fury)**

**Focus:** C# programming · Gameplay systems · Object-oriented design · UI · Persistence · Online services · Git & GitHub

## Featured Project — Light Fury: 2.5D Survival Game

**[▶ Play Light Fury on Unity Play](https://play.unity.com/en/games/b84ca2e7-8dc9-438a-8c25-f764539218af/light-fury)** · **[Explore the Personal Project source](./Personal%20Project)**

Light Fury is a wave-based survival platformer in which the player fights enemies, survives progressively harder waves, collects powerups, and competes for high scores. The WebGL build is published on Unity Play and can be launched in a supported browser.

### Features

- **Movement and combat:** Rigidbody-based movement, jumping, ranged attacks, and melee attacks
- **Survival gameplay:** Enemy spawning, wave progression, increasing difficulty, and multiple enemy behaviors
- **Player systems:** Health, lives, respawning, and temporary invulnerability
- **Powerups:** Temporary movement boosts and gameplay feedback
- **Game flow:** Main menu, pause/resume, game over, and restart
- **Scoring:** Current score and locally stored high score
- **Save and continue:** JSON-based local game progress, including player state, score, and wave progress
- **Player profiles:** Offline username registration and returning-player welcome screen
- **Online leaderboard:** Unity Gaming Services Authentication and Leaderboards; internet required for online rankings
- **Audio feedback:** Gameplay and UI sound effects for a more responsive experience

> **Persistence notes:** Continuing a saved game restarts the saved wave rather than restoring every individual enemy and projectile. A local profile does not provide a recoverable cross-device account. Browser storage and online-service behavior may differ from the Unity Editor.

### Technical architecture

| Component | Purpose |
| --- | --- |
| `GameManager` | Game state and scene transitions |
| `PlayerController`, `PlayerCombat`, `PlayerHealth` | Player movement, attacks, damage, and lives |
| `SurvivalManager` | Wave progression and enemy difficulty |
| `ScoreManager` | Score tracking and local high score |
| `SaveGameManager`, `JsonSaveRepository` | Game progress serialization and persistence |
| `PlayerRegistrationUI`, `LocalProfileRepository` | Offline player registration and menu experience |
| `LeaderboardService`, `LeaderboardController` | Online leaderboard integration |
| `AudioManager` | Sound-effect playback |

The project demonstrates component-based development, object-oriented programming, event-driven interactions, and separation of responsibilities.

## All Projects in This Repository

The following table reflects **all nine Unity project directories currently present in the repository**.

| Project | Path | Learning context |
| --- | --- | --- |
| **Mod the Cube** | [Open project](./Mod%20the%20Cube) | Introductory Unity scripting modification exercise |
| **Prototype 1** | [Open project](./Prototype%201) | Junior Programmer course prototype |
| **Prototype 2** | [Open project](./Prototype%202) | Junior Programmer course prototype |
| **Prototype 3** | [Open project](./Prototype%203) | Junior Programmer course prototype |
| **Prototype 4** | [Open project](./Prototype%204) | Junior Programmer course prototype |
| **Prototype 5** | [Open project](./Prototype%205) | Additional Junior Programmer course prototype |
| **Prototype 7 JP Starter-U6** | [Open project](./Prototype%207%20JP%20Starter-U6) | Unit 6 starter project retained in the repository |
| **Data-Persistence-Starter** | [Open project](./Data-Persistence-Starter) | Data persistence starter project and practice |
| **Personal Project — Light Fury** | [Play on Unity Play](https://play.unity.com/en/games/b84ca2e7-8dc9-438a-8c25-f764539218af/light-fury) · [Source folder](./Personal%20Project) | Expanded 2.5D survival game, including persistence, profiles, online leaderboard, and audio |

The course prototypes and starter projects document learning and experimentation; the Personal Project is the main integrated showcase. Starter folders are identified as starter projects rather than presented as independently completed games.

## Skills & Technologies

- **Unity and C#:** MonoBehaviours, GameObjects, components, prefabs, and scene management
- **Gameplay:** Physics, collisions, input, coroutines, enemy behavior, and game-state management
- **User interfaces:** Unity UI and TextMeshPro
- **Software design:** OOP, interfaces, events, modular responsibilities, and debugging
- **Persistence:** JSON files, `Application.persistentDataPath`, and `PlayerPrefs`
- **Online features:** Unity Gaming Services Authentication and Leaderboards
- **Audio:** `AudioSource`, `AudioClip`, and sound-effect triggers
- **Version control:** Git branches, commits, pull requests, and merges

## Play Light Fury — WebGL

**[Launch Light Fury on Unity Play](https://play.unity.com/en/games/b84ca2e7-8dc9-438a-8c25-f764539218af/light-fury)**

The browser-playable WebGL build is published on Unity Play. Open the link above to play.

When testing the browser version, note that browser storage, input behavior, audio playback, and Unity Gaming Services connectivity may differ from running the game in the Unity Editor. Online leaderboard access requires an internet connection.

## Open a Project in Unity

1. Clone the repository:

   ```bash
   git clone https://github.com/AliphaticGuru/unity-junior-programmer.git
   ```

2. Open **Unity Hub** and add the project folder you want to explore, such as `Personal Project` or `Prototype 5`.
3. Use the Unity Editor version specified in that project's `ProjectSettings/ProjectVersion.txt`.
4. Allow Unity to import assets and packages.
5. Open an appropriate scene under `Assets/Scenes` and press **Play**.

For the Personal Project, start from the Main Menu scene. Before creating a build, verify the scene list in Unity's build settings.

**Online services:** A fresh clone may need its own Unity Cloud project linkage and service configuration. The Personal Project uses the leaderboard ID `survival_highscores`. Local gameplay and save/profile functionality are designed for offline use.

## Repository Layout

```text
unity-junior-programmer/
├── Data-Persistence-Starter/
├── Mod the Cube/
├── Prototype 1/
├── Prototype 2/
├── Prototype 3/
├── Prototype 4/
├── Prototype 5/
├── Prototype 7 JP Starter-U6/
├── Personal Project/
│   ├── Assets/
│   ├── Packages/
│   └── ProjectSettings/
├── .gitignore
└── README.md
```

Each Unity project is maintained in its own directory. Generated Unity cache and build files are excluded through `.gitignore`.

## Development Workflow

Git and GitHub were used to track the work, including feature branches and pull requests:

- [Pull Request #1 — Player profiles, save/continue, and leaderboard integration](https://github.com/AliphaticGuru/unity-junior-programmer/pull/1)
- [Pull Request #2 — Gameplay and UI sound effects](https://github.com/AliphaticGuru/unity-junior-programmer/pull/2)

Both were merged into `main`, documenting the workflow **feature branch → implementation → testing → pull request → merge**.

## Project Status

- **Junior Programmer Pathway:** Completed
- **Personal Project — Light Fury:** Gameplay, menus, persistence, profiles, leaderboard, and audio implemented
- **WebGL publication:** Published on Unity Play — [Play Light Fury](https://play.unity.com/en/games/b84ca2e7-8dc9-438a-8c25-f764539218af/light-fury)
- **Portfolio:** README maintained as a record of projects and technical progress

## Acknowledgment

The pathway prototypes and exercises originate from [Unity Learn](https://learn.unity.com/). This repository documents my learning progress and the application of those skills in an extended personal game project.
