using System;
using UnityEngine;
using Random = System.Random;

[Serializable]
public class TileVariant
{
    [SerializeField] GameObject[] variants = new GameObject[0];

    public GameObject GetRandomTile()
    {
        if (variants.Length == 0) return null;
        Random random = SharedLevelData.Instance.Rand;
        int randIndex = random.Next(variants.Length);
        return variants[randIndex];
    }
}
