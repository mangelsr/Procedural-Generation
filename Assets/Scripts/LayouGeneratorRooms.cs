using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LayouGeneratorRooms : MonoBehaviour
{
    [SerializeField] int width = 64;
    [SerializeField] int length = 64;

    [SerializeField] int roomWidthMin = 3;
    [SerializeField] int roomWidthMax = 5;
    [SerializeField] int roomLengthMin = 3;
    [SerializeField] int roomLengthMax = 5;

    [SerializeField] GameObject levelLayouDisplay;

    [SerializeField] List<Hallway> openDoorways;

    System.Random random;
    Level level;

    [ContextMenu("Generat Level Layout")]
    public void GenerateLevel()
    {
        random = new System.Random();
        openDoorways = new List<Hallway>();
        level = new Level(width, length);

        var roomRect = GetStartRoomRect();
        Room room = new Room(roomRect);
        Debug.Log(room);

        List<Hallway> hallways = room.CalcualteAllPossibleDoorways(
            room.Area.width, room.Area.height, 1
        );

        hallways.ForEach(h =>
        {
            h.StartRoom = room;
            openDoorways.Add(h);
            Debug.Log(h);
        });
        level.AddRoom(room);

        Hallway selectedEntryway = openDoorways[random.Next(openDoorways.Count)];
        Hallway selectedExit = SelectHallwayCandidate(
            new RectInt(0, 0, 5, 7),
            selectedEntryway
        );

        Debug.Log(selectedExit.StartPosition);
        Debug.Log(selectedExit.StartDirection);

        DrawLayout(selectedEntryway, roomRect);
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
        RectInt roomCandidateRect = new RectInt()
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

        layoutTexture.DrawRectangle(roomCandidateRect, Color.white);

        Debug.Log(openDoorways.Count);
        foreach (Hallway hallway in openDoorways)
        {
            layoutTexture.SetPixel(
                hallway.StartPositionAbsolute.x,
                hallway.StartPositionAbsolute.y,
                hallway.StartDirection.GetColor()
            );
        }

        if (selectedEntryway != null)
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
        List<Hallway> candidates = room.CalcualteAllPossibleDoorways(room.Area.width, room.Area.height, 1);
        HallwayDirection requiredDirection = entryway.StartDirection.GetOppositeDirection();

        List<Hallway> filterHallwayCandidates = candidates
            .Where(hallwayCandidate =>
                hallwayCandidate.StartDirection == requiredDirection)
            .ToList();

        return filterHallwayCandidates.Count > 0
            ? filterHallwayCandidates[random.Next(filterHallwayCandidates.Count)]
            : null;
    }

}
