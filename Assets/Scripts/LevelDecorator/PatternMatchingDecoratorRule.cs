using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

[Serializable]
[CreateAssetMenu(fileName = "DecoratorRule", menuName = "Custom/Procedural Generation/Pattern Decorator Rule")]
public class PatternMatchingDecoratorRule : BaseDecoratorRule
{
    [SerializeField] GameObject prefab;
    [SerializeField] float prefabRotation = 0.0f;
    [SerializeField] Array2DWrapper<TileType> placement;
    [SerializeField] Array2DWrapper<TileType> fill;
    [SerializeField] bool centerHorizontally = false;
    [SerializeField] bool centerVertically = false;


    internal override void Apply(TileType[,] levelDecorated, Room room, Transform parent)
    {
        Vector2Int[] ocurrences = FindOccurences(levelDecorated, room);

        if (ocurrences.Length == 0) return;

        Random random = SharedLevelData.Instance.Rand;
        int randomIndex = random.Next(ocurrences.Length);

        Vector2Int occurrence = ocurrences[randomIndex];

        for (int y = 0; y < placement.Height; y++)
        {
            for (int x = 0; x < placement.Width; x++)
            {
                TileType tileType = fill[x, y];
                if (!TileType.Noop.Equals(tileType))
                    levelDecorated[occurrence.x + x, occurrence.y + y] = tileType;
            }
        }

        if (prefab == null) return;

        GameObject decoration = Instantiate(prefab, parent.transform);

        Vector3 currentRotation = decoration.transform.eulerAngles;
        decoration.transform.eulerAngles = currentRotation + new Vector3(0, prefabRotation, 0);

        Vector3 center = new(occurrence.x, 0, occurrence.y);
        int scale = SharedLevelData.Instance.Scale;

        decoration.transform.position = center * scale;
        decoration.transform.localScale = Vector3.one * scale;

        PropVariationGenerator variationGenerator = decoration.GetComponent<PropVariationGenerator>();
        if (variationGenerator != null)
            variationGenerator.GenerateVariation();        
    }

    internal override bool CanBeApplied(TileType[,] levelDecorated, Room room)
    {
        return FindOccurences(levelDecorated, room).Length > 0;
    }

    private Vector2Int[] FindOccurences(TileType[,] levelDecorated, Room room)
    {
        List<Vector2Int> occurences = new();

        int centerX = Mathf.RoundToInt(room.Area.position.x + (room.Area.width - placement.Width) / 2.0f);
        int centerY = Mathf.RoundToInt(room.Area.position.y + (room.Area.height - placement.Height) / 2.0f);

        for (int y = room.Area.position.y - 1; y < room.Area.position.y + room.Area.height + 2 - placement.Height; y++)
        {
            for (int x = room.Area.position.x - 1; x < room.Area.position.x + room.Area.width + 2 - placement.Width; x++)
            {
                if (centerHorizontally && x != centerX) continue;
                if (centerVertically && y != centerY) continue;

                if (IsPatternAtPosition(levelDecorated, placement, x, y))
                {
                    occurences.Add(new Vector2Int(x, y));
                }
            }
        }
        return occurences.ToArray();
    }

    private bool IsPatternAtPosition(TileType[,] levelDecorated, Array2DWrapper<TileType> pattern, int startX, int startY)
    {
        int maxX = levelDecorated.GetLength(0);
        int maxY = levelDecorated.GetLength(1);

        for (int y = 0; y < pattern.Height; y++)
        {
            for (int x = 0; x < pattern.Width; x++)
            {
                int targetX = startX + x;
                int targetY = startY + y;

                if (targetX < 0 || targetY < 0 || targetX >= maxX || targetY >= maxY)
                {
                    return false;
                }

                if (
                    !TileType.Noop.Equals(pattern[x, y]) &&
                    levelDecorated[targetX, targetY] != pattern[x, y]
                )
                {
                    return false;
                }
            }
        }
        return true;
    }
}
