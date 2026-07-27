using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum HallwayDirection
{
    UNDEFINED,
    TOP,
    RIGTH,
    BOTTOM,
    LEFT,
};

public static class HallwayDirectionExtension
{
    private static Color yellow = new Color(1, 1, 0, 1);
    private static readonly Dictionary<HallwayDirection, Color> DirectionToColorMap = new()
    {
        { HallwayDirection.TOP, Color.cyan },
        { HallwayDirection.RIGTH, Color.magenta },
        { HallwayDirection.BOTTOM, Color.green },
        { HallwayDirection.LEFT, yellow },
    };

    public static Color GetColor(this HallwayDirection direction)
    {
        return DirectionToColorMap.TryGetValue(direction, out Color color) ? color : Color.gray;
    }

    public static Dictionary<Color, HallwayDirection> GetColorDirectionMap()
    {
        return DirectionToColorMap.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
    }

    public static HallwayDirection GetOppositeDirection(this HallwayDirection direction)
    {
        Dictionary<HallwayDirection, HallwayDirection> oppositeDirectionMap = new()
        {
            { HallwayDirection.TOP, HallwayDirection.BOTTOM },
            { HallwayDirection.RIGTH, HallwayDirection.LEFT },
            { HallwayDirection.BOTTOM, HallwayDirection.TOP },
            { HallwayDirection.LEFT, HallwayDirection.RIGTH },
        };
        return oppositeDirectionMap.TryGetValue(direction, out HallwayDirection oppositeDirection) ? oppositeDirection : HallwayDirection.UNDEFINED;
    }
}