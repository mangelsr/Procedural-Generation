using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;


public class LayoutGeneratorRooms : MonoBehaviour
{
    [SerializeField] RoomLevelLayoutConfig levelConfig;
    [SerializeField] int maxRoomCount = 10;

    [SerializeField] GameObject levelLayouDisplay;

    [SerializeField] List<Hallway> openDoorways;

    Random random;
    Level level;
    Dictionary<RoomTemplate, int> availableRooms;

    [ContextMenu("Generat Level Layout")]
    public Level GenerateLevel()
    {
        SharedLevelData.Instance.ResetRandom();
        random = SharedLevelData.Instance.Rand;
        availableRooms = levelConfig.GetAvaialbleRooms();

        openDoorways = new List<Hallway>();
        level = new Level(levelConfig.Width, levelConfig.Length);

        RoomTemplate startRoomTemplate = availableRooms.Keys.ElementAt(random.Next(availableRooms.Count));

        var roomRect = GetStartRoomRect(startRoomTemplate);
        Room room = CreateNewRoom(roomRect, startRoomTemplate);

        List<Hallway> hallways = room.CalculateAllPossibleDoorways(
            room.Area.width, room.Area.height, levelConfig.DoorDistanceFromEdge
        );

        hallways.ForEach(h =>
        {
            h.StartRoom = room;
            openDoorways.Add(h);
        });
        level.AddRoom(room);

        Hallway selectedEntryway = openDoorways[random.Next(openDoorways.Count)];

        AddRooms();

        DrawLayout(selectedEntryway, roomRect);
        
        int startRoomIndex = random.Next(level.Rooms.Length);
        level.PlayerStartRoom = level.Rooms[startRoomIndex];
    
        return level;
    }


    [ContextMenu("Generate new Seed")]
    public void GenerateNewSeed()
    {
        SharedLevelData.Instance.GenerateSeed();
    }

    [ContextMenu("Generate new Seed and Level")]
    public void GenerateNewSeedAndLevel()
    {
        GenerateNewSeed();
        GenerateLevel();
    }


    RectInt GetStartRoomRect(RoomTemplate roomTemplate)
    {
        RectInt roomSize = roomTemplate.GenerateRoomCandidateRect(random);

        int roomWidth = roomSize.width;
        int availableWidthX = (levelConfig.Width / 2) - roomWidth;
        int randomX = random.Next(0, availableWidthX);
        int roomX = randomX + (levelConfig.Width / 4);

        int roomLength = roomSize.height;
        int availableLengthY = (levelConfig.Length / 2) - roomLength;
        int randomY = random.Next(0, availableLengthY);
        int roomY = randomY + (levelConfig.Length / 4);

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

        layoutTexture.Reinitialize(levelConfig.Width, levelConfig.Length);
        levelLayouDisplay.transform.localScale = new Vector3(levelConfig.Width, levelConfig.Length, 1);
        layoutTexture.FillWithColor(Color.black);

        Array.ForEach(level.Rooms, room =>
        {
            if (room.LayoutTexture != null)
                layoutTexture.DrawTexture(room.LayoutTexture, room.Area);
            else
                layoutTexture.DrawRectangle(room.Area, Color.white);
        });
        Array.ForEach(level.Hallways,
            hallway => layoutTexture.DrawLine(
                hallway.StartPositionAbsolute, hallway.EndPositionAbsolute, Color.white
            )
        );
        layoutTexture.ConvertToBlackAndWhite();

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
        layoutTexture.SaveAsAsset("LayoutMap");
    }

    Hallway SelectHallwayCandidate(
        RectInt roomCandidateRect, RoomTemplate roomTemplate, 
        Hallway entryway)
    {
        Room room = CreateNewRoom(roomCandidateRect, roomTemplate, false);
        List<Hallway> candidates = room.CalculateAllPossibleDoorways(
            room.Area.width, room.Area.height, levelConfig.DoorDistanceFromEdge
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
        RoomTemplate roomTemplate = availableRooms.Keys.ElementAt(random.Next(availableRooms.Count));
        RectInt newRoomRect = new()
        {
            width = random.Next(roomTemplate.RoomWidthMin, roomTemplate.RoomWidthMax),
            height = random.Next(roomTemplate.RoomLengthMin, roomTemplate.RoomLengthMax),
        };

        Hallway selectedExit = SelectHallwayCandidate(
            newRoomRect, roomTemplate, selectedEntryway
        );

        if (selectedExit == null) return null;

        Vector2Int newRoomPosition = CalculateRoomPosition(
            selectedEntryway,
            newRoomRect.width, newRoomRect.height,
            random.Next(levelConfig.HallwayLengthMin, levelConfig.HallwayLengthMax + 1),
            selectedExit.StartPosition
        );

        newRoomRect.position = newRoomPosition;

        if (!IsRoomCandidateValid(newRoomRect))
        {
            return null;
        }

        Room newRoom = CreateNewRoom(newRoomRect, roomTemplate);

        selectedEntryway.EndRoom = newRoom;
        selectedEntryway.EndPosition = selectedExit.StartPosition;

        return newRoom;
    }

    void AddRooms()
    {
        while (openDoorways.Count > 0 && level.Rooms.Length < maxRoomCount && availableRooms.Count > 0)
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

            List<Hallway> newOpenHallways = newRoom.CalculateAllPossibleDoorways(
                newRoom.Area.width, newRoom.Area.height, levelConfig.DoorDistanceFromEdge
            );
            newOpenHallways.ForEach(h => h.StartRoom = newRoom);

            openDoorways.Remove(selectedEntryway);
            openDoorways.AddRange(newOpenHallways);
        }
    }

    bool IsRoomCandidateValid(RectInt roomCandidateRect)
    {
        RectInt levelRect = new(1, 1, levelConfig.Width - 2, levelConfig.Length - 2);
        return levelRect.Contains(roomCandidateRect) &&
            !CheckRoomOverlap(roomCandidateRect, level.Rooms, level.Hallways, levelConfig.MinRoomDistance);
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

    private void UseUpRoomTemplate(RoomTemplate roomTemplate)
    {
        availableRooms[roomTemplate] -= 1;
        if (availableRooms[roomTemplate] == 0)
            availableRooms.Remove(roomTemplate);
    }

    Room CreateNewRoom(RectInt roomCandidateRect, 
        RoomTemplate roomTemplate, bool useUp = true)
    {
        if (useUp)
            UseUpRoomTemplate(roomTemplate);

        if (roomTemplate.LayoutTexture == null)
            return new Room(roomCandidateRect);

        return new Room(
            roomCandidateRect.x, roomCandidateRect.y,
            roomTemplate.LayoutTexture
        );
    }

}
