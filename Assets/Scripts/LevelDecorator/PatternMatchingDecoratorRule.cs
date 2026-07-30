using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "DecoratorRule", menuName = "Custom/Procedural Generation/Pattern Decorator Rule")]
public class PatternMatchingDecoratorRule : BaseDecoratorRule
{
    [SerializeField] GameObject prefab;
    [SerializeField] Array2DWrapper<TileType> placement;
    [SerializeField] Array2DWrapper<TileType> fill;

    internal override void Apply(TileType[,] levelDecorator, Room room, Transform parent)
    {
    }

    internal override bool CanBeApplied(TileType[,] levelDecorated, Room room)
    {
        return FindOccurences(levelDecorated, room).Length > 0;
    }

    private Vector2Int[] FindOccurences(TileType[,] levelDecorated, Room room)
    {
        List<Vector2Int> occurences = new();
        for (int y = room.Area.position.y - 1; y < room.Area.position.y + room.Area.height + 2 - placement.Height; y++)
        {
            for (int x = room.Area.position.x - 1; x < room.Area.position.x + room.Area.width + 2 - placement.Width; x++)
            {
                if (IsPatternAtPosition(levelDecorated, placement, x, y))
                {
                    occurences.Add(new Vector2Int(x, y));
                    levelDecorated[x, y] = TileType.Item;
                }
            }
        }
        return occurences.ToArray();
    }

    private bool IsPatternAtPosition(TileType[,] levelDecorated, Array2DWrapper<TileType> pattern, int startX, int startY)
    {
        for (int y = 0; y < pattern.Height; y++)
        {
            for (int x = 0; x < pattern.Width; x++)
            {
                if (levelDecorated[startX + x, startY + y] != pattern[x, y])
                {
                    return false;
                }
            }
        }
        return true;
    }
}
