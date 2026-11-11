#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MarchingSquares))]
public class MarchingSquaresButtons : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MarchingSquares marchingSquares = (MarchingSquares)target;
        if (GUILayout.Button("Create Level Geometry"))
        {
            marchingSquares.CreateLevelGeometry();
        }
    }
}
#endif