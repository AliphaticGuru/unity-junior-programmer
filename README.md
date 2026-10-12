# Unity Junior Programmer — Projects & Portfolio

A collection of Unity projects completed during the **Unity Learn Junior Programmer Pathway**, featuring course prototypes, programming challenges, and an expanded **2.5D survival game** developed as a personal project.

This repository documents my progression in **Unity, C#, gameplay systems, software architecture, debugging, and Git-based development**.

## Featured Project: 2.5D Survival Game

**Location:** [Personal Project](./Personal%20Project)

A wave-based survival platformer built in Unity. The player fights increasingly challenging enemies, manages health and limited lives, collects powerups, and competes for high scores.

### Gameplay features

- **Player movement and combat:** Rigidbody-based movement, jumping, melee attacks, and ranged attacks.
- **Health and lives:** Three health points per life, three lives, respawning, and temporary invulnerability.
- **Enemy behaviors:** Rusher and ranged enemy types with shared damage-handling abstractions.
- **Wave survival:** Progressive enemy waves, scaling difficulty, and enemy tracking.
- **Powerups and feedback:** Temporary movement boosts, protection while powered up, and visual effects.
- **Game flow:** Playing, paused, and game-over states with menu and HUD controls.
- **Scoring:** Enemy-defeat points and a locally persisted high score.
- **Save and continue:** Local JSON save data for resuming player progress, score, and wave difficulty.
- **Local player profiles:** Username registration and a returning-player welcome flow, including offline play.
- **Online leaderboard:** Unity Gaming Services Authentication and Leaderboards for submitting and viewing global scores when connected.

> **Save/load scope:** A saved wave is restarted when continuing; individual live enemies and projectiles are not serialized. Local player profiles are not equivalent to recoverable cross-device online accounts. Internet access is required for online leaderboard operations, not for core gameplay.

### Engineering approach

The project practices object-oriented programming, event-driven communication, component-based design, and separation of responsibilities. Examples include:

| System | Responsibility |
| --- | --- |
| `GameManager` | Game states, pause/resume, restart, and scene navigation |
| `PlayerHealth` | Damage, lives, respawn, and death events |
| `SurvivalManager` | Enemy waves, spawn progression, and difficulty |
| `ScoreManager` | Current score and local high score |
| `SaveGameManager` / `JsonSaveRepository` | Game-state capture and local JSON persistence |
| `PlayerRegistrationUI` / `LocalProfileRepository` | Local profile creation and main-menu flow |
| `LeaderboardService` / `LeaderboardController` | Online score operations and leaderboard presentation |

## Unity Learn Projects

| Project | Description |
| --- | --- |
| [Mod the Cube](./Mod%20the%20Cube) | Unity Learn scripting modification exercise |
| [Prototype 1](./Prototype%201) | Course prototype and challenge work |
| [Prototype 2](./Prototype%202) | Course prototype and challenge work |
| [Prototype 3](./Prototype%203) | Course prototype and challenge work |
| [Prototype 4](./Prototype%204) | Course prototype and challenge work |
| [Personal Project](./Personal%20Project) | Expanded wave-based survival game with persistence and online leaderboard features |

The prototypes and challenges are **Unity Learn coursework**. The Personal Project demonstrates how those concepts were combined and extended into a larger playable game.

## Technologies & Skills

- **Engine:** Unity
- **Programming:** C#
- **Gameplay:** Input System, Rigidbody physics, collisions, triggers, prefabs, coroutines, and particle effects
- **UI:** Unity UI and TextMeshPro
- **Persistence:** JSON files and Unity `PlayerPrefs`
- **Online services:** Unity Gaming Services Authentication and Leaderboards
- **Engineering:** OOP, interfaces, events, debugging, and separation of concerns
- **Version control:** Git, GitHub, feature branches, and pull requests

## Getting Started

1. Clone the repository:

   ```bash
   git clone https://github.com/AliphaticGuru/unity-junior-programmer.git
   ```

2. Open **Unity Hub** and add the desired project folder (for example, `Personal Project`).
3. Use the Unity Editor version specified by that project's `ProjectSettings/ProjectVersion.txt`. Allow Unity to import assets and resolve packages.
4. Open the project's main menu scene in `Assets/Scenes` and press **Play**.

**Online features:** Unity Gaming Services requires a properly linked Unity Cloud project, configured Authentication and Leaderboards services, and network access. The project uses the leaderboard ID `survival_highscores`. A fresh clone may require your own service configuration and permissions. Local gameplay and profile/save features are designed to work offline.

**Build setup:** Confirm that the main menu and gameplay scenes are included in the project's build scene list before building a standalone player.

## Repository Structure

```text
unity-junior-programmer/
├── Mod the Cube/
├── Prototype 1/
├── Prototype 2/
├── Prototype 3/
├── Prototype 4/
├── Personal Project/
│   ├── Assets/
│   │   ├── Scenes/
│   │   └── Scripts/
│   │       ├── Game Manager/
│   │       ├── Online/
│   │       ├── Persistence/
│   │       └── Player/
│   ├── Packages/
│   └── ProjectSettings/
└── README.md
```

Unity-generated caches and build artifacts are excluded through `.gitignore`.

## Git Workflow

Development is tracked through Git commits and feature branches. The player profile, persistence, and leaderboard integration work was merged into `main` through [Pull Request #1](https://github.com/AliphaticGuru/unity-junior-programmer/pull/1).

This documents a practical workflow: **feature branch → implementation → testing → pull request → merge**.

## Project Status & Next Steps

- **Unity Junior Programmer Pathway:** Completed.
- **Personal Project:** Core gameplay, menu, scoring, local persistence, player profiles, and leaderboard integration implemented.
- **Next portfolio improvements:** Gameplay screenshots or GIFs, a packaged playable build, release notes, and a tagged release.

## Acknowledgment

The learning prototypes are based on exercises from the [Unity Learn Junior Programmer Pathway](https://learn.unity.com/pathway/junior-programmer). This repository serves as a record of learning and an evolving programming portfolio.
