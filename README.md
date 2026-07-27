# Procedural Generation

A Unity 6 project implementing procedural room-and-corridor dungeon generation.

## Features

- **Randomized room placement** with configurable size ranges and minimum spacing
- **Hallway system** connecting rooms through directional doorways
- **Layout visualization** rendered to a texture displayed on a quad in the scene
- **Seed-based generation** for reproducible layouts
- **Collision detection** preventing room/hallway overlap
- **Debug rendering** showing doorway positions and candidate rooms
- **Room templates** with finite supply counts for varied layouts

## Requirements

- Unity 6 (6000.0.75f1)
- Universal Render Pipeline (URP)

## Usage

1. Open `Assets/Scenes/SampleScene.unity`
2. Select the layout generator GameObject in the hierarchy
3. Use the Inspector buttons:
   - **Generate New Map** - generates a layout with the current seed
   - **Generate New Seed** - randomizes the seed
   - **Generate New Seed and Level** - randomizes seed and generates
4. Or use the context menu: right-click the component > **Generat Level Layout**

## Configuration

Create a `RoomLevelLayoutConfig` ScriptableObject via **Assets > Custom > Procedural Generation > Room Level Layout Config**.

| Parameter | Default | Description |
|-----------|---------|-------------|
| Width | 64 | Level width in pixels |
| Length | 64 | Level height in pixels |
| Door Distance From Edge | 1 | Min distance of doorways from room corners |
| Hallway Length Min/Max | 2-6 | Hallway length range |
| Min Room Distance | 1 | Min spacing between rooms |
| Room Templates | - | Array of room blueprints with size ranges and supply counts |

## Project Structure

```
Assets/Scripts/
├── Room.cs                        # Room data class
├── Hallway.cs                     # Hallway data class
├── HallwayDirection.cs            # Direction enum and extensions
├── Level.cs                       # Level container
├── RoomTemplate.cs                # Room blueprint definition
├── RoomLevelLayoutConfig.cs       # ScriptableObject for level config
├── LayouGeneratorRooms.cs         # Main generation controller
├── LayoutGeneratorButtons.cs      # Custom editor buttons
└── Utilities/                     # Helper classes and extensions
```

## Algorithm

1. Place a seed room at a random position in the center region
2. Calculate all possible doorways on the room walls
3. Iteratively pick a random doorway and attempt to place an adjacent room
4. Match doorways by opposite direction (e.g., TOP connects to BOTTOM)
5. Validate placement against bounds and existing rooms/hallways
6. Repeat until room count is reached or no valid placements remain

## Utilities

- **Texture2DExtension** - Drawing, saving, and manipulation methods for textures
- **PriorityQueue** - Quaternary min-heap implementation (available for future pathfinding)
- **Array2DWrapper** - Generic 2D array wrapper with Inspector grid editor
- **TileType** - Tile type enum for potential tilemap-based rendering
