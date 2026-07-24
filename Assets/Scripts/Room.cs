using System.Collections.Generic;
using UnityEngine;

public class Room
{
    RectInt area;
    public RectInt Area { get { return area; } }

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
        hallwaysCandidates.Add(new Hallway(Vector2Int.zero));
        hallwaysCandidates.Add(new Hallway(new Vector2Int(width, length)));
        return hallwaysCandidates;
    }

    
}
