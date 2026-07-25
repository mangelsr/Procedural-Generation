using System.Collections.Generic;
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
    private static readonly Dictionary<HallwayDirection, Color> DirectionToColorMap = new()
    {
        { HallwayDirection.TOP, Color.cyan },
        { HallwayDirection.RIGTH, Color.magenta },
        { HallwayDirection.BOTTOM, Color.green },
        { HallwayDirection.LEFT, Color.yellow },
    };

    public static Color GetColor(this HallwayDirection direction)
    {
        return DirectionToColorMap.TryGetValue(direction, out Color color) ? color : Color.gray;
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