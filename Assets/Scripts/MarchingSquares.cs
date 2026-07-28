using UnityEngine;

public class MarchingSquares : MonoBehaviour
{
    [SerializeField] Texture2D levelTexture;
    [SerializeField] GameObject generatedLevel;
    [SerializeField] Tileset tileset;


    [ContextMenu("Create Level Geometry")]
    public void CreateLevelGeometry()
    {
        generatedLevel.transform.DestroyAllChildren();
        int scale = SharedLevelData.Instance.Scale;
        Vector3 scaleVector = new(scale, scale, scale);

        TextureBasedLevel level = new(levelTexture);

        for (int y = level.Length - 1; y >= 0; y--)
        {
            for (int x = 0; x < level.Width - 1; x++)
            {
                int tileIndex = CalculateTileIndex(level, x, y);
                GameObject prefab = tileset.GetTile(tileIndex);
                if (prefab == null) continue;
                GameObject tile = Instantiate(prefab, generatedLevel.transform);
                tile.transform.localScale = scaleVector;
                tile.transform.position = new Vector3(x * scale, 0, y * scale);
                tile.name = $"x: {x} | y: {y} | tileIndex: {tileIndex}";
            }
        }

    }

    int CalculateTileIndex(ILevel level, int x, int y)
    {
        int topLeft = level.IsBlocked(x, y + 1) ? 1 : 0;
        int topRigth = level.IsBlocked(x + 1, y + 1) ? 1 : 0;
        int bottomLeft = level.IsBlocked(x, y) ? 1 : 0;
        int bottomRigth = level.IsBlocked(x + 1, y) ? 1 : 0;
        int tileIndex = topLeft + (topRigth * 2)
            + (bottomLeft * 4) + (bottomRigth * 8);
        return tileIndex;
    }


}
