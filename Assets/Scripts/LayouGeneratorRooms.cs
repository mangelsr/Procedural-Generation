using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;


public class LayoutGeneratorRooms : MonoBehaviour
{
    [SerializeField] int seed = Environment.TickCount;
    [SerializeField] int width = 64;
    [SerializeField] int length = 64;

    [SerializeField] int roomWidthMin = 3;
    [SerializeField] int roomWidthMax = 5;
    [SerializeField] int roomLengthMin = 3;
    [SerializeField] int roomLengthMax = 5;
    [SerializeField] int doorDistanceFromEdge = 1;
    [SerializeField] int hallwayLengthMin = 2;
    [SerializeField] int hallwayLengthMax = 6;
    [SerializeField] int minRoomDistance = 1;


    [SerializeField] int maxRoomCount = 10;

    [SerializeField] GameObject levelLayouDisplay;

    [SerializeField] List<Hallway> openDoorways;

    Random random;
    Level level;

    [ContextMenu("Generat Level Layout")]
    public void GenerateLevel()
    {
        random = new Random(seed);
        openDoorways = new List<Hallway>();
        level = new Level(width, length);

        var roomRect = GetStartRoomRect();
        Room room = new Room(roomRect);
        Debug.Log(room);

        List<Hallway> hallways = room.CalcualteAllPossibleDoorways(
            room.Area.width, room.Area.height, doorDistanceFromEdge
        );

        hallways.ForEach(h =>
        {
            h.StartRoom = room;
            openDoorways.Add(h);
            Debug.Log(h);
        });
        level.AddRoom(room);

        Hallway selectedEntryway = openDoorways[random.Next(openDoorways.Count)];

        AddRooms();

        DrawLayout(selectedEntryway, roomRect);
    }


    [ContextMenu("Generate new Seed")]
    public void GenerateNewSeed()
    {
        seed = Environment.TickCount;
    }

    [ContextMenu("Generate new Seed and Level")]
    public void GenerateNewSeedAndLevel()
    {
        GenerateNewSeed();
        GenerateLevel();
    }


    RectInt GetStartRoomRect()
    {
        int roomWidth = random.Next(roomWidthMin, roomWidthMax);
        int availableWidthX = (width / 2) - roomWidth;
        int randomX = random.Next(0, availableWidthX);
        int roomX = randomX + (width / 4);

        int roomLength = random.Next(roomLengthMin, roomLengthMax);
        int availableLengthY = (length / 2) - roomLength;
        int randomY = random.Next(0, availableLengthY);
        int roomY = randomY + (length / 4);

        return new RectInt(roomX, roomY, roomWidth, roomLength);
    }

    void DrawLayout(
        Hallway selectedEntryway = null,
        RectInt roomCandidateRect = new RectInt(),
        bool isDebug = false
    )
    {
        var renderer = levelLayouDisplay.GetComponent<Renderer>();

        var layoutTexture = (Texture2D)renderer.sharedMaterial.mainTexture;

        layoutTexture.Reinitialize(width, length);
        levelLayouDisplay.transform.localScale = new Vector3(width, length, 1);
        layoutTexture.FillWithColor(Color.black);

        Array.ForEach(level.Rooms, room => layoutTexture.DrawRectangle(room.Area, Color.white));
        Array.ForEach(level.Hallways,
            hallway => layoutTexture.DrawLine(
                hallway.StartPositionAbsolute, hallway.EndPositionAbsolute, Color.white
            )
        );

        if (isDebug)
        {
            layoutTexture.DrawRectangle(roomCandidateRect, Color.blue);
            foreach (Hallway hallway in openDoorways)
            {
                layoutTexture.SetPixel(
                    hallway.StartPositionAbsolute.x,
                    hallway.StartPositionAbsolute.y,
                    hallway.StartDirection.GetColor()
                );
            }
        }



        if (isDebug && selectedEntryway != null)
        {
            layoutTexture.SetPixel(
                selectedEntryway.StartPositionAbsolute.x,
                selectedEntryway.StartPositionAbsolute.y,
                Color.red
            );
        }

        layoutTexture.Apply();
        layoutTexture.SaveAsAsset("rooms");
    }

    Hallway SelectHallwayCandidate(RectInt roomCandidateRect, Hallway entryway)
    {
        Room room = new Room(roomCandidateRect);
        List<Hallway> candidates = room.CalcualteAllPossibleDoorways(
            room.Area.width, room.Area.height, doorDistanceFromEdge
        );
        HallwayDirection requiredDirection = entryway.StartDirection.GetOppositeDirection();

        List<Hallway> filterHallwayCandidates = candidates
            .Where(hallwayCandidate =>
                hallwayCandidate.StartDirection == requiredDirection)
            .ToList();

        return filterHallwayCandidates.Count > 0
            ? filterHallwayCandidates[random.Next(filterHallwayCandidates.Count)]
            : null;
    }

    Vector2Int CalculateRoomPosition(
        Hallway entryway, int roomWidth, int roomLength,
        int distance, Vector2Int endPosition
    )
    {
        Vector2Int roomPosition = entryway.StartPositionAbsolute;
        switch (entryway.StartDirection)
        {
            case HallwayDirection.TOP:
                roomPosition.x -= endPosition.x;
                roomPosition.y += distance + 1;
                break;

            case HallwayDirection.RIGTH:
                roomPosition.x += distance + 1;
                roomPosition.y -= endPosition.y;
                break;

            case HallwayDirection.BOTTOM:
                roomPosition.x -= endPosition.x;
                roomPosition.y -= distance + roomLength;
                break;

            case HallwayDirection.LEFT:
                roomPosition.x -= distance + roomWidth;
                roomPosition.y -= endPosition.y;
                break;
        }

        return roomPosition;
    }

    Room ConstructAdjacentRoom(Hallway selectedEntryway)
    {
        RectInt newRoomRect = new()
        {
            width = random.Next(roomWidthMin, roomWidthMax),
            height = random.Next(roomLengthMin, roomLengthMax),
        };

        Hallway selectedExit = SelectHallwayCandidate(
            newRoomRect, selectedEntryway
        );

        if (selectedExit == null) return null;

        Vector2Int newRoomPosition = CalculateRoomPosition(
            selectedEntryway,
            newRoomRect.width, newRoomRect.height,
            random.Next(hallwayLengthMin, hallwayLengthMax + 1),
            selectedExit.StartPosition
        );

        newRoomRect.position = newRoomPosition;

        if (!IsRoomCandidateValid(newRoomRect))
        {
            return null;
        }

        Room newRoom = new(newRoomRect);

        selectedEntryway.EndRoom = newRoom;
        selectedEntryway.EndPosition = selectedExit.StartPosition;

        return newRoom;
    }

    void AddRooms()
    {
        while (openDoorways.Count > 0 && level.Rooms.Length < maxRoomCount)
        {
            Hallway selectedEntryway = openDoorways[random.Next(openDoorways.Count)];
            Room newRoom = ConstructAdjacentRoom(selectedEntryway);

            if (newRoom == null)
            {
                openDoorways.Remove(selectedEntryway);
                continue;
            }

            level.AddRoom(newRoom);
            level.AddHallway(selectedEntryway);

            selectedEntryway.EndRoom = newRoom;

            List<Hallway> newOpenHallways = newRoom.CalcualteAllPossibleDoorways(
                newRoom.Area.width, newRoom.Area.height, doorDistanceFromEdge
            );
            newOpenHallways.ForEach(h => h.StartRoom = newRoom);

            openDoorways.Remove(selectedEntryway);
            openDoorways.AddRange(newOpenHallways);
        }
    }

    bool IsRoomCandidateValid(RectInt roomCandidateRect)
    {
        RectInt levelRect = new(1, 1, width - 2, length - 2);
        return levelRect.Contains(roomCandidateRect) &&
            !CheckRoomOverlap(roomCandidateRect, level.Rooms, level.Hallways, minRoomDistance);
    }

    bool CheckRoomOverlap(
        RectInt roomCandidateRect, Room[] rooms,
        Hallway[] hallways, int minRoomDistance
    )
    {
        RectInt paddedRoomRect = new()
        {
            x = roomCandidateRect.x - minRoomDistance,
            y = roomCandidateRect.y - minRoomDistance,
            width = roomCandidateRect.width + 2 * minRoomDistance,
            height = roomCandidateRect.height + 2 * minRoomDistance,
        };

        foreach (Room room in rooms)
        {
            if (paddedRoomRect.Overlaps(room.Area))
                return true;
        }

        foreach (Hallway hallway in hallways)
        {
            if (paddedRoomRect.Overlaps(hallway.Area))
                return true;
        }

        return false;
    }





}
