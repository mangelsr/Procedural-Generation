#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LayoutGeneratorRooms))]
public class LayoutGeneratorButtons : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LayoutGeneratorRooms layoutGeneratorRooms = (LayoutGeneratorRooms)target;
        if (GUILayout.Button("Generate New Map"))
        {
            layoutGeneratorRooms.GenerateLevel();
        }

        if(GUILayout.Button("Generate New Seed"))
        {
            layoutGeneratorRooms.GenerateNewSeed();
        }

        if(GUILayout.Button("Generate New Seed and Level"))
        {
            layoutGeneratorRooms.GenerateNewSeedAndLevel();
        }
    }
}
#endif