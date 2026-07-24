using UnityEngine;

public class Hallway
{
    Vector2Int startPosition;
    Vector2Int endPosition;

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

    public Hallway(
        Vector2Int startPosition, Room startRoom = null
    )
    {
        this.startPosition = startPosition;
        this.startRoom = startRoom;
    }

    public override string ToString()
    {
        return $"Hallway(start: {startPosition}, end: {endPosition}, startRoom: {startRoom?.ToString() ?? "null"}, endRoom: {endRoom?.ToString() ?? "null"})";
    }
}
