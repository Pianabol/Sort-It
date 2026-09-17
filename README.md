# Sort It! 🧪

A highly optimized, hyper-casual liquid sort puzzle game built in Unity. Pour, sort, and solve! This project demonstrates a strong emphasis on clean architecture, single-scene state management, memory optimization, and satisfying "game juice."

## 🎮 Gameplay Showcase

<div align="center">
  <a href="https://www.youtube.com/shorts/nikh8vGqiug">
    <img src="https://img.youtube.com/vi/nikh8vGqiug/0.jpg" alt="Sort It! Gameplay Showcase" width="350">
  </a>
  <br>
  <i>Click the image above to watch the gameplay short on YouTube!</i>
</div>

## ⚙️ Technical Architecture & Features

* **Single-Scene State Machine:** Completely bypassed traditional, heavy multi-scene loading. The entire game flow (Menu, Game, Level Complete, Game Over) is managed dynamically via `GameManager` and `IGameStateListener` interfaces for zero-latency transitions.
* **Deadlock Detection Algorithm:** A custom, lightweight algorithm (`HasAvailableMoves`) constantly evaluates the state of all bottles and color layers. If no valid moves remain, it instantly triggers a Game Over, preventing the player from ever getting stuck.
* **Data-Driven Levels:** Implemented a modular `Level.cs` structure allowing per-level custom durations, layouts, and automated timer integrations without hardcoding.
* **Game Feel & "Juice":** Integrated `LeanTween` for non-physics animations. Buttons naturally breathe (idle heartbeat), pop-ups bounce satisfyingly (easeOutBack), and screen transitions are instant to keep the hyper-casual tempo flowing.
* **Zero-Latency Audio System:** A centralized Singleton `SoundManager` handles all UI and gameplay SFX seamlessly, ensuring pour and click sounds trigger instantly without cutting each other off.

## 🛠️ Tech Stack

* **Engine:** Unity
* **Language:** C#
* **Architecture:** Observer Pattern, Singleton Managers, Single-Scene State Machine, Coroutine-based sequencing.

## 📜 Credits & Assets

A huge thanks to the creators of the following assets and sound effects that brought this project to life:

| Asset Type | Source / Creator | Link |
| :--- | :--- | :--- |
| **UI Elements** | Hyper Casual UI Pack | [Unity Asset Store](https://assetstore.unity.com/packages/2d/gui/hyper-casual-ui-pack-375832) |
| **Pouring SFX** | Pixabay | [Water Pouring Sound](https://pixabay.com/sound-effects/film-special-effects-water-pouring-405458/) |
| **Bottle Select SFX** | Pixabay | [Select Sound](https://pixabay.com/sound-effects/film-special-effects-select-sound-121244/) |
| **UI Click SFX** | Pixabay | [Click Sound](https://pixabay.com/sound-effects/film-special-effects-click-sound-432501/) |
| **Level Complete SFX** | Pixabay | [Casino Winning Pop](https://pixabay.com/sound-effects/film-special-effects-pop-win-casino-winning-398059/) |
| **Game Over SFX** | Pixabay | [Marimba Game Over](https://pixabay.com/sound-effects/film-special-effects-marimba-game-over-250960/) |