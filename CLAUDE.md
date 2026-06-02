# PixelFlow3D — AI Development Guide

## Project Overview
Reimplementing Pixel Flow by Loom Games — a top-down shooter-puzzle game.
- Engine: Unity 6 (6000.4.5f1), URP
- Platform: Mobile portrait
- Status: Phase 0 (project setup)

## How To Work With This Project

### Phase Kickoff (per module)
1. Read the relevant YAML/JSON data structures first
2. Define C# data classes before writing systems
3. Check `Assets/GameObject/` for prefab names before instantiation
4. Test after each module, report errors for fixes

### Coding Style
- Flat, minimal — no DI frameworks, no abstract bases unless 3+ implementations
- Each class is small, single-purpose
- `for` loops over LINQ in hot paths (avoid GC)
- Cache references, no `GameObject.Find()` in Update
- Unity Input System (new), not legacy Input Manager
- DOTween available for tweening

## Folder Structure
```
Assets/_Game/
  Scenes/           Gameplay.unity (main), MainMenu.unity (future)
  Scripts/
    Core/           GameManager, GridManager, LevelLoader
    Data/           Plain C# data classes (no MonoBehaviour)
    Player/         InputHandler, SelectionRenderer
    Mechanics/      ColorSwapHandler, TargetManager
    Features/       ShooterManager, WallHandler, ConveyorManager, etc.
    UI/             HUDController, LevelCompleteUI, MainMenuUI
  Prefabs/          Our runtime prefabs
  Materials/        Custom materials
```

Never modify code in `Assets/Scripts/` (obfuscated reference code).

## Key Asset Paths
- Level JSON: `/Users/huanghao/workspace/2026-5-7/ExportedProject/levels_json/Level_{N}.json` (699 levels)
- Prefabs: `Assets/GameObject/` (Pixel 1x1..8x8, HardPixel, Wall, Shooter, etc.)
- Materials: `Assets/Material/`
- Color config: `Assets/MonoBehaviour/Main Pixel Unity Materials.asset` (32 entries, index 0-31)
- Audio: `Assets/AudioClip/`

## Gameplay Mechanics (Confirmed)
- Top half: pixel grid (width × height), surrounded by counter-clockwise conveyor belt
- Bottom half: 2-5 columns of shooters, only first in each column is tappable
- Tap shooter → flies onto conveyor at bottom-left entry → moves counter-clockwise
- Fires at pixels that are: same row/column + same color + no obstacles in path
- Pixel health: default 1, `pixelHealths[]` can set higher values
- areaX/areaY defines pixel visual size (1x1 to 8x8), ONE data entry per block
- Win: all pixels destroyed. Lose: 5 slot slots full + another shooter needs to return

## Level JSON Structure
```json
{
  "levelNumber": 1,
  "difficulty": "Easy",
  "slotCount": 5,
  "conveyorLimit": 5,
  "queueGroup": {
    "shooterQueues": [
      { "shooters": [{"id":0, "ammo":40, "material":5}, ...] },
      ...
    ]
  },
  "pixelGrid": {
    "width": 16, "height": 15,
    "pixels": [{"x":0,"y":0,"material":5,"areaX":1,"areaY":1}, ...],
    "pixelHealths": [{"x":11,"y":8,"health":10}, ...],
    "walls": {"Walls": [...]},
    "pixelIceBlocks": {"IceBlocks": [...]},
    "pixelWoodBlocks": {"WoodBlocks": [...]},
    ...more obstacle types...
  }
}
```

## Color/Material System
- `material` field (0-31) indexes into Main Pixel Unity Materials.asset
- Each shooter has `material` matching pixel colors
- Projectile must match pixel color to damage it
