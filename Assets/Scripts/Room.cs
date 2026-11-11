using System.Collections.Generic;
using UnityEngine;
using Flags = System.FlagsAttribute;

[Flags]
public enum RoomType
{
    Default = 1,
    Start = 1 << 1,
    Exit = 1 << 2,
    Boss = 1 << 3,
    Treasure = 1 << 4,
    Prision = 1 << 5,
    Library = 1 << 6,
    Garden = 1 << 7,
}

public class Room
{
    List<Hallway> hallways;
    RectInt area;
    public RectInt Area { get => area; }
    public Texture2D LayoutTexture { get; }
    public RoomType Type { get; set; } = RoomType.Default;
    public int Conectedness => hallways.Count;

    public Room(RectInt area)
    {
        this.area = area;
        hallways = new();
    }

    public Room(int x, int y, Texture2D layoutTexture)
    {
        area = new RectInt(x, y, layoutTexture.width, layoutTexture.height);
        LayoutTexture = layoutTexture;
        hallways = new();
    }

    public override string ToString()
    {
        return $"Room(pos: {area.position}, size: {area.width}x{area.height})";
    }

    public List<Hallway> CalculateAllPossibleDoorways(int width, int length, int minDisntanceFromEdge)
    {
        if (LayoutTexture == null)
            return CalculateAllPossibleDoorwaysForRectangularRooms(width, length, minDisntanceFromEdge);
        else
            return CalculateAllPossibleDoorwayPosition(LayoutTexture);
    }


    public List<Hallway> CalculateAllPossibleDoorwaysForRectangularRooms(int width, int length, int minDisntanceFromEdge)
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

    List<Hallway> CalculateAllPossibleDoorwayPosition(Texture2D layoutTexture)
    {
        List<Hallway> possibleHallwayPositions = new();
        int width = layoutTexture.width;
        int heigth = layoutTexture.height;

        for (int y = 0; y < heigth; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Color pixelColor = layoutTexture.GetPixel(x, y);
                HallwayDirection direcion = GetHallwayDirection(pixelColor);
                if (direcion != HallwayDirection.UNDEFINED)
                {
                    Hallway hallway = new Hallway(direcion, new Vector2Int(x, y));
                    possibleHallwayPositions.Add(hallway);
                }
            }
        }

        return possibleHallwayPositions;
    }

    HallwayDirection GetHallwayDirection(Color color)
    {
        Dictionary<Color, HallwayDirection> map = HallwayDirectionExtension.GetColorDirectionMap();
        return map.TryGetValue(color, out HallwayDirection direction)
                ? direction
                : HallwayDirection.UNDEFINED;
    }

    public void AddHallway(Hallway selectedHallway)
    {
        hallways.Add(selectedHallway);
    }
}
