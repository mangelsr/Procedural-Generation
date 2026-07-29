using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelBuilder))]
public class LevelBuilderButtons : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LevelBuilder levelBuilder = (LevelBuilder)target;
        if (GUILayout.Button("Generate Random"))
        {
            levelBuilder.GenerateRandom();
        }

        if(GUILayout.Button("Generate"))
        {
            levelBuilder.Generate();
        }
    }
}