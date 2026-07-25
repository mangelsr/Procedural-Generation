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
        get { return startRoom; }
        set { startRoom = value; }
    }

    public Room EndRoom
    {
        get { return endRoom; }
        set { endRoom = value; }
    }

    public Vector2Int StartPositionAbsolute
    {
        get { return startPosition + startRoom.Area.position; }
    }

    public Vector2Int EndPositionAbsolute
    {
        get { return endPosition + endRoom.Area.position; }
    }

    public HallwayDirection StartDirection
    {
        get { return startDirection; }
    }

    public HallwayDirection EndDirection
    {
        get { return endDirection; }
        set { endDirection = value; }
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
