using System.Collections.Generic;
using UnityEngine;

public class Room
{
    RectInt area;
    public RectInt Area { get => area; }

    public Room(RectInt area)
    {
        this.area = area;
    }

    public override string ToString()
    {
        return $"Room(pos: {area.position}, size: {area.width}x{area.height})";
    }

    public List<Hallway> CalcualteAllPossibleDoorways(int width, int length, int minDisntanceFromEdge)
    {
        List<Hallway> hallwaysCandidates = new List<Hallway>();

        int start = minDisntanceFromEdge;

        int top = length - 1;
        int maxX = width - minDisntanceFromEdge;

        for (int x = start; x < maxX; x++)
        {
            hallwaysCandidates.Add(new Hallway(HallwayDirection.BOTTOM, new Vector2Int(x, 0)));
            hallwaysCandidates.Add(new Hallway(HallwayDirection.TOP, new Vector2Int(x, top)));
        }

        int rigth = width - 1;
        int maxY = length - minDisntanceFromEdge;

        for (int y = start; y < maxY; y++)
        {
            hallwaysCandidates.Add(new Hallway(HallwayDirection.LEFT, new Vector2Int(0, y)));
            hallwaysCandidates.Add(new Hallway(HallwayDirection.RIGTH, new Vector2Int(rigth, y)));
        }

        return hallwaysCandidates;
    }

}
