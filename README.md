# Unity Junior Programmer — Learning Archive

> Completed Unity Junior Programmer Pathway | C# • Unity • Gameplay Systems • Version Control

This repository documents my progression through the **Unity Junior Programmer Pathway**, including course prototypes, challenges, and personal development work. It is a **learning archive**, not a collection of independently produced commercial games.

My longer-term direction is **Unity VR/XR development for architectural visualization and interactive simulation**.

## Project index

| Project | Focus | Context |
| --- | --- | --- |
| Mod the Cube | GameObject manipulation and Unity scripting | Unity Learn exercise |
| Prototype 1 | Foundational Unity gameplay | Unity Learn prototype/challenge |
| Prototype 2 | Foundational Unity gameplay | Unity Learn prototype/challenge |
| Prototype 3 | Foundational Unity gameplay | Unity Learn prototype/challenge |
| Prototype 4 | Player input, enemies, power-ups, waves | Unity Learn prototype/challenge |
| Personal Project | Independent practice and applied gameplay systems | Personal project developed during the pathway |

> These descriptions summarize the existing project index and commit history. Project-specific scene locations, playable builds, and demonstrations will be added after verification.

## Technical work documented in Git history

The commit history provides examples of work on:

- **Input and movement:** Unity Input System integration and Rigidbody-based player movement.
- **Gameplay systems:** enemy tracking, spawning, power-ups, and increasing wave difficulty.
- **Combat architecture:** melee/ranged interactions, enemy inheritance, and an `IDamageable` interface.
- **Game state and UI:** menu navigation, pause controls, game-over UI, health/lives HUD, and wave announcements.
- **Persistence:** scoring and persistent high scores.
- **Animation and feedback:** player/enemy animations and movement visual effects.

These are descriptions of committed work, not claims of independent production-ready implementation or verified performance.

### Selected development milestones

- [Unity Input System movement](https://github.com/AliphaticGuru/unity-junior-programmer/commit/e829967142804f348f64093fcd0488ab2c08a7b3)
- [Enemy tracking](https://github.com/AliphaticGuru/unity-junior-programmer/commit/4ec64de82b12998fe9e06f9744cb0883c5bd09b6)
- [Interface-based damage system](https://github.com/AliphaticGuru/unity-junior-programmer/commit/7529bd43d222e1ac62def539bb8c1345a7106a1a)
- [Event-driven waves and difficulty scaling](https://github.com/AliphaticGuru/unity-junior-programmer/commit/bc122bf8606db1e3877ed57f5564b5b0d6666fec)
- [Persistent high score](https://github.com/AliphaticGuru/unity-junior-programmer/commit/93ec792f16bc1194d7b6449a027253e6a30109dc)
- [Rigidbody movement and speed-trail VFX](https://github.com/AliphaticGuru/unity-junior-programmer/commit/8076272d9080176896c0083ba2455214b1e1cea7)

## Skills practiced

**Programming:** C#, Unity scripting, object-oriented programming, event-driven gameplay, debugging.

**Unity:** GameObjects, components, prefabs, physics/collisions, input, UI, animation, and gameplay state.

**Development workflow:** Git, GitHub, incremental commits, project organization, and iteration.

## Running the projects

This archive contains multiple Unity learning projects. Open the specific project directory in **Unity Hub** using the Unity Editor version recorded in that project's `ProjectSettings/ProjectVersion.txt`. Allow Unity to import the assets, then open the relevant scene.

Do not open the repository root as a Unity project unless it contains its own `Assets`, `Packages`, and `ProjectSettings` directories.

Generated Unity folders such as `Library`, `Temp`, and `Builds` are excluded via `.gitignore`.

## Demonstrations and portfolio

Screenshots, short videos, and playable releases will be linked here as they become available and verified. Source code and commit history are already available in this repository.

- [GitHub profile](https://github.com/AliphaticGuru)
- [Related data-persistence repository](https://github.com/AliphaticGuru/Unity-Junior-Programmer-Data-Persistency)

## Attribution

The prototypes and challenges originate from **Unity Learn's Junior Programmer Pathway**. This repository records my learning, exercises, and subsequent personal modifications. Individual project pages will distinguish course-provided assets/instructions from my original contributions.

## Next steps

- Verify and document exact project directories and Unity versions.
- Add representative screenshots and short demonstration videos.
- Document the strongest personal project as a detailed case study.
- Publish suitable builds separately from source code.

---

**Status:** Junior Programmer Pathway completed; portfolio documentation in progress.
