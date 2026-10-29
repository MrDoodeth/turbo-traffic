# Turbo Traffic

A 3D endless racing game for the browser, built with Unity. Dodge traffic, overtake cars,
collect gems and earn money to unlock new cars. Published on Yandex Games.

This is a personal learning project and is not perfect :)

## 📦 Features

- Endless road with traffic, overtaking and an oncoming lane.
- Score from speed, overtakes and driving in the oncoming lane.
- Four types of gems to collect, spawned along the lanes.
- Garage and shop: 20 cars, each with several color variants, bought with in-game money.
- Rewarded ads (continue after a crash, x2 reward, extra money) via the Yandex Games SDK.
- Cloud saves and a global score leaderboard.
- Sound and music toggles, quality settings, pause menu and a mobile HUD.

## 📋 Requirements

- Unity **2022.3.34f1** (Universal Render Pipeline 14).
- Windows, macOS or Linux with Unity Hub.

## 🚀 Build and run

```bash
git clone https://github.com/MrDoodeth/turbo-traffic.git
```

1. Open the folder in Unity Hub with editor version 2022.3.34f1.
2. Open a scene from `Assets/_RaceRunner/` and press Play.
3. For a release build, switch the platform to **WebGL** and build. Ads, saves and the
   leaderboard work only inside Yandex Games, so they are inactive in the editor.

## 🛠️ Project structure

```text
Assets/
├── _RaceRunner/        # game code, prefabs, UI
│   ├── Scripts/        # RaceManager, CarController, spawners, shop, panels
│   ├── Prefabs/        # PlayerCars, EnemyCars
│   └── UI/
└── PluginYourGames/    # Yandex Games SDK wrapper
```

Main scripts: `RaceManager` (score, money, finish), `CarController` (player car),
`EnemyCarSpawner`, `GemSpawner` (object pools), `RoadSpawner`, `GarageManager` and
`ShopPanel`, `CarsConfig` (cars and prices as a ScriptableObject).

## 🧰 Stack

- **Engine:** Unity 2022.3, C#, URP, Input System, Cinemachine.
- **Libraries:** UniTask, NaughtyAttributes, PluginYourGames (Yandex Games SDK).
- **Platform:** WebGL, Yandex Games.

## 🎨 Credits

The project uses third-party assets: ARCADE Ultimate Vehicles Pack, SimplePoly City,
Cartoon FX Remaster, LowPoly gems pack and a UI click sound pack. They belong to their
authors and keep their own licenses.

## 📜 License

MIT for the original code, see [LICENSE](LICENSE). Third-party assets are not covered by it.
