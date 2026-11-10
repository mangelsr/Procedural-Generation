using UnityEngine;
using Random = System.Random;

public class PropVariationOr : MonoBehaviour
{
    [Range(0, 1)]
    [SerializeField] double probability;

    [SerializeField] GameObject[] gameObjects;

    public void GenerateVariation()
    {
        Random random = SharedLevelData.Instance.Rand;
        foreach (GameObject gameObject in gameObjects)
        {
            gameObject.SetActive(random.NextDouble() <= probability);
        }        
    }
}
