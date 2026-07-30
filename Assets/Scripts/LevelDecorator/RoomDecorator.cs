using UnityEngine;
using Random = System.Random;

public class RoomDecorator : MonoBehaviour
{
    [SerializeField] GameObject parent;
    [SerializeField] LayoutGeneratorRooms layoutGenerator;
    [SerializeField] Texture2D levelTexture;
    [SerializeField] Texture2D decoratedTexture;
    [SerializeField] BaseDecoratorRule[] availableRules;

    Random random;

    [ContextMenu("Place Items")]
    public void PlaceItemsFromMenu()
    {
        SharedLevelData.Instance.ResetRandom();
        Level level = layoutGenerator.GenerateLevel();
        PlaceItems(level);
    }

    public void PlaceItems(Level level)
    {
        random = SharedLevelData.Instance.Rand;
        Transform decorationsTransform = parent.transform.Find("Decorations");

        if (decorationsTransform == null)
        {
            GameObject decorationsGameObject = new GameObject("Decorations");
            decorationsTransform = decorationsGameObject.transform;
            decorationsTransform.SetParent(parent.transform);
        }
        else
        {
            decorationsTransform.DestroyAllChildren();
        }

        TileType[,] levelDecorated = InitializeDecoratorArray();
        DecorateRoom(levelDecorated, level.Rooms[0], decorationsTransform);
        GenerateTectureFromTileType(levelDecorated);

    }

    private void DecorateRoom(TileType[,] levelDecorated, Room room, Transform decorationsTransform)
    {
        BaseDecoratorRule selectedRule = availableRules[0];
        if (selectedRule.CanBeApplied(levelDecorated, room))
        {
            selectedRule.Apply(levelDecorated, room, decorationsTransform);
        }
    }

    private TileType[,] InitializeDecoratorArray()
    {
        TileType[,] levelDecorated = new TileType[levelTexture.width, levelTexture.height];

        for (int y = levelTexture.height - 1; y >= 0; y--)
        {
            for (int x = 0; x < levelTexture.width - 1; x++)
            {
                Color pixelColor = levelTexture.GetPixel(x, y);
                if (pixelColor == Color.black)
                {
                    levelDecorated[x, y] = TileType.Wall;
                }
                else
                {
                    levelDecorated[x, y] = TileType.Floor;
                }
            }
        }

        return levelDecorated;
    }

    private void GenerateTectureFromTileType(TileType[,] tileTypes)
    {
        int width = tileTypes.GetLength(0);
        int length = tileTypes.GetLength(1);

        Color32[] pixels = new Color32[width * length];
        for (int y = 0; y < length; y++)
        {
            for (int x = 0; x < width; x++)
            {
                pixels[x + (y * width)] = tileTypes[x, y].GetColor();
            }
        }

        decoratedTexture.Reinitialize(width, length);
        decoratedTexture.SetPixels32(pixels);
        decoratedTexture.Apply();
        decoratedTexture.SaveAsset();
    }
}
