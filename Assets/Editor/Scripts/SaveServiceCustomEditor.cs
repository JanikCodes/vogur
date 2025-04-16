using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SaveService))]
public class SaveServiceCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SaveService myScript = (SaveService)target;

        if (GUILayout.Button("Save"))
        {
            myScript.Save();
        }

        if (GUILayout.Button("Load"))
        {
            myScript.Load();
        }
    }
}