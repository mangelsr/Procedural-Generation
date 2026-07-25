using UnityEngine;

public class Hallway
{
    Vector2Int startPosition;
    Vector2Int endPosition;

    HallwayDirection startDirection;
    HallwayDirection endDirection;

    Room startRoom;
    Room endRoom;

    public Room StartRoom
    {
        get => startRoom;
        set => startRoom = value;
    }

    public Room EndRoom
    {
        get => endRoom;
        set => endRoom = value;
    }

    public Vector2Int StartPositionAbsolute
    {
        get => startPosition + startRoom.Area.position;
    }

    public Vector2Int EndPositionAbsolute
    {
        get => endPosition + endRoom.Area.position;
    }

    public HallwayDirection StartDirection
    {
        get => startDirection;
    }

    public HallwayDirection EndDirection
    {
        get => endDirection;
        set => endDirection = value;
    }

    public Vector2Int StartPosition
    {
        get => startPosition;
        set => startPosition = value;
    }

    public Vector2Int EndPosition
    {
        get => endPosition;
        set => endPosition = value;
    }


    public Hallway(
        HallwayDirection startDirection,
        Vector2Int startPosition,
        Room startRoom = null
    )
    {
        this.startDirection = startDirection;
        this.startPosition = startPosition;
        this.startRoom = startRoom;
    }

    public override string ToString()
    {
        return $"Hallway(start: {startPosition}, end: {endPosition}, startRoom: {startRoom?.ToString() ?? "null"}, endRoom: {endRoom?.ToString() ?? "null"})";
    }
}
