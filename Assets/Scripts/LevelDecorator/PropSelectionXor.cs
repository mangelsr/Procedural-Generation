using UnityEngine;
using Random = System.Random;

public class PropSelectionXor : MonoBehaviour
{
    [SerializeField] GameObject[] gameObjects;

    [ContextMenu("GenerateVariation")]
    public void GenerateVariation()
    {
        if (gameObjects == null || gameObjects.Length == 0) return;
        Random random = SharedLevelData.Instance.Rand;
        foreach (GameObject gameObject in gameObjects)
        {
            if (gameObject != null)
                gameObject.SetActive(false);
        }
        int selectedIndex = random.Next(0, gameObjects.Length);
        GameObject selectedGameObject = gameObjects[selectedIndex];
        if (selectedGameObject != null)
            selectedGameObject.SetActive(true);
    }

}
