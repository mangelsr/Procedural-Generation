using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class LayouGeneratedRooms : MonoBehaviour
{
    [SerializeField] int width = 64;
    [SerializeField] int length = 64;

    [SerializeField] int roomWidthMin = 3;
    [SerializeField] int roomWidthMax = 5;
    [SerializeField] int roomLengthMin = 3;
    [SerializeField] int roomLengthMax = 5;

    [SerializeField] GameObject levelLayouDisplay;

    System.Random random;

    [ContextMenu("Generat Level Layout")]
    public void GenerateLevel()
    {
        random = new System.Random();
        var roomRect = GetStartRoomRect();
        Debug.Log(roomRect);
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

        var layoutTexture = (Texture2D) renderer.sharedMaterial.mainTexture;
        
        layoutTexture.Reinitialize(width, length);
        levelLayouDisplay.transform.localScale = new Vector3(width, length , 1);
        layoutTexture.FillWithColor(Color.black);
        layoutTexture.DrawRectangle(roomCandidateRect, Color.cyan);
        layoutTexture.SaveAsAsset("rooms");
    }

}
