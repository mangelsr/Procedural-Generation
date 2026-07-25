using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
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

    [ContextMenu("Generat Level Layout")]
    public void GenerateLevel()
    {
        random = new System.Random();
        openDoorways = new List<Hallway>();
        var roomRect = GetStartRoomRect();
        Room room = new Room(roomRect);
        Debug.Log(room);
        List<Hallway> hallways = room.CalcualteAllPossibleDoorways(room.Area.width, room.Area.height, 1);
        hallways.ForEach(h =>
        {
            h.StartRoom = room;
            openDoorways.Add(h);
            Debug.Log(h);
        });

        DrawLayout(roomRect);
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

    void DrawLayout(RectInt roomCandidateRect = new RectInt())
    {
        var renderer = levelLayouDisplay.GetComponent<Renderer>();

        var layoutTexture = (Texture2D)renderer.sharedMaterial.mainTexture;

        layoutTexture.Reinitialize(width, length);
        levelLayouDisplay.transform.localScale = new Vector3(width, length, 1);
        layoutTexture.FillWithColor(Color.black);
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
        layoutTexture.Apply();
        
        layoutTexture.SaveAsAsset("rooms");
    }

}
